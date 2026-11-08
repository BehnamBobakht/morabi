using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using Panel.Areas.Store.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Mvc;

namespace Panel.Areas.Store.Controllers
{
    public class MediaVideoController : Controller
    {
        private readonly UnitOfWork _context = new UnitOfWork();

        [HttpGet]
        public ActionResult Index(int? productId = null, string search = null, int page = 1)
        {
            CheckPermission(Enum_Permission.Product);
            const int pageSize = 20;
            page = Math.Max(1, page);
            var videos = GetCourseVideos(productId);
            if (!string.IsNullOrWhiteSpace(search))
                videos = videos.Where(x => (x.Title ?? "").Contains(search) || (x.ProductName ?? "").Contains(search) || (x.Description ?? "").Contains(search)).ToList();
            var model = new MediaVideoIndexModel
            {
                ProductId = productId,
                Products = GetProducts(productId),
                TotalCount = videos.Count,
                PageIndex = page,
                PageSize = pageSize,
                CourseVideos = videos.Skip((page - 1) * pageSize).Take(pageSize).ToList(),
                Collections = new List<VideoCollectionModel>()
            };
            return View(model);
        }

        [HttpGet]
        public ActionResult Collections(string search = null, int page = 1)
        {
            CheckPermission(Enum_Permission.Product);
            const int pageSize = 20;
            page = Math.Max(1, page);
            var collections = GetCollections();
            if (!string.IsNullOrWhiteSpace(search))
                collections = collections.Where(x => (x.Title ?? "").Contains(search) || (x.Description ?? "").Contains(search)).ToList();
            return View(new MediaCollectionIndexModel
            {
                Search = search,
                TotalCount = collections.Count,
                PageIndex = page,
                PageSize = pageSize,
                Collections = collections.Skip((page - 1) * pageSize).Take(pageSize).ToList()
            });
        }

