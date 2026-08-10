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
    public class DiscountGroupController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.DiscountGroup);
            return View(_context.DiscountGroup.GetAll().ToView());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.DiscountGroup_Details);
            DiscountGroup group = _context.DiscountGroup.GetById(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.DiscountGroup_New);
            FillDropDowns(null);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(new DiscountGroup());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DiscountGroup group, string DateStart, string DateEnd, HttpPostedFileBase file)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.DiscountGroup_New);
            var typeCode = _context.Code.GetByLabel(Enum_Code.DISCOUNTGROUP_TYPE_PERCENT);
            group.TypeId = typeCode.ID;
            if (IsModelValid(group, DateStart, DateEnd,file))
            {
                group.UserId = _context.SiteUser.GetCurrentUser().ID;
                _context.DiscountGroup.Insert(group);
                _context.Save();
                return RedirectToAction("Index");
            }

            FillDropDowns(group);
            return View(group);
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.DiscountGroup_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            DiscountGroup group = _context.DiscountGroup.GetById(id);
            if (group == null)
                return HttpNotFound();

            FillDropDowns(group);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(group);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DiscountGroup group, string DateStart, string DateEnd,HttpPostedFileBase file)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.DiscountGroup_Edit);
            var typeCode = _context.Code.GetByLabel(Enum_Code.DISCOUNTGROUP_TYPE_PERCENT);
            group.TypeId = typeCode.ID;
            if (IsModelValid(group, DateStart, DateEnd,file))
            {
                _context.DiscountGroup.Update(group);
                _context.Save();
                if(group.Active==false)
                {
                    var discountGroup = _context.DiscountGroup.GetById(group.ID);
                    foreach (var d in discountGroup.Discount)
                    {
                        d.Active = false;
                        d.Synced = false;
                        _context.Discount.Update(d);
                    }
                    _context.Save();
                }
                return RedirectToAction("Index");
            }
            FillDropDowns(group);
            return View(group);
        }

        private bool IsModelValid(DiscountGroup group, string DateStart, string DateEnd,HttpPostedFileBase file)
        {
            bool result = false;
            group.StartDatetime = DateStart.GetGregorian();
            group.EndDatetime = DateEnd.GetGregorian();
            if (group.Name == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_DISCOUNTGROUP_NAME);
            else
                result = true;
            if (file != null)
            {
                group.PictureId = _context.Picture.CreateAndUpload(Enum_Code.SYSTEM_TYPE_PANEL, file).ID;
            }
            return result;
        }

        private void FillDropDowns(DiscountGroup group)
        {
            ViewBag.TypeId = new SelectList(_context.Code.GetAllByCodeGroup(Enum_CodeGroup.DISCOUNTGROUP_TYPE), "ID", "Name", group != null ? group.TypeId : 0);
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.DiscountGroup_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            DiscountGroup group = _context.DiscountGroup.GetById(id);
            if (group == null)
                return HttpNotFound();

            return View(group);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.DiscountGroup_Delete);
            DiscountGroup group = _context.DiscountGroup.GetById(id);
            try
            {
                group.Active = false;
                group.Deleted = true;
                _context.DiscountGroup.Update(group);
                foreach (var d in group.Discount)
                {
                    d.Active = false;
                    d.Synced = false;
                    _context.Discount.Update(d);
                }
                _context.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(group);
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