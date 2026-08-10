using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLayer.Api
{
    public class ApiResult
    {
        public enum ResponseCode : byte
        {
            Exception,
            Success,
            InvalidKey,
            Error
        }

        public ResponseCode Code { get; set; }
        public string Message { get; set; }
        public Object Value { get; set; }
    }
}