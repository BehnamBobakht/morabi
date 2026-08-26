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
 public class MyVideosController:BaseMediaController
 {
  public ActionResult Index(){var list=new List<MediaCardViewModel>();using(var cn=MediaDatabase.Open())using(var cmd=new SqlCommand(@"SELECT c.ID,c.Title,REPLACE(pic.Url,'SYSTEM_TYPE_PANEL','/panel') ImageUrl,COUNT(DISTINCT s.ID) SessionCount,COUNT(DISTINCT v.VideoCollectionSessionId) CompletedCount FROM dbo.VideoCollection c INNER JOIN dbo.VideoCollectionAccount ca ON ca.VideoCollectionId=c.ID AND ca.AccountId=@A AND ca.Active=1 LEFT JOIN dbo.Picture pic ON pic.ID=c.PictureId LEFT JOIN dbo.VideoCollectionSession s ON s.VideoCollectionId=c.ID AND s.Active=1 LEFT JOIN dbo.VideoCollectionVisit v ON v.VideoCollectionSessionId=s.ID AND v.AccountId=@A WHERE c.Active=1 GROUP BY c.ID,c.Title,pic.Url ORDER BY c.ID DESC",cn)){cmd.Parameters.Add("@A",SqlDbType.BigInt).Value=CurrentAccountId;using(var r=cmd.ExecuteReader())while(r.Read())list.Add(new MediaCardViewModel{ID=(int)r["ID"],Title=Convert.ToString(r["Title"]),ImageUrl=Convert.ToString(r["ImageUrl"]),SessionCount=(int)r["SessionCount"],CompletedCount=(int)r["CompletedCount"]});}return View(list);}
  public ActionResult Watch(int id,int? sessionId){if(!Owns(id))return new HttpStatusCodeResult(HttpStatusCode.Forbidden);var sessions=Sessions(id);if(!sessions.Any())return HttpNotFound();var current=sessionId.HasValue?sessions.FirstOrDefault(x=>x.ID==sessionId):sessions.First();if(current==null)return HttpNotFound();MarkViewed("VideoCollectionVisit","VideoCollectionSessionId",current.ID);current.IsCompleted=true;return View(new MediaWatchViewModel{OwnerId=id,OwnerTitle=Title(id),Current=current,Sessions=sessions,StreamUrl=GetVideoUrl("VideoCollectionSession",current.ID)});}
  bool Owns(int id){using(var cn=MediaDatabase.Open())using(var cmd=new SqlCommand("SELECT CASE WHEN EXISTS(SELECT 1 FROM dbo.VideoCollectionAccount WHERE VideoCollectionId=@ID AND AccountId=@A AND Active=1) THEN 1 ELSE 0 END",cn)){cmd.Parameters.Add("@ID",SqlDbType.Int).Value=id;cmd.Parameters.Add("@A",SqlDbType.BigInt).Value=CurrentAccountId;return Convert.ToBoolean(cmd.ExecuteScalar());}}
  string Title(int id){using(var cn=MediaDatabase.Open())using(var cmd=new SqlCommand("SELECT Title FROM dbo.VideoCollection WHERE ID=@ID",cn)){cmd.Parameters.Add("@ID",SqlDbType.Int).Value=id;return Convert.ToString(cmd.ExecuteScalar());}}
  List<MediaSessionViewModel> Sessions(int id){var list=new List<MediaSessionViewModel>();using(var cn=MediaDatabase.Open())using(var cmd=new SqlCommand(@"SELECT s.ID,s.Title,s.Description,s.SortOrder,REPLACE(pic.Url,'SYSTEM_TYPE_PANEL','/panel') ImageUrl,CONVERT(bit,CASE WHEN v.VideoCollectionSessionId IS NULL THEN 0 ELSE 1 END) IsCompleted FROM dbo.VideoCollectionSession s LEFT JOIN dbo.Picture pic ON pic.ID=s.PictureId LEFT JOIN dbo.VideoCollectionVisit v ON v.VideoCollectionSessionId=s.ID AND v.AccountId=@A WHERE s.VideoCollectionId=@ID AND s.Active=1 ORDER BY s.SortOrder,s.ID",cn)){cmd.Parameters.Add("@A",SqlDbType.BigInt).Value=CurrentAccountId;cmd.Parameters.Add("@ID",SqlDbType.Int).Value=id;using(var r=cmd.ExecuteReader())while(r.Read())list.Add(ReadSession(r));}return list;}
 }
}