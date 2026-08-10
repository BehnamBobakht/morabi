using DataLayer.Api;
using DataLayer.Api.Model;
using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cms.Controllers.Api
{
    public class AccountAddressController : ApiController
    {
        UnitOfWork _context = new UnitOfWork();

        [HttpGet]
        public ApiResult Get()
        {
            ViewAccount account = _context.Account.GetCurrentAccount();
            if (account != null)
                return ApiAccountAddress.Get(_context, account.Id);
            else
                return ApiResponse.CreateInvalidKeyResult();
        }
        [HttpGet]
        public ApiResult Get(int addressId)
        {
            ViewAccount account = _context.Account.GetCurrentAccount();
            if (account != null)
                return ApiAccountAddress.Get(_context, account.Id, addressId);
            else
                return ApiResponse.CreateInvalidKeyResult();
        }
        [HttpPost]
        public ApiResult Post(AccountAddress address, bool returnAddresses = true)
        {
            ViewAccount account = _context.Account.GetCurrentAccount();
            if (account != null)
                return ApiAccountAddress.Post(_context, account.Id, address, returnAddresses);
            else
                return ApiResponse.CreateInvalidKeyResult();
        }

        [HttpPut]
        public ApiResult Put(AccountAddress address)
        {
            var websiteSetting = BaseWebsite.WebsiteSetting;
                      
            ViewAccount account = _context.Account.GetCurrentAccount();
            if (account != null)
                return ApiAccountAddress.Put(_context, account.Id, address);
            else
                return ApiResponse.CreateInvalidKeyResult();
        }

        [HttpDelete]
        public ApiResult Delete(int addressId)
        {
            ViewAccount account = _context.Account.GetCurrentAccount();
            if (account != null)
                return ApiAccountAddress.Delete(_context, account.Id, addressId);
            else
                return ApiResponse.CreateInvalidKeyResult();
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
