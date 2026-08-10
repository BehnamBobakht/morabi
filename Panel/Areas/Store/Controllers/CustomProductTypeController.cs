using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer.ViewModels;

namespace Panel.Areas.Store.Controllers
{
    public class CustomProductTypeController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(int? typeId = null, int? categoryId = null, int? subcategoryId = null, int? brandId = null)
        {

            if (typeId == null && categoryId == null && subcategoryId == null && brandId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ViewBag.ProductTypeId = typeId;
            ViewBag.ProductCategoryId = categoryId;
            ViewBag.ProductSubCategoryId = subcategoryId;
            ViewBag.ProductBrandId = brandId;
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCustomField);

            List<ProductCustomField> list = _context.ProductCustomField.Search(typeId: typeId, categoryId: categoryId, brandId: brandId, subcategoryId: subcategoryId, pageSize: 2000);
            return View(list.ToView());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCustomField_Details);
            ProductCustomField field = _context.ProductCustomField.GetById(id);
            if (field == null)
            {
                return HttpNotFound();
            }
            return View(field);
        }

        public ActionResult Create(int? typeId = null, int? categoryId = null, int? subcategoryId = null, int? brandId = null)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCustomField_New);
            FillDropDowns(null);

            ViewBag.ProductTypeId = typeId;
            ViewBag.ProductCategoryId = categoryId;
            ViewBag.ProductSubCategoryId = subcategoryId;
            ViewBag.ProductBrandId = brandId;

            ViewBag.ProductType = _context.ProductType.GetById(typeId);
            ViewBag.ProductCategory = _context.ProductCategory.GetById(categoryId);
            ViewBag.ProductSubCategory = _context.ProductSubCategory.GetById(subcategoryId);
            ViewBag.ProductBrand = _context.ProductBrand.GetById(brandId);

            ProductCustomField field = new ProductCustomField()
            {
                ProductTypeId = typeId,
                ProductCategoryId = categoryId,
                ProductSubCategoryId = subcategoryId,
                ProductBrandId = brandId
            };

            ViewBag.Message = BaseMessage.GetMessage();

            return View(field);
        }

        [HttpPost]
        public ActionResult Create(ProductCustomField field)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCustomField_New);

            List<ProductCustomFieldLanguage> listLanguage = field.ProductCustomFieldLanguage.ToList();
            field.ProductCustomFieldLanguage.Clear();

            ViewMessage result = IsModelValid(field);
            if (result.Type == Enum_MessageType.SUCCESS)
            {
                _context.ProductCustomField.Insert(field);
                _context.Save();

                foreach (ProductCustomFieldLanguage item in listLanguage)
                {
                    item.FieldId = field.ID;
                    _context.ProductCustomFieldLanguage.Insert(item);
                    _context.Save();
                }
            }
            return new JsonResult() { Data = result };
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCustomField_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ProductCustomField field = _context.ProductCustomField.GetById(id);
            if (field == null)
                return HttpNotFound();

            FillDropDowns(field);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(field);
        }

        [HttpPost]
        public ActionResult Edit(ProductCustomField field)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCustomField_Edit);
            List<ProductCustomFieldLanguage> listLanguage = field.ProductCustomFieldLanguage.ToList();
            field.ProductCustomFieldLanguage.Clear();

            ViewMessage result = IsModelValid(field);
            if (result.Type == Enum_MessageType.SUCCESS)
            {
                _context.ProductCustomField.Update(field);
                _context.Save();

                _context.ProductCustomFieldLanguage.DeleteByProductCustomFieldId(field.ID);
                _context.Save();

                foreach (ProductCustomFieldLanguage item in listLanguage)
                {
                    item.FieldId = field.ID;
                    _context.ProductCustomFieldLanguage.Insert(item);
                    _context.Save();
                }
            }
            return new JsonResult() { Data = result };
        }

        private ViewMessage IsModelValid(ProductCustomField field)
        {
            ViewMessage result = new ViewMessage();
            if (field.Name == null)
                result = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_CUSTOM_FIELD_NAME);
            return result;
        }

        private void FillDropDowns(ProductCustomField field)
        {
            ViewBag.TypeId = new SelectList(_context.Code.GetAllByCodeGroup(Enum_CodeGroup.FIELD_TYPE), "ID", "Name", field != null ? field.TypeId : 0);
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCustomField_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ProductCustomField field = _context.ProductCustomField.GetById(id);
            if (field == null)
                return HttpNotFound();

            return View(field);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int ID)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCustomField_Delete);
            ProductCustomField field = _context.ProductCustomField.GetById(ID);
            try
            {
                int? typeId = field.ProductTypeId;
                int? ProSubCategoryId = field.ProductSubCategoryId;
                int? PrCategoryId = field.ProductCategoryId;
                int? ProBrandId = field.ProductBrandId;
                _context.ProductCustomField.Delete(field);
                _context.Save();

                return RedirectToAction("Index", new
                {
                    @typeId = typeId,
                    @categoryId = PrCategoryId,
                    @subCategoryId = ProSubCategoryId,
                    @brandId = ProBrandId
                });
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(field);
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