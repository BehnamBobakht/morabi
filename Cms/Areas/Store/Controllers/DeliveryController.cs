using Cms.Areas.Store.Security;
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

namespace Cms.Areas.Store.Controllers
{
    [AuthorizeFilter(Enum_StorePanel = DataLayer.Enumarables.Enum_StorePanel.StoreState)]

    public class DeliveryController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(
            int? index = null,
            int? pagesize = null,            
            bool? active = null)
        {
            var currentAccount = DataLayer.Base.BaseWebsite.CurrentAccount;

            int? storeId = currentAccount.StoreId;
            index = index == null ? 1 : index;
            pagesize = pagesize == null ? 10 : pagesize;
            List<DeliveryVm> list = _context.Delivery.Search(index: index.GetValueOrDefault(), pageSize: pagesize.GetValueOrDefault(), storeId: storeId, active: active);
            ViewBag.TotalCount = _context.Delivery.SearchCount(storeId: storeId, active: active);
            ViewBag.StoreId = new SelectList(_context.Store.GetActives(withNull: true), "ID", "Name", storeId);

            return View(list);
        }


        public ActionResult Create()
        {
            DeliveryVm delivery = new DeliveryVm()
            {
                Active = true
            };
            ViewBag.Message = BaseMessage.GetMessage();
            FillDropDowns(delivery);
            return View(delivery);
        }


        [HttpPost]
        public ActionResult Create(DeliveryVm delivery)
        {
            var currentAccount = DataLayer.Base.BaseWebsite.CurrentAccount;

            delivery.StoreId = currentAccount.StoreId;
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

      


        private void FillDropDowns(DeliveryVm delivery)
        {
            ViewBag.DeliveryTypeId = new SelectList(_context.DeliveryType.GetActives(), "ID", "Name", delivery.DeliveryTypeId);           
        }

        public ActionResult Delete(int id)
        {
            if (id == 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if (!BaseStore.StoreDelivery_IsValid(id, _context))
                return View("Invalid");
            Delivery delivery = _context.Delivery.GetById(id);
            if (delivery == null)
                return HttpNotFound();
            return View(delivery);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                if (!BaseStore.StoreDelivery_IsValid(id, _context))
                    return View("Invalid");
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
        public ActionResult Active(int id)
        {
            if (id == 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if (!BaseStore.StoreDelivery_IsValid(id, _context))
                return View("Invalid");
            Delivery delivery = _context.Delivery.GetById(id);
            if (delivery == null)
                return HttpNotFound();

            return View(delivery);
        }

        [HttpPost, ActionName("Active")]
        public ActionResult ActiveConfirmed(int id)
        {
            try
            {
                if (!BaseStore.StoreDelivery_IsValid(id, _context))
                    return View("Invalid");
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