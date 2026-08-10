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
    public class PaymentController : Controller
    {
        UnitOfWork _context = new UnitOfWork();

        public ActionResult Start(int id, bool isMobile = false)
        {
            Payment payment = _context.Payment.GetById(id);
            payment.IsMobile = isMobile;
            _context.Payment.Update(payment);
            _context.Save();
            var paying = new PayingVm();

            if (payment.Code.Label == Enum_Code.PAYMENT_STATUS_INSERTED.ToString())
            {
                try
                {
                    payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_STARTED);
                    _context.Payment.Save();

                    if (payment.MerchantId == null)
                    {
                        paying = BaseCashOnDelivery.StartPayment(_context, payment);
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.PASARGAD.ToString())
                    {
                        paying = BasePasargad.StartPayment(_context, payment);
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.UP.ToString())
                    {
                        paying = BaseAsanPardakht.StartPayment(_context, payment);
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.ZARINPAL.ToString())
                    {
                        paying = BaseZarinPal.StartPayment(_context, payment);
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.NEXTPAY.ToString())
                    {
                        paying = BaseNextPay.StartPayment(_context, payment);
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.MELLAT.ToString())
                    {
                        paying = BaseBehPardakht.StartPayment(_context, payment);
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.SAMAN.ToString())
                    {
                        paying = BaseSaman.StartPayment(_context, payment);
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.PARSIAN.ToString())
                    {
                        paying = BaseParsian.StartPayment(_context, payment);
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.MELLI.ToString())
                    {
                        paying = BaseMelli.StartPayment(_context, payment);
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.PAYPAL.ToString())
                    {
                        paying = BasePayPal.StartPayment(_context, payment);
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.EGHTESAD_NOVIN.ToString())
                    {
                        paying = BaseEghtesadNovin.StartPayment(_context, payment);
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.TEJART.ToString())
                    {
                        paying = BaseTejart.StartPayment(_context, payment);
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.CASHBILL.ToString())
                    {
                        paying = BaseCashbill.StartPayment(_context, payment);
                        if (payment.PaymentSubject.Label == "EXTERNAL")
                        {
                            ViewBag.PaymentForm = paying.Link;
                            ViewBag.PaymentId = payment.ID.ToString();
                            ViewBag.Currency = payment.CurrencyTypeId.HasValue ? payment.Code1.Label : "";
                            ViewBag.Domain = payment.PaymentWebsiteId.HasValue ? payment.PaymentWebsite.Domain : "";
                            ViewBag.Amount = payment.Amount;
                            ViewBag.PaymentGateway = Enum_Bank.CASHBILL.ToString();
                            return PartialView(BaseController.GetView(this));
                        }
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.FONDY.ToString())
                    {
                        paying = BaseFondy.StartPayment(_context, payment);
                        if (payment.PaymentSubject.Label == "EXTERNAL")
                        {
                            ViewBag.PaymentForm = paying.Link;
                            ViewBag.PaymentId = payment.ID.ToString();
                            ViewBag.Currency = "EUR";
                            ViewBag.Domain = payment.PaymentWebsiteId.HasValue ? payment.PaymentWebsite.Domain : "";
                            ViewBag.Amount = payment.Amount;
                            ViewBag.PaymentGateway = Enum_Bank.FONDY.ToString();
                            return PartialView(BaseController.GetView(this));
                        }
                    }
                    if (payment.Merchant != null)
                    {
                        if (payment.Merchant.Bank != null)
                        {
                            paying.BankName = payment.Merchant.Bank.Name;
                        }
                    }

                }
#pragma warning disable CS0168 // The variable 'e' is declared but never used
                catch (Exception e)
#pragma warning restore CS0168 // The variable 'e' is declared but never used
                {
                    payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_START_ERROR);
                    payment.ExternalInfo7 = e.Message;
                    _context.Payment.Update(payment);
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
            Payment payment = new Payment();
            var result = new PayingVm();

            payment = _context.Payment.GetById(id);
            if (payment != null)
            {
                if (payment.Code.Label == Enum_Code.PAYMENT_STATUS_STARTED.ToString())
                {
                    if (payment.MerchantId == null)
                    {
                        BaseCashOnDelivery.DoCallBack(_context, payment);
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.PASARGAD.ToString())
                    {
                        BasePasargad.DoCallBack(_context, payment);
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.UP.ToString())
                    {
                        BaseAsanPardakht.DoCallBack(_context, payment);
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.ZARINPAL.ToString())
                    {
                        BaseZarinPal.DoCallBack(_context, payment);
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.NEXTPAY.ToString())
                    {
                        BaseNextPay.DoCallBack(_context, payment);
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.MELLAT.ToString())
                    {
                        BaseBehPardakht.DoCallBack(_context, payment);
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.SAMAN.ToString())
                    {
                        BaseSaman.DoCallBack(_context, payment);
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.PARSIAN.ToString())
                    {
                        BaseParsian.DoCallBack(_context, payment);
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.MELLI.ToString())
                    {
                        BaseMelli.DoCallBack(_context, payment);
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.PAYPAL.ToString())
                    {
                        BasePayPal.DoCallBack(_context, payment);
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.CASHBILL.ToString())
                    {
                        BaseCashbill.DoCallBack(_context, payment);
                    }

                    else if (payment.Merchant.Bank.Label == Enum_Bank.EGHTESAD_NOVIN.ToString())
                    {
                        BaseEghtesadNovin.DoCallBack(_context, payment);
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.TEJART.ToString())
                    {
                        BaseTejart.DoCallBack(_context, payment);
                    }

                }
              
                var paymentvm = _context.Payment.GetPaymentVm(payment.ID);
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
            //return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, "CallBack", payment);
            var listProdcut = _context.AccountOrder.Where( s=> s.AccountId == payment.AccountId /*&& s.PaymentProductOrder.Any(x=>x.PaymentId==id)*/).OrderByDescending(s=>s.ID).Take(1);
            ViewPay model = new ViewPay();
            if (listProdcut.FirstOrDefault().AccountOrderProduct.Any(s=>s.Product.ProductTypeId==2))
            {
                model.Payment = payment;
                List<string> list = new List<string>();
                List<string> names = new List<string>();

                foreach (var item in listProdcut.FirstOrDefault().AccountOrderProduct)
                {
                    list.Add(item.Product.LinkTelegram);
                    names.Add(item.Product.Name);

                    model.IsNationalId = item.Product.IsNationalId;
                    model.IsPictureId = item.Product.IsPictureId;
                    model.IsParport = item.Product.IsParport;
                }
                model.Links = list;
                model.Names = names;

                return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, "CallBack", model);
            }
            else if(listProdcut.FirstOrDefault().AccountOrderProduct.Any(s => s.Product.ProductTypeId == 3))
            {
                model.Payment = payment;
                List<string> list = new List<string>();
                List<string> names = new List<string>();

                foreach (var item in listProdcut.FirstOrDefault().AccountOrderProduct)
                {
                    
                    names.Add(item.Product.Name);

                }
               
                model.Names = names;
                return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, "CallBackBook", model);

            }
            else if (listProdcut.FirstOrDefault().AccountOrderProduct.Any(s => s.Product.ProductTypeId == 1001))
            {
                model.Payment = payment;
                List<string> list = new List<string>();
                List<string> names = new List<string>();

                foreach (var item in listProdcut.FirstOrDefault().AccountOrderProduct)
                {

                    names.Add(item.Product.Name);

                }

                model.Names = names;
                return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, "CallBackExame", model);

            }
            else if (listProdcut.FirstOrDefault().AccountOrderProduct.Any(s => s.Product.ProductTypeId == 1))
            {
                model.Payment = payment;
                List<string> list = new List<string>();
                List<string> names = new List<string>();

                foreach (var item in listProdcut.FirstOrDefault().AccountOrderProduct)
                {

                    names.Add(item.Product.Name);

                }

                model.Names = names;
                return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, "CallBackCertificate", model);

            }
            else
            {
                return PartialView("~/Views/Default/Payment/Callback.cshtml", model: result);

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




