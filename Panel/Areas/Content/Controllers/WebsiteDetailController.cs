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
    [ValidateInput(false)]

    public class WebsiteDetailController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.WebsiteDetail);
            return View(_context.WebsiteDetail.GetAll());
        }
     
        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.WebsiteDetail_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            WebsiteDetail websiteDetail = _context.WebsiteDetail.GetById(id);
            if (websiteDetail == null)
                return HttpNotFound();

            ViewBag.Message = BaseMessage.GetMessage();
            return View(websiteDetail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(WebsiteDetail websiteDetail)
        {
            try
            {
                //_context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.WebsiteDetail_Edit);

                _context.WebsiteDetail.Update(websiteDetail);
                _context.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

    }
}