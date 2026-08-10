using DataLayer.Entities;
using DataLayer.Api;
using DataLayer.Api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cms.Controllers.Api
{
    public class ShopFollowController : ApiController
    {
        UnitOfWork _context = new UnitOfWork();
        
        [HttpPost]
        public ApiResult Post(int shopId)
        {
            return ApiShopFollow.Post(_context, shopId);
        }

        [HttpDelete]
        public ApiResult Delete(int shopId)
        {
            return ApiShopFollow.Delete(_context, shopId);
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
