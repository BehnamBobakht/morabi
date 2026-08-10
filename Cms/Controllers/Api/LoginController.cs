using DataLayer.Api;
using DataLayer.Api.Model;
using DataLayer.Entities;
using DataLayer.ViewModels;
using DataLayer.Helpers.Google;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataLayer.Base;
using DataLayer.Enumarables;

namespace Cms.Controllers.Api
{
    public class LoginController : ApiController
    {
        UnitOfWork _context = new UnitOfWork();

        [HttpPost]
        public ApiResult Post(Account account, string remember = "false", string recaptcha = null)
        {
 
            bool remembered = Convert.ToBoolean(remember);
            ApiResult result = new ApiResult();
            if (Recaptcha.Check(recaptcha) == false)
                result = ApiResponse.CreateErrorResult(DataLayer.Enumarables.Enum_Message.INVALID_RECAPTCHA);
            else
                result = ApiAccount.Post_DoLogin(_context, account, account.Password, true,password2:account.Password);
            if (result.Code == ApiResult.ResponseCode.Success)
            {
                var viewAccount = (ViewAccount)result.Value;
                _context.Account.SetCurrentAccount(viewAccount, account.Password, remembered);
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
