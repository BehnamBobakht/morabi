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
    public class PostSearchController : ApiController
    {
        UnitOfWork _context = new UnitOfWork();


         [HttpGet]
        public ApiResult Get(   
             string categoryLabel,
            int? notId = null,
            int? categoryId = null,
            int? typeId = null,
            int index = 1,
            int pageSize = 10,
            string keyword = null,
            string name = null,
            Enum_PostOrder order = Enum_PostOrder.NONE
            )
        {
            ApiResult result = ApiPost.Search(
                _context: _context,
                index: index,
                pageSize: pageSize,
                 notId: notId,
                              categoryId: categoryId,
                              categoryLabel: categoryLabel,
                              typeId: typeId,
                              keyword: keyword,
                              name: name,
                              order:order,
                              active: true);
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
