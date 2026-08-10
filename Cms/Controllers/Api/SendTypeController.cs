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
    public class SendTypeController : ApiController
    {
        UnitOfWork _context = new UnitOfWork();

        [HttpGet]
        public ApiResult Get(int? storeId, int orderPrice)
        {
            SendType shipping = _context.SendType.GetByStoreId(storeId);
            var result = new ViewSendType(shipping);
            result.CurrentPrice = _context.SendType.GetShippingPrice(storeId, orderPrice);
            return ApiResponse.CreateSuccessResult(result);
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
