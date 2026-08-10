using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Base
{
    public static class BaseDate
    {
        //میلادی به شمسی 
        public static DateTime? GetGregorian(this string date)
        {
            if (string.IsNullOrEmpty(date) == false)
            {
                CustomDate temp = new CustomDate(date);
                return GetGregorian(temp);
            }
            else
                return null;
        }

        public static DateTime? GetGregorian(this DateTime? date)
        {
            if (date != null)
            {
                CustomDate temp = new CustomDate(date.Value);
                return BaseDate.GetGregorian(temp);
            }
            else
                return null;
        }



        public static DateTime? GetGregorian(CustomDate date)
        {
         
         
            if (date.Year == 0)
                return null;
            if (date.Year < 1500)
                return Persia.Calendar.ConvertToGregorian(date.Year, date.Month, date.Day, Persia.DateType.Persian);
            else
                return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
        }

        public static string GetTimeString(this TimeSpan time)
        {
            string output = time.Hours.ToPersian();
            if (time.Minutes != 0)
                output += ":" + time.Minutes.ToPersian();
            return output;
        }


        public static string GetDateString(this DateTime datetime)
        {
            return datetime.Year + "-" + datetime.Month + "-" + datetime.Day + " " + datetime.Hour + ":" + datetime.Minute + ":" + datetime.Second;
        }
        public static string GetDateString2(this DateTime datetime)
        {
           

            int m = datetime.Month;
            int d = datetime.Day;
            string m2 = m.ToString();
            string d2 = d.ToString();
            if (m < 10)
            {
                m2 = "0" + m;
            }
            if (d < 10)
            {
                d2 = "0" + d;
            }
            return datetime.Year + "-" + m2 + "-" + d2 ;
        }
        public static string GetTimeString(this DateTime datetime)
        {
            return  datetime.Hour + ":" + datetime.Minute ;
        }
        public static string GetDateStringOnly(this DateTime datetime)
        {
            return " " + datetime.DayOfWeek+" "+ datetime.Year + "." + datetime.Month + "." + datetime.Day  ;
        }
        public static string GetDateByMonth(this DateTime datetime)
        {
            string fullMonthName = new DateTime(datetime.Year, datetime.Month, 1).ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));
            return " " + datetime.Day+ " "+ fullMonthName + " " + datetime.Year ;
        }

        public static string GetDateString(this DateTime? datetime)
        {
            if (datetime == null)
                return "1880-01-01";
            else
            {
                DateTime dt = datetime.Value;
                return dt.GetDateString();
            }
        }

        public static string GetPersianMonth(int MonthNumber)
        {
            switch (MonthNumber)
            {
                case 1:
                    return "فروردین";
                case 2:
                    return "اردیبهشت";
                case 3:
                    return "خرداد";
                case 4:
                    return "تیر";
                case 5:
                    return "مرداد";
                case 6:
                    return "شهریور";
                case 7:
                    return "مهر";
                case 8:
                    return "آبان";
                case 9:
                    return "آذر";
                case 10:
                    return "دی";
                case 11:
                    return "بهمن";
                case 12:
                    return "اسفند";
                default:
                    return "";
            }
        }
    }

    public class CustomDate
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public CustomDate() { }
        public CustomDate(DateTime date)
        {
            if (date.IsPersian() == false)
            {
                string temp = date.ToPersian();
                temp = Persia.Number.ConvertToLatin(temp);
                string[] split = temp.Split('/');
                this.Year = split[0].GetInteger();
                this.Month = split[1].GetInteger();
                this.Day = split[2].GetInteger();
            }
            else
            {
                this.Year = date.Year;
                this.Month = date.Month;
                this.Day = date.Day;
            }
        }

        public CustomDate(string date)
        {
            date = Persia.Number.ConvertToLatin(date);
            string[] dateSplit = date.Split('/');
            if (dateSplit.Count() > 2)
            {
                int year = dateSplit[0].GetInteger();
                if (year < 1500)
                {
                    string temp = Persia.Number.ConvertToLatin(date);
                    string[] split = temp.Split('/');
                    this.Year = split[0].GetInteger();
                    this.Month = split[1].GetInteger();
                    this.Day = split[2].GetInteger();
                }
                else
                {
                    this.Year = date.Split('/')[0].GetInteger();
                    this.Month = date.Split('/')[1].GetInteger();
                    this.Day = date.Split('/')[2].GetInteger();
                }
            }
        }
    }
}
