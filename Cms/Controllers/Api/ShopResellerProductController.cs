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
    public class ShopResellerProductController : ApiController
    {
        UnitOfWork _context = new UnitOfWork();

        [HttpGet]
        public ApiResult Get()
        {
            ApiResult result = new ApiResult();
            SiteUser CurrentUser = _context.SiteUser.GetCurrentUser();
            if (CurrentUser == null)
                return ApiResponse.CreateInvalidKeyResult();
            ShopReseller shopreseller = _context.ShopReseller.GetByUserId(CurrentUser.ID);
            if (shopreseller == null)
                return ApiResponse.CreateInvalidKeyResult();
            return ApiShopResellerProduct.Get(_context, shopreseller.ID);
        }

        [HttpPost]
        public ApiResult Post(List<ShopResellerProduct> list)
        {
            ApiResult result = new ApiResult();
            SiteUser CurrentUser = _context.SiteUser.GetCurrentUser();
            if (CurrentUser == null)
                return ApiResponse.CreateInvalidKeyResult();
            ShopReseller shopreseller = _context.ShopReseller.GetByUserId(CurrentUser.ID);
            if (shopreseller == null)
                return ApiResponse.CreateInvalidKeyResult();
            return ApiShopResellerProduct.Post(_context, list, shopreseller.ID);
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
