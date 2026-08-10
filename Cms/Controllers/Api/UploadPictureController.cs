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
using System.Web.Http;

namespace Cms.Controllers.Api
{
    public class UploadPictureController : ApiController
    {
        UnitOfWork _context = new UnitOfWork();
        [HttpPost]
        public ApiResult Post(int pictureId)
        {
            ViewAccount account = _context.Account.GetCurrentAccount();
            int? accountID = account != null ? account.Id : default(int?);

            if (accountID == null)
                return ApiResponse.CreateInvalidKeyResult();

            account.PictureID = pictureId;
            ApiResult result = new ApiResult();
            result = ApiAccount.Put_Account(_context, account, account.Id);
            //if (result.Code == ApiResult.ResponseCode.Success)
            //    _context.Account.SetCurrentAccount((ViewAccount)result.Value, account.Password, true);
            return result;


        }
        [HttpPost]
        public ApiResult Post()
        {
            ViewAccount Account = BaseWebsite.CurrentAccount;
            SiteUser SiteUser = BaseWebsite.CurrentSiteUser;

            if (Account != null || SiteUser != null)
                return ApiUploadPicture.Post(_context, Enum_Code.SYSTEM_TYPE_SHOP);

            return ApiResponse.CreateInvalidKeyResult();
        }


        [HttpDelete]
        public ApiResult Delete()
        {
            ViewAccount account = _context.Account.GetCurrentAccount();
            int? accountID = account != null ? account.Id : default(int?);
            if (accountID == null)
                return ApiResponse.CreateInvalidKeyResult();
            account.PictureID = null;
            ApiResult result = new ApiResult();

            if (account != null)
                result = ApiAccount.Put_Account(_context, account, account.Id);
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
