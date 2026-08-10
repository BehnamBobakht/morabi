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
    public class Entity_ExameReportByCourse : BaseRepository<ExameReportByCourse>
    {
        private DatabaseEntities _context;
        public Entity_ExameReportByCourse(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public void CheckTotalRate()
        {
            var list = _context.ExameReportByCourse.Where(s => s.RateInCountry == null && s.ExameAccountAnswer.Exame.Active!=true && s.ExameAccountAnswer.Exame.CheckResult==true).ToList();
            if (list.Count > 0)
            {
                foreach (var item in list.GroupBy(s=>s.ExameCourse))
                {
                    int i = 1;
                    var countUser = item.Key.ExameReportByCourse.Count();
                    var sumRateUsers = item.Key.ExameReportByCourse.Sum(x => x.TotalRate);
                    double ? avrageRateUsers = item.Key.ExameReportByCourse.Average(x => x.TotalRate);
                    //double mod = sumRateUsers.Value % countUser;
                    //avrageRateUsers = avrageRateUsers + mod;
                    double s = 0;
                    double sum = 0;

                    foreach (var answer in item.Key.ExameReportByCourse.Where(x=>x.RateInCountry==null).OrderByDescending(x => x.TotalRate)) 
                    {
                        var number = answer.TotalRate - avrageRateUsers;
                        sum += number.Value * number.Value;
                    }
                    s = sum / countUser;
                    var sqr = Math.Sqrt(s);
                    
                    foreach (var answer in item.Key.ExameReportByCourse.Where(x => x.RateInCountry == null).OrderByDescending(x => x.TotalRate))
                    {
                        var z = (answer.TotalRate - avrageRateUsers) / sqr;
                        var t = 1000 * (z) + 5000;
                        answer.TotalLevel = t;

                        answer.RateInCountry = i;
                        i++;
                    }
                    i = 1;

                    _context.SaveChanges();
                    //foreach (var city in item.Key.ExameReportByCourse.GroupBy(s => s.ExameAccountAnswer.Account.City))
                    //{
                    //    foreach (var account in city.Key.Account)
                    //    {
                    //        foreach (var answer in account.ExameAccountAnswer)
                    //        {
                    //            foreach (var report in answer.ExameReportByCourse.OrderByDescending(s => s.TotalRate))
                    //            {
                    //                report.RateInCity = i;
                                    
                    //            }
                    //        }
                            
                    //    }
                    //    i++;
                    //    _context.SaveChanges();
                    //}
                  
                    //_context.SaveChanges();
                }
                
                _context.SaveChanges();
                

            }
        }
        public List<ExameReportByCourse> Search(int? notId = null, int? categoryId = null, string categoryLabel = null, int index = 1, int pageSize = 10, string name = null)
        {
            var MyQuery = PredicateBuilder.True<ExameReportByCourse>();
            if (notId != null && notId != 0)
                MyQuery = MyQuery.And(p => p.ID != notId);

            

            int skipValue = pageSize * (index - 1);
            int pageValue = pageSize;

            return _context.ExameReportByCourse.OrderBy(p => p.ID).Where(MyQuery).Skip(skipValue).Take(pageValue).ToList();
        }

    }
}
