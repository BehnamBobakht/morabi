using DataLayer.Entities;
using DataLayer.Enumarables;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
//using System.Web.Script.Serialization;

namespace DataLayer.Api
{
    public class ApiRequest
    {
        public static ApiResult CreateApiRequest<T>(WebsiteDomain domain, Enum_RequestMethod method, Enum_Api api, List<ApiUrlParameter> urlList = null) where T : new()
        {
            Method sharpMethod = GetMethod(method);
            var client = new RestClient(domain.Domain);
            var request = new RestRequest(api.ToString(), sharpMethod);
            if (urlList != null)
            {
                foreach (ApiUrlParameter item in urlList)
                    request.AddQueryParameter(item.Name, item.Value);
            }

            string header_key_value = WebConfigurationManager.AppSettings["HEADER_KEY_VALUE"];
            if (header_key_value != null)
            {
                request.AddHeader("KEY", header_key_value);
            }

            try
            {
                IRestResponse response = client.Execute(request);
                ApiResult result = JsonConvert.DeserializeObject<ApiResult>(response.Content);
                if (result.Code == ApiResult.ResponseCode.Success)
                {
                    if (result.Value != null)
                    {
                        T obj = JsonConvert.DeserializeObject<T>(result.Value.ToString());
                        result.Value = obj;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                return new ApiResult()
                {
                    Code = ApiResult.ResponseCode.Exception,
                    Message = ex.Message,
                    Value = null
                };
            }
        }

        public static ApiResult CreateApiRequest<T>(string domain, Enum_RequestMethod method, Enum_Api api, List<ApiHeaderParameter> headerList = null, List<ApiUrlParameter> urlList = null, object entity = null) where T : new()
        {
            try
            {
                Method sharpMethod = GetMethod(method);
                string apiString = api == Enum_Api.NONE ? "" : api.ToString();
                var client = new RestClient(domain);
                var request = new RestRequest(apiString, sharpMethod);
                if (urlList != null)
                {
                    foreach (ApiUrlParameter item in urlList)
                        request.AddQueryParameter(item.Name, item.Value);
                }
                if (headerList != null)
                {
                    foreach (ApiHeaderParameter item in headerList)
                        request.AddHeader(item.Name, item.Value);
                }
                if (entity != null)
                {
                    request.RequestFormat = DataFormat.Json;
                    request.AddBody(entity);
                }
                IRestResponse response = client.Execute(request);
                ApiResult result = new ApiResult();
                try
                {

                    result = JsonConvert.DeserializeObject<ApiResult>(response.Content);

                    if (result.Code == ApiResult.ResponseCode.Success)
                    {
                        if (result.Value != null)
                        {
                            T obj = JsonConvert.DeserializeObject<T>(result.Value.ToString());
                            result.Value = obj;
                        }
                        return result;
                    }
                    else
                    {
                        T obj = JsonConvert.DeserializeObject<T>(response.Content);
                        result.Value = obj;
                        return result;
                    }

                }
                catch
                {
                    return result;
                }

            }
            catch (Exception ex)
            {
                return new ApiResult()
                {
                    Code = ApiResult.ResponseCode.Exception,
                    Message = ex.InnerException != null ? ex.InnerException.Message : ex.Message
                };
            }
        }

        private static Method GetMethod(Enum_RequestMethod method)
        {
            Method sharpMethod = Method.GET;
            switch (method)
            {
                case Enum_RequestMethod.GET:
                    sharpMethod = Method.GET;
                    break;
                case Enum_RequestMethod.POST:
                    sharpMethod = Method.POST;
                    break;
                case Enum_RequestMethod.PUT:
                    sharpMethod = Method.PUT;
                    break;
                case Enum_RequestMethod.DELETE:
                    sharpMethod = Method.DELETE;
                    break;
                default:
                    break;
            }
            return sharpMethod;
        }
    }
}
