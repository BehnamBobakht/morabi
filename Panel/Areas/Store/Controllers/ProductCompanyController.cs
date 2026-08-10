using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Panel.Areas.Store.Controllers
{
    public class ProductCompanyController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCompany);
            return View(_context.ProductCategoryCompany.GetAll()/*.ToView()*/);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCompany_Details);
            ProductBrand brand = _context.ProductBrand.GetById(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }

        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCompany_New);
       
            ViewBag.Message = BaseMessage.GetMessage();
            return View(new ProductCategoryCompany());
        }

        [HttpPost]
        [ValidateInput(false)]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SubCategoryId,Active,ShowNumber,Label,Description,Title,Name")] ProductCompany company)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCompany_New);

            if (IsModelValid(company))
            {
                _context.ProductCompany.Insert(company);
                _context.Save();
                return RedirectToAction("Index");
            }

        
            return View(company);
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCompany_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ProductCompany brand = _context.ProductCompany.GetById(id);
            if (brand == null)
                return HttpNotFound();

        
            ViewBag.Message = BaseMessage.GetMessage();
            return View(brand);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "ID,Name,Active,ShowNumber,Label,Description,Title,SyncName")] ProductCompany company)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductBrand_Edit);
            if (IsModelValid(company))
            {
                _context.ProductCompany.Update(company);
                _context.Save();
                return RedirectToAction("Index");
            }
           
            return View(company);
        }

        private bool IsModelValid(ProductCompany company)
        {
            bool result = false;
            if (company.Name == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PRODUCT_BRAND_NAME); 
            else
                result = true;

            

            return result;
        }

     


        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCompany_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ProductCompany company = _context.ProductCompany.GetById(id);
            if (company == null)
                return HttpNotFound();

            return View(company);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCompany_Delete);
            ProductCompany company = _context.ProductCompany.GetById(id);
            try
            {
                _context.ProductCompany.Delete(company);
                _context.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(company);
            }

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}