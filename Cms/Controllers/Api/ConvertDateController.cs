using DataLayer.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using DataLayer.Entities;
using DataLayer.Base;
using System.Globalization;

namespace Cms.Controllers.Api
{

    public class ConvertDateController : ApiController
    {
        // GET: ConvertDate

        [HttpGet]
        public ApiResult Get(string transformerType = "", string day = "", string month = "", string year = "")
        {
            string dateTeransfer = year + "/" + month + "/" + day;

            if (transformerType == "shamsiToMiladi")
            {
                //شمسی به میلادی
                DateTime date = BaseDate.GetGregorian(new CustomDate(DateTime.Parse(dateTeransfer))).GetValueOrDefault();
                return ApiResponse.CreateSuccessResult(date);
            }
            else
            {
                // میلادی به شمسی
                var date = dateTeransfer.GetGregorian();
                return ApiResponse.CreateSuccessResult(date.ToPersian());
            }
        }
    }
}