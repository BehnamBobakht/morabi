using DataLayer.Api;
using DataLayer.Api.Model;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Cms.Controllers.Api
{
    public class ProductNotifyController : ApiController
    {
        UnitOfWork _context = new UnitOfWork();

        [HttpPost]
        public ApiResult Post(int productId, int? colorId = null, int? sizeId = null)
        {
            return ApiProductNotify.Post(_context, productId, colorId, sizeId);
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