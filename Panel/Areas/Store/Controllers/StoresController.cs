using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Panel.Areas.Store.Controllers
{
    public class StoresController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Store);
            return View(_context.Store.Where(s=>s.Website!="1" && s.Deleted!=true).ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Store_Details);
            DataLayer.Entities.Store store = _context.Store.GetById(id);
            if (store == null)
            {
                return HttpNotFound();
            }
            return View(store);
        }

        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Store_New);
            FillDropDowns(null);
            return View(new DataLayer.Entities.Store());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DataLayer.Entities.Store store)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Store_New);

            if (IsModelValid(store))
            {
                store.CrateDate = DateTime.Now;
                _context.Store.Insert(store);
                _context.Save();
                return RedirectToAction("Index");
            }

            FillDropDowns(store);
            return View(store);
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Store_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            DataLayer.Entities.Store store = _context.Store.GetById(id);
            if (store == null)
                return HttpNotFound();

            FillDropDowns(store);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(store);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DataLayer.Entities.Store store)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Store_Edit);
            if (IsModelValid(store))
            {
                _context.Store.Update(store);
                _context.Save();
                BaseStore.UpdateProductQuantity(store, _context);
                return RedirectToAction("Index");
            }
            FillDropDowns(store);
            return View(store);
        }

        private bool IsModelValid(DataLayer.Entities.Store store)
        {
            if (store.Name != null)
            {
                return true;
            }
            else
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_STORE_NAME);
            return false;
        }

        private void FillDropDowns(DataLayer.Entities.Store store)
        {
            ViewBag.AccountId = new SelectList(_context.Account.GetAll(), "ID", "FullName", store != null ? store.AccountId : 0);
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Store_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            DataLayer.Entities.Store store = _context.Store.GetById(id);
            if (store == null)
                return HttpNotFound();

            return View(store);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Store_Delete);
            DataLayer.Entities.Store store = _context.Store.GetById(id);
            try
            {
                _context.Store.Delete(store);
                _context.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(store);
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

        public ActionResult ApproveProduct(int storeProductId)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Store_Edit);

            StoreProduct storeProduct = _context.StoreProduct.GetById(storeProductId);
            storeProduct.StatusId = _context.Code.GetByLabel(Enum_Code.STOREPRODUCT_REQUEST_APPROVED).ID;
            _context.StoreProduct.Update(storeProduct);
            _context.Save();
            BaseStore.UpdateProductQuantity(storeProduct.ProductId, _context);
            return Redirect("~/");
        }

        public ActionResult RejectProduct(int storeProductId)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Store_Edit);

            StoreProduct storeProduct = _context.StoreProduct.GetById(storeProductId);
            storeProduct.StatusId = _context.Code.GetByLabel(Enum_Code.STOREPRODUCT_REQUEST_REJECTED).ID;
            _context.StoreProduct.Update(storeProduct);
            _context.Save();

            return Redirect("~/");
        }


    }
}