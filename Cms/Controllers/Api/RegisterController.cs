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
    public class RegisterController : ApiController
    {
        UnitOfWork _context = new UnitOfWork();

        [HttpPost]
        public ApiResult Post(Account account, bool remember = false, string confirm = "", string captcha = "", string bothEmailMobile = "false")
        {
            Boolean remembered = Convert.ToBoolean(remember);

            if (System.Web.HttpContext.Current.Session["Captcha"] != null && string.IsNullOrEmpty(captcha))
                return ApiResponse.CreateErrorResult(Enum_Message.REQUIRED_ACCOUNT_CAPTCHA);

            if (System.Web.HttpContext.Current.Session["Captcha"] != null && captcha != System.Web.HttpContext.Current.Session["Captcha"].ToString())
                return ApiResponse.CreateErrorResult(Enum_Message.CHECK_ACCOUNT_CAPTCHA);
            string password = account.Password;

            Boolean iswebsiteSetting = DataLayer.Base.BaseWebsite.WebsiteSetting.IsLoginWithNationalCode;

            ApiResult result = ApiAccount.Post_DoRegister(_context, account, true, confirm, bothEmailMobile, iswebsiteSetting);
            if (result.Code == ApiResult.ResponseCode.Success)
            {
                var viewAccount = (ViewAccount)result.Value;
                _context.Account.SetCurrentAccount(viewAccount, password, remembered);
                var currentCartAccount = BaseCart.GetCurrentCartAccount();
                currentCartAccount.UpdateType = CartUpdateType.SetAccountId;
                currentCartAccount.AccountId = viewAccount.Id;
                var cart = BaseCart.MergCart(_context: _context, currentCartAccount);
                BaseCookie.SetCookie(key: CookieName.CardCookie.ToString(), cart.Model.SyncCode, 5);
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
