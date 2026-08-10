using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Base
{
    public static class BaseString
    {
        public static string GetSubString(this string text, int count = 150, bool hasDot = false)
        {
            if (string.IsNullOrEmpty(text) == false)
            {
                if (text.Length > count)
                {
                    text = text.Substring(0, count);
                    if (hasDot)
                    {
                        text += "...";
                    }
                }

                return text;
            }
            else
                return "";
        }


    }
}
