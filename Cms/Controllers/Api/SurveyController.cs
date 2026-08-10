using DataLayer.Api;
using DataLayer.Api.Model;
using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace Cms.Controllers.Api
{
    public class SurveyController : ApiController
    {
        UnitOfWork _context = new UnitOfWork();

        [HttpPost]
        public ApiResult Post(List<ViewSurveyAnswerModel> model, int? surveyId)
        {
            var survey = _context.Survey.FirstOrDefault(s => s.ID == surveyId && s.Active == true);
            if (survey != null)
            {
                ViewAccount account = _context.Account.GetCurrentAccount();
                int? accountId = account != null ? account.Id : default(int?);
                SurveyAnswer surveyAnswer = new SurveyAnswer();
                surveyAnswer.CreateDateTime = DateTime.Now;
                surveyAnswer.AccountId = accountId;
                surveyAnswer.SurveyId = surveyId;
                surveyAnswer.Name = survey.Name;
                _context.SurveyAnswer.Insert(surveyAnswer);
                _context.Save();
                EmailAddress address = _context.EmailAddress.FirstOrDefault();
                if (address != null)
                {

                    StringBuilder body = new StringBuilder();
                    body.AppendLine("یک نظرسنجی جدید در سایت ثبت شد");
                    body.AppendLine("مشاهده فرم :"+BaseWebsite.ShopUrl+ "/panel/content/SurveyAnswer/Details/"+surveyAnswer.ID);
                    _context.Email.SaveNewEmail(BaseContent.WebsiteDetail.Email, Enum_EmailType.SURVEY, body.ToString(), "نظرسنجی");
                    _context.Save();
                }
                
                foreach (var item in model)
                {
                    SurveyAnswerDetail answer = new SurveyAnswerDetail();
                   
                    answer.AnswerId = surveyAnswer.ID;
                    answer.QuestionId = item.QuestionId;
                    if(item.Answerchecked!="" && item.Answerchecked != null)
                    {
                        answer.AnswerText = item.Answerchecked;
                    }
                    else
                    {
                        answer.AnswerText = item.Answer;
                    }
                    if (item.FileId != 0)
                    {
                        answer.FileId = item.FileId;
                    }
                    

                   
                    _context.SurveyAnswerDetail.Insert(answer);
                    _context.Save();
                   
                }
            }

            ApiResult result = new ApiResult();
            result.Code = ApiResult.ResponseCode.Success;
            result.Message = Resource.Lang.SuccessfulyOrder;

            return result;


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
