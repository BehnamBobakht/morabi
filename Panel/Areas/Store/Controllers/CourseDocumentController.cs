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
using System.Web.Mvc;

namespace Panel.Areas.Store.Controllers
{
    public class CourseDocumentController : Controller
    {
        private readonly UnitOfWork _context=new UnitOfWork();

        public ActionResult Index(int? productId=null,string search=null)
        {
            Check(); var items=new List<CourseDocumentModel>();
            const string sql=@"SELECT d.ID,d.ProductId,p.Name ProductTitle,d.Title,d.Description,d.PdfUrl,d.SortOrder,d.Active FROM dbo.CourseDocument d INNER JOIN dbo.Product p ON p.ID=d.ProductId WHERE (@P IS NULL OR d.ProductId=@P) AND (@S IS NULL OR d.Title LIKE N'%'+@S+N'%' OR p.Name LIKE N'%'+@S+N'%') ORDER BY p.Name,d.SortOrder,d.ID";
            using(var cn=Open())using(var cmd=new SqlCommand(sql,cn)){cmd.Parameters.Add("@P",SqlDbType.Int).Value=(object)productId??DBNull.Value;cmd.Parameters.Add("@S",SqlDbType.NVarChar,250).Value=Db(search);using(var r=cmd.ExecuteReader())while(r.Read())items.Add(Read(r));}
            return View(new CourseDocumentIndexModel{Items=items,Products=Products(productId),ProductId=productId,Search=search});
        }

        public ActionResult Form(int? id)
        {
            Check(); var model=id.HasValue?Get(id.Value):new CourseDocumentModel{Active=true}; if(model==null)return HttpNotFound(); ViewBag.Products=Products(model.ProductId); return View(model);
        }

        [HttpPost,ValidateAntiForgeryToken]
        public ActionResult Form(CourseDocumentModel model)
        {
            Check(); if(!ModelState.IsValid){ViewBag.Products=Products(model.ProductId);return View(model);} using(var cn=Open())using(var cmd=cn.CreateCommand())
            {
                cmd.CommandText=model.ID==0?@"INSERT dbo.CourseDocument(ProductId,Title,Description,PdfUrl,SortOrder,Active) VALUES(@P,@T,@D,@U,@O,@A)":@"UPDATE dbo.CourseDocument SET ProductId=@P,Title=@T,Description=@D,PdfUrl=@U,SortOrder=@O,Active=@A,UpdateDateTime=GETDATE() WHERE ID=@ID";
                cmd.Parameters.Add("@P",SqlDbType.Int).Value=model.ProductId;cmd.Parameters.Add("@T",SqlDbType.NVarChar,250).Value=model.Title.Trim();cmd.Parameters.Add("@D",SqlDbType.NVarChar,-1).Value=Db(model.Description);cmd.Parameters.Add("@U",SqlDbType.NVarChar,2000).Value=model.PdfUrl.Trim();cmd.Parameters.Add("@O",SqlDbType.Int).Value=model.SortOrder;cmd.Parameters.Add("@A",SqlDbType.Bit).Value=model.Active;cmd.Parameters.Add("@ID",SqlDbType.Int).Value=model.ID;cmd.ExecuteNonQuery();
            }
            TempData["MediaSuccess"]="PDF دوره با موفقیت ذخیره شد.";return RedirectToAction("Index",new{productId=model.ProductId});
        }

        [HttpPost,ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Check(); using(var cn=Open())using(var cmd=new SqlCommand("DELETE dbo.CourseDocument WHERE ID=@ID AND NOT EXISTS(SELECT 1 FROM dbo.CourseDocumentVisit WHERE CourseDocumentId=@ID)",cn)){cmd.Parameters.Add("@ID",SqlDbType.Int).Value=id;if(cmd.ExecuteNonQuery()==0)TempData["MediaError"]="این PDF سابقه مشاهده دارد؛ آن را غیرفعال کنید.";else TempData["MediaSuccess"]="PDF حذف شد.";}return RedirectToAction("Index");
        }

