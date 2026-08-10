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
    [ValidateInput(false)]
    public class DeliveryController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(
            int? index = null,
            int? pagesize = null,
            int? storeId = null,
            bool? active = null)
        {
            index = index == null ? 1 : index;
            pagesize = pagesize == null ? 10 : pagesize;
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Delivery);
            List<DeliveryVm> list = _context.Delivery.Search(index: index.GetValueOrDefault(), pageSize: pagesize.GetValueOrDefault(), storeId: storeId, active: active);
            ViewBag.TotalCount = _context.Delivery.SearchCount(storeId: storeId, active: active);
            ViewBag.StoreId = new SelectList(_context.Store.GetActives(withNull: true), "ID", "Name", storeId);

            return View(list);
        }


        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Delivery_New);
            DeliveryVm delivery = new DeliveryVm()
            {
                Active = true
            };
            ViewBag.Message = BaseMessage.GetMessage();
            FillDropDowns(delivery);
            return View(delivery);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DeliveryVm delivery)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Delivery_New);

            var result = _context.Delivery.Insert(delivery);
            if (result.Status == ResultStatus.Success)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Message = new ViewMessage()
            {
                Body = result.Message,
                Type = Enum_MessageType.ERROR
            };

            FillDropDowns(delivery);
            return View(delivery);
        }      
        public ActionResult Edit(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Delivery_New);
            var deliveryVm = _context.Delivery.GetByIdVm(id);
            return View(deliveryVm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DeliveryVm deliveryVm)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Delivery_New);
            var delivery = _context.Delivery.GetById(deliveryVm.ID);
            if(deliveryVm.IsUnknownPrice)
            {
                delivery.BasePrice = null;
            }
            else
            {
                delivery.BasePrice = deliveryVm.BasePrice == null ? 0 : deliveryVm.BasePrice;

            }
            delivery.MinPriceForFree = deliveryVm.MinPriceForFree;
            delivery.CashOnDeliveryMaxPrice = deliveryVm.CashOnDeliveryMaxPrice;
            delivery.MaxDays = deliveryVm.MaxDays;
             _context.Delivery.Update(delivery);
            _context.Save();

            return RedirectToAction("Index");
        }

        private void FillDropDowns(DeliveryVm delivery)
        {
            ViewBag.StoreId = new SelectList(_context.Store.GetActives(withNull: true), "ID", "Name", delivery != null ? delivery.StoreId : null);
            ViewBag.DeliveryTypeId = new SelectList(_context.DeliveryType.GetActives(), "ID", "Name", delivery.DeliveryTypeId);
                   }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Delivery_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Delivery delivery = _context.Delivery.GetById(id);
            if (delivery == null)
                return HttpNotFound();

            return View(delivery);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Delivery_Delete);
            try
            {
                _context.Delivery.Delete(id);
                _context.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                Delivery delivery = _context.Delivery.GetById(id);
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(delivery);
            }

        }
        public ActionResult Active(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Delivery_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Delivery delivery = _context.Delivery.GetById(id);
            if (delivery == null)
                return HttpNotFound();

            return View(delivery);
        }

        [HttpPost, ActionName("Active")]
        [ValidateAntiForgeryToken]
        public ActionResult ActiveConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Delivery_Edit);
            try
            {
                _context.Delivery.Active(id);
                return RedirectToAction("Index");
            }
            catch
            {
                Delivery delivery = _context.Delivery.GetById(id);
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(delivery);
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