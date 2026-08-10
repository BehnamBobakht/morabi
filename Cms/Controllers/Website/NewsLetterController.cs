using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cms.Controllers.Website
{
    public class NewsLetterController : Controller
    {
        UnitOfWork _context = new UnitOfWork();
        public ActionResult Index()
        {
            return PartialView(BaseController.GetView(this));
        }

        public ActionResult Search(
          string nlEmail = null,
          string nlMobile = null,
          string nlviewName = null
    )
        {
            nlviewName = nlviewName == null ? "Search" : nlviewName;
            var currentLang = BaseLanguage.GetCurrentLanguage();
            List<Newsletter> results = _context.NewsLetter.Search(
                Email: nlEmail,
                Mobile: nlMobile,
                lang: currentLang
                );


            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, nlviewName, results);
        }
    }
}