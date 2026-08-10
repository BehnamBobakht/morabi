using DataLayer.Api;
using DataLayer.Api.Model;
using DataLayer.Entities;
using DataLayer.Helpers.Google;
using DataLayer.ViewModels;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Text;

namespace Cms.Controllers.Api
{
    public class AccountReagentController : ApiController
    {
        UnitOfWork _context = new UnitOfWork();

        [HttpPost]
        public ApiResult Post(string fullName, string mobile, string email, string recaptcha)
        {
            ViewAccount account = _context.Account.GetCurrentAccount();
            if (account == null)
                return ApiResponse.CreateInvalidKeyResult();

            ApiResult result = new ApiResult();
            if (Recaptcha.Check(recaptcha) == false)
                result = ApiResponse.CreateErrorResult(Enum_Message.INVALID_RECAPTCHA);
            else
                result = ApiAccountReagent.Post_RegisterAccountReagent(_context, account, fullName, mobile, email);

            if (result.Code == ApiResult.ResponseCode.Success)
            {
                string token_1 = account.FullName;
                string token_2 = account.ReagentCode;
                string token_3 = account.ReagentCode;
                string reagentType = Enum_SmsType.REAGENT.ToString();
                SmsType smsType = _context.SmsType.FirstOrDefault(p => p.Label == reagentType);
                if (smsType != null)
                {
                    string body = smsType.Body;
                    body = body.Replace("%token3", token_3);
                    body = body.Replace("%token2", token_2);
                    body = body.Replace("%token", token_1);
                    _context.Sms.SaveNewSms(mobile, Enum_SmsType.REAGENT, body.ToString(), token_1, token_2, token_3);
                    _context.Save();
                }
            }

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
