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
 public class CoursesController:BaseMediaController
 {
  public ActionResult Index(){var list=new List<MediaCardViewModel>();using(var cn=MediaDatabase.Open())using(var cmd=new SqlCommand(@"SELECT p.ID,p.Name,REPLACE(pic.Url,'SYSTEM_TYPE_PANEL','/panel') ImageUrl,COUNT(DISTINCT s.ID) SessionCount,COUNT(DISTINCT v.CourseVideoSessionId) CompletedCount FROM dbo.Product p INNER JOIN dbo.CourseVideoSession s ON s.ProductId=p.ID AND s.Active=1 INNER JOIN dbo.AccountOrderProduct aop ON aop.ProductId=p.ID INNER JOIN dbo.AccountOrder ao ON ao.ID=aop.OrderId AND ao.AccountId=@A LEFT JOIN dbo.Code st ON st.ID=ao.StatusId LEFT JOIN dbo.Picture pic ON pic.ID=p.PictureId LEFT JOIN dbo.CourseVideoVisit v ON v.CourseVideoSessionId=s.ID AND v.AccountId=@A WHERE st.Label='ORDER_STATUS_SUCCESS' OR ao.StatusId IN(2068,2073) GROUP BY p.ID,p.Name,pic.Url ORDER BY p.ID DESC",cn)){cmd.Parameters.Add("@A",SqlDbType.BigInt).Value=CurrentAccountId;using(var r=cmd.ExecuteReader())while(r.Read())list.Add(new MediaCardViewModel{ID=(int)r["ID"],Title=Convert.ToString(r["Name"]),ImageUrl=Convert.ToString(r["ImageUrl"]),SessionCount=(int)r["SessionCount"],CompletedCount=(int)r["CompletedCount"]});}return View(list);}
  public ActionResult Watch(int id,int? sessionId){if(!Owns(id))return new HttpStatusCodeResult(HttpStatusCode.Forbidden);var sessions=Sessions(id);if(!sessions.Any())return HttpNotFound();var current=sessionId.HasValue?sessions.FirstOrDefault(x=>x.ID==sessionId):sessions.First();if(current==null)return HttpNotFound();MarkViewed("CourseVideoVisit","CourseVideoSessionId",current.ID);current.IsCompleted=true;return View(new MediaWatchViewModel{OwnerId=id,OwnerTitle=CourseName(id),Current=current,Sessions=sessions,StreamUrl=GetVideoUrl("CourseVideoSession",current.ID)});}
  bool Owns(int p){using(var cn=MediaDatabase.Open())using(var cmd=new SqlCommand(@"SELECT CASE WHEN EXISTS(SELECT 1 FROM dbo.AccountOrderProduct aop INNER JOIN dbo.AccountOrder ao ON ao.ID=aop.OrderId LEFT JOIN dbo.Code c ON c.ID=ao.StatusId WHERE ao.AccountId=@A AND aop.ProductId=@P AND(c.Label='ORDER_STATUS_SUCCESS' OR ao.StatusId IN(2068,2073))) THEN 1 ELSE 0 END",cn)){cmd.Parameters.Add("@A",SqlDbType.BigInt).Value=CurrentAccountId;cmd.Parameters.Add("@P",SqlDbType.Int).Value=p;return Convert.ToBoolean(cmd.ExecuteScalar());}}
  string CourseName(int id){using(var cn=MediaDatabase.Open())using(var cmd=new SqlCommand("SELECT Name FROM dbo.Product WHERE ID=@ID",cn)){cmd.Parameters.Add("@ID",SqlDbType.Int).Value=id;return Convert.ToString(cmd.ExecuteScalar());}}
  List<MediaSessionViewModel> Sessions(int p){var list=new List<MediaSessionViewModel>();using(var cn=MediaDatabase.Open())using(var cmd=new SqlCommand(@"SELECT s.ID,s.Title,s.Description,s.SortOrder,REPLACE(pic.Url,'SYSTEM_TYPE_PANEL','/panel') ImageUrl,CONVERT(bit,CASE WHEN v.CourseVideoSessionId IS NULL THEN 0 ELSE 1 END) IsCompleted FROM dbo.CourseVideoSession s LEFT JOIN dbo.Picture pic ON pic.ID=s.PictureId LEFT JOIN dbo.CourseVideoVisit v ON v.CourseVideoSessionId=s.ID AND v.AccountId=@A WHERE s.ProductId=@P AND s.Active=1 ORDER BY s.SortOrder,s.ID",cn)){cmd.Parameters.Add("@A",SqlDbType.BigInt).Value=CurrentAccountId;cmd.Parameters.Add("@P",SqlDbType.Int).Value=p;using(var r=cmd.ExecuteReader())while(r.Read())list.Add(ReadSession(r));}return list;}
 }
}