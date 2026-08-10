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
    public class PaymentCertificateController : Controller
    {
        UnitOfWork _context = new UnitOfWork();

        public ActionResult Start(int type,int productId)
        {
            PaymentCertificate payment = new PaymentCertificate();
            Code statusEntity = _context.Code.GetByLabel(DataLayer.Enumarables.Enum_Code.PAYMENT_STATUS_INSERTED);
            ViewAccount tempAccount = _context.Account.GetCurrentAccount();
            payment.AccountId = tempAccount.Id;
            payment.StatusId = statusEntity.ID;
            payment.Datetime = DateTime.Now;
            payment.ProductId = productId;
            var WebsiteDetail = _context.WebsiteDetail.GetById(1);
            if (type == 1)
            {
                payment.Amount = WebsiteDetail.CerOnlinePrice.Value;
            }
            else
            {
                payment.Amount = WebsiteDetail.CerPyzicalPrice.Value;
            }
            payment.Type = type;
            payment.MerchantId = 2;
            payment.Description = "پرداخت گواهی ";
            _context.PaymentCertificate.Insert(payment);
            _context.Save();
            var paying = new PayingVm();

            if (payment.Code.Label == Enum_Code.PAYMENT_STATUS_INSERTED.ToString())
            {
                try
                {
                    payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_STARTED);
                    _context.Payment.Save();

                    var temp = _context.PaymentCertificate.GetById(payment.ID);
                    temp.Merchant = _context.Merchant.GetById(payment.MerchantId);
                    temp.Account = _context.Account.GetById(payment.AccountId);
                    paying = BaseZarinPal.StartPayment2(_context, temp);



                }
#pragma warning disable CS0168 // The variable 'e' is declared but never used
                catch (Exception e)
#pragma warning restore CS0168 // The variable 'e' is declared but never used
                {
                    payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_START_ERROR);
                    payment.ExternalInfo7 = e.Message;
                    _context.PaymentCertificate.Update(payment);
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
            PaymentCertificate payment = new PaymentCertificate();
            var result = new PayingVm2();

            payment = _context.PaymentCertificate.GetById(id);
            if (payment != null)
            {
                if (payment.Code.Label == Enum_Code.PAYMENT_STATUS_STARTED.ToString())
                {
                    if (payment.Merchant.Bank.Label == Enum_Bank.ZARINPAL.ToString())
                    {
                        BaseZarinPal.DoCallBack2(_context, payment);
                    }


                }

                var paymentvm = _context.PaymentCertificate.GetPaymentVm(payment.ID);
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

            return PartialView("~/Views/Default/Payment/Callback2.cshtml", model: result);



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




