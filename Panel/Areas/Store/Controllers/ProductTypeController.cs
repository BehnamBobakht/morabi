using DataLayer.ViewModels;
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

namespace Panel.Areas.Store.Controllers
{
    public class ProductTypeController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductType);
            return View(_context.ProductType.GetAll().Where(s => s.Deleted != true).ToList().ToView());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductType_Details);
            ProductType productType = _context.ProductType.GetById(id);
            if (productType == null)
            {
                return HttpNotFound();
            }
            return View(productType);
        }

        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductType_New);
            return View(new ProductType());
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(ProductType productType)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductType_New);

            List<ProductTypeLanguage> listLanguage = productType.ProductTypeLanguage.ToList();
            productType.ProductTypeLanguage.Clear();

            ViewMessage result = IsModelValid(productType);
            if (result.Type == Enum_MessageType.SUCCESS)
            {
                var tempProductType = _context.ProductType.Where(s => s.CustomLabel == productType.CustomLabel).FirstOrDefault();
                var tempProductCategory = _context.ProductCategory.Where(s => s.CustomLabel == productType.CustomLabel).FirstOrDefault();
                var tempProductSubCategory = _context.ProductSubCategory.Where(s => s.CustomLabel == productType.CustomLabel).FirstOrDefault();
                if (tempProductSubCategory == null && tempProductCategory == null && tempProductType == null)
                {
                    if (productType.Priority != null)
                    {
                        List<ProductType> lst = _context.ProductType.Where(x => x.Priority == productType.Priority).ToList();
                        if (lst != null && lst.Count() > 0)
                        {
                            result.Body = "اولویت نمایش تکراری است";
                            result.Type = Enum_MessageType.ERROR;
                            return new JsonResult() { Data = result };
                        }
                    }
                    productType.SyncDatetime = DateTime.Now;
                    _context.ProductType.Insert(productType);
                    _context.Save();

                    foreach (ProductTypeLanguage item in listLanguage)
                    {
                        item.ProductTypeId = productType.ID;
                        _context.ProductTypeLanguage.Insert(item);
                        _context.Save();
                    }
                    BaseWebsite.UpdatingLastContent();

                }
                else
                {
                    return new JsonResult()
                    {
                        Data = Resource.Lang.Error,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }

            }

            return new JsonResult() { Data = result };
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductType_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ProductType productType = _context.ProductType.GetById(id);
            if (productType == null)
                return HttpNotFound();

            ViewBag.Message = BaseMessage.GetMessage();
            return View(productType);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(ProductType productType)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductType_Edit);
            List<ProductTypeLanguage> listLanguage = productType.ProductTypeLanguage.ToList();
            productType.ProductTypeLanguage.Clear();

            ViewMessage result = IsModelValid(productType);
            if (result.Type == Enum_MessageType.SUCCESS)
            {

                var tempProductType = _context.ProductType.Where(s => s.CustomLabel == productType.CustomLabel && s.ID != productType.ID).FirstOrDefault();
                var tempProductCategory = _context.ProductCategory.Where(s => s.CustomLabel == productType.CustomLabel).FirstOrDefault();
                var tempProductSubCategory = _context.ProductSubCategory.Where(s => s.CustomLabel == productType.CustomLabel).FirstOrDefault();
                if (tempProductSubCategory == null && tempProductCategory == null && tempProductType == null)
                {
                    if (productType.Priority != null)
                    {
                        List<ProductType> lst = _context.ProductType.Where(x => x.Priority == productType.Priority && x.ID != productType.ID).ToList();
                        if (lst != null && lst.Count() > 0)
                        {
                            result.Body = "اولویت نمایش تکراری است";
                            result.Type = Enum_MessageType.ERROR;
                            return new JsonResult() { Data = result };
                        }
                    }
                    productType.SyncDatetime = DateTime.Now;

                    _context.ProductType.Update(productType);
                    _context.ProductTypeLanguage.DeleteByProductTypeId(productType.ID);
                    foreach (ProductTypeLanguage item in listLanguage)
                    {
                        item.ProductTypeId = productType.ID;
                        _context.ProductTypeLanguage.Insert(item);
                    }
                    _context.Save();
                    BaseWebsite.UpdatingLastContent();

                }
                else
                {
                    return new JsonResult()
                    {
                        Data = Resource.Lang.Error,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
            }


            return new JsonResult() { Data = result };
        }

        public ActionResult PriorityList()
        {
            return PartialView(_context.ProductType.GetAll().OrderBy(x => x.Priority));
        }

        private ViewMessage IsModelValid(ProductType productType)
        {
            ViewMessage result = new ViewMessage();
            if (productType.Name == null)
                result = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PRODUCT_TYPE_NAME);
            return result;
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductType_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ProductType productType = _context.ProductType.GetById(id);
            if (productType == null)
                return HttpNotFound();

            return View(productType);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductType_Delete);
            ProductType item = _context.ProductType.GetById(id);
            try
            {
                _context.ProductType.Delete(item);
                _context.Save();
            }
            catch
            {
                item.Deleted = true;
                _context.ProductType.Update(item);
                _context.Save();
            }
            return RedirectToAction("Index");

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