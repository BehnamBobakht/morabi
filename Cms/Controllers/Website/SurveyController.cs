using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace Cms.Controllers
{
    public class SurveyController : Controller
    {
        UnitOfWork _context = new UnitOfWork();
        public ActionResult Index(int? quid, string quname = "")
        {
            SurveyQuestion question = _context.SurveyQuestion.GetById(quid);
            return PartialView(BaseController.GetView(this), question);
        }
        public ActionResult Search(string quViewName = null)

        {

            quViewName = quViewName == null ? "Search" : quViewName;
            List<Survey> results = _context.Survey.Where(s => s.Active == true).ToList();

            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, quViewName, results);
        }
        public ActionResult Start(int surveyId)

        {
            Survey Survey = _context.Survey.FirstOrDefault(s => s.Active == true && s.ID == surveyId);
            if (Survey != null)
            {
                var results = _context.SurveyQuestion.Where(s => s.SurveyId == surveyId && s.Survey.Active == true && s.Active == true).ToList();
                ViewSurveyQuestionUser model = new ViewSurveyQuestionUser();
                List<ViewSurveyQuestionUserItem> Questions = new List<ViewSurveyQuestionUserItem>();

                foreach (var item in results.OrderBy(s => s.ShowNumber))
                {
                    ViewSurveyQuestionUserItem question = new ViewSurveyQuestionUserItem();
                    if (item.PictureId != null)
                    {
                        question.PictureUrl = item.Picture.GetUrl();
                    }
                    question.QuestionId = item.ID;
                    question.Question = item.Question;
                    question.RelatedId = item.ReletedQuestionId;
                    var listItem = _context.SurveyQuestionItem.Where(s => s.QuestionId == item.ID && s.Active == true).ToList();
                    List<SurveyQuestionItem> surveritem = new List<SurveyQuestionItem>();
                    foreach (var questionItem in listItem)
                    {
                        SurveyQuestionItem temp = new SurveyQuestionItem();
                        temp.Name = questionItem.Name;
                        temp.ID = questionItem.ID;

                        surveritem.Add(temp);
                    }
                    question.SurveyQuestionItems = surveritem;
                    if (question.RelatedId != null)
                    {

                        question.HaseRelated = true;



                    }
                    else
                    {
                        question.HaseRelated = false;
                    }
                    var code = _context.Code.GetById(item.QuestionType);
                    question.QuestionType = code.Label.ToString();
                    Questions.Add(question);
                }
                model.Questions = Questions;
                model.SurveyId = Survey.ID;
                model.SurveyName = Survey.Name;

                return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, "Start", model);

            }
            else
            {
                return Redirect(BaseController.RedirectToError());
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