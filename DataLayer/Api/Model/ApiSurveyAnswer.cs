using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Api.Model
{
    public class ApiSurveyAnswer : ApiResponse
    {
        public static ApiResult Post(UnitOfWork _context, SurveyAnswer surveyAnswer)
        {
            ViewAccount account = _context.Account.GetCurrentAccount();
            int? accountId = account != null ? account.Id : default(int?);
            surveyAnswer.CreateDateTime = DateTime.Now;
            surveyAnswer.AccountId = accountId;
            ApiResult result = new ApiResult()
            {
                Code = ApiResult.ResponseCode.Success,
                Message = BaseMessage.GetMessage(Enum_Message.SUCCESSFULL_CONTACTUS).Body
            };

            _context.SurveyAnswer.Insert(surveyAnswer);
            _context.Save();

            return result;/* CreateSuccessResult(Enum_Message.SUCCESSFULL_CONTACTUS, contactUs);*/
        }
    }
}
