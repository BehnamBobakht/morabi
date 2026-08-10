using DataLayer.Api;
using DataLayer.Api.Model;
using DataLayer.Entities;
using DataLayer.Helpers.Google;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cms.Controllers.Api
{
    public class TrackingController : ApiController
    {
        UnitOfWork _context = new UnitOfWork();

        [HttpPost]
        public ApiResult Post(string code, string recaptcha = null)
        {
            ApiResult result = new ApiResult();
            if (Recaptcha.Check(recaptcha) == false)
                result = ApiResponse.CreateErrorResult(DataLayer.Enumarables.Enum_Message.INVALID_RECAPTCHA);
            else
                result = ApiOrderTracking.Post(_context, code);
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
