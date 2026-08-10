using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cms.Controllers.Website
{
    public class WebsiteViewController : Controller
    {
        UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult CountVisit()
        //{
        //    if (Session["SITEVIEW"] == null)
        //    {
        //        try
        //        {
        //            _context.WebsiteView.IncreaseView();
        //            _context.Save();
        //        }
        //        catch (Exception) {
                    
        //        }
        //        Session["SITEVIEW"] = Session.SessionID;
        //    }
        //    return null;
        //}
        

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