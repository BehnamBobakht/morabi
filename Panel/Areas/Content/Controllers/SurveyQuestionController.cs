using DataLayer.ViewModels;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Panel.Areas.Content.Controllers
{
    public class SurveyQuestionController : Controller
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
            List<SurveyQuestion> list = _context.SurveyQuestion.Search(
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
            SurveyQuestion surveyQuestion = _context.SurveyQuestion.GetById(id);
            if (surveyQuestion == null)
            {
                return HttpNotFound();
            }
            return View(surveyQuestion);
        }

        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Survey);
            FillDropDowns(null);
            SurveyQuestion surveyQuestion = new SurveyQuestion();
            ViewBag.Message = BaseMessage.GetMessage();
            return View(surveyQuestion);
        }

        [HttpPost]
        public ActionResult Create(SurveyQuestion surveyQuestion)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Survey);

            List<SurveyQuestionLanguage> listLanguage = surveyQuestion.SurveyQuestionLanguage.ToList();
            surveyQuestion.SurveyQuestionLanguage.Clear();

            ViewMessage result = IsModelValid(surveyQuestion);
            if (result.Type == Enum_MessageType.SUCCESS)
            {
                if (surveyQuestion.PictureId == 0)
                {
                    surveyQuestion.PictureId = null;
                }
                _context.SurveyQuestion.Insert(surveyQuestion);
                _context.Save();

                foreach (SurveyQuestionLanguage item in listLanguage)
                {
                    item.QuestionId = surveyQuestion.ID;
                    _context.SurveyQuestionLanguage.Insert(item);
                    _context.Save();
                }

                //return new JsonResult()
                //{
                //    Data = Resource.Lang.Error,
                //    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                //};

            }
            return new JsonResult() { Data = result };
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Survey);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            SurveyQuestion surveyQuestion = _context.SurveyQuestion.GetById(id);
            if (surveyQuestion == null)
                return HttpNotFound();

            FillDropDowns(surveyQuestion);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(surveyQuestion);
        }

        [HttpPost]
        public ActionResult Edit(SurveyQuestion surveyQuestion)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Survey);
            List<SurveyQuestionLanguage> listLanguage = surveyQuestion.SurveyQuestionLanguage.ToList();
            surveyQuestion.SurveyQuestionLanguage.Clear();

            ViewMessage result = IsModelValid(surveyQuestion);
            if (result.Type == Enum_MessageType.SUCCESS)
            {
                if (surveyQuestion.PictureId == 0)
                {
                    surveyQuestion.PictureId = null;
                }
                _context.SurveyQuestion.Update(surveyQuestion);
                _context.SurveyQuestionLanguage.DeleteBySurveyQuestionId(surveyQuestion.ID);
                _context.Save();
                foreach (SurveyQuestionLanguage item in listLanguage)
                {
                    item.QuestionId = surveyQuestion.ID;
                    _context.SurveyQuestionLanguage.Insert(item);
                    _context.Save();
                }

                //return new JsonResult()
                //{
                //    Data = Resource.Lang.Error,
                //    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                //};

            }
            return new JsonResult() { Data = result };
        }

        private ViewMessage IsModelValid(SurveyQuestion surveyQuestion)
        {
            ViewMessage result = new ViewMessage();
            if (surveyQuestion.Question == null)
                result = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_SURVEY_QUESTION);
            else
                result = BaseMessage.GetMessage(Enum_MessageType.SUCCESS);
            return result;
        }

        private void FillDropDowns(SurveyQuestion surveyQuestion)
        {
            ViewBag.SurveyId = new SelectList(_context.Survey.GetAll(), "ID", "Name", surveyQuestion != null ? surveyQuestion.SurveyId : 0);
            ViewBag.QuestionType = new SelectList(_context.Code.GetAllByCodeGroup(Enum_CodeGroup.SURVEY_TYPE), "ID", "Name", surveyQuestion != null ? surveyQuestion.QuestionType : 0);
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Survey);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            SurveyQuestion surveyQuestion = _context.SurveyQuestion.GetById(id);
            if (surveyQuestion == null)
                return HttpNotFound();

            return View(surveyQuestion);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Survey);
            SurveyQuestion surveyQuestion = _context.SurveyQuestion.GetById(id);
            List<SurveyQuestionLanguage> lang = _context.SurveyQuestionLanguage.GetAllBySurveyQuestionId(id);
            try
            {
                if (lang.Any())
                {
                    foreach (var item in lang)
                    {
                        _context.SurveyQuestionLanguage.Delete(item);
                    }
                }
                _context.SurveyQuestion.Delete(surveyQuestion);
                _context.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(surveyQuestion);
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