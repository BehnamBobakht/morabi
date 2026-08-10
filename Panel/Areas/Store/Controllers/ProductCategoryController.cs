using DataLayer.ViewModels;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Panel.Areas.Store.Controllers
{
    public class ProductCategoryController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(
            int? typeId = null,
            int? index = null,
            int? pagesize = null,
            string name = null)
        {
            index = index == null ? 1 : index;
            pagesize = pagesize == null ? 10000 : pagesize;
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCategory);
            List<ProductCategoryVm> list = _context.ProductCategory.Search(
                typeId: typeId,
                pageSize: pagesize.GetValueOrDefault(),
                index: index,
                name: name);
            return View(list);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCategory_Details);
            ProductCategory category = _context.ProductCategory.GetById(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCategory_New);
            FillDropDowns(null);
            ProductCategory category = new ProductCategory()
            {
                UpdateDatetime = DateTime.Now
            };
            ViewBag.Message = BaseMessage.GetMessage();
            return View(category);
        }

        [HttpPost]
        public ActionResult Create(ProductCategory category)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCategory_New);

            List<ProductCategoryLanguage> listLanguage = category.ProductCategoryLanguage.ToList();
            category.ProductCategoryLanguage.Clear();

            ViewMessage result = IsModelValid(category);
            if (result.Type == Enum_MessageType.SUCCESS)
            {
                var tempProductType = _context.ProductType.Where(s => s.CustomLabel == category.CustomLabel).FirstOrDefault();
                var tempProductCategory = _context.ProductCategory.Where(s => s.CustomLabel == category.CustomLabel).FirstOrDefault();
                var tempProductSubCategory = _context.ProductSubCategory.Where(s => s.CustomLabel == category.CustomLabel).FirstOrDefault();
                if (tempProductSubCategory == null && tempProductCategory == null && tempProductType == null)
                {
                    if (category.Priority != null)
                    {
                        List<ProductCategory> lst = _context.ProductCategory.Where(x => x.Priority == category.Priority && x.TypeId == category.TypeId).ToList();
                        if (lst != null && lst.Count() > 0)
                        {
                            result.Body = "اولویت نمایش تکراری است";
                            result.Type = Enum_MessageType.ERROR;
                            return new JsonResult() { Data = result };
                        }
                    }
                    category.UpdateDatetime = DateTime.Now;
                    _context.ProductCategory.Insert(category);
                    _context.Save();

                    foreach (ProductCategoryLanguage item in listLanguage)
                    {
                        item.ProductCategoryId = category.ID;
                        _context.ProductCategoryLanguage.Insert(item);
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
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCategory_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ProductCategory category = _context.ProductCategory.GetById(id);
            if (category == null)
                return HttpNotFound();

            FillDropDowns(category);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(category);
        }

        [HttpPost]
        public ActionResult Edit(ProductCategory category)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCategory_Edit);
            List<ProductCategoryLanguage> listLanguage = category.ProductCategoryLanguage.ToList();
            category.ProductCategoryLanguage.Clear();

            ViewMessage result = IsModelValid(category);
            if (result.Type == Enum_MessageType.SUCCESS)
            {
                var tempProductType = _context.ProductType.Where(s => s.CustomLabel == category.CustomLabel).FirstOrDefault();
                var tempProductCategory = _context.ProductCategory.Where(s => s.CustomLabel == category.CustomLabel && s.ID != category.ID).FirstOrDefault();
                var tempProductSubCategory = _context.ProductSubCategory.Where(s => s.CustomLabel == category.CustomLabel).FirstOrDefault();
                if (tempProductSubCategory == null && tempProductCategory == null && tempProductType == null)
                {
                    if (category.Priority != null)
                    {
                        List<ProductCategory> lst = _context.ProductCategory.Where(x => x.Priority == category.Priority && x.TypeId == category.TypeId && x.ID != category.ID).ToList();
                        if (lst != null && lst.Count() > 0)
                        {
                            result.Body = "اولویت نمایش تکراری است";
                            result.Type = Enum_MessageType.ERROR;
                            return new JsonResult() { Data = result };
                        }
                    }
                    category.UpdateDatetime = DateTime.Now;
                    _context.ProductCategory.Update(category);
                    _context.Save();

                    _context.ProductCategoryLanguage.DeleteByProductCategoryId(category.ID);
                    _context.Save();
                    foreach (ProductCategoryLanguage item in listLanguage)
                    {
                        item.ProductCategoryId = category.ID;
                        _context.ProductCategoryLanguage.Insert(item);
                        bool c = _context.Save();
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
        public ActionResult PriorityList(int typeId)
        {
            List<ProductCategory> lst = _context.ProductCategory.Where(x => x.TypeId == typeId).OrderBy(x => x.Priority).ToList();
            return PartialView(lst);
        }

        private ViewMessage IsModelValid(ProductCategory category)
        {
            ViewMessage result = new ViewMessage();
            if (category.Name == null)
                result = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PRODUCT_CATEGORY_NAME);
            else
                result = BaseMessage.GetMessage(Enum_MessageType.SUCCESS);
            return result;
        }

        private void FillDropDowns(ProductCategory category)
        {
            ViewBag.TypeId = new SelectList(_context.ProductType.GetAll(), "ID", "Name", category != null ? category.TypeId : 0);
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCategory_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ProductCategory category = _context.ProductCategory.GetById(id);
            if (category == null)
                return HttpNotFound();

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCategory_Delete);
            ProductCategory category = _context.ProductCategory.GetById(id);
            try
            {
                _context.ProductCategory.Delete(category);
                _context.Save();
            }
            catch
            {
                category.Deleted = true;
                _context.ProductCategory.Update(category);

                _context.Save();
            }
            return RedirectToAction("Index");

        }
        public ActionResult ProductCategoryDropDown(int? typeId = null, int? value = null)
        {
            ViewBag.ProductCategoryId = new SelectList(_context.ProductCategory.Search(typeId: typeId, pageSize: 1000), "ID", "Name", value);
            return PartialView();
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