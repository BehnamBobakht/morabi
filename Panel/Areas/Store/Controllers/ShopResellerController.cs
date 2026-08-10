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
    [ValidateInput(false)]
    public class ShopResellerController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ShopReseller);
            return View(_context.ShopReseller.GetAll());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ShopReseller_Details);
            ShopReseller reseller = _context.ShopReseller.GetById(id);
            if (reseller == null)
            {
                return HttpNotFound();
            }
            return View(reseller);
        }

        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ShopReseller_New);
            FillDropDowns(null);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ShopId,CityId,UserId,Name,AddressValue,Manager,Phone1,Phone2,Fax,Website,Description")] ShopReseller reseller, HttpPostedFileBase file, HttpPostedFileBase filePersonal, HttpPostedFileBase fileCover)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ShopReseller_New);

            if (IsModelValid(reseller, file, fileCover, filePersonal))
            {
                _context.ShopReseller.Insert(reseller);
                _context.Save();
                return RedirectToAction("Index");
            }

            FillDropDowns(reseller);
            return View(reseller);
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ShopReseller_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ShopReseller reseller = _context.ShopReseller.GetById(id);
            if (reseller == null)
                return HttpNotFound();

            FillDropDowns(reseller);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(reseller);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ShopId,CityId,UserId,PictureId,Name,AddressValue,Manager,Phone1,Phone2,Fax,Website,Description")] ShopReseller reseller, HttpPostedFileBase file, HttpPostedFileBase filePersonal, HttpPostedFileBase fileCover)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ShopReseller_Edit);
            if (IsModelValid(reseller, file, fileCover, filePersonal))
            {
                _context.ShopReseller.Update(reseller);
                _context.Save();
                return RedirectToAction("Index");
            }
            FillDropDowns(reseller);
            return View(reseller);
        }

        private bool IsModelValid(ShopReseller reseller, HttpPostedFileBase file, HttpPostedFileBase fileCover, HttpPostedFileBase filePersonal)
        {
            if (file != null)
            {
                reseller.PictureId = _context.Picture.CreateAndUpload(Enum_Code.SYSTEM_TYPE_PANEL, file).ID;
            }

            if (fileCover != null)
            {
                reseller.CoverPictureId = _context.Picture.CreateAndUpload(Enum_Code.SYSTEM_TYPE_PANEL, fileCover).ID;
            }

            if (filePersonal != null)
            {
                reseller.PersonalPictureId = _context.Picture.CreateAndUpload(Enum_Code.SYSTEM_TYPE_PANEL, filePersonal).ID;
            }

            return true;
        }

        private void FillDropDowns(ShopReseller reseller)
        {
            ViewBag.ShopId = new SelectList(_context.Shop.GetAll(), "ID", "Name", reseller != null ? reseller.ShopId : 0);
            ViewBag.CityId = new SelectList(_context.City.GetAll(), "ID", "Name", reseller != null ? reseller.CityId : 0);
            ViewBag.UserId = new SelectList(_context.SiteUser.GetAllByUserRole(Enum_UserRole.RESELLER), "ID", "FullName", reseller != null ? reseller.UserId : 0);
        }

        public ActionResult ApproveProduct(int resellerProductId)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ShopReseller_Edit);

            ShopResellerProduct resellerProduct = _context.ShopResellerProduct.GetById(resellerProductId);
            resellerProduct.StatusId = _context.Code.GetByLabel(Enum_Code.STOREPRODUCT_REQUEST_APPROVED).ID;
            _context.ShopResellerProduct.Update(resellerProduct);
            _context.Save();

            return Redirect("~/");
        }

        public ActionResult RejectProduct(int resellerProductId)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ShopReseller_Edit);

            ShopResellerProduct resellerProduct = _context.ShopResellerProduct.GetById(resellerProductId);
            resellerProduct.StatusId = _context.Code.GetByLabel(Enum_Code.STOREPRODUCT_REQUEST_REJECTED).ID;
            _context.ShopResellerProduct.Update(resellerProduct);
            _context.Save();

            return Redirect("~/");
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ShopReseller_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ShopReseller reseller = _context.ShopReseller.GetById(id);
            if (reseller == null)
                return HttpNotFound();

            return View(reseller);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ShopReseller_Delete);
            ShopReseller reseller = _context.ShopReseller.GetById(id);
            _context.ShopReseller.Delete(reseller);
            _context.Save();
            return RedirectToAction("Index");
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