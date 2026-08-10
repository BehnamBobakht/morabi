using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer.Api;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace Cms.Controllers.Api
{
    public class ProductQuantityController : ApiController
    {
        UnitOfWork _context = new UnitOfWork();

        [HttpGet]
        public ApiResult GetProductByColorSize(int productId, int? colorId = null, int? sizeId = null,int? optionItemId=null)
        {
            ProductQuantity quantity = _context.ProductQuantity.GetByProductIdAndColorAndSize(productId, colorId, sizeId, optionItemId);
            if (quantity != null)
                return ApiResponse.CreateSuccessResult(quantity.ToProductApi());
            else
            {
                Product product = _context.Product.GetById(productId);
                return ApiResponse.CreateSuccessResult(product.ToApi());
            }
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