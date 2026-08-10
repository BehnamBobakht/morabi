using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Enumarables;
using Persia;
using RestSharp.Extensions;

namespace DataLayer.Base
{
    public static class BasePersian
    {
        public static bool IsPersian(this DateTime datetime)
        {
            if (datetime.Year < 1500)
                return true;
            return false;
        }

        public static string ToPersian(this string str)
        {
            var currentLang = BaseLanguage.GetCurrentLanguage().ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
            {
                return (string.IsNullOrEmpty(str) == false) ? Number.ConvertToPersian(str) : "";
            }
            return str;
        }

        public static string ToPersian(this int input)
        {
            var currentLang = BaseLanguage.GetCurrentLanguage().ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
            {
                return Number.ConvertToPersian(input);
            }
            return input.ToString();
        }

        public static string ToPersian(this int? input)
        {
            var currentLang = BaseLanguage.GetCurrentLanguage().ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
            {
                return input == null ? 0.ToPersian() : input.Value.ToPersian();
            }
            return input == null ? 0.ToPersian() : input.Value.ToString();
        }

        public static string ToPersianComplete(this DateTime? datetime)
        {
            if (datetime == null)
                return "";
            return datetime.Value.ToPersianComplete();
        }

        public static string ToPersianComplete(this DateTime datetime)
        {
            //میلادی به شمسی 
            var currentLang = BaseLanguage.GetCurrentLanguage().ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
            {
                return Calendar.ConvertToPersian(datetime).Weekday;
            }
            return datetime.ToString();
        }
        public static string ToComplete(this DateTime datetime)
        {
            //میلادی به شمسی 
            var currentLang = BaseLanguage.GetCurrentLanguage().ToString();
            
            return datetime.ToString();
        }
        public static string ToPersianWeakDayComplete(this DateTime datetime)
        {
            var temp = datetime.DayOfWeek;
            string day = "";
            switch (temp)
            {
                case DayOfWeek.Sunday:
                    day = "یک شنبه";
                    break;
                case DayOfWeek.Monday:
                    day = "دو شنبه";
                    break;
                case DayOfWeek.Tuesday:
                    day = "سه شنبه";
                    break;
                case DayOfWeek.Wednesday:
                    day = "چهار شنبه";
                    break;
                case DayOfWeek.Thursday:
                    day = "پنج شنبه";
                    break;
                case DayOfWeek.Friday:
                    day = "جمعه";
                    break;
                case DayOfWeek.Saturday:
                    day = " شنبه";
                    break;
                default:
                    day = "شنبه";
                    break;
            }
            return day;
        }

        public static string ToPersianWithTime(this DateTime? datetime)
        {
            if (datetime == null)
                return "";
            return datetime.Value.ToPersianWithTime();
        }

        public static string ToPersianWithTime(this DateTime datetime)
        {
            return Calendar.ConvertToPersian(datetime).Simple + " " + datetime.Hour.ToPersian() + ":" + datetime.Minute.ToPersian();
        }
        public static string ToPersianWithjustHourMinets(this DateTime datetime)
        {
            return datetime.Hour.ToPersian() + ":" + datetime.Minute.ToPersian();
        }
        public static string ToPersianWithjustDay(this DateTime datetime)
        {
            System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
            return pc.GetDayOfMonth(datetime).ToString();
        }
        public static string ToPersianWithjustYear(this DateTime datetime)
        {
            System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
            return pc.GetYear(datetime).ToString();
        }
        public static string ToPersianWithjustMonth(this DateTime datetime)
        {
            System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
            return pc.GetMonth(datetime).ToString();
        }
        public static string ToPersianWithjustMonthComplete(this DateTime datetime)
        {
            System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
            var temp = pc.GetMonth(datetime);
            string month = "";
            switch (temp)
            {
                case 1:
                    month = "فروردین";
                    break;
                case 2:
                    month = "اردیبهشت";
                    break;
                case 3:
                    month = "خرداد";
                    break;
                case 4:
                    month = "تیر";
                    break;
                case 5:
                    month = "مرداد";
                    break;
                case 6:
                    month = "شهریور";
                    break;
                case 7:
                    month = " مهر";
                    break;
                case 8:
                    month = "آبان";
                    break;
                case 9:
                    month = "آذر";
                    break;
                case 10:
                    month = "دی";
                    break;
                case 11:
                    month = "بهمن";
                    break;
                case 12:
                    month = "اسفند";
                    break;
                default:
                    month = "";
                    break;
            }
            return month;
        }

        public static string ToPersian(this DateTime? datetime)
        {
            if (datetime == null)
                return "";
            return datetime.Value.ToPersian();
        }

        public static string ToPersian(this DateTime datetime)
        {
            if (datetime != default(DateTime))
                return Number.ConvertToLatin(Calendar.ConvertToPersian(datetime).Simple.ToString());
            return "";
        }

        public static string ToPersianGiv(this DateTime datetime)
        {
            string temp = "";
            if (datetime != default(DateTime))
            {
                temp += Calendar.ConvertToPersian(datetime).ArrayType[0];
                temp += Calendar.ConvertToPersian(datetime).ArrayType[1].ToString("00");
                temp += Calendar.ConvertToPersian(datetime).ArrayType[2].ToString("00");
            }
            return Number.ConvertToLatin(temp);
        }
        public static string ToPersianChar(this string chare)
        {
            if (chare.HasValue())
            {
                string temp = chare.Replace("ي", "ی").Replace("ك", "ک");
                return temp;
            }
            else
            {
                return "";
            }
        }
        public static string ToPersianNumber(this string number)
        {
            
                string temp = number.Replace("10", "ده").
                Replace("11", "یازده").Replace("12", "دوازده").Replace("13", "سیزده").
                Replace("14", "چهارده").Replace("15", "پانزده").Replace("16", "شانزده").
                Replace("17", "هفده").Replace("18", "هجده").Replace("19", "نوزده").Replace("20", "بیست").Replace("0", "صفر")
                .Replace("/1", " و یک دهم ").
                Replace("/2", " و دو دهم").Replace("/3", " و سه دهم ").Replace("/4", " و چهار دهم ").
                Replace("/5", " و نیم ").Replace("/6", " و شش دهم ").Replace("/7", " و هفت دهم ").
                Replace("/8", " وهشت دهم ").Replace("/9", " و نه دهم ")
                .Replace("1", "یک").
                Replace("2", "دو").Replace("3", "سه").Replace("4", "چهار").
                Replace("5", "پنج").Replace("6", "شش").Replace("7", "هفت").
                Replace("8", "هشت").Replace("9", "نه");
                return temp;
           
        }
        public static string GetEnglish(this string number)
        {
            if (number == null)
                return "";
            return Number.ConvertToLatin(number);
        }
        public static string ToJustTime(this DateTime datetime)
        {
            return datetime.Hour.ToPersian() + ":" + datetime.Minute.ToPersian();
        }

        public static string ToDateLanguage(this DateTime datetime)
        {
            var currentLang = BaseLanguage.GetCurrentLanguage().ToString();
            if (datetime != default(DateTime))
            {
                if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                {
                    return Number.ConvertToLatin(Calendar.ConvertToPersian(datetime).Simple.ToString());
                }
                else
                {
                    return datetime.Month + "/" + datetime.Day + "/" + datetime.Year;
                }
            }
            return "";
        }
    }
}
