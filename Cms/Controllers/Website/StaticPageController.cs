using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cms.Controllers.Website;
using DataLayer.Entities;
using DataLayer.Enumarables;

namespace Cms.Controllers
{
    public class StaticPageController : Controller
    {

        UnitOfWork _context = new UnitOfWork();
   //     [CustomCache(Duration = 1, cacheType = Enum_OutputCache.StaticPage,
   //VaryByParam = "*")]
        public ActionResult Index(string id,string parameter=null,
                        string lang = "")
        {
            ViewBag.parameter = parameter;
            
            return PartialView(BaseController.GetView(this, id));             
        }


        public ActionResult Static(string id)
        {

            var model = _context.StaticPage.GetByCategory(id);
            if (model == null)
                return Redirect("/");
            return PartialView(BaseController.GetView(this),model);
        }
    }

}