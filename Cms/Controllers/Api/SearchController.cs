using DataLayer.Api;
using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Cms.Api;

namespace Cms.Controllers.Api
{

    public class SearchController : ApiController
    {
        UnitOfWork _context = new UnitOfWork();

        [HttpGet]
        public SearchVm Search(string q=null,int size=5)
        {
            var searchModel = new SearchVm() { Q = q.Trim(), Size = size };
           return BaseSearch.Search(_context:_context,search: searchModel);
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
