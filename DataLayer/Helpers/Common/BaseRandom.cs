using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Base
{
    public static class BaseRandom
    {
        public static string GetRandomNumber(int length = 4)
        {
            string random = "";
            Random _rdm = new Random();
            for (int i = 0; i < length; i++)
            {
                if (i == 0)
                    random += _rdm.Next(1, 9).ToString();
                else
                    random += _rdm.Next(0, 9).ToString();

            }
            return random;
        }
        public static string GetRandomUnique()
        {
            DateTime now = DateTime.Now;
            string random = "";
            Random _rdm = new Random();
            string ran1 = _rdm.Next(1, 9).ToString();
            string ran2 = _rdm.Next(1, 9).ToString();
            string ran3 = _rdm.Next(1, 9).ToString();
            string ran4 = _rdm.Next(1, 9).ToString();
            string year = now.Year.ToString().Substring(2, 2);
            string month = now.Month.ToString();
            string day = now.Day.ToString();
            string hour = now.Hour.ToString();
            string minute = now.Minute.ToString();
            string second = now.Second.ToString();
            //string millisecond = now.Millisecond.ToString();
            random = year + ran1 + month + ran2 + day + ran3 + hour + ran4 + minute + second;
            return random;
        }
    }
}
