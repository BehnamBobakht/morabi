using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Panel.Areas.Content.Controllers
{
    public class WebsiteFormAccountController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.WebsiteForm);
            ViewBag.WebsiteForm = _context.WebsiteForm.GetById(id);
            return View(_context.WebsiteFormAccount.GetAllByFormId(id).OrderByDescending(p => p.ID));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.WebsiteForm_Details);
            WebsiteFormAccount formAccount = _context.WebsiteFormAccount.GetById(id);
            if (formAccount == null)
            {
                return HttpNotFound();
            }
            return View(formAccount);
        }
        
        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.WebsiteForm_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            WebsiteFormAccount formAccount = _context.WebsiteFormAccount.GetById(id);
            if (formAccount == null)
                return HttpNotFound();

            return View(formAccount);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.WebsiteForm_Delete);
            WebsiteFormAccount formAccount = _context.WebsiteFormAccount.GetById(id);
            try
            {

                _context.WebsiteFormAccount.Delete(formAccount);
                _context.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(formAccount);
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