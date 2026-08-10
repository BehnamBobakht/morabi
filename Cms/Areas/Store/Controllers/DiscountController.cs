using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Text;
using DataLayer.Helpers.Common;
using System.Data;
using DataLayer.ViewModels;
using Cms.Areas.Store.Security;


namespace Cms.Areas.Store.Controllers
{
    [AuthorizeFilter(Enum_StorePanel = DataLayer.Enumarables.Enum_StorePanel.StoreState)]
    public class DiscountController : Controller
    {
        UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(int? index = null, int? pageSize = null)
        {

            var currentAccount = BaseWebsite.CurrentAccount;
            var storeId = _context.Store.GetById(currentAccount.StoreId);
            var model = _context.Discount.Search(storeId: storeId.ID, index: index, pageSize: pageSize);
            ViewBag.TotalCount = _context.Discount.SearchCount(storeId: storeId.ID);
            ViewBag.pageSize = pageSize.ToString();
            return View(model);
        }
        [AuthorizeFilter(Enum_StorePanel = DataLayer.Enumarables.Enum_StorePanel.StoreState)]

        public ActionResult Create()
        {

            var model = new Discount();
            InitialPage(model);

            return View(model);
        }
        [HttpPost]
        [AuthorizeFilter(Enum_StorePanel = DataLayer.Enumarables.Enum_StorePanel.StoreState)]

