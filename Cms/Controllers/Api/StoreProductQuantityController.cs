using DataLayer.Api;
using DataLayer.Entities;
using DataLayer.Base;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataLayer.ViewModels;

namespace Cms.Controllers.Api
{
    public class StoreProductQuantityController : ApiController
    {
        UnitOfWork _context = new UnitOfWork();

        [HttpGet]
        public ApiResult Get(int productId, int? colorId = null, int? sizeId = null, int? optionItemId = null,int count=1)
        {
            var variety = BaseStore.GetStoreProductVariety(_context, productId, colorId, sizeId, optionItemId,count);
            if (BaseWebsite.WebsiteSetting.HasStore)
            {
                if (!((List<ViewStoreProductQuantity>)variety.ProductQuantity).Any())
                {
                    return ApiResponse.CreateErrorResult(Enum_Message.LIST_EMPTY);
                }
            }
            else
            {
                if (!((List<ViewProductQuantity>)variety.ProductQuantity).Any())
                {
                    return ApiResponse.CreateErrorResult(Enum_Message.LIST_EMPTY);
                }
            }
            return ApiResponse.CreateSuccessResult(variety);
        }
    }
}
