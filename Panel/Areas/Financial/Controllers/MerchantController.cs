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

namespace Panel.Areas.Financial.Controllers
{
    [ValidateInput(false)]
    public class MerchantController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            SiteUser CurrentUser = _context.SiteUser.GetCurrentUser();
            _context.Permission.CheckPagePermission(CurrentUser, Enum_Permission.Merchant);
            return View(_context.Merchant.GetAll().ToView());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Merchant_Details);
            Merchant merchant = _context.Merchant.GetById(id);
            if (merchant == null)
            {
                return HttpNotFound();
            }
            return View(merchant);
        }

        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Merchant_New);
            Merchant merchant = new Merchant()
            {
                Active = true
            };
            ViewBag.Message = BaseMessage.GetMessage();
            FillDropDowns(null);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Merchant merchant)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Merchant_New);

            if (IsModelValid(merchant).Type == Enum_MessageType.SUCCESS)
            {
                _context.Merchant.Insert(merchant);
                _context.Save();

                return RedirectToAction("index");
            }

            FillDropDowns(merchant);
            return View(merchant);
        }
        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Merchant_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Merchant merchant = _context.Merchant.GetById(id);
            if (merchant == null)
                return HttpNotFound();

            FillDropDowns(merchant);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(merchant);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Merchant merchant)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Merchant_Edit);
            if (IsModelValid(merchant).Type == Enum_MessageType.SUCCESS)
            {
                _context.Merchant.Update(merchant);
                _context.Save();
                return RedirectToAction("Index");
            }
            FillDropDowns(merchant);
            return View(merchant);
        }

        private ViewMessage IsModelValid(Merchant merchant)
        {
            ViewMessage result = new ViewMessage();
            ViewBag.Message = result;
            return result;
        }

        private void FillDropDowns(Merchant merchant)
        {
            ViewBag.BankId = new SelectList(_context.Bank.GetAll(), "ID", "Name", merchant != null ? merchant.BankId: 0);
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Merchant_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Merchant merchant = _context.Merchant.GetById(id);
            if (merchant == null)
                return HttpNotFound();

            return View(merchant);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Merchant_Delete);
            Merchant merchant = _context.Merchant.GetById(id);
            try
            {
                _context.Merchant.Delete(merchant);
                _context.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(merchant);
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