using DataLayer.Api;
using DataLayer.Api.Model;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cms.Controllers.Api
{
    public class ContactUsController : ApiController
    {
        UnitOfWork _context = new UnitOfWork();

        [HttpPost]
        public ApiResult Post(ContactUs contactUs)
        {
            return ApiContactUs.Post(_context, contactUs);
        }
        [HttpGet]
        public ApiResult getTrackingCode(string TrackingCode)
        {
            return ApiContactUs.GetTrackingCode(_context, TrackingCode);
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
