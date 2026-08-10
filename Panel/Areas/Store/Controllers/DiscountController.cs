using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.Base;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Panel.Areas.Store.Controllers
{
    public class DiscountController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(int? storeId=null,int? typeId=null,string name=null,bool? active=null, int? index = null,int? pageSize = null)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Discount);
            var model = _context.Discount.Search(storeId:storeId,typeId:typeId,name:name,active:active,index:index,pageSize:pageSize);
            ViewBag.typeId = new SelectList(_context.Code.GetAllByCodeGroup(Enum_CodeGroup.DISCOUNT_TYPE), "ID", "Name",typeId);
            ViewBag.storeId = new SelectList(_context.Store.GetAll(), "ID", "Name", storeId);
            return View(model);
        }


        public ActionResult Create(int? id)//groupId
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Discount_New);
            CreateDiscountVm discount = new CreateDiscountVm();
            if (id != null)
            {
                DiscountGroup group = _context.DiscountGroup.GetById(id.Value);
                if (group != null)
                {
                    discount.GroupId = group.ID;
                    discount.StartDatetime = group.StartDatetime;
                    discount.EndDatetime = group.EndDatetime;

                }
            }
            FillDropDowns(discount);
            return View(discount);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateDiscountVm discount)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Discount_New);
            var result = _context.Discount.Insert(discount);
            if (result.Status != ResultStatus.Success)
            {
                FillDropDowns(discount);
                ModelState.AddModelError("", result.Message);
                return View(discount);
            }
            return RedirectToAction("index");
        }
        public ActionResult Edit(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Discount_New);
            var discount = _context.Discount.GetDiscountVm(id);
            if (discount == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ViewBag.ValueTypeId = new SelectList(_context.Code.GetAllByCodeGroup(Enum_CodeGroup.DISCOUNTGROUP_TYPE), "ID", "Name", discount != null ? discount.ValueTypeId : 0);
            return View(discount);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CreateDiscountVm discount)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Discount_New);
            var result = _context.Discount.Update(discount);
            if (result.Status != ResultStatus.Success)
            {
                FillDropDowns(discount);

                ModelState.AddModelError("", result.Message);
                return View(discount);
            }
            return RedirectToAction("index");
        }

        private void FillDropDowns(CreateDiscountVm discount)
        {
            ViewBag.TypeId = new SelectList(_context.Code.GetAllByCodeGroup(Enum_CodeGroup.DISCOUNT_TYPE), "ID", "Name", discount != null ? discount.TypeId : 0);
            ViewBag.ValueTypeId = new SelectList(_context.Code.GetAllByCodeGroup(Enum_CodeGroup.DISCOUNTGROUP_TYPE), "ID", "Name", discount != null ? discount.ValueTypeId : 0);
            ViewBag.ProductTypeId = new SelectList(_context.ProductType.GetAll(), "ID", "Name", discount != null ? discount.ProductTypeId : null);
            ViewBag.ProductBrandId = new SelectList(_context.ProductBrand.GetAll(), "ID", "Name", discount != null ? discount.ProductBrandId : null);
            ViewBag.StoreId = new SelectList(_context.Store.GetAll(), "ID", "Name", discount != null ? discount.ProductBrandId : null);
            ViewBag.GroupId = new SelectList(_context.DiscountGroup.Search(active: true,activeDate:true), "ID", "Name", discount != null ? discount.GroupId : null);

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

           var result= _context.Discount.Delete(id);
            if(result.Status==ResultStatus.Success)
            return RedirectToAction("index");
            else
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View();
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