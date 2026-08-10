using DataLayer.Api;
using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.ViewModels;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Cms.Controllers.Api
{
    public class UploadDocumentController : ApiController
    {
        UnitOfWork _context = new UnitOfWork();

        [HttpPost]
        public ApiResult Post()
        {
            ViewAccount Account = BaseWebsite.CurrentAccount;
            SiteUser SiteUser = BaseWebsite.CurrentSiteUser;

            if (Account != null || SiteUser != null)
                return ApiUploadDocument.Post(_context, Enum_Code.SYSTEM_TYPE_SHOP);

            return ApiResponse.CreateInvalidKeyResult();
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
