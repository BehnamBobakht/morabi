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
    public class PostController : ApiController
    {
        UnitOfWork _context = new UnitOfWork();


        [HttpGet]
        public ApiResult Get(
            int postid
           
            )
        {
            ApiResult result = ApiPost.Get(
                _context: _context,
                id:postid
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
