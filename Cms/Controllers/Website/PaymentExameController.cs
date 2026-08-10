using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.Helpers.Merchant;
using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer.ViewModels;
using System.Text;
using System.Security.Cryptography;
using System.Net;
using DataLayer.Base;

namespace Cms.Controllers.Website
{
    public class PaymentExameController : Controller
    {
        UnitOfWork _context = new UnitOfWork();

        public ActionResult Start(int AccountAnswerId, int productId, int CourseId)
        {
            PaymentExame payment = new PaymentExame();
            Code statusEntity = _context.Code.GetByLabel(DataLayer.Enumarables.Enum_Code.PAYMENT_STATUS_INSERTED);
            ViewAccount tempAccount = _context.Account.GetCurrentAccount();
            payment.AccountId = tempAccount.Id;
            payment.StatusId = statusEntity.ID;
            payment.Datetime = DateTime.Now;
            payment.ProductId = productId;
            var Variety = BaseStore.GetProductVariety(_context: _context, productId: productId);
            StoreProductQuantityVm firstStoreProductQuantity = null;
            if (Variety != null)
            {
                firstStoreProductQuantity = Variety.storeProductQuantities[0];
                payment.Amount = firstStoreProductQuantity.Price;
            }
            var accountanswer = _context.ExameAccountAnswer.FirstOrDefault(s => s.AccountId == tempAccount.Id && s.ID == AccountAnswerId);
            double? number = _context.ExameReportByCourse.FirstOrDefault(s => s.CourseId == CourseId && s.AnswerId == accountanswer.ID).TotalRate;
            payment.AccountAnswerId = AccountAnswerId;
            //payment.NewNumber = number.ToString();
            payment.CourseId = CourseId;
            payment.IsMulti = false;

            payment.ExameId = accountanswer.ExameId;
            payment.MerchantId = 2;
            payment.Description = "پرداخت آزمون ";
            _context.PaymentExame.Insert(payment);
            _context.Save();
            PaymentExameCourseId courseids = new PaymentExameCourseId();
            courseids.CourseId = CourseId;
            courseids.PaymentExameId = payment.ID;
            courseids.Number = number;
            courseids.ProductId = productId;

            _context.PaymentExameCourseId.Insert(courseids);
            _context.Save();
            var paying = new PayingVm();

            if (payment.Code.Label == Enum_Code.PAYMENT_STATUS_INSERTED.ToString())
            {
                try
                {
                    payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_STARTED);
                    _context.Payment.Save();

                    var temp = _context.PaymentExame.GetById(payment.ID);
                    temp.Merchant = _context.Merchant.GetById(payment.MerchantId);
                    temp.Account = _context.Account.GetById(payment.AccountId);
                    paying = BaseZarinPal.StartPayment3(_context, temp);



                }
#pragma warning disable CS0168 // The variable 'e' is declared but never used
                catch (Exception e)
#pragma warning restore CS0168 // The variable 'e' is declared but never used
                {
                    payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_START_ERROR);
                    payment.ExternalInfo7 = e.Message;
                    _context.PaymentExame.Update(payment);
                    _context.Save();
                    paying.Status = ResultStatus.Exception;
                    paying.Message = Resource.Notify.PaymentWasProblematic;
                    paying.IsLink = false;
                }

