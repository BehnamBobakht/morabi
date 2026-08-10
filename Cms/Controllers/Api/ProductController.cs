using DataLayer.Api;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cms.Controllers.Api
{
    public class ProductController : ApiController
    {
        UnitOfWork _context = new UnitOfWork();

        [HttpGet]
        public ApiResult Get(int id)
        {
            Product product = _context.Product.GetById(id);
            return ApiResponse.CreateSuccessResult(product.ToApi());
        }

        [HttpGet]
        public ApiResult Get(int? notId = null,
            int? index = null,
            int? pageSize = null,
            string shopId = null,
            string typeId = null,
            string categoryId = null,
            string subCategoryId = null,
            string brandId = null,
            string name = null,
            string custom = null,
            int? pricefrom = null,
            int? priceto = null,
            bool? isService = null,
            bool activeLocation = false,
            bool exteraResult = false,

            Enum_ProductOrder order = Enum_ProductOrder.MORE_VISIT)
        {
            ApiResult result = new ApiResult();
            index = index.HasValue ? index.Value : 1;
            pageSize = pageSize.HasValue ? pageSize.Value : 8;
            result.Value = _context.Product.SearchVm(
                index: index.Value,
                pageSize: pageSize.Value,
                notId: notId,
                typeId: typeId,
                categoryId: categoryId,
                brandId: brandId,
                custom: custom,
                name: name,
                order: order,
                pricefrom: pricefrom,
                priceto: priceto,
                subCategoryId: subCategoryId,
                isService: isService
                );
            result.Code = ApiResult.ResponseCode.Success;
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