        public ActionResult Create(Discount discount, string start_date = null, string end_date = null)
        {
            var currentAccount = BaseWebsite.CurrentAccount;
            if (!IsModelValid(discount))
            {
                InitialPage(discount, start_date, end_date);
                return View(discount);
            }
            var store = _context.Store.GetById(currentAccount.StoreId);
            DateTime? startDate = null;
            DateTime? endDate = null;

            if (!string.IsNullOrEmpty(start_date))
            {
                startDate = BaseDate.GetGregorian(new CustomDate(DateTime.Parse(start_date))).GetValueOrDefault();
            }
            if (!string.IsNullOrEmpty(end_date))
            {
                endDate = BaseDate.GetGregorian(new CustomDate(DateTime.Parse(end_date))).GetValueOrDefault();
            }
            discount.StoreId = store.ID;
            discount.StartDatetime = startDate;
            discount.EndDatetime = endDate;
            _context.Discount.Insert(discount);
            _context.Save();
           BaseStore.UpdateProductQuantity(store,_context);
            return RedirectToAction("Index");


            //return View();
        }
        public ActionResult Edit(int id)
        {
            if (!BaseStore.StoreDiscount_IsValid(id, _context))
                return View("Invalid");
            var model = _context.Discount.GetById(id);
            string start_date = null;
            string end_date = null;
            if (model.StartDatetime != null)
                start_date = model.StartDatetime.Value.ToPersian();
            if (model.EndDatetime != null)
                end_date = model.EndDatetime.Value.ToPersian();

            InitialPage(model,start_date,end_date);

            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(Discount discount, string start_date = null, string end_date = null)
        {
            if (!BaseStore.StoreDiscount_IsValid( discount.ID,_context))
                return View("Invalid");

            var currentAccount = BaseWebsite.CurrentAccount;
            if (!IsModelValid(discount))
            {
                InitialPage(discount, start_date, end_date);
                return View(discount);
            }
            DateTime? startDate = null;
            DateTime? endDate = null;

            if (!string.IsNullOrEmpty(start_date))
            {
                startDate = BaseDate.GetGregorian(new CustomDate(DateTime.Parse(start_date))).GetValueOrDefault();
            }
            if (!string.IsNullOrEmpty(end_date))
            {
                endDate = BaseDate.GetGregorian(new CustomDate(DateTime.Parse(end_date))).GetValueOrDefault();
            }
            _context = new UnitOfWork();
            var store = _context.Store.GetById(currentAccount.StoreId);
            discount.StoreId = store.ID;
            discount.StartDatetime = startDate;
            discount.EndDatetime = endDate;
            _context.Discount.Update(discount);
            _context.Save();
            BaseStore.UpdateProductQuantity(store, _context);

            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            if (!BaseStore.StoreDiscount_IsValid(id, _context))
                return View("Invalid");

            _context.Discount.Delete(id);
            _context.Save();
            return RedirectToAction("Index");
        }
        private bool IsModelValid(Discount discount)
        {
            bool result = false;
            var DiscountType = _context.Code.GetById(discount.TypeId);
            int DiscountPriceCodeId = _context.Code.GetByLabel(Enum_Code.DISCOUNTGROUP_TYPE_PRICE).ID;
            int DiscountPercentCodeId = _context.Code.GetByLabel(Enum_Code.DISCOUNTGROUP_TYPE_PERCENT).ID;
            if (DiscountType != null)
            {
                var DiscountTypeString = DiscountType.Label;
                discount.ProductTypeId = DiscountTypeString != Enum_Code.DISCOUNT_TYPE_PRODUCTTYPE.ToString() ? null : discount.ProductTypeId;
                discount.ProductCategoryId = DiscountTypeString != Enum_Code.DISCOUNT_TYPE_CATEGORY.ToString() ? null : discount.ProductCategoryId;
                discount.ProductSubCategoryId = DiscountTypeString != Enum_Code.DISCOUNT_TYPE_SUBCATEGORY.ToString() ? null : discount.ProductSubCategoryId;
                discount.ProductId = DiscountTypeString != Enum_Code.DISCOUNT_TYPE_PRODUCT.ToString() ? null : discount.ProductId;
                discount.ProductBrandId = DiscountTypeString != Enum_Code.DISCOUNT_TYPE_BRAND.ToString() ? null : discount.ProductBrandId;
                discount.StoreProductQuantityId = DiscountTypeString != Enum_Code.DISCOUNT_TYPE_VARIENTY.ToString() ? null : discount.StoreProductQuantityId;

                if (DiscountTypeString == Enum_Code.DISCOUNT_TYPE_PRODUCTTYPE.ToString() && discount.ProductTypeId == null)
                    ViewBag.Message = BaseMessage.GetMessage(Enum_Message.REQUIRED_DISCOUNT_PRODUCTTYPE);
                else if (DiscountTypeString == Enum_Code.DISCOUNT_TYPE_CATEGORY.ToString() && discount.ProductCategoryId == null)
                    ViewBag.Message = BaseMessage.GetMessage(Enum_Message.REQUIRED_DISCOUNT_CATEGORY);
                else if (DiscountTypeString == Enum_Code.DISCOUNT_TYPE_SUBCATEGORY.ToString() && discount.ProductSubCategoryId == null)
                    ViewBag.Message = BaseMessage.GetMessage(Enum_Message.REQUIRED_DISCOUNT_SUBCATEGORY);
                else if (DiscountTypeString == Enum_Code.DISCOUNT_TYPE_BRAND.ToString() && discount.ProductBrandId == null)
                    ViewBag.Message = BaseMessage.GetMessage(Enum_Message.REQUIRED_DISCOUNT_BRAND);
                else if (DiscountTypeString == Enum_Code.DISCOUNT_TYPE_PRODUCT.ToString() && discount.ProductId == null)
                    ViewBag.Message = BaseMessage.GetMessage(Enum_Message.REQUIRED_DISCOUNT_PRODUCT);
                else if (DiscountTypeString == Enum_Code.DISCOUNT_TYPE_VARIENTY.ToString() && discount.StoreProductQuantityId == null)
                    ViewBag.Message = BaseMessage.GetMessage(Enum_Message.REQUIRED_DISCOUNT_VARIENTY);
            }
            else
            {
                ViewBag.Message = BaseMessage.GetMessage(Enum_Message.REQUIRED_DISCOUNT_TYPE);
                return result;
            }

            if (discount.ValueTypeId == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_Message.REQUIRED_DISCOUNTGROUP);
            else if (DiscountPriceCodeId == discount.ValueTypeId && discount.PriceValue == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_Message.REQUIRED_DISCOUNT_PRICE);
            else if (DiscountPercentCodeId == discount.ValueTypeId && discount.PercentValue == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_Message.REQUIRED_DISCOUNT_PERCENT);
            else if (DiscountPercentCodeId == discount.ValueTypeId && discount.PercentValue == null || (discount.PercentValue != null && discount.PercentValue > 100 && discount.PercentValue < 1))
                ViewBag.Message = BaseMessage.GetMessage(Enum_Message.INVALID_DISCOUNT_PERCENT);
            else
                result = true;
            return result;
        }
        private void InitialPage(Discount discount, string start_date = null, string end_date = null)
        {
            var currentAccount = BaseWebsite.CurrentAccount;
            var store = _context.Store.GetById(currentAccount.StoreId);
            var productTypes = _context.ProductType.GetStoreProductTypes(store.ID);
            var productCategories = _context.ProductCategory.GetStoreProductCategories(store.ID);
            var productSubCategories = _context.ProductSubCategory.GetStoreProductSubCategories(store.ID);
            var storeProducts = _context.StoreProduct.Search(storeId: store.ID, pageSize: 2000).ToView(complate:false);
            var storeProductQuantity = _context.StoreProductQuantity.GetForStore(store.ID).ToView();
            var types = _context.Code.GetAllByCodeGroup(Enum_CodeGroup.DISCOUNT_TYPE);
            var valueTypes = _context.Code.GetAllByCodeGroup(Enum_CodeGroup.DISCOUNTGROUP_TYPE);
            ViewBag.ValueTypeId = new SelectList(valueTypes, "ID", "Name", discount.ValueTypeId);
            ViewBag.TypeId = new SelectList(types, "ID", "Name", discount.TypeId);
            ViewBag.ProductTypeId = new SelectList(productTypes, "ID", "Name", discount.ProductTypeId);
            ViewBag.ProductId = new SelectList(storeProducts, "ProductId", "ProductName", discount.ProductId);
            ViewBag.ProductCategoryId = new SelectList(productCategories, "ID", "Name", discount.ProductCategoryId);
            ViewBag.ProductSubCategoryId = new SelectList(productSubCategories, "ID", "Name", discount.ProductSubCategoryId);
            ViewBag.StoreProductQuantityId = new SelectList(storeProductQuantity, "ID", "FullName", discount.ProductCategoryId);
            ViewBag.start_date = start_date;
            ViewBag.end_date = end_date;
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