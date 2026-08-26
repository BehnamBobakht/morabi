using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using ZarinPal;

namespace DataLayer.Helpers.Merchant
{
    public class BaseZarinPal : BasePayment
    {
        public static PayingVm StartPayment(UnitOfWork _context, Payment payment)
        {
            var paying = new PayingVm();
            paying.Status = ResultStatus.Error;
            paying.Message = Resource.Notify.PaymentWasProblematic;
            try
            {
                string host = string.IsNullOrEmpty(payment.Merchant.ExternalInfo2) ? BaseWebsite.ShopUrl : payment.Merchant.ExternalInfo2;
                string redirectAddress = host + "payment/callback?id=" + payment.ID; 
                string amount = (payment.Amount * 10).ToString();

                ZarinPal.ZarinPal zarinpal = ZarinPal.ZarinPal.Get();
                PaymentRequest pr = new PaymentRequest(payment.Merchant.MerchantId, (long)payment.Amount, redirectAddress, payment.Description)
                {
                    Email = "",
                    Mobile = payment.Account.Mobile
                };

                if (!string.IsNullOrEmpty(payment.Merchant.Username) && payment.Merchant.Username.ToUpper() == "SANDBOX")
                    zarinpal.EnableSandboxMode();
                else
                    zarinpal.DisableSandboxMode();

                var res = zarinpal.InvokePaymentRequest(pr);
                if (res.Status == 100)
                {
                    paying.Link = res.PaymentURL;
                    paying.IsLink = true;
                    paying.Status = ResultStatus.Success;
                }
                else
                {
                    paying.Message = res.Status.ToString();
                }
            }
            catch (Exception ex)
            {
                paying.Status = ResultStatus.Exception;
                paying.Message = ex.Message;
            }
            if (paying.Status != ResultStatus.Success)
            {
                payment.ExternalInfo7 = paying.Message;
                _context.Payment.Update(payment);
                _context.Save();
                paying.Message = Resource.Notify.PaymentWasProblematic;
            }
            return paying;
        }
        public static PayingVm StartPayment2(UnitOfWork _context, PaymentCertificate payment)
        {
            var paying = new PayingVm();
            paying.Status = ResultStatus.Error;
            paying.Message = Resource.Notify.PaymentWasProblematic;
            try
            {
                string host = string.IsNullOrEmpty(payment.Merchant.ExternalInfo2) ? BaseWebsite.ShopUrl : payment.Merchant.ExternalInfo2;
                string redirectAddress = host + "PaymentCertificate/callback?id=" + payment.ID;
                string amount = (payment.Amount * 10).ToString();

                ZarinPal.ZarinPal zarinpal = ZarinPal.ZarinPal.Get();
                PaymentRequest pr = new PaymentRequest(payment.Merchant.MerchantId, (long)payment.Amount, redirectAddress, payment.Description)
                {
                    Email = "",
                    Mobile = payment.Account.Mobile
                };

                if (!string.IsNullOrEmpty(payment.Merchant.Username) && payment.Merchant.Username.ToUpper() == "SANDBOX")
                    zarinpal.EnableSandboxMode();
                else
                    zarinpal.DisableSandboxMode();

                var res = zarinpal.InvokePaymentRequest(pr);
                if (res.Status == 100)
                {
                    paying.Link = res.PaymentURL;
                    paying.IsLink = true;
                    paying.Status = ResultStatus.Success;
                }
                else
                {
                    paying.Message = res.Status.ToString();
                }
            }
            catch (Exception ex)
            {
                paying.Status = ResultStatus.Exception;
                paying.Message = ex.Message;
            }
            if (paying.Status != ResultStatus.Success)
            {
                payment.ExternalInfo7 = paying.Message;
                _context.PaymentCertificate.Update(payment);
                _context.Save();
                paying.Message = Resource.Notify.PaymentWasProblematic;
            }
            return paying;
        }
        public static PayingVm StartPayment3(UnitOfWork _context, PaymentExame payment)
        {
            var paying = new PayingVm();
            paying.Status = ResultStatus.Error;
            paying.Message = Resource.Notify.PaymentWasProblematic;
            try
            {
                string host = string.IsNullOrEmpty(payment.Merchant.ExternalInfo2) ? BaseWebsite.ShopUrl : payment.Merchant.ExternalInfo2;
                string redirectAddress = host + "PaymentExame/callback?id=" + payment.ID;
                string amount = (payment.Amount * 10).ToString();

                ZarinPal.ZarinPal zarinpal = ZarinPal.ZarinPal.Get();
                PaymentRequest pr = new PaymentRequest(payment.Merchant.MerchantId, (long)payment.Amount, redirectAddress, payment.Description)
                {
                    Email = "",
                    Mobile = payment.Account.Mobile
                };

                if (!string.IsNullOrEmpty(payment.Merchant.Username) && payment.Merchant.Username.ToUpper() == "SANDBOX")
                    zarinpal.EnableSandboxMode();
                else
                    zarinpal.DisableSandboxMode();

                var res = zarinpal.InvokePaymentRequest(pr);
                if (res.Status == 100)
                {
                    paying.Link = res.PaymentURL;
                    paying.IsLink = true;
                    paying.Status = ResultStatus.Success;
                }
                else
                {
                    paying.Message = res.Status.ToString();
                }
            }
            catch (Exception ex)
            {
                paying.Status = ResultStatus.Exception;
                paying.Message = ex.Message;
            }
            if (paying.Status != ResultStatus.Success)
            {
                payment.ExternalInfo7 = paying.Message;
                _context.PaymentExame.Update(payment);
                _context.Save();
                paying.Message = Resource.Notify.PaymentWasProblematic;
            }
            return paying;
        }
        public static Payment DoCallBack(UnitOfWork _context, Payment payment)
        {
            try
            {
                String Status = HttpContext.Current.Request.QueryString["Status"];
                String Authority = HttpContext.Current.Request.QueryString["Authority"];

                if (Status != "OK")
                {
                    payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY_ERROR);
                }

                var zarinpal = ZarinPal.ZarinPal.Get();
                var pv = new PaymentVerification(payment.Merchant.MerchantId, (long)payment.Amount, Authority);
                var verificationResponse = zarinpal.InvokePaymentVerification(pv);
                if (verificationResponse.Status == 100)
                {
                    try
                    {
                        payment.RefNumber = verificationResponse.RefID;
                        payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_SUCCESSFUL);
                        //_context.Payment.DoPaymentServices(_context, payment);
                        BaseCart.DoPaymentCallBack(_context, payment);
                        _context.Payment.Save();
                    }
                    catch (Exception)
                    {
                        payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_SERVICE_FAILED);
                    }
                }
                else
                {
                    payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY_ERROR);
                }
                _context.Save();
                payment = _context.Payment.GetById(payment.ID);
                return payment;
            }
            catch
            {
                return null;
            }
        }
        public static PaymentCertificate DoCallBack2(UnitOfWork _context, PaymentCertificate payment)
        {
            try
            {
                String Status = HttpContext.Current.Request.QueryString["Status"];
                String Authority = HttpContext.Current.Request.QueryString["Authority"];

                if (Status != "OK")
                {
                    payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY_ERROR);
                }

                var zarinpal = ZarinPal.ZarinPal.Get();
                var pv = new PaymentVerification(payment.Merchant.MerchantId, (long)payment.Amount, Authority);
                var verificationResponse = zarinpal.InvokePaymentVerification(pv);
                if (verificationResponse.Status == 100)
                {
                    try
                    {
                        payment.RefNumber = verificationResponse.RefID;
                        payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_SUCCESSFUL);
                        //_context.Payment.DoPaymentServices(_context, payment);
                        BaseCart.DoPaymentCallBack2(_context, payment);
                        _context.Payment.Save();
                    }
                    catch (Exception)
                    {
                        payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_SERVICE_FAILED);
                    }
                }
                else
                {
                    payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY_ERROR);
                }
                _context.Save();
                payment = _context.PaymentCertificate.GetById(payment.ID);
                return payment;
            }
            catch
            {
                return null;
            }
        }
        public static PaymentExame DoCallBack3(UnitOfWork _context, PaymentExame payment)
        {
            try
            {
                String Status = HttpContext.Current.Request.QueryString["Status"];
                String Authority = HttpContext.Current.Request.QueryString["Authority"];

                if (Status != "OK")
                {
                    payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY_ERROR);
                }

                var zarinpal = ZarinPal.ZarinPal.Get();
                var pv = new PaymentVerification(payment.Merchant.MerchantId, (long)payment.Amount, Authority);
                var verificationResponse = zarinpal.InvokePaymentVerification(pv);
                if (verificationResponse.Status == 100)
                {
                    try
                    {
                        payment.RefNumber = verificationResponse.RefID;
                        payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_SUCCESSFUL);
                        //_context.Payment.DoPaymentServices(_context, payment);
                        BaseCart.DoPaymentCallBack3(_context, payment);
                        _context.Payment.Save();
                    }
                    catch (Exception)
                    {
                        payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_SERVICE_FAILED);
                    }
                }
                else
                {
                    payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY_ERROR);
                }
                _context.Save();
                payment = _context.PaymentExame.GetById(payment.ID);
                return payment;
            }
            catch
            {
                return null;
            }
        }

    }
}
