using DataLayer.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cms.Controllers.Api
{
    public class LanguageController : ApiController
    {

        [HttpGet]
        public ApiResult Get(string name)
        {
            var value = Resource.Helper.GetNotify(name);
            if (string.IsNullOrEmpty(value))
                value = Resource.Helper.GetLang(name);
            if (string.IsNullOrEmpty(value))
                value = name;
            return ApiResponse.CreateSuccessResult(value);
        }
    }
}
