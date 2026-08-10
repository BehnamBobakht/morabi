using DataLayer.Entities;
using DataLayer.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.ViewModels;
using DataLayer.Api.Model;
using System.Web;

namespace DataLayer.Base
{
    public static class BaseCookie
    {

        public static void SetCookie(string key, string value, int expireDays)
        {
            if(!string.IsNullOrEmpty(value))
            {
            HttpCookie encodedCookie = new HttpCookie(key, value);

            if (HttpContext.Current.Request.Cookies[key] != null)
            {
                var cookieOld = HttpContext.Current.Request.Cookies[key];
                cookieOld.Expires = DateTime.Now.AddDays(expireDays);
                cookieOld.Value = encodedCookie.Value;
                HttpContext.Current.Response.Cookies.Add(cookieOld);
            }
            else
            {
                encodedCookie.Expires = DateTime.Now.AddDays(expireDays);
                HttpContext.Current.Response.Cookies.Add(encodedCookie);
            }
            }

        }
        public static string GetCookie(string key)
        {
            string value = null;
            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];

            if (cookie != null)
            {
                value = cookie.Value;
            }
            return value;
        }
    }
}
