using DataLayer.Api;
using DataLayer.Api.Model;
using DataLayer.Base;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cms.Controllers.Api
{
    public class ShopResellerCollectionController : ApiController
    {
        UnitOfWork _context = new UnitOfWork();

        [HttpGet]
        public ApiResult Get()
        {
            SiteUser user = BaseWebsite.CurrentSiteUser;
            if (user == null)
                return ApiResponse.CreateInvalidKeyResult();
            ShopReseller reseller = _context.ShopReseller.GetByUserId(user.ID);
            if (reseller == null)
                return ApiResponse.CreateInvalidKeyResult();
            return ApiShopResellerCollection.Get(_context, resellerId: reseller.ID);
        }

        [HttpPost]
        public ApiResult Post(ShopResellerCollection collection)
        {
            SiteUser user = BaseWebsite.CurrentSiteUser;
            if (user == null)
                return ApiResponse.CreateInvalidKeyResult();
            ShopReseller reseller = _context.ShopReseller.GetByUserId(user.ID);
            if (reseller == null)
                return ApiResponse.CreateInvalidKeyResult();
            collection.ResellerId = reseller.ID;
            return ApiShopResellerCollection.Post(_context, collection);
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
