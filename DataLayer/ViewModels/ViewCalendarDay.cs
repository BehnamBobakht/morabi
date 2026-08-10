using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewCalendarDay
    {
        public int WeekDay { get; set; }
        public string Day { get; set; }
        public DateTime Datetime { get; set; }
        public bool IsPast { get; set; }
        public bool IsToday { get; set; }
        public bool IsClosed { get; set; }
        public List<ViewCalendarEvent> Events { get; set; }
    }
}
