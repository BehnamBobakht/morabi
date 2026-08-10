using DataLayer.Base;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicDate
{
    public static class Helper
    {
        public static string ConvertToPersianDateTime(this DateTime datetime)
        {
            PersianCalendar pc = new PersianCalendar();

            int year = pc.GetYear(datetime);
            int month = pc.GetMonth(datetime);
            int day = pc.GetDayOfMonth(datetime);
            int hour = datetime.Hour;
            int minute = datetime.Minute;

            return $"{year:0000}/{month:00}/{day:00} {hour}:{minute}";
        }
        public static string ConvertToPersianDate(this DateTime datetime)
        {
            PersianCalendar pc = new PersianCalendar();

            int year = pc.GetYear(datetime);
            int month = pc.GetMonth(datetime);
            int day = pc.GetDayOfMonth(datetime);
           

            return $"{year:0000}/{month:00}/{day:00}";
        }
        public static string ToPersianComplete2(this DateTime datetime)
        {
            var currentLang = BaseLanguage.GetCurrentLanguage().ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
            {
                PersianCalendar pc = new PersianCalendar();
                var year = pc.GetYear(datetime);
                var month = pc.GetMonth(datetime);
                var day = pc.GetDayOfMonth(datetime);
                var dayOfWeek = pc.GetDayOfWeek(datetime);

                string[] persianWeekDays = {
            "یکشنبه", "دوشنبه", "سه‌شنبه", "چهارشنبه", "پنج‌شنبه", "جمعه", "شنبه"
               };

                return $"{persianWeekDays[(int)dayOfWeek]} {year:0000}/{month:00}/{day:00}";
            }

            return datetime.ToString("yyyy/MM/dd");
        }
    }
}
