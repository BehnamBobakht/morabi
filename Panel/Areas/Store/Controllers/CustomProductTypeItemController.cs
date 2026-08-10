using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer.Entities;
using DataLayer.Base;
using System.Net;
using DataLayer.Enumarables;
using DataLayer.ViewModels;

namespace Panel.Areas.Store.Controllers
{
    public class CustomProductTypeItemController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCustomFieldItem);

            ProductCustomField field = _context.ProductCustomField.GetById(id);

            ViewBag.ProductCustomField = field;
            ViewBag.ProductTypeId = field.ProductTypeId;
            ViewBag.ProductCategoryId = field.ProductCategoryId;
            ViewBag.ProductSubCategoryId = field.ProductSubCategoryId;
            ViewBag.ProductBrandId = field.ProductBrandId;

            ViewBag.Id = id;
            return View(_context.ProductCustomItem.Search(fieldId:id.Value, pageSize: 1000).ToView());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCustomFieldItem_Details);
            ProductCustomItem customItem = _context.ProductCustomItem.GetById(id);
            if (customItem == null)
            {
                return HttpNotFound();
            }
            return View(customItem);
        }

        public ActionResult Create(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCustomFieldItem_New);
            ViewBag.Id = id;
            ProductCustomField field = _context.ProductCustomField.GetById(id);
            ProductCustomItem item = new ProductCustomItem()
            {
                FieldId = field.ID,
                ProductCustomField = field
            };

            ViewBag.Message = BaseMessage.GetMessage();
            return View(item);
        }

        [HttpPost]
        public ActionResult Create(ProductCustomItem customItem)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCustomFieldItem_New);

            List<ProductCustomItemLanguage> listLanguage = customItem.ProductCustomItemLanguage.ToList();
            customItem.ProductCustomItemLanguage.Clear();

            ViewMessage result = IsModelValid(customItem);
            if (result.Type == Enum_MessageType.SUCCESS)
            {
                _context.ProductCustomItem.Insert(customItem);
                _context.Save();

                foreach (ProductCustomItemLanguage item in listLanguage)
                {
                    item.FieldItemId = customItem.ID;
                    _context.ProductCustomItemLanguage.Insert(item);
                    _context.Save();
                }
            }

            return new JsonResult() { Data = result };
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCustomFieldItem_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ProductCustomItem customItem = _context.ProductCustomItem.GetById(id);
            if (customItem == null)
                return HttpNotFound();
            
            ViewBag.Message = BaseMessage.GetMessage();
            return View(customItem);
        }

        [HttpPost]
        public ActionResult Edit(ProductCustomItem customItem)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCustomFieldItem_Edit);
            List<ProductCustomItemLanguage> listLanguage = customItem.ProductCustomItemLanguage.ToList();
            customItem.ProductCustomItemLanguage.Clear();

            ViewMessage result = IsModelValid(customItem);
            if (result.Type == Enum_MessageType.SUCCESS)
            {
                _context.ProductCustomItem.Update(customItem);
                _context.Save();

                _context.ProductCustomItemLanguage.DeleteByProductCustomItemId(customItem.ID);
                _context.Save();

                foreach (ProductCustomItemLanguage item in listLanguage)
                {
                    item.FieldItemId = customItem.ID;
                    _context.ProductCustomItemLanguage.Insert(item);
                    _context.Save();
                }
            }
            return new JsonResult() { Data = result };
        }

        private ViewMessage IsModelValid(ProductCustomItem customItem)
        {
            ViewMessage result = new ViewMessage();
            if (customItem.Value == null)
                result = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PRODUCT_CUSTOM_ITEM_VALUE);
            return result;
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCustomFieldItem_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ProductCustomItem customItem = _context.ProductCustomItem.GetById(id);
            if (customItem == null)
                return HttpNotFound();

            return View(customItem);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCustomFieldItem_Delete);
            ProductCustomItem customItem = _context.ProductCustomItem.GetById(id);
            try
            {
                _context.ProductCustomItem.Delete(customItem);
                _context.Save();
                return RedirectToAction("index", new { id = customItem.FieldId });
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(customItem);
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