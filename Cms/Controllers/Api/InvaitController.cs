using DataLayer.Api;
using DataLayer.Api.Model;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using DataLayer.Repositories;

namespace Cms.Controllers.Api
{
    public class InvaitController : ApiController
    {
        UnitOfWork _context = new UnitOfWork();

        [HttpPost]
        public ApiResult Post(string emailOrPhone)
        {
            string emailBody = this.getInvitationEmail();
            UnitOfWork mainContext = new UnitOfWork();
            StringBuilder str_1 = new StringBuilder();
            string token_1 = emailOrPhone;
            string token_2 = "";
            string token_3 = "";
            ApiResult result = new ApiResult();
            if (emailOrPhone == null || emailOrPhone.Trim() == string.Empty)
            {
                return ApiResponse.CreateErrorResult(Enum_Message.REQUIRED_PRODUCT_CUSTOM_ITEM_VALUE);
            }
            else if (emailOrPhone.Contains("@") == true)
            {
                mainContext.Email.SaveNewEmail(emailOrPhone.Trim(), Enum_EmailType.REAGENT, emailBody, "دعوت به خوش کالا");
                return ApiResponse.CreateSuccessResult();
            }
            else if (emailOrPhone.Trim().Substring(0, 2) == "09")
            {
                mainContext.Sms.SaveNewSms(emailOrPhone.Trim(), Enum_SmsType.REAGENT, this.getInvitationSMS(), token_1, token_2, token_3);
                return ApiResponse.CreateSuccessResult();
            }
            else
            {
                return ApiResponse.CreateErrorResult(Enum_Message.INVALID_ACCOUNT_DATA);
            }


        }
        private string getInvitationSMS()
        {
            string str = "دعوت به خوش کالا" + "\r\n" +
                         "خوشحال می شویم از سایت خوش کالا دیدن فرمایید" + "\r\n" +
                         "https://khoshkala.com/";
            //string str = "دعوت";
            return str ;
        }
        private string getInvitationEmail()
        {
            StringBuilder html = new StringBuilder();
            try
            {
                html.Append("<b>دعوت به خوش کالا </b>" + "<br /><br />");
                html.Append("<b>خوشحال می شویم از سایت خوش کالا دیدن فرمایید </b>" + "<br /><br />");
                html.Append("https://khoshkala.com/" + "<br />");
                html.Append("<hr />");
            }
            catch (Exception) { }
            return html.ToString();
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
