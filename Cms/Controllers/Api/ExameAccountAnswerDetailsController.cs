using DataLayer.Api;
using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cms.Controllers.Api
{
    public class ExameAccountAnswerDetailsController : ApiController
    {
        // GET: City
        UnitOfWork _context = new UnitOfWork();

        [HttpPost]
        public ApiResult Get(int? accountAnswerId, List<ViewExameAnswer> model, int? courseId,string cId="" /*int? answerId, int? questionId*/, bool isfinish = false, string open ="False")
        {
            foreach (var item in model)
            {
                //var listExameAccountAnswerDetails = ;
                if (_context.ExameAccountAnswerDetails.Isvalid(accountAnswerId,item.QuestionId) == false)
                {
                    ExameAccountAnswerDetails answer = new ExameAccountAnswerDetails();
                    var Idkey = _context.ExameQuestion.GetById(item.QuestionId).AnswerId;
                    if (item.AnswerId != null)
                    {
                        answer.AnswerId = item.AnswerId;
                        answer.IsTrue = Idkey == item.AnswerId ? true : false;
                    }
                    else
                    {
                        answer.AnswerId = null;
                        answer.IsTrue = null;
                    }
                    answer.ExameAnswerId = accountAnswerId;
                    answer.QuestionId = item.QuestionId;

                    _context.ExameAccountAnswerDetails.Insert(answer);
                    _context.Save();
                }
                else
                {
                    var listExameAccountAnswerDetails = _context.ExameAccountAnswerDetails.FirstOrDefault(s => s.ExameAnswerId == accountAnswerId && s.QuestionId == item.QuestionId);
                    var Idkey = _context.ExameQuestion.GetById(item.QuestionId).AnswerId;
                    if (item.AnswerId != null)
                    {
                        listExameAccountAnswerDetails.AnswerId = item.AnswerId;
                        listExameAccountAnswerDetails.IsTrue = Idkey == item.AnswerId ? true : false;
                    }
                    else
                    {
                        listExameAccountAnswerDetails.AnswerId = null;
                        listExameAccountAnswerDetails.IsTrue = null;
                    }
                    listExameAccountAnswerDetails.ExameAnswerId = accountAnswerId;
                    listExameAccountAnswerDetails.QuestionId = item.QuestionId;
                    _context.ExameAccountAnswerDetails.Update(listExameAccountAnswerDetails);
                    _context.Save();
                }
            }
            var exam = _context.ExameAccountAnswer.GetById(accountAnswerId);
            if (exam.QuestionAnswer == null || exam.QuestionAnswer == "")
            {
                exam.QuestionAnswer = "-";
            }
            //var text = "-" + courseId.ToString() + "-";
            //exam.QuestionAnswer = exam.QuestionAnswer.Replace(text, "");
            //exam.QuestionAnswer += "-"+text+"-";
           
            exam.QuestionAnswer = exam.QuestionAnswer.Replace(cId, "");
            exam.QuestionAnswer += cId;
            _context.ExameAccountAnswer.Update(exam);
            _context.Save();
            ApiResult result = new ApiResult();
            result.Code = ApiResult.ResponseCode.Success;
            result.Message = "آزمون با موفقیت ثبت شد";

            if (isfinish == true)
            {


                exam.Finish = true;
                exam.Open = null;

                exam.TotalRate = null;

                _context.ExameAccountAnswer.Update(exam);
                _context.Save();

                var list = _context.ExameReportByCourse.Where(s => s.AnswerId == exam.ID).ToList();
                foreach (var item in list)
                {
                    var t = _context.ExameAccountAnswerDetails.Where(s => s.ExameQuestion.ExameCourseId == item.CourseId && s.ExameAnswerId == exam.ID).Count();
                    if (t > 0)
                    {
                        _context.ExameReportByCourse.Delete(item);
                        _context.Save();

                    }



                }
            }
            else if (isfinish == false)
            {


               
                if(open == "True")
                {
                    exam.Open = true;
                    exam.Finish = true;
                    exam.TotalRate = null;
                   
                    exam.QuestionAnswer = exam.QuestionAnswer.Replace("-" + courseId.ToString() + "-", "");
                    exam.QuestionAnswer = exam.QuestionAnswer + courseId.ToString() + "-";
                    _context.ExameAccountAnswer.Update(exam);
                    _context.Save();

                    var list = _context.ExameReportByCourse.Where(s => s.AnswerId == exam.ID).ToList();
                    foreach (var item in list)
                    {
                        var t = _context.ExameAccountAnswerDetails.Where(s => s.ExameQuestion.ExameCourseId == item.CourseId && s.ExameAnswerId == exam.ID).Count();
                        if (t > 0)
                        {
                            _context.ExameReportByCourse.Delete(item);
                            _context.Save();

                        }



                    }
                }
                

                
            }
            else if (open == "True")
            {
                var item = _context.ExameReportByCourse.Where(s => s.AnswerId == exam.ID && s.CourseId == courseId).FirstOrDefault();
                _context.ExameReportByCourse.Delete(item);
                _context.Save();

                var ExameAccountAnswer = _context.ExameAccountAnswer.FirstOrDefault(p => p.ID == accountAnswerId);
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
                    _context.ExameAccountAnswer.Update(ExameAccountAnswer);
                    _context.Save();
                    var courses = _context.ExameCourse.Where(s=>s.ID==courseId);
                    var AccountAnswer = _context.ExameAccountAnswer.FirstOrDefault(p => p.ID == accountAnswerId);
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
                            examreport.AnswerId = accountAnswerId;
                            var temp = _context.ExameReportByCourse.FirstOrDefault(s => s.ExameAnswerId == examreport.ExameAnswerId && s.CourseId == course.ID);
                            if (temp == null)
                            {
                                _context.ExameReportByCourse.Insert(examreport);
                               
                            }
                            else
                            {
                                examreport.ID = temp.ID;

                                _context.ExameReportByCourse.Update(examreport);
                               
                            }

                        }

                    } _context.Save();
                }
            }
            if (isfinish == true)
            {
                exam.Finish = true;
                exam.Open = null;

                exam.TotalRate = null;

                _context.ExameAccountAnswer.Update(exam);
                _context.Save();
            }

            return result;
        }
    }
}