using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Base
{
    public static class BaseConvert
    {
        public static string GetCurrencyFormat(this double value)
        {
            return (value).ToString("N0").ToString();
        }
        public static string GetCurrencyFormat(this int value)
        {
            return (value).ToString("N0").ToString();
        }
        public static string GetCurrencyFormat(this double? value)
        {
            return value == null ? "0" : value.Value.GetCurrencyFormat();
        }

        public static int GetInteger(this string str)
        {
            if (str.IsInteger())
                return int.Parse(str);
            return 0;
        }



        public static Guid GetGuid(this string str)
        {
            if (str.IsGuid())
                return Guid.Parse(str);
            return default(Guid);
        }

        public static bool IsInteger(this string str)
        {
            int a = 0;
            return int.TryParse(str, out a);
        }


        public static bool IsGuid(this string str)
        {
            Guid a = new Guid();
            return Guid.TryParse(str, out a);
        }

        public static bool IsBoolean(this string input)
        {
            bool n;
            return bool.TryParse(input, out n);
        }

        public static bool IsLong(this string input)
        {
            long n;
            return long.TryParse(input, out n);
        }





        public static string ToEnglishDigit(this string input)
        {
            if(!string.IsNullOrEmpty(input))
            {
                input= input.Replace('۰', '0').Replace('۱', '1').Replace('۲', '2').Replace('۳', '3').Replace('۴', '4').Replace('۵', '5').Replace('۶', '6').Replace('۷', '7').Replace('۸', '8').Replace('۹', '9');
            }
            return input;
        }

    }
}
