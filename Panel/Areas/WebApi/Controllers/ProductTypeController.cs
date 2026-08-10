using DataLayer.Api;
using DataLayer.Api.Model;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Panel.Areas.WebApi.Controllers
{
    public class ProductTypeController : ApiController
    {
        private UnitOfWork _context = new UnitOfWork();

        [HttpPost]
        public ApiResult Post(ProductType productType)
        {
            return ApiProductType.Post(_context, productType);
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
