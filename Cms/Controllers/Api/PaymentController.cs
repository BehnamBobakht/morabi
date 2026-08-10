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
    public class PaymentController : ApiController
    {
        UnitOfWork _context = new UnitOfWork();

        [HttpPost]
        public ApiResult Post(ViewPayment payment)
        {
            ViewAccount account = _context.Account.GetCurrentAccount();
            if (account != null)
                return ApiPayment.Post(_context, account.Id, null, payment);
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
