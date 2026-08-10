using DataLayer.Api;
using DataLayer.Api.Model;
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
    public class RebateController : ApiController
    {
        UnitOfWork _context = new UnitOfWork();

        //[HttpGet]
        //public ApiResult Get( string rebateValue)
        //{
        //    ViewAccount currentUser = _context.Account.GetCurrentAccount();
        //    if (currentUser == null)
        //        return ApiRebate.CreateInvalidKeyResult();
        //    return ApiRebate.Get(_context: _context,accountId: currentUser.Id,rebateValue: rebateValue);
        //}
        //[HttpPost]
        //public ApiResult Post(int orderId, string rebateValue)
        //{
        //    ViewAccount currentUser = _context.Account.GetCurrentAccount();
        //    if (currentUser == null)
        //        return ApiRebate.CreateInvalidKeyResult();
        //    return ApiRebate.Post(_context, currentUser.Id, orderId, rebateValue);
        //}

        //[HttpPut]
        //public ApiResult Put(string rebateMobile)
        //{
        //    return ApiRebate.SendSms(_context, rebateMobile);
        //}

        //[HttpDelete]
        //public ApiResult Delete(int orderId)
        //{
        //    return ApiRebate.Delete(_context, orderId);
        //}

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
