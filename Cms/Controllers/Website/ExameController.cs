using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cms.Controllers.Website
{
    public class ExameController : Controller
    {
        UnitOfWork _context = new UnitOfWork();
        public ActionResult Index(int id)
        {
            Exame exame = (from s in this._context.Exame.Where((Exame s) => s.ID == id && s.IsOpen == (bool?)true && s.Active == (bool?)true, null, "")
                           select new Exame
                           {
                               ID = s.ID,
                               Name = s.Name,
                               ProductId = s.ProductId,
                               ExameQuestion = s.ExameQuestion
                           }).FirstOrDefault<Exame>();
            ViewAccount tempAccount = this._context.Account.GetCurrentAccount();
            bool flag = tempAccount == null;
            ActionResult result;
            if (flag)
            {
                result = base.RedirectToAction("Index", new
                {
                    controller = "Home"
                });
            }
            else
            {
                Account byId = this._context.Account.GetById(tempAccount.Id);
                bool flag2 = exame != null;
                if (flag2)
                {
                    bool flag3 = this._context.AccountOrderProduct.Isvalid(new int?(byId.ID), exame.ProductId);
                    if (flag3)
                    {
                        ViewExameStart viewExameStart = new ViewExameStart();
                        viewExameStart.Exame = exame;
                        bool flag4 = !this._context.ExameAccountAnswer.Isvalid(exame.ID, tempAccount.Id);
                        if (flag4)
                        {
                            ExameAccountAnswer exameAccountAnswer = new ExameAccountAnswer();
                            exameAccountAnswer.ExameId = new int?(exame.ID);
                            exameAccountAnswer.AccountId = new int?(tempAccount.Id);
                            exameAccountAnswer.Date = new DateTime?(DateTime.Now);
                            Random random = new Random();
                            exameAccountAnswer.IDRandom = new int?(random.Next(0, 1000));
                            this._context.ExameAccountAnswer.Insert(exameAccountAnswer);
                            this._context.Save();
                            exameAccountAnswer.ExameAccountAnswerDetails = new List<ExameAccountAnswerDetails>();
                            viewExameStart.AccountAnswer = exameAccountAnswer;
                        }
                        else
                        {
                            ExameAccountAnswer exameAccountAnswer2 = this._context.ExameAccountAnswer.FirstOrDefault((ExameAccountAnswer s) => s.ExameId == (int?)exame.ID && s.AccountId == (int?)tempAccount.Id, null, "");
                            exameAccountAnswer2.ExameAccountAnswerDetails = new List<ExameAccountAnswerDetails>();
                            viewExameStart.AccountAnswer = exameAccountAnswer2;
                        }
                        result = this.PartialView(BaseController.GetView(this), viewExameStart);
                    }
                    else
                    {
                        result = this.Redirect("/");
                    }
                }
                else
                {
                    Exame exame2 = (from s in this._context.Exame.Where((Exame s) => s.ID == id && s.Active == (bool?)true, null, "")
                                    select new Exame
                                    {
                                        ID = s.ID,
                                        Name = s.Name,
                                        ProductId = s.ProductId,
                                        ExameQuestion = s.ExameQuestion
                                    }).FirstOrDefault<Exame>();
                    bool flag5 = this._context.ExameAccountAnswer.Isvalid2(exame2.ID, byId.ID);
                    if (flag5)
                    {
                        exame2.ExameAccountAnswer = new List<ExameAccountAnswer>();
                        bool flag6 = this._context.AccountOrderProduct.Isvalid(new int?(byId.ID), exame2.ProductId);
                        if (flag6)
                        {
                            ViewExameStart viewExameStart2 = new ViewExameStart();
                            viewExameStart2.Exame = exame2;
                            ExameAccountAnswer exameAccountAnswer3 = this._context.ExameAccountAnswer.FirstOrDefault((ExameAccountAnswer s) => s.ExameId == (int?)exame2.ID && s.AccountId == (int?)tempAccount.Id, null, "");
                            bool flag7 = exameAccountAnswer3 == null;
                            if (flag7)
                            {
                                ExameAccountAnswer exameAccountAnswer4 = new ExameAccountAnswer();
                                exameAccountAnswer4.ExameId = new int?(exame2.ID);
                                exameAccountAnswer4.AccountId = new int?(tempAccount.Id);
                                exameAccountAnswer4.Date = new DateTime?(DateTime.Now);
                                Random random2 = new Random();
                                exameAccountAnswer4.IDRandom = new int?(random2.Next(0, 1000));
                                this._context.ExameAccountAnswer.Insert(exameAccountAnswer4);
                                this._context.Save();
                                exameAccountAnswer4.ExameAccountAnswerDetails = new List<ExameAccountAnswerDetails>();
                                viewExameStart2.AccountAnswer = exameAccountAnswer4;
                            }
                            else
                            {
                                exameAccountAnswer3.ExameAccountAnswerDetails = new List<ExameAccountAnswerDetails>();
                                viewExameStart2.AccountAnswer = exameAccountAnswer3;
                            }
                            result = this.PartialView(BaseController.GetView(this), viewExameStart2);
                        }
                        else
                        {
                            result = this.Redirect("/");
                        }
                    }
                    else
                    {
                        result = this.Redirect("/Exame/finish");
                    }
                }
            }
            return result;
        }
        //     public ActionResult Index(int id)
        //     {

        //         Exame exame = _context.Exame.Where(s => s.ID == id && s.IsOpen == true && s.Active == true)
        // .Select(s => new Exame()
        // {
        //     ID = s.ID,
        //     Name = s.Name,
        //     ProductId = s.ProductId,
        //     ExameQuestion = s.ExameQuestion,
        // })
        // .FirstOrDefault();
        //         ViewAccount tempAccount = _context.Account.GetCurrentAccount();
        //         if (tempAccount == null)
        //         {
        //             return RedirectToAction("Index", new { controller = "Home" });
        //         }
        //         Account account = _context.Account.GetById(tempAccount.Id);
        //         if (exame != null)
        //         {
        //             //exame.ExameAccountAnswer = new List<ExameAccountAnswer>();
        //             //if (exame.ExameAccountAnswer.FirstOrDefault(s => s.AccountId == account.ID) != null)
        //             //{
        //             //    return Redirect("/Exame/expire");
        //             //}
        //             //var accountorder = ;
        //             //var coursePayment = _context.CoursePayment.FirstOrDefault(s => s.UserId == account.ID && s.CourseId == exame.ProductId && s.PaidStatus == 1);
        //             //var student = _context.Students.FirstOrDefault(s => s.CourseId == exame.ProductId && s.UserId == account.ID);

        //             if (_context.AccountOrderProduct.Isvalid(account.ID, exame.ProductId) != false /*|| coursePayment != null || student != null*/)
        //             {
        //                 ViewExameStart result = new ViewExameStart();
        //                 result.Exame = exame;
        //                 //var checkanswer =;
        //                 if (_context.ExameAccountAnswer.Isvalid(exame.ID, tempAccount.Id) == false)
        //                 {

        //                     ExameAccountAnswer res = new ExameAccountAnswer();
        //                     res.ExameId = exame.ID;
        //                     res.AccountId = tempAccount.Id;
        //                     res.Date = DateTime.Now;
        //                     Random rnd = new Random();
        //                     res.IDRandom = rnd.Next(0, 1000);
        //                     _context.ExameAccountAnswer.Insert(res);
        //                     _context.Save();
        //                     res.ExameAccountAnswerDetails = new List<ExameAccountAnswerDetails>();
        //                     result.AccountAnswer = res;
        //                 }
        //                 else
        //                 {
        //                     var checkanswer = _context.ExameAccountAnswer.FirstOrDefault(s => s.ExameId == exame.ID && s.AccountId == tempAccount.Id);
        //                     checkanswer.ExameAccountAnswerDetails = new List<ExameAccountAnswerDetails>();
        //                     result.AccountAnswer = checkanswer;
        //                 }
        //                 return PartialView(BaseController.GetView(this), result);
        //             }
        //             else
        //             {
        //                 return Redirect("/");
        //             }

        //         }
        //         else
        //         {
        //             //if (exame.ExameAccountAnswer.FirstOrDefault(s => s.AccountId == account.ID) != null)
        //             //{
        //             //    return Redirect("/Exame/expire");
        //             //}
        //             Exame exame2 = _context.Exame.Where(s => s.ID == id && s.Active == true)
        //.Select(s => new Exame()
        //{
        //    ID = s.ID,
        //    Name = s.Name,
        //    ProductId = s.ProductId,
        //    ExameQuestion = s.ExameQuestion,
        //}).FirstOrDefault()
        //;
        //             //var exame2 = _context.Exame.FirstOrDefault(s => s.ID == id && s.Active == true);
        //             //var examanswer = ;
        //             if (_context.ExameAccountAnswer.Isvalid2(exame2.ID, account.ID) != false)
        //             {
        //                 //exame2.ExameAccountAnswer = new List<ExameAccountAnswer>();
        //                 //var accountorder = ;
        //                 //var coursePayment = _context.CoursePayment.FirstOrDefault(s => s.UserId == account.ID && s.CourseId == exame2.ProductId && s.PaidStatus == 1);
        //                 //var student = _context.Students.FirstOrDefault(s => s.CourseId == exame2.ProductId && s.UserId == account.ID);

        //                 if (_context.AccountOrderProduct.Isvalid(account.ID, exame2.ProductId) != false/*|| coursePayment != null || student != null*/)
        //                 {
        //                     ViewExameStart result = new ViewExameStart();
        //                     result.Exame = exame2;
        //                     var checkanswer = _context.ExameAccountAnswer.FirstOrDefault(s => s.ExameId == exame2.ID && s.AccountId == tempAccount.Id);
        //                     if (checkanswer == null)
        //                     {

        //                         ExameAccountAnswer res = new ExameAccountAnswer();
        //                         res.ExameId = exame2.ID;
        //                         res.AccountId = tempAccount.Id;
        //                         res.Date = DateTime.Now;
        //                         Random rnd = new Random();
        //                         res.IDRandom = rnd.Next(0, 1000);
        //                         _context.ExameAccountAnswer.Insert(res);
        //                         _context.Save();

        //                         res.ExameAccountAnswerDetails = new List<ExameAccountAnswerDetails>();

        //                         result.AccountAnswer = res;
        //                     }
        //                     else
        //                     {
        //                         checkanswer.ExameAccountAnswerDetails = new List<ExameAccountAnswerDetails>();
        //                         result.AccountAnswer = checkanswer;
        //                     }
        //                     return PartialView(BaseController.GetView(this), result);
        //                 }
        //                 else
        //                 {
        //                     return Redirect("/");
        //                 }
        //             }
        //             else
        //             {
        //                 return Redirect("/Exame/finish");

        //             }

        //         }

        //     }

        public ActionResult Finish()
        {

            return View(BaseController.GetView(this));
        }
        public ActionResult Expire()
        {

            return View(BaseController.GetView(this));
        }
        public ActionResult Error()
        {

            return View(BaseController.GetView(this));
        }
        public ActionResult Success(int id)
        {
            var accountId = _context.Account.GetCurrentAccount().Id;

            var tempAnswer = _context.ExameAccountAnswer.FirstOrDefault(s => s.ExameId == id && s.AccountId == accountId);
            tempAnswer.Finish = true;
            tempAnswer.TotalRate = null;
            _context.ExameAccountAnswer.Update(tempAnswer);
            _context.Save();
            if (tempAnswer == null)
            {
                return Redirect("/Exame/error");
            }
            else
            {
                return View(BaseController.GetView(this));

            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}