using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Panel
{
    public class BaseRouteView
    {
        public static String GetStringValue(HttpRequestBase request, string par)
        {
            object obj = GetRouteValue(request, par);
            obj = obj == null ? request.Params[par] : obj;
            return obj != null ? obj.ToString() : null;
        }

        private static object GetRouteValue(HttpRequestBase request, string par)
        {
            object obj = request.RequestContext.RouteData.Values[par];
            return obj;
        }
    }
}