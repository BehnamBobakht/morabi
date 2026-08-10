using Binbin.Linq;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_Exame : BaseRepository<Exame>
    {
        private DatabaseEntities _context;
        public Entity_Exame(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<Exame> Search(int? notId = null, int? categoryId = null, string categoryLabel = null, int index = 1, int pageSize = 10, string name = null)
        {
            var MyQuery = PredicateBuilder.True<Exame>();
            if (notId != null && notId != 0)
                MyQuery = MyQuery.And(p => p.ID != notId);

            if (name != null)
                MyQuery = MyQuery.And(p => p.Name.Contains(name));

            int skipValue = pageSize * (index - 1);
            int pageValue = pageSize;

            return _context.Exame.OrderBy(p => p.ID).Where(MyQuery).Skip(skipValue).Take(pageValue).ToList();
        }
        public void CheckActive()
        {
            List<Exame> list = _context.Exame.Where(p => p.Active == true && p.StartTime!=null).ToList();
            if (list.Count > 0)
            {
                foreach (Exame item in list)
                {
                    DateTime date1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);
                    DateTime date2 = new DateTime(item.StartDate.Value.Year, item.StartDate.Value.Month, item.StartDate.Value.Day, item.StartTime.Value.Hours, item.StartTime.Value.Minutes, 0);
                    var minute = item.Time % 60;
                    var hours = item.Time / 60;
                    if (minute >= 60)
                    {
                        hours = hours + 1;
                        minute = minute - 60;
                    }
                    hours = item.StartTime.Value.Hours + hours;
                    minute = item.StartTime.Value.Minutes + minute;
                    if (minute >= 60)
                    {
                        hours = hours + 1;
                        minute = minute - 60;
                    }
                    if (hours > 24)
                    {
                        hours = hours - 24;
                        var less = hours % 24;
                        item.StartDate.Value.AddDays(less.Value);
                    }
                    DateTime date3 = new DateTime(item.StartDate.Value.Year, item.StartDate.Value.Month, item.StartDate.Value.Day, hours.Value, minute.Value, 0);
                    int result = DateTime.Compare(date1, date2);
                    int result2 = DateTime.Compare(date1, date3);
                    //int result4 = DateTime.Compare(date3, date1);
                    if (result > 0 && date1.Day > date2.Day)
                    {
                        item.IsOpen = false;

                        item.Active = false;
                    }
                    else if ((result > 0 || result == 0) && date1.Day == date2.Day && result2 < 0)
                    {
                        item.IsOpen = true;
                    }
                    else if (date1.Day == date2.Day && result2 > 0)
                    {
                        item.IsOpen = false;

                        item.Active = false;
                    }
                }
                _context.SaveChanges();


            }
        }

        public void CheckTotalRate()
        {
            var list = _context.Exame.Where(s => s.CheckResult == null && s.Active != true).ToList();
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    int i = 1;
                    foreach (var answer in item.ExameAccountAnswer.OrderByDescending(s => s.TotalRate))
                    {

                        answer.TotalRateInCountry = i;

                        i++;
                    }
                    i = 1;
                    _context.SaveChanges();

                    foreach (var city in item.ExameAccountAnswer.GroupBy(s => s.Account.City))
                    {
                        i = 1;
                        foreach (var account in city.ToList().OrderByDescending(s => s.TotalRate))
                        {

                            account.TotalRateInCity = i;
                            i++;

                        }

                    }
                    _context.SaveChanges();
                    //var listCourse = item.ExameQuestion.Select(s => s.ExameCourse).Distinct();
                    //foreach (var course in listCourse)
                    //{
                    //    var listreport = _context.ExameReportByCourse.Where(x => x.ExameAccountAnswer.ExameId == item.ID && x.CourseId == course.ID).ToList();
                    //    i = 1;
                    //    foreach (var accoutAnswer in listreport.OrderByDescending(s=>s.TotalRate))
                    //    {
                    //        var countUser = listreport.Count();
                    //        var sumRateUsers = listreport.Sum(x => x.TotalRate);
                    //        double? avrageRateUsers = listreport.Average(x => x.TotalRate);
                    //        double s = 0;
                    //        double sum = 0;

                    //        foreach (var answer2 in listreport)
                    //        {
                    //            var number = answer2.TotalRate - avrageRateUsers;
                    //            sum += number.Value * number.Value;
                    //        }
                    //        s = sum / countUser;
                    //        s = s == 0 ? 1 : s;
                    //        var sqr = Math.Sqrt(s);
                    //        var z = (accoutAnswer.TotalRate - avrageRateUsers) / sqr;
                    //        var t = 1000 * (z) + 5000;
                    //        accoutAnswer.TotalLevel = t;
                    //        accoutAnswer.RateInCountry = i;
                    //        i++;
                    //    }
                        
                    //    _context.SaveChanges();
                    //}

                    //_context.SaveChanges();

                    item.CheckResult = true;
                    _context.SaveChanges();
                }
                _context.SaveChanges();


            }
        }
        public void CheckTotalLevel()
        {
            var list = _context.ExameAccountAnswer.Where(s => s.TotalLevel == null && s.Exame.Active != true && s.Exame.CheckResult == true).ToList();
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    
                    item.TotalLevel = _context.ExameReportByCourse.Where(s => s.AnswerId == item.ID).Average(s => s.TotalLevel);
                }
                _context.SaveChanges();


            }
        }

    }
}