                if (paying.IsLink)
                    return Redirect(paying.Link);
                else
                    return PartialView("~/Views/Default/Payment/Start.cshtml", model: paying);

            }
            else
            {
                paying.Status = ResultStatus.Exception;
                paying.Message = Resource.Notify.PaymentWasProblematic;
                return PartialView("~/Views/Default/Payment/Start.cshtml", model: paying);
            }
        }
        public ActionResult Start2(int AccountAnswerId, string productId, string CourseId)
        {
            PaymentExame payment = new PaymentExame();
            Code statusEntity = _context.Code.GetByLabel(DataLayer.Enumarables.Enum_Code.PAYMENT_STATUS_INSERTED);
            ViewAccount tempAccount = _context.Account.GetCurrentAccount();
            payment.AccountId = tempAccount.Id;
            payment.StatusId = statusEntity.ID;
            payment.Datetime = DateTime.Now;
            payment.Amount = 0;
            foreach (var item in productId.Split(','))
            {
                var Variety = BaseStore.GetProductVariety(_context: _context, productId:int.Parse(item));
                StoreProductQuantityVm firstStoreProductQuantity = null;
                if (Variety != null)
                {
                    firstStoreProductQuantity = Variety.storeProductQuantities[0];
                    payment.Amount += firstStoreProductQuantity.Price;
                }
            }

            var accountanswer = _context.ExameAccountAnswer.FirstOrDefault(s => s.AccountId == tempAccount.Id && s.ID == AccountAnswerId);
            payment.AccountAnswerId = AccountAnswerId;
            payment.IsMulti = true;

            payment.ExameId = accountanswer.ExameId;
            payment.MerchantId = 2;
            payment.Description = "پرداخت آزمون چند تایی";
            _context.PaymentExame.Insert(payment);
            _context.Save();
            foreach (var item in CourseId.Split(','))
            {
                var id = int.Parse(item);
                double? number = _context.ExameReportByCourse.FirstOrDefault(s => s.CourseId == id && s.AnswerId == accountanswer.ID).TotalRate;
                PaymentExameCourseId courseids = new PaymentExameCourseId();
                courseids.CourseId = id;
                courseids.PaymentExameId = payment.ID;
                courseids.Number = number;
                _context.PaymentExameCourseId.Insert(courseids);
                _context.Save();
            }

            var paying = new PayingVm();

            if (payment.Code.Label == Enum_Code.PAYMENT_STATUS_INSERTED.ToString())
            {
                try
                {
                    payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_STARTED);
                    _context.Payment.Save();

                    var temp = _context.PaymentExame.GetById(payment.ID);
                    temp.Merchant = _context.Merchant.GetById(payment.MerchantId);
                    temp.Account = _context.Account.GetById(payment.AccountId);
                    paying = BaseZarinPal.StartPayment3(_context, temp);



                }
#pragma warning disable CS0168 // The variable 'e' is declared but never used
                catch (Exception e)
#pragma warning restore CS0168 // The variable 'e' is declared but never used
                {
                    payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_START_ERROR);
                    payment.ExternalInfo7 = e.Message;
                    _context.PaymentExame.Update(payment);
                    _context.Save();
                    paying.Status = ResultStatus.Exception;
                    paying.Message = Resource.Notify.PaymentWasProblematic;
                    paying.IsLink = false;
                }

                if (paying.IsLink)
                    return Redirect(paying.Link);
                else
                    return PartialView("~/Views/Default/Payment/Start.cshtml", model: paying);

            }
            else
            {
                paying.Status = ResultStatus.Exception;
                paying.Message = Resource.Notify.PaymentWasProblematic;
                return PartialView("~/Views/Default/Payment/Start.cshtml", model: paying);
            }
        }

        public ActionResult Callback(int id = 0)
        {
            PaymentExame payment = new PaymentExame();
            var result = new PayingVm3();

            payment = _context.PaymentExame.GetById(id);
            if (payment != null)
            {
                if (payment.Code.Label == Enum_Code.PAYMENT_STATUS_STARTED.ToString())
                {
                    if (payment.Merchant.Bank.Label == Enum_Bank.ZARINPAL.ToString())
                    {
                        BaseZarinPal.DoCallBack3(_context, payment);
                    }


                }

                var paymentvm = _context.PaymentExame.GetPaymentVm(payment.ID);
                result.Payment = paymentvm;
                if (payment.Code.Label == Enum_Code.PAYMENT_STATUS_SUCCESSFUL.ToString())
                {


                    result.Status = ResultStatus.Success;
                }
                else
                {
                    result.Status = ResultStatus.Error;

                }

            }
            else
            {
                result.Message = Resource.Notify.PaymentWasProblematic;
                result.Status = ResultStatus.Exception;
            }

            return PartialView("~/Views/Default/Payment/Callback3.cshtml", model: result);



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




