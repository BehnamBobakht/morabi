using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace Cms.Controllers
{
    public class SurveyQuestionController : Controller
    {
        UnitOfWork _context = new UnitOfWork();
        public ActionResult Index(int? quid, string quname = "")
        {
            SurveyQuestion question = _context.SurveyQuestion.GetById(quid);
            return PartialView(BaseController.GetView(this), question);
        }
        public ActionResult Search(
            int? quNotId = null,
            string quQuestion = null,
            string quSurveyLabel = null,
            int? quSurveyId = null,
            int? quPageSize = 0,
            int? quIndex = null,
            string quViewName = null,
            Boolean? quActive = null,
            Enum_SurveyQuestion quOrder = Enum_SurveyQuestion.NONE)

        {
            quIndex = quIndex != null ? quIndex.Value : 1;
            quPageSize = quPageSize != null ? quPageSize.Value : 40;
            quViewName = quViewName == null ? "Search" : quViewName;
            List<SurveyQuestion> results = _context.SurveyQuestion.Search(
                notId: quNotId,
                surveyId: quSurveyId,
                pageSize: quPageSize,
                index: quIndex,
                order: quOrder,
                question: quQuestion,
                surveyLabel: quSurveyLabel,
                active: quActive
                );

            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, quViewName, results);
        }

        public ActionResult Create(int id)//SurveyId
        {
            var model = _context.Survey.GetSurveyVm(id);
            return PartialView(BaseController.GetView(this), model);
        }
        [HttpPost]
        public ActionResult Create(SurveyAnswerVm surveyAnswer)
        {
            var survey = _context.Survey.GetSurveyVm(surveyAnswer.ID);
            survey.SurveyAnswerDetailsVm = surveyAnswer.SurveyAnswerDetailsVm;
            survey.Name = surveyAnswer.Name;
            survey.SurveyId = surveyAnswer.SurveyId;
            if (string.IsNullOrEmpty(surveyAnswer.Name))
            {
                survey.Message = Resource.Notify.PleaseInsertTheName;
                return PartialView(BaseController.GetView(this), survey);
            }
            foreach (var su in survey.SurveyVm.SurveyQuestionsVm)
            {
                var existQuestion = surveyAnswer.SurveyAnswerDetailsVm.FirstOrDefault(s => s.QuestionId == su.ID);
                if (existQuestion != null)
                {
                    if (existQuestion.SelectedItemIds.Any() == false && string.IsNullOrEmpty(existQuestion.AnswerText))
                    {
                        survey.Message = Resource.Notify.PleaseAnswerAllQuestions;
                        return PartialView(BaseController.GetView(this), survey);
                    }
                }
                else
                {
                    survey.Message = Resource.Notify.PleaseAnswerAllQuestions;
                    return PartialView(BaseController.GetView(this), survey);
                }
            }
            _context.SurveyAnswer.Insert(surveyAnswer);
            survey.Success = true;
            survey.Message = Resource.Notify.OperationSuccessful;
            return PartialView(BaseController.GetView(this), survey);
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