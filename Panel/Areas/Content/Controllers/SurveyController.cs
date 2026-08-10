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
    public class SurveyController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Survey);
            return View(_context.Survey.Search(pageSize: 9999999));
            //return View(_context.Survey.GetAll().ToView());
        }

        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Survey);
            Survey survey = new Survey() { DateTime = DateTime.Now };

            return View(survey);
        }

        [HttpPost]

        public ActionResult Create(Survey survey)
        {

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Survey);
            ViewMessage error = new ViewMessage() { Type = Enum_MessageType.SUCCESS };
            //----
            var tempLabel = _context.Survey.Where(s => s.Label == survey.Label).FirstOrDefault();
            if (tempLabel != null)
            {
                error.Body = Resource.Notify.LabelDuplicate;
                error.Type = Enum_MessageType.ERROR;
            }
            else
            {
                List<SurveyLanguage> listLanguage = survey.SurveyLanguage.ToList();
                survey.SurveyLanguage.Clear();
                if (IsModelValid(survey, out error))
                {
                    _context.Survey.Insert(survey);
                    bool result = _context.Save();
                    if (result == true)
                    {
                        foreach (SurveyLanguage item in listLanguage)
                        {
                            item.SurveyId = survey.ID;
                            _context.SurveyLanguage.Insert(item);
                        }
                        _context.Save();
                    }
                }
            }
            return new JsonResult() { Data = error };
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Survey);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Survey survey = _context.Survey.GetById(id);
            if (survey == null)
                return HttpNotFound();

            ViewBag.Message = BaseMessage.GetMessage();
            return View(survey);
        }

        [HttpPost]
        public ActionResult Edit(Survey survey)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Survey);
            ViewMessage error = new ViewMessage() { Type = Enum_MessageType.SUCCESS };
            //----
            var tempLabel = _context.Survey.Where(s => s.Label == survey.Label && s.ID != survey.ID).FirstOrDefault();
            if (tempLabel != null)
            {
                error.Body = Resource.Notify.LabelDuplicate;
                error.Type = Enum_MessageType.ERROR;
            }
            else
            {
                List<SurveyLanguage> listLanguage = survey.SurveyLanguage.ToList();
                survey.SurveyLanguage.Clear();
                _context.SurveyLanguage.DeleteBySurveyId(survey.ID);

                if (IsModelValid(survey, out error))
                {
                    _context.Survey.Update(survey);
                    _context.Save();
                    foreach (SurveyLanguage item in listLanguage)
                    {
                        item.SurveyId = survey.ID;
                        _context.SurveyLanguage.Insert(item);
                    }
                    _context.Save();
                }
            }
            return new JsonResult() { Data = error };
        }
        private bool IsModelValid(Survey survey, out ViewMessage msg)
        {
            bool result = false;
            if (survey.Name == null)
            {
                msg = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PRODUCT_NAME);
            }
            else
            {
                msg = BaseMessage.GetMessage(Enum_MessageType.SUCCESS);
                result = true;
            }
            return result;
        }


        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Survey);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Survey survey = _context.Survey.GetById(id);
            if (survey == null)
                return HttpNotFound();

            return View(survey);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Survey);
            Survey survey = _context.Survey.GetById(id);
            try
            {
                List<SurveyLanguage> surveyLanguage = _context.SurveyLanguage.Where(x => x.SurveyId == survey.ID).ToList();
                if (surveyLanguage.Count > 0)
                {
                    foreach (var item in surveyLanguage)
                    {
                        _context.SurveyLanguage.Delete(item);
                    }

                }
                _context.Survey.Delete(survey);
                _context.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(survey);
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