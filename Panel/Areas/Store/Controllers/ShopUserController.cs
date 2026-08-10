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
    public class ShopUserController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ShopUser);
            ViewBag.ShopId = id;
            return View(_context.ShopUser.GetAllByShopId(id.Value).ToView());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ShopUser_Details);
            ShopUser shopuser = _context.ShopUser.GetById(id);
            if (shopuser == null)
            {
                return HttpNotFound();
            }
            ViewBag.ShopId = shopuser.ShopId;
            return View(shopuser);
        }

        public ActionResult Create(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ShopUser_New);
            FillDropDowns(null);
            Shop shop = _context.Shop.GetById(id.Value);
            ShopUser shopuser = new ShopUser() {
                ShopId = id.Value,
                Shop = shop
            };
            ViewBag.ShopId = id.Value;
            ViewBag.Message = BaseMessage.GetMessage();
            return View(shopuser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ShopUser shopuser)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ShopUser_New);

            if (IsModelValid(shopuser))
            {
                _context.ShopUser.Insert(shopuser);
                _context.Save();
                return RedirectToAction("Index", new { @id=shopuser.ShopId });
            }
            ViewBag.ShopId = shopuser.ShopId;
            FillDropDowns(shopuser);
            return View(shopuser);
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ShopUser_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ShopUser shopuser = _context.ShopUser.GetById(id);
            if (shopuser == null)
                return HttpNotFound();

            FillDropDowns(shopuser);
            ViewBag.Message = BaseMessage.GetMessage();
            ViewBag.ShopId = id.Value;
            return View(shopuser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ShopUser shopuser)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ShopUser_Edit);
            if (IsModelValid(shopuser))
            {
                _context.ShopUser.Update(shopuser);
                _context.Save();
                return RedirectToAction("Index", new { @id = shopuser.ShopId });
            }
            FillDropDowns(shopuser);
            ViewBag.ShopId = shopuser.ShopId;
            return View(shopuser);
        }

        private bool IsModelValid(ShopUser shopuser)
        {
            return true;
        }

        private void FillDropDowns(ShopUser shopuser)
        {
            ViewBag.UserId = new SelectList(_context.SiteUser.GetAll(), "ID", "FullName", shopuser != null ? shopuser.UserId : 0);
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ShopUser_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ShopUser shopuser = _context.ShopUser.GetById(id);
            if (shopuser == null)
                return HttpNotFound();

            ViewBag.ShopId = shopuser.ShopId;
            return View(shopuser);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ShopUser_Delete);
            ShopUser shopuser = _context.ShopUser.GetById(id);

            try
            {
                _context.ShopUser.Delete(shopuser);
                _context.Save();
                return RedirectToAction("Index", new { @id = shopuser.ShopId });
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(shopuser);
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