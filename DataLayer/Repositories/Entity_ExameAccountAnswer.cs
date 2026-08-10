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
    public class Entity_ExameAccountAnswer : BaseRepository<ExameAccountAnswer>
    {
        private DatabaseEntities _context;
        UnitOfWork _UnitOfWork = new UnitOfWork();
        public Entity_ExameAccountAnswer(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public void AddDetails()
        {
            var list = _context.ExameAccountAnswer.Where(s => s.TotalRate == null && s.Finish == true).ToList();
            foreach (var item in list)
            {
                CheckRate(item.ID);
            }


        }
        public bool Isvalid(int ExameId,int AccountId)
        {
            return _context.ExameAccountAnswer.Any(s => s.ExameId == ExameId && s.AccountId == AccountId);
            


        }
        public bool Isvalid2(int ExameId, int AccountId)
        {
            return _context.ExameAccountAnswer.Any(s => s.ExameId == ExameId && s.AccountId == AccountId && s.Open == true);



        }
        public void CheckRate(int id)
        {
            var ExameAccountAnswer = _context.ExameAccountAnswer.FirstOrDefault(p => p.ID == id);
            if (ExameAccountAnswer != null)
            {
                ExameAccountAnswer.CountNull = ExameAccountAnswer.ExameAccountAnswerDetails.Where(s => s.AnswerId == null).Count();
                var countfalse = ExameAccountAnswer.ExameAccountAnswerDetails.Where(s => s.IsTrue == false).Count();
                var counttrue = ExameAccountAnswer.ExameAccountAnswerDetails.Where(s => s.IsTrue == true).Count();
                var countNull = ExameAccountAnswer.ExameAccountAnswerDetails.Where(s => s.IsTrue == null).Count();

                ExameAccountAnswer.CountFalse = countfalse;
                ExameAccountAnswer.CountTrue = counttrue;
                var total1 = counttrue + countfalse + countNull;

                ExameAccountAnswer.TotalRate = counttrue;


                _context.SaveChanges();
            }

            CheckReport(id);
        }
        public void CheckReport(int id)
        {
            var courses = _context.ExameCourse/*.Where(s => s.Active != false)*/.ToList();
            var AccountAnswer = _context.ExameAccountAnswer.FirstOrDefault(p => p.ID == id);
            //string idsquestion = "-";
            foreach (var course in courses.Where(s => s.ExameQuestion.Count() != 0))
            {

                ExameReportByCourse examreport = new ExameReportByCourse();
                if (AccountAnswer.ExameAccountAnswerDetails.Any(s => s.ExameQuestion.ExameCourseId == course.ID))
                {
                    var countTrue = AccountAnswer.ExameAccountAnswerDetails.Where(s => s.ExameQuestion.ExameCourseId == course.ID && s.IsTrue == true).Count();
                    var countfalse2 = AccountAnswer.ExameAccountAnswerDetails.Where(s => s.ExameQuestion.ExameCourseId == course.ID && s.IsTrue == false).Count();
                    var countNull2 = AccountAnswer.ExameAccountAnswerDetails.Where(s => s.ExameQuestion.ExameCourseId == course.ID && s.IsTrue == null).Count();
                    examreport.CountTrue = countTrue;
                    var total22 = countTrue + countfalse2 + countNull2;

                    if (total22 != 20)
                    {
                        double? avrageRateUsers = 0;
                        avrageRateUsers = (countTrue * 20);
                        avrageRateUsers = (double)avrageRateUsers / total22;
                        examreport.TotalRate = avrageRateUsers;

                    }
                    else
                    {
                        examreport.TotalRate = countTrue;
                    }

                    examreport.ExameAnswerId = AccountAnswer.ID;
                    examreport.CourseId = course.ID;
                    examreport.CountFalse = countfalse2;
                    examreport.CountNull = countNull2;
                    //examreport.TotalRate = countTrue;
                    examreport.AnswerId = id;
                    var temp = _context.ExameReportByCourse.FirstOrDefault(s => s.ExameAnswerId == examreport.ExameAnswerId && s.CourseId == course.ID);
                    if (temp == null)
                    {
                        _context.ExameReportByCourse.Add(examreport);
                        _context.SaveChanges();
                    }
                    else
                    {
                        examreport.ID = temp.ID;

                        _UnitOfWork.ExameReportByCourse.Update(examreport);
                        _UnitOfWork.Save();
                    }
                    //idsquestion += course.ID.ToString() + "-";
                }

            }
            //AccountAnswer.QuestionAnswer = idsquestion ;
            //_context.SaveChanges();
            //_UnitOfWork.ExameAccountAnswer.Update(AccountAnswer);
            _UnitOfWork.Save();
        }

        //public void CheckTotalRate()
        //{
        //    var list = _context.Exame.Where(s => s.CheckResult == null && s.Active != true).ToList();
        //    if (list.Count > 0)
        //    {
        //        foreach (var item in list)
        //        {

        //            var listCourse = item.ExameQuestion.Select(s => s.ExameCourse).Distinct();
        //            foreach (var course in listCourse)
        //            {
        //                var listreport = _context.ExameReportByCourse.Where(x => x.ExameAccountAnswer.ExameId == item.ID && x.CourseId == course.ID).ToList();
        //                i = 1;
        //                foreach (var accoutAnswer in listreport.OrderByDescending(s => s.TotalRate))
        //                {
        //                    var countUser = listreport.Count();
        //                    var sumRateUsers = listreport.Sum(x => x.TotalRate);
        //                    double? avrageRateUsers = listreport.Average(x => x.TotalRate);
        //                    double s = 0;
        //                    double sum = 0;

        //                    foreach (var answer2 in listreport)
        //                    {
        //                        var number = answer2.TotalRate - avrageRateUsers;
        //                        sum += number.Value * number.Value;
        //                    }
        //                    s = sum / countUser;
        //                    s = s == 0 ? 1 : s;
        //                    var sqr = Math.Sqrt(s);
        //                    var z = (accoutAnswer.TotalRate - avrageRateUsers) / sqr;
        //                    var t = 1000 * (z) + 5000;
        //                    accoutAnswer.TotalLevel = t;
        //                    accoutAnswer.RateInCountry = i;
        //                    i++;
        //                }

        //                _context.SaveChanges();
        //            }

        //            _context.SaveChanges();

        //            item.CheckResult = true;
        //            _context.SaveChanges();
        //        }
        //        _context.SaveChanges();


        //    }
        //}
        public int SearchCount(
       int? notId = null, int? exameId = null, int? categoryId = null, string categoryLabel = null, int index = 1, int pageSize = 10000, string name = null, bool? IsPass = null)
        {
            var MyQuery = PredicateBuilder.True<ExameAccountAnswer>();
            if (notId != null && notId != 0)
                MyQuery = MyQuery.And(p => p.ID != notId);
            if (exameId != null)
            {
                MyQuery = MyQuery.And(p => p.ExameId == exameId);
            }
            if (name != "" && name != null)
            {
                MyQuery = MyQuery.And(p => p.Account.FullName.Contains(name) || p.Account.LatName.Contains(name));
            }
            if (IsPass == true)
            {
                MyQuery = MyQuery.And(p => p.ExameReportByCourse.Sum(s => s.TotalRate) / p.ExameReportByCourse.Count() >= p.Exame.PassValue);
            }
            return _context.ExameAccountAnswer.Count(MyQuery);
        }
        public List<ExameAccountAnswer> Search(int? notId = null,int ? userId=null, int? exameId = null, int? categoryId = null, string categoryLabel = null, int index = 1, int pageSize = 100000, string name = null, bool? IsPass = null)
        {
            var MyQuery = PredicateBuilder.True<ExameAccountAnswer>();
            if (notId != null && notId != 0)
                MyQuery = MyQuery.And(p => p.ID != notId);
            if (exameId != null)
            {
                MyQuery = MyQuery.And(p => p.ExameId == exameId);
            }
            if (name != "" && name != null)
            {
                MyQuery = MyQuery.And(p => p.Account.FullName.Contains(name) || p.Account.LatName.Contains(name));
            }
            if (userId != null)
            {
                MyQuery = MyQuery.And(p => p.Account.ID==userId);
            }
            if (IsPass == true)
            {
                MyQuery = MyQuery.And(p => p.ExameReportByCourse.Sum(s => s.TotalRate) / p.ExameReportByCourse.Count() >= p.Exame.PassValue);
            }

            int skipValue = pageSize * (index - 1);
            int pageValue = pageSize;

            return _context.ExameAccountAnswer.OrderByDescending(p => p.ID).Where(MyQuery).Skip(skipValue).Take(pageValue).ToList();
        }

    }
}
