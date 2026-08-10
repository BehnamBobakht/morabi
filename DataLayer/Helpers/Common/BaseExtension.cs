using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Persia;
using DataLayer.Entities;
using DataLayer.ViewModels;
using Newtonsoft.Json;
using DataLayer.Enumarables;

namespace DataLayer.Base
{
    public static class BaseExtension
    {


        public static string GetImageType(this string url)
        {
            if (url.EndsWith("jpg"))
                return "image/jpeg";
            else if (url.EndsWith("png"))
                return "image/png";
            else if (url.EndsWith("gif"))
                return "image/gif";
            else
                return "";
        }

        public static string GetMaxZero<T>(this List<T> list)
        {
            string MaxZero = "";
            for (int i = 0; i < list.Count.ToString().Length; i++)
                MaxZero += "0";

            return MaxZero;
        }        
    }
}
