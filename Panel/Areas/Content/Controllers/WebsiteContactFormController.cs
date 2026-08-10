using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Panel.Areas.Content.Controllers
{
    public class WebsiteContactFormController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.WebsiteContactForm);
            return View(_context.WebsiteContactForm.GetAll().OrderByDescending(x => x.ID).ToList().ToView());
        }

        private bool IsModelValid(WebsiteContactForm contactForm)
        {
            bool result = false;
            if (contactForm.FullName == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_GALLERY_NAME);
            else
                result = true;
            return result;
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.WebsiteContactForm_Details);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            WebsiteContactForm contactForm = _context.WebsiteContactForm.GetById(id);
            if (contactForm == null)
                return HttpNotFound();
            return View(contactForm);
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