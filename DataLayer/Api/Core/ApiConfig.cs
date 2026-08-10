using Newtonsoft.Json;
using System;
using System.Linq;
using System.Web;

namespace DataLayer.Api
{
    public class ApiConfig
    {
        public const string KEY_VALUE_KETABLOVE = "KETABLOVEKEYCODE";
        public const string KEY_VALUE_KIFKETAB = "KIFKETABKEYCODE";
        public const bool DEBUG_MODE = true;

        public class Request
        {
            public static bool ValidationRequest(out string target)
            {
                try
                {
                    string key = HeaderForm.GetStringHeaderValue(HeaderForm.HEADER_KEY);

                    if (key == KEY_VALUE_KETABLOVE)
                    {
                        target = KEY_VALUE_KETABLOVE;
                        return true;
                    }
                    else if (key == KEY_VALUE_KIFKETAB)
                    { 
                        target = KEY_VALUE_KIFKETAB;
                        return true;
                    }
                    else
                    {
                        target = null;
                        return false;
                    }
                }
                catch (Exception ee)
                {
                    throw ee;
                }
            }
        }

        public class HeaderForm
        {
            public const string HEADER_KEY = "KEY";
            public static string GetStringHeaderValue(string key)
            {
                try
                {
                    string val = HttpContext.Current.Request.Headers.GetValues(key).First();
                    return val;
                }
                catch (Exception)
                {
                    return "";
                }
            }
            
            public static int GetIntHeaderValue(string key)
            {
                try
                {
                    string val = HttpContext.Current.Request.Headers.GetValues(key).First();
                    return int.Parse(val);
                }
                catch (Exception)
                {
                    return -1;
                }
            }
            
            public static long GetLongHeaderValue(string key)
            {
                try
                {
                    string val = HttpContext.Current.Request.Headers.GetValues(key).First();
                    return long.Parse(val);
                }
                catch (Exception)
                {
                    return -1;
                }
            }

            public static DateTime GetDateTimeHeaderValue(string key)
            {
                try
                {
                    string val = HttpContext.Current.Request.Headers.GetValues(key).First();
                    return DateTime.Parse(val);
                }
                catch (Exception)
                {
                    return DateTime.MinValue;
                }
            }
        }
        
        public class Gson
        {
            public static string ToJson(object obj)
            {
                return JsonConvert.SerializeObject(obj, new JsonSerializerSettings() { DateFormatString = "yyyy-MM-dd HH:mm:ss" });
            }
        }
    }
}