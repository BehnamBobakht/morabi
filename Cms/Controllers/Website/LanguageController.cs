using DataLayer.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace Cms.Controllers
{
    public class LanguageController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(string langId)
        {
            string BackUrl = Request.QueryString["backurl"];
            if(!string.IsNullOrEmpty(BackUrl) && (BackUrl.Contains("http:")|| BackUrl.Contains("https:")))
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.NotFound;
                Response.Redirect("/error/404");
            }
            else if(string.IsNullOrEmpty(BackUrl))
            {
                BackUrl = "/";
            }
            _context.WebsiteLanguage.SetCurrentLanguage(langId);
            return Redirect(BackUrl);
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