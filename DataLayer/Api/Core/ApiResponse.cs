using DataLayer.Base;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Api
{
    public class ApiResponse
    {
        public static ApiResult CreateSuccessResult(Enum_Message msg, object obj = null)
        {
            ApiResult result = new ApiResult()
            {
                Code = ApiResult.ResponseCode.Success,
                Message = BaseMessage.GetMessage(msg).Body
            };
            if (obj != null)
                result.Value = obj;
            //result.Value = obj.ToJson();
            return result;
        }

        public static ApiResult CreateSuccessResult(object obj = null)
        {
            ApiResult result = new ApiResult() {
                Code = ApiResult.ResponseCode.Success,
                Message = BaseMessage.GetMessage(Enum_Message.SUCCESSFULL_API).Body
            };
            if (obj != null)
                result.Value = obj;
            //result.Value = obj.ToJson();
            return result;
        }

        public static ApiResult CreateInvalidKeyResult()
        {
            ApiResult result = new ApiResult()
            {
                Code = ApiResult.ResponseCode.InvalidKey,
                Message = BaseMessage.GetMessage(Enum_Message.INVALID_API_KEY).Body
            };
            return result;
        }
        public static ApiResult CreateInvalidKeyResult(Enum_Message message)
        {
            ApiResult result = new ApiResult()
            {
                Code = ApiResult.ResponseCode.InvalidKey,
                Message = BaseMessage.GetMessage(message).Body
            };
            return result;
        }
        public static ApiResult CreateErrorResult(Enum_Message message)
        {                      
            ApiResult result = new ApiResult()
            {
                Code = ApiResult.ResponseCode.Error,
                Message = BaseMessage.GetMessage(message).Body
            };
            return result;
        }

        public static ApiResult CreateExceptionResult(Exception e)
        {
            ApiResult result = new ApiResult()
            {
                Code = ApiResult.ResponseCode.Exception,
                Message = e.Message
            };
            return result;
        }
    }
}