        public ActionResult Visits(int id,string search=null)
        {
            Check();var list=new List<CourseDocumentVisitModel>();string title=null;const string sql=@"SELECT d.Title,a.ID AccountId,LTRIM(RTRIM(COALESCE(a.FullName,N'')+N' '+COALESCE(a.LatName,N''))) FullName,a.Mobile,a.NationalCode,v.FirstVisitDateTime,v.LastVisitDateTime FROM dbo.CourseDocument d LEFT JOIN dbo.CourseDocumentVisit v ON v.CourseDocumentId=d.ID LEFT JOIN dbo.Account a ON a.ID=v.AccountId WHERE d.ID=@ID AND (v.ID IS NULL OR @S IS NULL OR a.FullName LIKE N'%'+@S+N'%' OR a.LatName LIKE N'%'+@S+N'%' OR a.Mobile LIKE N'%'+@S+N'%' OR a.NationalCode LIKE N'%'+@S+N'%') ORDER BY v.LastVisitDateTime DESC";
            using(var cn=Open())using(var cmd=new SqlCommand(sql,cn)){cmd.Parameters.Add("@ID",SqlDbType.Int).Value=id;cmd.Parameters.Add("@S",SqlDbType.NVarChar,100).Value=Db(search);using(var r=cmd.ExecuteReader())while(r.Read()){title=Convert.ToString(r["Title"]);if(r["AccountId"]!=DBNull.Value)list.Add(new CourseDocumentVisitModel{AccountId=(long)r["AccountId"],FullName=Convert.ToString(r["FullName"]),Mobile=Convert.ToString(r["Mobile"]),NationalCode=Convert.ToString(r["NationalCode"]),FirstVisitDateTime=(DateTime)r["FirstVisitDateTime"],LastVisitDateTime=(DateTime)r["LastVisitDateTime"]});}}
            if(title==null)return HttpNotFound();return View(new CourseDocumentVisitsModel{DocumentId=id,DocumentTitle=title,Search=search,Visits=list});
        }

        private CourseDocumentModel Get(int id){using(var cn=Open())using(var cmd=new SqlCommand("SELECT d.ID,d.ProductId,p.Name ProductTitle,d.Title,d.Description,d.PdfUrl,d.SortOrder,d.Active FROM dbo.CourseDocument d INNER JOIN dbo.Product p ON p.ID=d.ProductId WHERE d.ID=@ID",cn)){cmd.Parameters.Add("@ID",SqlDbType.Int).Value=id;using(var r=cmd.ExecuteReader())return r.Read()?Read(r):null;}}
        private static CourseDocumentModel Read(IDataRecord r){return new CourseDocumentModel{ID=(int)r["ID"],ProductId=(int)r["ProductId"],ProductTitle=Convert.ToString(r["ProductTitle"]),Title=Convert.ToString(r["Title"]),Description=Convert.ToString(r["Description"]),PdfUrl=Convert.ToString(r["PdfUrl"]),SortOrder=(int)r["SortOrder"],Active=(bool)r["Active"]};}
        private IEnumerable<SelectListItem> Products(int? selected){return _context.Product.GetAll().Where(x=>x.Deleted!=true).OrderByDescending(x=>x.ID).Select(x=>new SelectListItem{Value=x.ID.ToString(),Text=x.Name,Selected=x.ID==selected}).ToList();}
        private void Check(){_context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(),Enum_Permission.Product);}
        private static object Db(string value){return string.IsNullOrWhiteSpace(value)?(object)DBNull.Value:value.Trim();}
        private static SqlConnection Open(){var b=new EntityConnectionStringBuilder(ConfigurationManager.ConnectionStrings["DatabaseEntities"].ConnectionString);var c=new SqlConnection(b.ProviderConnectionString);c.Open();return c;}
        protected override void Dispose(bool disposing){if(disposing)_context.Dispose();base.Dispose(disposing);}
    }
}
