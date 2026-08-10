using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Panel.Areas.Email.Controllers
{
    public class ComposeController : Controller
    {
        // GET: Email/Compose
        public ActionResult Index()
        {
            return View();
        }
    }
}