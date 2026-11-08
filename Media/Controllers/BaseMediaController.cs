using DataLayer.Entities;
using DataLayer.ViewModels;
using Media.Infrastructure;
using Media.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Mvc;

namespace Media.Controllers
{
    public abstract class BaseMediaController : Controller
    {
        protected readonly UnitOfWork Context = new UnitOfWork();
        protected ViewAccount CurrentAccount { get; private set; }
        protected long CurrentAccountId { get { return CurrentAccount.Id; } }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            CurrentAccount = Context.Account.GetCurrentAccount();
            if (CurrentAccount == null)
            {
                var returnUrl = Request.Url == null ? VirtualPathUtility.ToAbsolute("~/") : Request.Url.PathAndQuery;
                filterContext.Result = RedirectToRoute("MediaLogin", new { back = returnUrl });
                return;
            }
            ViewBag.CurrentAccount = CurrentAccount;
            base.OnActionExecuting(filterContext);
        }

        protected MediaSessionViewModel ReadSession(IDataRecord row)
        {
            return new MediaSessionViewModel { ID=(int)row["ID"],Title=Convert.ToString(row["Title"]),Description=Convert.ToString(row["Description"]),ImageUrl=Convert.ToString(row["ImageUrl"]),SortOrder=(int)row["SortOrder"],WatchedSeconds=(int)row["WatchedSeconds"],DurationSeconds=row["DurationSeconds"]==DBNull.Value?(int?)null:(int)row["DurationSeconds"],IsCompleted=(bool)row["IsCompleted"] };
        }

        protected void MarkViewed(string table,string key,int id)
        {
            var update="UPDATE dbo."+table+" SET LastVisitDateTime=GETDATE(),IsCompleted=1 WHERE "+key+"=@ID AND AccountId=@AccountId;";
            var sql=update+" IF @@ROWCOUNT=0 BEGIN TRY INSERT dbo."+table+"("+key+",AccountId,FirstVisitDateTime,LastVisitDateTime,WatchedSeconds,DurationSeconds,IsCompleted) VALUES(@ID,@AccountId,GETDATE(),GETDATE(),0,NULL,1); END TRY BEGIN CATCH IF ERROR_NUMBER() IN(2601,2627) "+update+" ELSE THROW; END CATCH";
            using(var cn=MediaDatabase.Open())using(var cmd=new SqlCommand(sql,cn)){cmd.Parameters.Add("@ID",SqlDbType.Int).Value=id;cmd.Parameters.Add("@AccountId",SqlDbType.BigInt).Value=CurrentAccountId;cmd.ExecuteNonQuery();}
        }
        protected void SaveProgress(string table,string key,int id,int watched,int? duration,bool completed)
        {
            var update="UPDATE dbo."+table+" SET LastVisitDateTime=GETDATE(),WatchedSeconds=CASE WHEN WatchedSeconds>@Watched THEN WatchedSeconds ELSE @Watched END,DurationSeconds=COALESCE(@Duration,DurationSeconds),IsCompleted=CASE WHEN @Completed=1 THEN 1 ELSE IsCompleted END WHERE "+key+"=@ID AND AccountId=@AccountId;";
            var sql=update+" IF @@ROWCOUNT=0 BEGIN TRY INSERT dbo."+table+"("+key+",AccountId,FirstVisitDateTime,LastVisitDateTime,WatchedSeconds,DurationSeconds,IsCompleted) VALUES(@ID,@AccountId,GETDATE(),GETDATE(),@Watched,@Duration,@Completed); END TRY BEGIN CATCH IF ERROR_NUMBER() IN(2601,2627) "+update+" ELSE THROW; END CATCH";
            using(var cn=MediaDatabase.Open())using(var cmd=new SqlCommand(sql,cn)){cmd.Parameters.Add("@ID",SqlDbType.Int).Value=id;cmd.Parameters.Add("@AccountId",SqlDbType.BigInt).Value=CurrentAccountId;cmd.Parameters.Add("@Watched",SqlDbType.Int).Value=Math.Max(0,watched);cmd.Parameters.Add("@Duration",SqlDbType.Int).Value=(object)duration??DBNull.Value;cmd.Parameters.Add("@Completed",SqlDbType.Bit).Value=completed;cmd.ExecuteNonQuery();}
        }
        protected string GetVideoUrl(string table,int id)
        {
            if(table!="CourseVideoSession" && table!="VideoCollectionSession") return null;
            using(var cn=MediaDatabase.Open()) using(var cmd=new SqlCommand("SELECT VideoUrl FROM dbo."+table+" WHERE ID=@ID AND Active=1",cn)){cmd.Parameters.Add("@ID",SqlDbType.Int).Value=id;return Convert.ToString(cmd.ExecuteScalar());}
        }

        protected ActionResult RedirectVideo(string source)
        {
            Uri uri;
            if(!Uri.TryCreate(source,UriKind.Absolute,out uri)||(uri.Scheme!="http"&&uri.Scheme!="https"))return HttpNotFound();
            Response.Cache.SetCacheability(HttpCacheability.NoCache);Response.Cache.SetNoStore();return Redirect(source);
        }

        protected override void Dispose(bool disposing){if(disposing)Context.Dispose();base.Dispose(disposing);}
    }
}
