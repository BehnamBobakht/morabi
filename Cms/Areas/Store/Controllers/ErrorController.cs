using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer.Entities;
using DataLayer.Base;
using DataLayer.Api;


namespace Cms.Areas.Store.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Error(string back=null)
        {
            ViewBag.back = back;  
            return View();
        }
        public ActionResult Invalid(string back=null)
        {
            ViewBag.back = back;
            return View();
        }      
    }
}