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
    public class ShopContactController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ShopContact);
            if (id == null)
                ViewBag.Shop = _context.Shop.FirstOrDefault();
            else
                ViewBag.Shop = _context.Shop.GetById(id.GetValueOrDefault());
            ViewBag.ShopId = id;
            return View(_context.ShopContact.GetAllByShopId(id.Value).ToView());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ShopContact_Details);
            ShopContact contact = _context.ShopContact.GetById(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            ViewBag.ShopId = contact.ShopId;
            return View(contact);
        }

        public ActionResult Create(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ShopContact_New);
            FillDropDowns(null);
            ViewBag.ShopId = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ShopContact contact)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ShopContact_New);

            if (IsModelValid(contact))
            {
                _context.ShopContact.Insert(contact);
                _context.Save();
                return RedirectToAction("Index", new { @id = contact.ShopId });
            }

            FillDropDowns(contact);
            ViewBag.ShopId = contact.ShopId;
            return View(contact);
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ShopContact_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ShopContact contact = _context.ShopContact.GetById(id);
            if (contact == null)
                return HttpNotFound();

            FillDropDowns(contact);
            ViewBag.Message = BaseMessage.GetMessage();
            ViewBag.ShopId = contact.ShopId;
            return View(contact);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ShopContact contact)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ShopContact_Edit);
            if (IsModelValid(contact))
            {
                _context.ShopContact.Update(contact);
                _context.Save();
                return RedirectToAction("Index", new { @id = contact.ShopId });
            }
            FillDropDowns(contact);
            ViewBag.ShopId = contact.ShopId;
            return View(contact);
        }

        private bool IsModelValid(ShopContact contact)
        {
            bool result = false;
            if (contact.Value == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_SHOPCONTACT_VALUE);
            else
                result = true;
            return result;
        }

        private void FillDropDowns(ShopContact contact)
        {
            SiteUser user = _context.SiteUser.GetCurrentUser();
            ViewBag.TypeId = new SelectList(_context.Code.GetAllByCodeGroup(Enum_CodeGroup.CONTACT_TYPE), "ID", "Name", contact != null ? contact.TypeId : 0);
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ShopContact_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ShopContact contact = _context.ShopContact.GetById(id);
            if (contact == null)
                return HttpNotFound();
            ViewBag.ShopId = contact.ShopId;
            return View(contact);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ShopContact_Delete);
            ShopContact contact = _context.ShopContact.GetById(id);
            _context.ShopContact.Delete(contact);
            _context.Save();
            return RedirectToAction("Index", new { @id = contact.ShopId });
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