        [HttpGet]
        public ActionResult CourseForm(int? id)
        {
            CheckPermission(id.HasValue ? Enum_Permission.Product_Edit : Enum_Permission.Product_New);
            var model = id.HasValue ? GetCourseVideo(id.Value) : new CourseVideoSessionModel { Active = true };
            if (model == null) return HttpNotFound();
            ViewBag.Products = GetProducts(model.ProductId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CourseForm(CourseVideoSessionModel model, HttpPostedFileBase coverFile)
        {
            CheckPermission(model.ID > 0 ? Enum_Permission.Product_Edit : Enum_Permission.Product_New);
            if (!ModelState.IsValid)
            {
                ViewBag.Products = GetProducts(model.ProductId);
                return View(model);
            }

            if (coverFile != null)
                model.PictureId = _context.Picture.CreateAndUpload(Enum_Code.SYSTEM_TYPE_PANEL, coverFile).ID;
            if (!ModelState.IsValid)
            {
                ViewBag.Products = GetProducts(model.ProductId);
                return View(model);
            }
            using (var connection = CreateSqlConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = model.ID == 0
                    ? @"INSERT dbo.CourseVideoSession(ProductId,Title,Description,VideoUrl,SortOrder,Active,PictureId,IsDemo)
                        VALUES(@ProductId,@Title,@Description,@VideoUrl,@SortOrder,@Active,@PictureId,@IsDemo)"
                    : @"UPDATE dbo.CourseVideoSession SET ProductId=@ProductId,Title=@Title,
                        Description=@Description,VideoUrl=@VideoUrl,SortOrder=@SortOrder,
                        Active=@Active,PictureId=@PictureId,IsDemo=@IsDemo,UpdateDateTime=GETDATE() WHERE ID=@ID";
                AddSessionParameters(command, model.ProductId, model.Title, model.Description,
                    model.VideoUrl, model.SortOrder, model.Active, model.PictureId, model.IsDemo);
                command.Parameters.Add("@ID", SqlDbType.Int).Value = model.ID;
                connection.Open();
                command.ExecuteNonQuery();
            }
            TempData["MediaSuccess"] = "ویدئوی دوره با موفقیت ذخیره شد.";
            return RedirectToAction("Index", new { productId = model.ProductId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCourse(int id)
        {
            CheckPermission(Enum_Permission.Product_Delete);
            Execute("DELETE dbo.CourseVideoSession WHERE ID=@ID", new SqlParameter("@ID", id));
            TempData["MediaSuccess"] = "ویدئوی دوره حذف شد.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult CollectionForm(int? id)
        {
            CheckPermission(id.HasValue ? Enum_Permission.Product_Edit : Enum_Permission.Product_New);
            var model = id.HasValue ? GetCollection(id.Value) : new VideoCollectionModel { Active = true };
            if (model == null) return HttpNotFound();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CollectionForm(VideoCollectionModel model, HttpPostedFileBase coverFile)
        {
            CheckPermission(model.ID > 0 ? Enum_Permission.Product_Edit : Enum_Permission.Product_New);
            if (coverFile != null)
                model.PictureId = _context.Picture.CreateAndUpload(Enum_Code.SYSTEM_TYPE_PANEL, coverFile).ID;
            if (!ModelState.IsValid) return View(model);
            using (var connection = CreateSqlConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = model.ID == 0
                    ? @"INSERT dbo.VideoCollection(Title,Description,PictureId,Active)
                        VALUES(@Title,@Description,@PictureId,@Active)"
                    : @"UPDATE dbo.VideoCollection SET Title=@Title,Description=@Description,
                        PictureId=@PictureId,Active=@Active,UpdateDateTime=GETDATE() WHERE ID=@ID";
                command.Parameters.Add("@Title", SqlDbType.NVarChar, 250).Value = model.Title.Trim();
                command.Parameters.Add("@Description", SqlDbType.NVarChar, -1).Value = DbValue(model.Description);
                command.Parameters.Add("@PictureId", SqlDbType.Int).Value = (object)model.PictureId ?? DBNull.Value;
                command.Parameters.Add("@Active", SqlDbType.Bit).Value = model.Active;
                command.Parameters.Add("@ID", SqlDbType.Int).Value = model.ID;
                connection.Open();
                command.ExecuteNonQuery();
            }
            TempData["MediaSuccess"] = "مجموعه ویدئویی با موفقیت ذخیره شد.";
            return RedirectToAction("Collections");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCollection(int id)
        {
            CheckPermission(Enum_Permission.Product_Delete);
            try
            {
                Execute("DELETE dbo.VideoCollection WHERE ID=@ID", new SqlParameter("@ID", id));
                TempData["MediaSuccess"] = "مجموعه ویدئویی حذف شد.";
            }
            catch (SqlException)
            {
                TempData["MediaError"] = "این مجموعه جلسه، کاربر یا سابقه مشاهده دارد و قابل حذف نیست. ابتدا آن‌ها را حذف یا مجموعه را غیرفعال کنید.";
            }
            return RedirectToAction("Collections");
        }

        [HttpGet]
        public ActionResult Sessions(int id, string search = null, int page = 1)
        {
            CheckPermission(Enum_Permission.Product);
            const int pageSize = 20;
            page = Math.Max(1, page);
            var collection = GetCollection(id);
            if (collection == null) return HttpNotFound();
            var sessions = GetCollectionSessions(id);
            if (!string.IsNullOrWhiteSpace(search))
                sessions = sessions.Where(x => (x.Title ?? "").Contains(search) || (x.Description ?? "").Contains(search)).ToList();
            return View(new CollectionSessionsIndexModel
            {
                Collection = collection,
                Search = search,
                TotalCount = sessions.Count,
                PageIndex = page,
                PageSize = pageSize,
                Sessions = sessions.Skip((page - 1) * pageSize).Take(pageSize).ToList()
            });
        }

        [HttpGet]
        public ActionResult SessionForm(int collectionId, int? id)
        {
            CheckPermission(id.HasValue ? Enum_Permission.Product_Edit : Enum_Permission.Product_New);
            var collection = GetCollection(collectionId);
            if (collection == null) return HttpNotFound();
            var model = id.HasValue
                ? GetCollectionSession(id.Value)
                : new VideoCollectionSessionModel { VideoCollectionId = collectionId, Active = true };
            if (model == null || model.VideoCollectionId != collectionId) return HttpNotFound();
            model.CollectionTitle = collection.Title;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SessionForm(VideoCollectionSessionModel model, HttpPostedFileBase coverFile)
        {
            CheckPermission(model.ID > 0 ? Enum_Permission.Product_Edit : Enum_Permission.Product_New);
            if (!ModelState.IsValid)
            {
                var collection = GetCollection(model.VideoCollectionId);
                model.CollectionTitle = collection == null ? null : collection.Title;
                return View(model);
            }
            if (coverFile != null)
                model.PictureId = _context.Picture.CreateAndUpload(Enum_Code.SYSTEM_TYPE_PANEL, coverFile).ID;
            if (!ModelState.IsValid)
            {
                var collection = GetCollection(model.VideoCollectionId);
                model.CollectionTitle = collection == null ? null : collection.Title;
                return View(model);
            }
            using (var connection = CreateSqlConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = model.ID == 0
                    ? @"INSERT dbo.VideoCollectionSession(VideoCollectionId,Title,Description,VideoUrl,SortOrder,Active,PictureId)
                        VALUES(@CollectionId,@Title,@Description,@VideoUrl,@SortOrder,@Active,@PictureId)"
                    : @"UPDATE dbo.VideoCollectionSession SET Title=@Title,Description=@Description,
                        VideoUrl=@VideoUrl,SortOrder=@SortOrder,Active=@Active,PictureId=@PictureId,
                        UpdateDateTime=GETDATE() WHERE ID=@ID AND VideoCollectionId=@CollectionId";
                command.Parameters.Add("@CollectionId", SqlDbType.Int).Value = model.VideoCollectionId;
                AddSessionParameters(command, 0, model.Title, model.Description,
                    model.VideoUrl, model.SortOrder, model.Active, model.PictureId, false, false);
                command.Parameters.Add("@ID", SqlDbType.Int).Value = model.ID;
                connection.Open();
                command.ExecuteNonQuery();
            }
            TempData["MediaSuccess"] = "جلسه مجموعه با موفقیت ذخیره شد.";
            return RedirectToAction("Sessions", new { id = model.VideoCollectionId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSession(int id, int collectionId)
        {
            CheckPermission(Enum_Permission.Product_Delete);
            try
            {
                Execute("DELETE dbo.VideoCollectionSession WHERE ID=@ID AND VideoCollectionId=@CollectionId",
                    new SqlParameter("@ID", id), new SqlParameter("@CollectionId", collectionId));
                TempData["MediaSuccess"] = "جلسه مجموعه حذف شد.";
            }
            catch (SqlException)
            {
                TempData["MediaError"] = "این جلسه سابقه مشاهده دارد و قابل حذف نیست؛ آن را غیرفعال کنید.";
            }
            return RedirectToAction("Sessions", new { id = collectionId });
        }

        [HttpGet]
        public ActionResult Accounts(int id, string search = null, int page = 1)
        {
            CheckPermission(Enum_Permission.Product_Edit);
            var collection = GetCollection(id);
            if (collection == null) return HttpNotFound();
            const int pageSize = 20;
            page = Math.Max(1, page);
            var accounts = GetCollectionAccounts(id, search);
            return View(new CollectionAccountsModel
            {
                Collection = collection,
                Search = search,
                TotalCount = accounts.Count,
                PageIndex = page,
                PageSize = pageSize,
                Accounts = accounts.Skip((page - 1) * pageSize).Take(pageSize).ToList()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignAccounts(int collectionId, long[] accountIds)
        {
            CheckPermission(Enum_Permission.Product_Edit);
            var siteUser = _context.SiteUser.GetCurrentUser();
            var ids = (accountIds ?? new long[0]).Where(x => x > 0).Distinct().ToArray();
            using (var connection = CreateSqlConnection())
            {
                connection.Open();
                foreach (var accountId in ids)
                {
                    using (var command = new SqlCommand(
                        @"IF EXISTS (SELECT 1 FROM dbo.VideoCollectionAccount WHERE VideoCollectionId=@CollectionId AND AccountId=@AccountId)
                            UPDATE dbo.VideoCollectionAccount SET Active=1,AssignedBySiteUserId=@SiteUserId,AssignedDateTime=GETDATE()
                            WHERE VideoCollectionId=@CollectionId AND AccountId=@AccountId
                          ELSE
                            INSERT dbo.VideoCollectionAccount(VideoCollectionId,AccountId,AssignedBySiteUserId,Active)
                            VALUES(@CollectionId,@AccountId,@SiteUserId,1)", connection))
                    {
                        command.Parameters.Add("@CollectionId", SqlDbType.Int).Value = collectionId;
                        command.Parameters.Add("@AccountId", SqlDbType.BigInt).Value = accountId;
                        command.Parameters.Add("@SiteUserId", SqlDbType.Int).Value =
                            siteUser == null ? (object)DBNull.Value : siteUser.ID;
                        command.ExecuteNonQuery();
                    }
                }
            }
            TempData["MediaSuccess"] = ids.Length + " دسترسی ثبت شد.";
            return RedirectToAction("Accounts", new { id = collectionId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveAccount(int collectionId, long accountId)
        {
            CheckPermission(Enum_Permission.Product_Edit);
            Execute(@"UPDATE dbo.VideoCollectionAccount SET Active=0
                      WHERE VideoCollectionId=@CollectionId AND AccountId=@AccountId",
                new SqlParameter("@CollectionId", collectionId),
                new SqlParameter("@AccountId", accountId));
            TempData["MediaSuccess"] = "دسترسی کاربر لغو شد.";
            return RedirectToAction("Accounts", new { id = collectionId });
        }
        [HttpGet]
        public ActionResult Visits(string source, int id, string search = null, int page = 1)
        {
            CheckPermission(Enum_Permission.Product);
            if (source != "course" && source != "collection") return HttpNotFound();
            const int pageSize = 20;
            page = Math.Max(1, page);
            var visits = new List<MediaVisitModel>();
            string title;
            var parentId = 0;
            using (var connection = CreateSqlConnection())
            using (var command = connection.CreateCommand())
            {
                if (source == "course")
                {
                    command.CommandText = @"SELECT s.Title MediaTitle,0 ParentId,a.ID AccountId,
                        LTRIM(RTRIM(COALESCE(a.FullName,N'')+N' '+COALESCE(a.LatName,N''))) FullName,
                        a.Mobile,a.NationalCode,
                        v.LastVisitDateTime
                      FROM dbo.CourseVideoSession s
                      INNER JOIN dbo.CourseVideoVisit v ON v.CourseVideoSessionId=s.ID
                      INNER JOIN dbo.Account a ON a.ID=v.AccountId
                      WHERE s.ID=@ID AND (@Search IS NULL OR a.FullName LIKE N'%'+@Search+N'%'
                        OR a.LatName LIKE N'%'+@Search+N'%' OR a.Mobile LIKE N'%'+@Search+N'%'
                        OR a.NationalCode LIKE N'%'+@Search+N'%')
                      ORDER BY v.LastVisitDateTime DESC";
                }
                else
                {
                    command.CommandText = @"SELECT s.Title MediaTitle,s.VideoCollectionId ParentId,a.ID AccountId,
                        LTRIM(RTRIM(COALESCE(a.FullName,N'')+N' '+COALESCE(a.LatName,N''))) FullName,
                        a.Mobile,a.NationalCode,
                        v.LastVisitDateTime
                      FROM dbo.VideoCollectionSession s
                      INNER JOIN dbo.VideoCollectionVisit v ON v.VideoCollectionSessionId=s.ID
                      INNER JOIN dbo.Account a ON a.ID=v.AccountId
                      WHERE s.ID=@ID AND (@Search IS NULL OR a.FullName LIKE N'%'+@Search+N'%'
                        OR a.LatName LIKE N'%'+@Search+N'%' OR a.Mobile LIKE N'%'+@Search+N'%'
                        OR a.NationalCode LIKE N'%'+@Search+N'%')
                      ORDER BY v.LastVisitDateTime DESC";
                }
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                command.Parameters.Add("@Search", SqlDbType.NVarChar, 100).Value = DbValue(search);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    title = null;
                    while (reader.Read())
                    {
                        title = Convert.ToString(reader["MediaTitle"]);
                        parentId = (int)reader["ParentId"];
                        visits.Add(new MediaVisitModel
                        {
                            AccountId = (long)reader["AccountId"],
                            FullName = Convert.ToString(reader["FullName"]),
                            Mobile = DbString(reader["Mobile"]),
                            NationalCode = DbString(reader["NationalCode"]),
                            LastVisitDateTime = (DateTime)reader["LastVisitDateTime"]
                        });
                    }
                }
            }
            if (title == null)
            {
                title = source == "course" ? GetCourseVideo(id)?.Title : GetCollectionSession(id)?.Title;
                if (source == "collection") parentId = GetCollectionSession(id)?.VideoCollectionId ?? 0;
            }
            if (title == null) return HttpNotFound();
            return View(new MediaVisitIndexModel
            {
                Source = source, MediaId = id, ParentId = parentId, MediaTitle = title, Search = search,
                TotalCount = visits.Count, PageIndex = page, PageSize = pageSize,
                Visits = visits.Skip((page - 1) * pageSize).Take(pageSize).ToList()
            });
        }
        private IList<CourseVideoSessionModel> GetCourseVideos(int? productId)
        {
            var result = new List<CourseVideoSessionModel>();
            using (var connection = CreateSqlConnection())
            using (var command = new SqlCommand(
                @"SELECT s.ID,s.ProductId,p.Name ProductName,s.Title,s.Description,s.VideoUrl,s.PictureId,REPLACE(pic.Url,'SYSTEM_TYPE_PANEL','/panel') PictureUrl,s.SortOrder,s.Active,s.IsDemo,
                  (SELECT COUNT(*) FROM dbo.CourseVideoVisit v WHERE v.CourseVideoSessionId=s.ID) VisitCount
                  FROM dbo.CourseVideoSession s INNER JOIN dbo.Product p ON p.ID=s.ProductId LEFT JOIN dbo.Picture pic ON pic.ID=s.PictureId
                  WHERE @ProductId IS NULL OR s.ProductId=@ProductId
                  ORDER BY p.Name,s.SortOrder,s.ID", connection))
            {
                command.Parameters.Add("@ProductId", SqlDbType.Int).Value = (object)productId ?? DBNull.Value;
                connection.Open();
                using (var reader = command.ExecuteReader())
                    while (reader.Read()) result.Add(ReadCourseVideo(reader));
            }
            return result;
        }

        private CourseVideoSessionModel GetCourseVideo(int id)
        {
            using (var connection = CreateSqlConnection())
            using (var command = new SqlCommand(
                @"SELECT s.ID,s.ProductId,p.Name ProductName,s.Title,s.Description,s.VideoUrl,s.PictureId,REPLACE(pic.Url,'SYSTEM_TYPE_PANEL','/panel') PictureUrl,s.SortOrder,s.Active,s.IsDemo,
                  (SELECT COUNT(*) FROM dbo.CourseVideoVisit v WHERE v.CourseVideoSessionId=s.ID) VisitCount
                  FROM dbo.CourseVideoSession s INNER JOIN dbo.Product p ON p.ID=s.ProductId LEFT JOIN dbo.Picture pic ON pic.ID=s.PictureId WHERE s.ID=@ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                connection.Open();
                using (var reader = command.ExecuteReader())
                    return reader.Read() ? ReadCourseVideo(reader) : null;
            }
        }

        private static CourseVideoSessionModel ReadCourseVideo(IDataRecord row)
        {
            return new CourseVideoSessionModel
            {
                ID = (int)row["ID"], ProductId = (int)row["ProductId"],
                ProductName = Convert.ToString(row["ProductName"]), Title = Convert.ToString(row["Title"]),
                Description = DbString(row["Description"]), VideoUrl = Convert.ToString(row["VideoUrl"]),
                PictureId = row["PictureId"] == DBNull.Value ? (int?)null : (int)row["PictureId"],
                PictureUrl = DbString(row["PictureUrl"]),
                SortOrder = (int)row["SortOrder"], Active = (bool)row["Active"], IsDemo = (bool)row["IsDemo"],
                VisitCount = (int)row["VisitCount"]
            };
        }

        private IList<VideoCollectionModel> GetCollections()
        {
            var result = new List<VideoCollectionModel>();
            using (var connection = CreateSqlConnection())
            using (var command = new SqlCommand(
                @"SELECT c.ID,c.Title,c.Description,c.PictureId,REPLACE(pic.Url,'SYSTEM_TYPE_PANEL','/panel') PictureUrl,c.Active,
                  (SELECT COUNT(*) FROM dbo.VideoCollectionSession s WHERE s.VideoCollectionId=c.ID) SessionCount,
                  (SELECT COUNT(*) FROM dbo.VideoCollectionAccount a WHERE a.VideoCollectionId=c.ID AND a.Active=1) AccountCount
                  FROM dbo.VideoCollection c LEFT JOIN dbo.Picture pic ON pic.ID=c.PictureId ORDER BY c.ID DESC", connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                    while (reader.Read()) result.Add(ReadCollection(reader));
            }
            return result;
        }

        private VideoCollectionModel GetCollection(int id)
        {
            using (var connection = CreateSqlConnection())
            using (var command = new SqlCommand(
                @"SELECT c.ID,c.Title,c.Description,c.PictureId,REPLACE(pic.Url,'SYSTEM_TYPE_PANEL','/panel') PictureUrl,c.Active,
                  (SELECT COUNT(*) FROM dbo.VideoCollectionSession s WHERE s.VideoCollectionId=c.ID) SessionCount,
                  (SELECT COUNT(*) FROM dbo.VideoCollectionAccount a WHERE a.VideoCollectionId=c.ID AND a.Active=1) AccountCount
                  FROM dbo.VideoCollection c LEFT JOIN dbo.Picture pic ON pic.ID=c.PictureId WHERE c.ID=@ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                connection.Open();
                using (var reader = command.ExecuteReader())
                    return reader.Read() ? ReadCollection(reader) : null;
            }
        }

        private static VideoCollectionModel ReadCollection(IDataRecord row)
        {
            return new VideoCollectionModel
            {
                ID = (int)row["ID"], Title = Convert.ToString(row["Title"]),
                Description = DbString(row["Description"]), PictureId = row["PictureId"] == DBNull.Value ? (int?)null : (int)row["PictureId"],
                PictureUrl = DbString(row["PictureUrl"]),
                Active = (bool)row["Active"], SessionCount = (int)row["SessionCount"],
                AccountCount = (int)row["AccountCount"]
            };
        }

        private IList<VideoCollectionSessionModel> GetCollectionSessions(int collectionId)
        {
            var result = new List<VideoCollectionSessionModel>();
            using (var connection = CreateSqlConnection())
            using (var command = new SqlCommand(
                @"SELECT s.ID,s.VideoCollectionId,c.Title CollectionTitle,s.Title,s.Description,s.VideoUrl,s.PictureId,REPLACE(pic.Url,'SYSTEM_TYPE_PANEL','/panel') PictureUrl,s.SortOrder,s.Active
                  FROM dbo.VideoCollectionSession s INNER JOIN dbo.VideoCollection c ON c.ID=s.VideoCollectionId LEFT JOIN dbo.Picture pic ON pic.ID=s.PictureId
                  WHERE s.VideoCollectionId=@CollectionId ORDER BY s.SortOrder,s.ID", connection))
            {
                command.Parameters.Add("@CollectionId", SqlDbType.Int).Value = collectionId;
                connection.Open();
                using (var reader = command.ExecuteReader())
                    while (reader.Read()) result.Add(ReadCollectionSession(reader));
            }
            return result;
        }

        private VideoCollectionSessionModel GetCollectionSession(int id)
        {
            using (var connection = CreateSqlConnection())
            using (var command = new SqlCommand(
                @"SELECT s.ID,s.VideoCollectionId,c.Title CollectionTitle,s.Title,s.Description,s.VideoUrl,s.PictureId,REPLACE(pic.Url,'SYSTEM_TYPE_PANEL','/panel') PictureUrl,s.SortOrder,s.Active
                  FROM dbo.VideoCollectionSession s INNER JOIN dbo.VideoCollection c ON c.ID=s.VideoCollectionId LEFT JOIN dbo.Picture pic ON pic.ID=s.PictureId WHERE s.ID=@ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                connection.Open();
                using (var reader = command.ExecuteReader())
                    return reader.Read() ? ReadCollectionSession(reader) : null;
            }
        }

        private IList<CollectionAccountModel> GetCollectionAccounts(int collectionId, string search)
        {
            var result = new List<CollectionAccountModel>();
            using (var connection = CreateSqlConnection())
            using (var command = new SqlCommand(
                @"SELECT a.ID AccountId,
                    LTRIM(RTRIM(COALESCE(a.FullName,N'') + N' ' + COALESCE(a.LatName,N''))) FullName,
                    a.Mobile,a.NationalCode,
                    CONVERT(bit,CASE WHEN vca.Active=1 THEN 1 ELSE 0 END) Assigned
                  FROM dbo.Account a
                  LEFT JOIN dbo.VideoCollectionAccount vca
                    ON vca.AccountId=a.ID AND vca.VideoCollectionId=@CollectionId
                  WHERE (@Search IS NULL OR a.FullName LIKE N'%'+@Search+N'%'
                    OR a.LatName LIKE N'%'+@Search+N'%' OR a.Mobile LIKE N'%'+@Search+N'%'
                    OR a.NationalCode LIKE N'%'+@Search+N'%')
                    AND (vca.Active=1 OR @Search IS NOT NULL)
                  ORDER BY CASE WHEN vca.Active=1 THEN 0 ELSE 1 END,a.ID DESC", connection))
            {
                command.Parameters.Add("@CollectionId", SqlDbType.Int).Value = collectionId;
                command.Parameters.Add("@Search", SqlDbType.NVarChar, 100).Value = DbValue(search);
                connection.Open();
                using (var reader = command.ExecuteReader())
                    while (reader.Read())
                        result.Add(new CollectionAccountModel
                        {
                            AccountId = (long)reader["AccountId"],
                            FullName = Convert.ToString(reader["FullName"]),
                            Mobile = DbString(reader["Mobile"]),
                            NationalCode = DbString(reader["NationalCode"]),
                            Assigned = (bool)reader["Assigned"]
                        });
            }
            return result;
        }
        private static VideoCollectionSessionModel ReadCollectionSession(IDataRecord row)
        {
            return new VideoCollectionSessionModel
            {
                ID = (int)row["ID"], VideoCollectionId = (int)row["VideoCollectionId"],
                CollectionTitle = Convert.ToString(row["CollectionTitle"]), Title = Convert.ToString(row["Title"]),
                Description = DbString(row["Description"]), VideoUrl = Convert.ToString(row["VideoUrl"]),
                PictureId = row["PictureId"] == DBNull.Value ? (int?)null : (int)row["PictureId"],
                PictureUrl = DbString(row["PictureUrl"]),
                SortOrder = (int)row["SortOrder"], Active = (bool)row["Active"]
            };
        }

        private IEnumerable<SelectListItem> GetProducts(int? selected)
        {
            return _context.Product.GetAll()
                .Where(p => p.Deleted != true)
                .OrderByDescending(p => p.ID)
                .Select(p => new SelectListItem
                {
                    Value = p.ID.ToString(), Text = p.Name, Selected = selected == p.ID
                }).ToList();
        }

        private void CheckPermission(Enum_Permission permission)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), permission);
        }

        private static void AddSessionParameters(SqlCommand command, int productId, string title,
            string description, string videoUrl, int sortOrder, bool active, int? pictureId, bool isDemo = false, bool includeProduct = true)
        {
            if (includeProduct) command.Parameters.Add("@ProductId", SqlDbType.Int).Value = productId;
            command.Parameters.Add("@Title", SqlDbType.NVarChar, 250).Value = title.Trim();
            command.Parameters.Add("@Description", SqlDbType.NVarChar, -1).Value = DbValue(description);
            command.Parameters.Add("@VideoUrl", SqlDbType.NVarChar, 2000).Value = videoUrl.Trim();
            command.Parameters.Add("@SortOrder", SqlDbType.Int).Value = sortOrder;
            command.Parameters.Add("@Active", SqlDbType.Bit).Value = active;
            command.Parameters.Add("@PictureId", SqlDbType.Int).Value = (object)pictureId ?? DBNull.Value;
            if (includeProduct) command.Parameters.Add("@IsDemo", SqlDbType.Bit).Value = isDemo;
        }

        private static void Execute(string sql, params SqlParameter[] parameters)
        {
            using (var connection = CreateSqlConnection())
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddRange(parameters);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        private static object DbValue(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? (object)DBNull.Value : value.Trim();
        }

        private static string DbString(object value)
        {
            return value == DBNull.Value ? null : Convert.ToString(value);
        }

        private static SqlConnection CreateSqlConnection()
        {
            var setting = ConfigurationManager.ConnectionStrings["DatabaseEntities"];
            if (setting == null)
                throw new ConfigurationErrorsException("Connection string 'DatabaseEntities' was not found.");
            var builder = new EntityConnectionStringBuilder(setting.ConnectionString);
            return new SqlConnection(builder.ProviderConnectionString);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _context.Dispose();
            base.Dispose(disposing);
        }
    }
}
