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
    [ValidateInput(false)]
    public class Discount3Controller : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Discount);
            ViewBag.DiscountGroup = _context.DiscountGroup.GetById(id.Value);
            return View(_context.Discount.GetAllByDiscountGroupId(id.Value).ToView());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Discount_Details);
            Discount discount = _context.Discount.GetById(id);
            if (discount == null)
            {
                return HttpNotFound();
            }
            return View(discount);
        }

        public ActionResult Create(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Discount_New);
            FillDropDowns(null);
            DiscountGroup group = _context.DiscountGroup.GetById(id.Value);
            ViewBag.DiscountGroup = group;
            ViewBag.Message = BaseMessage.GetMessage();
            Discount discount = new Discount() {
                StartDatetime = group.StartDatetime,
                EndDatetime = group.EndDatetime,
                Active = group.Active,
                PriceValue = group.PriceValue,
                PercentValue = group.PercentValue,
                IsTop = false
            };
            return View(discount);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Discount discount)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Discount_New);

            if (IsModelValid(discount))
            {
                discount.ID = 0;
                discount.UserId = _context.SiteUser.GetCurrentUser().ID;
                _context.Discount.Insert(discount);
                _context.Save();
                return RedirectToAction("index", new { id = discount.GroupId });
            }

            ViewBag.DiscountGroup = _context.DiscountGroup.GetById(discount.GroupId);
            
            FillDropDowns(discount);
            return View(discount);
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Discount_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Discount discount = _context.Discount.GetById(id);
            if (discount == null)
                return HttpNotFound();

            DiscountGroup group = _context.DiscountGroup.GetById(discount.GroupId);
            ViewBag.DiscountGroup = group;
            FillDropDowns(discount);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(discount);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Discount discount)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Discount_Edit);
            if (IsModelValid(discount))
            {
                _context.Discount.Update(discount);
                _context.Save();
                return RedirectToAction("index", new { id = discount.GroupId });
            }
            FillDropDowns(discount);
            return View(discount);
        }

        private bool IsModelValid(Discount discount)
        {
            bool result = false;
            string DiscountTypeString = _context.Code.GetById(discount.TypeId).Label;
            discount.StartDatetime = discount.StartDatetime.GetGregorian();
            discount.EndDatetime = discount.EndDatetime.GetGregorian();

            discount.ShopId = DiscountTypeString != Enum_Code.DISCOUNT_TYPE_SHOP.ToString() ? null : discount.ShopId;
            discount.ProductTypeId = DiscountTypeString != Enum_Code.DISCOUNT_TYPE_PRODUCTTYPE.ToString() ? null : discount.ProductTypeId;
            discount.ProductCategoryId = DiscountTypeString != Enum_Code.DISCOUNT_TYPE_CATEGORY.ToString() ? null : discount.ProductCategoryId;
            discount.ProductSubCategoryId = DiscountTypeString != Enum_Code.DISCOUNT_TYPE_SUBCATEGORY.ToString() ? null : discount.ProductSubCategoryId;
            discount.ProductId = DiscountTypeString != Enum_Code.DISCOUNT_TYPE_PRODUCT.ToString() ? null : discount.ProductId;
            discount.ProductBrandId = DiscountTypeString != Enum_Code.DISCOUNT_TYPE_BRAND.ToString() ? null : discount.ProductBrandId;

            if (DiscountTypeString == Enum_Code.DISCOUNT_TYPE_SHOP.ToString() && discount.ShopId == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_Message.REQUIRED_DISCOUNT_SHOP);
            else if (DiscountTypeString == Enum_Code.DISCOUNT_TYPE_PRODUCTTYPE.ToString() && discount.ProductTypeId == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_Message.REQUIRED_DISCOUNT_PRODUCTTYPE);
            else if (DiscountTypeString == Enum_Code.DISCOUNT_TYPE_CATEGORY.ToString() && discount.ProductCategoryId == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_Message.REQUIRED_DISCOUNT_CATEGORY);
            else if (DiscountTypeString == Enum_Code.DISCOUNT_TYPE_SUBCATEGORY.ToString() && discount.ProductSubCategoryId == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_Message.REQUIRED_DISCOUNT_SUBCATEGORY);
            else if (DiscountTypeString == Enum_Code.DISCOUNT_TYPE_BRAND.ToString() && discount.ProductBrandId == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_Message.REQUIRED_DISCOUNT_BRAND);
            else if (DiscountTypeString == Enum_Code.DISCOUNT_TYPE_PRODUCT.ToString() && discount.ProductId == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_Message.REQUIRED_DISCOUNT_PRODUCT);
            else
                result = true;
            return result;
        }

        private void FillDropDowns(Discount discount)
        {
            ViewBag.TypeId = new SelectList(_context.Code.GetAllByCodeGroup(Enum_CodeGroup.DISCOUNT_TYPE), "ID", "Name", discount != null ? discount.TypeId : 0);
            ViewBag.ShopId = new SelectList(_context.Shop.GetAllShopForUserId(_context.SiteUser.GetCurrentUser().ID), "ID", "Name", discount != null ? discount.ShopId : 0);
            ViewBag.ProductTypeId = new SelectList(_context.ProductType.GetAll(), "ID", "Name", discount != null ? discount.ShopId : 0);
            ViewBag.ProductCategoryId = new SelectList(_context.ProductCategory.GetAll(), "ID", "Name", discount != null ? discount.ShopId : 0);
            ViewBag.ProductSubCategoryId = new SelectList(_context.ProductSubCategory.GetAll(), "ID", "Name", discount != null ? discount.ShopId : 0);
            ViewBag.ProductBrandId = new SelectList(_context.ProductBrand.GetAll(), "ID", "Name", discount != null ? discount.ProductBrandId : 0);
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Discount_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Discount discount = _context.Discount.GetById(id);
            if (discount == null)
                return HttpNotFound();

            return View(discount);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Discount_Delete);
            Discount discount = _context.Discount.GetById(id);
            try
            {
                int GroupId = discount.GroupId.Value;
                _context.Discount.Delete(discount);
                _context.Save();
                return RedirectToAction("index", new { id = GroupId });
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(discount);
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