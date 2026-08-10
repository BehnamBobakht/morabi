using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer.ViewModels;

namespace Panel.Areas.Store.Controllers
{
    public class ProductCategoryCompanyController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCategoryCompany);
            return View(/*_context.ProductCategoryCompany.GetAll()*/
            );
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCategoryCompany_Details);
            ProductCategoryCompany company = _context.ProductCategoryCompany.GetById(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCategoryCompany_New);
            FillDropDowns(null);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(new ProductCategoryCompany());
        }

        [HttpPost]
        [ValidateInput(false)]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Active,ShowNumber,Label,Description,Title")] ProductCategoryCompany company)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCategoryCompany_New);

            if (IsModelValid(company))
            {
                _context.ProductCategoryCompany.Insert(company);
                _context.Save();
                return RedirectToAction("Index");
            }

            FillDropDowns(company);
            return View(company);
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCategoryCompany);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ProductCategoryCompany company = _context.ProductCategoryCompany.GetById(id);
            if (company == null)
                return HttpNotFound();

            FillDropDowns(company);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(company);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "ID,Name,Active,ShowNumber,Label,Description,Title")]ProductCategoryCompany company)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCategoryCompany_Edit);
            if (IsModelValid(company)) { 
                _context.ProductCategoryCompany.Update(company);
                _context.Save();
                return RedirectToAction("Index");
            }
            FillDropDowns(company);
            return View(company);
        }

        private bool IsModelValid(ProductCategoryCompany company)
        {
            bool result = false;
            if (company.Name == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PRODUCT_BRAND_NAME);
            else
                result = true;

      

            return result;
        }

     

        private void FillDropDowns(ProductCategoryCompany company)
        {
            ViewBag.ProductSubCategoryId = new SelectList(_context.ProductSubCategory.GetAll(), "ID", "Name", company != null ? company.SubCategoryId : 0);
        }

     

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCategoryCompany_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ProductCategoryCompany company = _context.ProductCategoryCompany.GetById(id);
            if (company == null)
                return HttpNotFound();

            return View(company);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCategoryCompany_Delete);
            ProductCategoryCompany company = _context.ProductCategoryCompany.GetById(id);
            try
            {
                _context.ProductBrand.Delete(company);
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