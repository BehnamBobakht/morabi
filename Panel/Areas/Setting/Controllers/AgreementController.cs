using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Panel.Areas.Setting.Controllers
{
    public class AgreementController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Agreement);
            return View(_context.Agreement.GetAll().ToView());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Agreement_Details);
            Agreement agreement = _context.Agreement.GetById(id);
            if (agreement == null)
            {
                return HttpNotFound();
            }
            return View(agreement);
        }

        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Agreement_New);
            FillDropDowns(null);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Agreement agreement)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Agreement_New);

            if (IsModelValid(agreement))
            {
                _context.Agreement.Insert(agreement);
                _context.Save();
                return RedirectToAction("Index");
            }

            FillDropDowns(agreement);
            return View(agreement);
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Agreement_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Agreement agreement = _context.Agreement.GetById(id);
            if (agreement == null)
                return HttpNotFound();

            FillDropDowns(agreement);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(agreement);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Agreement agreement)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Agreement_Edit);
            if (IsModelValid(agreement))
            {
                _context.Agreement.Update(agreement);
                _context.Save();
                return RedirectToAction("Index");
            }
            FillDropDowns(agreement);
            return View(agreement);
        }

        private bool IsModelValid(Agreement agreement)
        {
            bool result = false;

            if (agreement.Name == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_AGREEMENT_NAME);
            else if (agreement.Employer == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_AGREEMENT_EMPLOYER);
            else
                result = true;
            
            return result;
        }

        private void FillDropDowns(Agreement agreement)
        {
            ViewBag.TypeId = new SelectList(_context.Code.GetAllByCodeGroup(Enum_CodeGroup.PLATFORM_TYPE), "ID", "Name", agreement != null ? agreement.TypeId : 0);
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Agreement_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Agreement agreement = _context.Agreement.GetById(id);
            if (agreement == null)
                return HttpNotFound();

            return View(agreement);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Agreement_Delete);
            Agreement agreement = _context.Agreement.GetById(id);
            try
            {
                _context.Agreement.Delete(agreement);
                _context.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(agreement);
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