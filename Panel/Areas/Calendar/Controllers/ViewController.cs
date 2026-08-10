using DataLayer.Entities;
using DataLayer.ViewModels;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Panel.Areas.Calendar.Controllers
{
    public class ViewController : Controller
    {
        private UnitOfWork Context = new UnitOfWork();

        public ActionResult Index()
        {
            DateTime mainDate = DateTime.Now;
            return GetCalendarTable(mainDate);
        }

        public ActionResult Next(int id)
        {
            int rem = 0;
            int dev = (id < 99999) ? Math.DivRem(id, 10, out rem) : Math.DivRem(id, 100, out rem);
            CustomDate date = new CustomDate();
            if (rem == 12)
            {
                date = new CustomDate()
                {
                    Day = 1,
                    Month = 1,
                    Year = dev + 1
                };
            }
            else
            {
                date = new CustomDate()
                {
                    Day = 1,
                    Month = rem + 1,
                    Year = dev
                };
            }
            DateTime mainDate = BaseDate.GetGregorian(date).GetValueOrDefault();
            return GetCalendarTable(mainDate);
        }

        public ActionResult Prev(int id)
        {
            int rem = 0;
            int dev = (id < 99999) ? Math.DivRem(id, 10, out rem) : Math.DivRem(id, 100, out rem);
            CustomDate date = new CustomDate();
            if (rem == 1)
            {
                date = new CustomDate()
                {
                    Day = 1,
                    Month = 12,
                    Year = dev - 1
                };
            }
            else
            {
                date = new CustomDate()
                {
                    Day = 1,
                    Month = rem - 1,
                    Year = dev
                };
            }
            DateTime mainDate = BaseDate.GetGregorian(date).GetValueOrDefault();
            return GetCalendarTable(mainDate);
        }

        private ActionResult GetCalendarTable(DateTime mainDate)
        {
            ViewCalendar model = new ViewCalendar();
            mainDate = mainDate.AddHours(-mainDate.Hour);
            mainDate = mainDate.AddMinutes(-mainDate.Minute);
            mainDate = mainDate.AddSeconds(-mainDate.Second);
            mainDate = mainDate.AddMilliseconds(-mainDate.Millisecond);

            List<ViewCalendarDay> list = new List<ViewCalendarDay>();

            CustomDate today = new CustomDate(DateTime.Now);
            CustomDate mainCustomDate = new CustomDate(mainDate);
            DateTime firstOfMonth = mainDate.AddDays(-(mainCustomDate.Day - 1));
            int firstOfMonthDayOfWeek = (int)firstOfMonth.DayOfWeek;
            int subValue = 0;
            if (firstOfMonthDayOfWeek != 6)
                subValue = firstOfMonthDayOfWeek + 1;

            DateTime firstOfCalendar = firstOfMonth.AddDays(-subValue);
            List<Event> Events = Context.Event.GetAllForAccount(Context.SiteUser.GetCurrentUser().ID, firstOfCalendar, firstOfCalendar.AddDays(42));


            for (int i = 0; i < 42; i++)
            {
                DateTime temp = firstOfCalendar.AddDays(i);
                CustomDate tempCustom = new CustomDate(temp);
                if (i == 35 && tempCustom.Month != mainCustomDate.Month)
                    break;

                List<ViewCalendarEvent> listEvents = new List<ViewCalendarEvent>();
                List<Event> listEventToday = Events.Where(p => p.Date.DayOfYear == temp.DayOfYear).OrderBy(p => p.StartTime).ToList();
                foreach (Event item in listEventToday)
                {
                    listEvents.Add(new ViewCalendarEvent(item));
                }

                list.Add(new ViewCalendarDay()
                {
                    Datetime = temp,
                    Day = tempCustom.Day.ToPersian(),
                    WeekDay = (int)temp.DayOfWeek,
                    IsPast = tempCustom.Month == mainCustomDate.Month ? false : true,
                    IsToday = tempCustom.Day == today.Day && tempCustom.Month == today.Month ? true : false,
                    IsClosed = temp.DayOfWeek == DayOfWeek.Friday ? true : false,
                    Events = listEvents
                });
            }

            model.Days = list;
            model.Year = mainCustomDate.Year;
            model.Month = mainCustomDate.Month;
            model.MonthString = BaseDate.GetPersianMonth(mainCustomDate.Month);
            return View("index", model);
        }
    }
}