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
    public class SurveyQuestionItemController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(
            int? questionId = null,
            int? index = null,
            int? pagesize = null)
        {
            index = index == null ? 1 : index;
            pagesize = pagesize == null ? 10000 : pagesize;
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Survey);
            List<SurveyQuestionItem> list = _context.SurveyQuestionItem.Search(
                questionId: questionId,
                pageSize: pagesize.GetValueOrDefault(),
                index: index);
            return View(list.ToView());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Survey);
            SurveyQuestionItem surveyQuestionItem = _context.SurveyQuestionItem.GetById(id);
            if (surveyQuestionItem == null)
            {
                return HttpNotFound();
            }
            return View(surveyQuestionItem);
        }

        public ActionResult Create(int questionId )
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Survey);
            FillDropDowns(null);
            SurveyQuestionItem surveyQuestionItem = new SurveyQuestionItem()
            {
                QuestionId = questionId
            };
            ViewBag.Question = _context.SurveyQuestion.FirstOrDefault(x=>x.ID==questionId).Question;
            ViewBag.Message = BaseMessage.GetMessage();
            return View(surveyQuestionItem);
        }

        [HttpPost]
        public ActionResult Create(SurveyQuestionItem surveyQuestionItem)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Survey);

            List<SurveyQuestionItemLanguage> listLanguage = surveyQuestionItem.SurveyQuestionItemLanguage.ToList();
            surveyQuestionItem.SurveyQuestionItemLanguage.Clear();

            ViewMessage result = IsModelValid(surveyQuestionItem);
            if (result.Type == Enum_MessageType.SUCCESS)
            {
                _context.SurveyQuestionItem.Insert(surveyQuestionItem);
                _context.Save();

                foreach (SurveyQuestionItemLanguage item in listLanguage)
                {
                    item.QuestionItemID = surveyQuestionItem.ID;
                    _context.SurveyQuestionItemLanguage.Insert(item);
                    _context.Save();
                }

                //return new JsonResult()
                //{
                //    Data = Resource.Lang.Error,
                //    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                //};

            }
            return new JsonResult() { Data = result, };
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Survey);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            SurveyQuestionItem surveyQuestionItem = _context.SurveyQuestionItem.GetById(id);
            if (surveyQuestionItem == null)
                return HttpNotFound();

            FillDropDowns(surveyQuestionItem);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(surveyQuestionItem);
        }

        [HttpPost]
        public ActionResult Edit(SurveyQuestionItem surveyQuestionItem)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Survey);
            List<SurveyQuestionItemLanguage> listLanguage = surveyQuestionItem.SurveyQuestionItemLanguage.ToList();
            surveyQuestionItem.SurveyQuestionItemLanguage.Clear();

            ViewMessage result = IsModelValid(surveyQuestionItem);
            if (result.Type == Enum_MessageType.SUCCESS)
            {
                _context.SurveyQuestionItem.Update(surveyQuestionItem);
                _context.SurveyQuestionItemLanguage.DeleteBySurveyQuestionItemId(surveyQuestionItem.ID);
                _context.Save();
                foreach (SurveyQuestionItemLanguage item in listLanguage)

                {
                    item.QuestionItemID = surveyQuestionItem.ID;
                    _context.SurveyQuestionItemLanguage.Insert(item);
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

        private ViewMessage IsModelValid(SurveyQuestionItem surveyQuestionItem)
        {
            ViewMessage result = new ViewMessage();
            if (surveyQuestionItem.Name == null)
                result = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_SURVEY_ITEM);
            else
                result = BaseMessage.GetMessage(Enum_MessageType.SUCCESS);
            return result;
        }

        private void FillDropDowns(SurveyQuestionItem surveyQuestionItem)
        {
            ViewBag.QuestionId = new SelectList(_context.SurveyQuestion.GetAll(), "ID", "Name", surveyQuestionItem != null ? surveyQuestionItem.QuestionId : 0);
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Survey);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            SurveyQuestionItem surveyQuestionItem = _context.SurveyQuestionItem.GetById(id);
            if (surveyQuestionItem == null)
                return HttpNotFound();

            return View(surveyQuestionItem);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Survey);
            SurveyQuestionItem surveyQuestionItem = _context.SurveyQuestionItem.GetById(id);
            List<SurveyQuestionItemLanguage> lang = _context.SurveyQuestionItemLanguage.GetAllBySurveyQuestionItemId(id);
            try
            {
                if (lang.Any())
                {
                    foreach (var item in lang)
                    {
                        _context.SurveyQuestionItemLanguage.Delete(item);
                    }
                }
                _context.SurveyQuestionItem.Delete(surveyQuestionItem);
                _context.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(surveyQuestionItem);
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