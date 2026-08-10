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
    public class ExameController : ApiController
    {
        // GET: City
        UnitOfWork _context = new UnitOfWork();

        [HttpPost]
        public ApiResult Get(List<ViewExameAnswer> model, int? exameId, int? accountId)
        {
            var checkanswer = _context.ExameAccountAnswer.FirstOrDefault(s => s.ExameId == exameId && s.AccountId == accountId);
            if (checkanswer != null)
            {
                foreach (var item in model)
                {
                    var listExameAccountAnswerDetails = _context.ExameAccountAnswerDetails.FirstOrDefault(s => s.ExameAnswerId == checkanswer.ID && s.QuestionId== item.QuestionId);
                    if (listExameAccountAnswerDetails == null)
                    {
                        ExameAccountAnswerDetails answer = new ExameAccountAnswerDetails();
                        var AnswerId = _context.ExameQuestion.GetById(item.QuestionId).AnswerId;
                        if (item.AnswerId != null)
                        {
                            answer.AnswerId = item.AnswerId;
                            answer.IsTrue = AnswerId == item.AnswerId ? true : false;
                        }
                        else
                        {
                            answer.AnswerId = null;
                            answer.IsTrue = null;
                        }
                        answer.ExameAnswerId = checkanswer.ID;
                        answer.QuestionId = item.QuestionId;

                        _context.ExameAccountAnswerDetails.Insert(answer);
                        _context.Save();
                    }
                   
                }
                checkanswer.Finish = true;
                checkanswer.Open = null;

                checkanswer.TotalRate = null;

                _context.ExameAccountAnswer.Update(checkanswer);
                _context.Save();
                _context.ExameAccountAnswer.Update(checkanswer);
                var list = _context.ExameReportByCourse.Where(s => s.AnswerId == checkanswer.ID).ToList();
                foreach(var item in list)
                {
                    _context.ExameReportByCourse.Delete(item);
                    _context.Save();
                }
            }
            //else
            ////{
            //    ExameAccountAnswer res = new ExameAccountAnswer();
            //    res.ExameId = exameId;
            //    res.AccountId = accountId;
            //    res.Date = DateTime.Now;
            //    _context.ExameAccountAnswer.Insert(res);
            //    _context.Save();
            //    foreach (var item in model)
            //    {
            //        ExameAccountAnswerDetails answer = new ExameAccountAnswerDetails();
            //        var AnswerId = _context.ExameQuestion.GetById(item.QuestionId).AnswerId;
            //        if (item.AnswerId != null)
            //        {
            //            answer.AnswerId = item.AnswerId;
            //            answer.IsTrue = AnswerId == item.AnswerId ? true : false;
            //        }
            //        else
            //        {
            //            answer.AnswerId = null;
            //            answer.IsTrue = null;
            //        }
            //        answer.ExameAnswerId = res.ID;
            //        answer.QuestionId = item.QuestionId;

            //        _context.ExameAccountAnswerDetails.Insert(answer);
            //        _context.Save();
            //    }
            //}

            var tempAnswer = _context.ExameAccountAnswer.FirstOrDefault(s => s.ExameId == exameId && s.AccountId == accountId && s.Finish==true).ID;
            if (tempAnswer > 0)
            {
                ApiResult result = new ApiResult();
                result.Code = ApiResult.ResponseCode.Success;
                result.Message = "آزمون با موفقیت ثبت شد";
                return result;
            }
            else
            {
                ApiResult result = new ApiResult();
                result.Code = ApiResult.ResponseCode.Error;
                result.Message = "خطا در انجام عملیات";
                return result;
            }










        }
    }
}