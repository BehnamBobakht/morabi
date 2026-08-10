using DataLayer.Api;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataLayer.Api.Model;

namespace Cms.Controllers.Api
{
    public class StoreController : ApiController
    {
        UnitOfWork _context = new UnitOfWork();

       
        [HttpGet]
        public ApiResult Get(
            int? index = null,
            int? pageSize = null,
            string name = null,
            string custom = null, bool active=true)
        {
            ApiResult result = ApiStore.Search(
                _context: _context, 
                index: index, 
                pageSize: pageSize,
                name: name,
                active:active
                );
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
