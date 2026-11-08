using Media.Infrastructure;
using Media.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.SessionState;

namespace Media.Controllers
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class CoursesController : BaseMediaController
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Learning");
        }

        public ActionResult Watch(int id, int? sessionId)
        {
            var ownsCourse = Owns(id);
            var sessions = Sessions(id, ownsCourse);
            if (!sessions.Any()) return HttpNotFound();

            var current = sessionId.HasValue
                ? sessions.FirstOrDefault(x => x.ID == sessionId.Value)
                : sessions.FirstOrDefault(x => !x.IsLocked);

            if (current == null) return HttpNotFound();
            if (current.IsLocked) return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            return View(new MediaWatchViewModel
            {
                OwnerId = id,
                OwnerTitle = CourseName(id),
                Current = current,
                Sessions = sessions,
                StreamUrl = Url.Action("Stream", new { id = current.ID }),
                ProgressUrl = Url.Action("Progress"),
                OwnsCourse = ownsCourse
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Progress(int id, int watchedSeconds, int? durationSeconds, bool completed = false)
        {
            if (!CanWatchSession(id)) return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            SaveProgress("CourseVideoVisit", "CourseVideoSessionId", id, watchedSeconds, durationSeconds, completed);
            return Json(new { ok = true });
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Stream(int id)
        {
            if (!CanWatchSession(id)) return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            return RedirectVideo(GetVideoUrl("CourseVideoSession", id));
        }

        private bool CanWatchSession(int sessionId)
        {
            using (var cn = MediaDatabase.Open())
            using (var cmd = new SqlCommand("SELECT ProductId,IsDemo FROM dbo.CourseVideoSession WHERE ID=@ID AND Active=1", cn))
            {
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = sessionId;
                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read()) return false;
                    var productId = (int)reader["ProductId"];
                    var isDemo = (bool)reader["IsDemo"];
                    return isDemo || Owns(productId);
                }
            }
        }
        private bool Owns(int productId)
        {
            const string sql = @"SELECT CASE WHEN EXISTS(
                SELECT 1 FROM dbo.AccountOrderProduct aop
                INNER JOIN dbo.AccountOrder ao ON ao.ID=aop.OrderId
                INNER JOIN dbo.Product purchasedProduct ON purchasedProduct.ID=aop.ProductId
                LEFT JOIN dbo.Code c ON c.ID=ao.StatusId
                WHERE ao.AccountId=@A
                  AND (c.Label='ORDER_STATUS_SUCCESS' OR ao.StatusId IN(2068,2073))
                  AND (aop.ProductId=@P OR ','+REPLACE(ISNULL(purchasedProduct.ProductsId,''),' ','')+',' LIKE '%,'+CONVERT(varchar(20),@P)+',%')
            ) THEN 1 ELSE 0 END";
            using (var cn = MediaDatabase.Open())
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.Add("@A", SqlDbType.BigInt).Value = CurrentAccountId;
                cmd.Parameters.Add("@P", SqlDbType.Int).Value = productId;
                return Convert.ToBoolean(cmd.ExecuteScalar());
            }
        }

        private string CourseName(int id)
        {
            using (var cn = MediaDatabase.Open())
            using (var cmd = new SqlCommand("SELECT Name FROM dbo.Product WHERE ID=@ID", cn))
            {
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                return Convert.ToString(cmd.ExecuteScalar());
            }
        }

        private List<MediaSessionViewModel> Sessions(int productId, bool ownsCourse)
        {
            var list = new List<MediaSessionViewModel>();
            const string sql = @"SELECT s.ID,s.Title,s.Description,s.SortOrder,s.IsDemo,
                REPLACE(pic.Url,'SYSTEM_TYPE_PANEL','/panel') ImageUrl,
                ISNULL(v.WatchedSeconds,0) WatchedSeconds,v.DurationSeconds,CONVERT(bit,ISNULL(v.IsCompleted,0)) IsCompleted
                FROM dbo.CourseVideoSession s
                LEFT JOIN dbo.Picture pic ON pic.ID=s.PictureId
                LEFT JOIN dbo.CourseVideoVisit v ON v.CourseVideoSessionId=s.ID AND v.AccountId=@A
                WHERE s.ProductId=@P AND s.Active=1
                ORDER BY s.SortOrder,s.ID";
            using (var cn = MediaDatabase.Open())
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.Add("@A", SqlDbType.BigInt).Value = CurrentAccountId;
                cmd.Parameters.Add("@P", SqlDbType.Int).Value = productId;
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var isDemo = (bool)reader["IsDemo"];
                        list.Add(new MediaSessionViewModel
                        {
                            ID = (int)reader["ID"],
                            Title = Convert.ToString(reader["Title"]),
                            Description = Convert.ToString(reader["Description"]),
                            ImageUrl = Convert.ToString(reader["ImageUrl"]),
                            SortOrder = (int)reader["SortOrder"],
                            WatchedSeconds = (int)reader["WatchedSeconds"],
                            DurationSeconds = reader["DurationSeconds"] == DBNull.Value ? (int?)null : (int)reader["DurationSeconds"],
                            IsCompleted = (bool)reader["IsCompleted"],
                            IsDemo = isDemo,
                            IsLocked = !ownsCourse && !isDemo
                        });
                    }
                }
            }
            return list;
        }
    }
}