using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer.ViewModels;


namespace Panel.Areas.Content.Controllers
{
    public class SurveyAnswerController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(
            int? surveyId = null,
            int? index = null,
            int? pagesize = null)
        {
            index = index == null ? 1 : index;
            pagesize = pagesize == null ? 10000 : pagesize;
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Survey);
            List<SurveyAnswer> list = _context.SurveyAnswer.Search(
                surveyId: surveyId,
                pageSize: pagesize.GetValueOrDefault(),
                index: index);
            return View(list.ToView());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Survey);
            SurveyAnswer surveyAnswer = _context.SurveyAnswer.GetById(id);
            if (surveyAnswer == null)
            {
                return HttpNotFound();
            }
            return View(surveyAnswer);
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