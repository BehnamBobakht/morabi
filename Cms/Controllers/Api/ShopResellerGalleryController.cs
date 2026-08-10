using DataLayer.Api;
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
    public class ShopResellerGalleryController : ApiController
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
            return ApiShopResellerGallery.Get(_context, reseller.ID);
        }

        [HttpPost]
        public ApiResult Post(ShopResellerGallery gallery)
        {
            SiteUser user = BaseWebsite.CurrentSiteUser;
            if (user == null)
                return ApiResponse.CreateInvalidKeyResult();
            ShopReseller reseller = _context.ShopReseller.GetByUserId(user.ID);
            if (reseller == null)
                return ApiResponse.CreateInvalidKeyResult();
            gallery.ResellerId = reseller.ID;
            ApiResult result = ApiShopResellerGallery.Post(_context, gallery);
            if (result.Code == ApiResult.ResponseCode.Success)
                return Get();
            else
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
