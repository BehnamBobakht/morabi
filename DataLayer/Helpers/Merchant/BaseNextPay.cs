using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DataLayer.Helpers.Merchant
{
    public class BaseNextPay : BasePayment
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
                Services.OnlinePayments.NextPay.ApplicationClient application = new Services.OnlinePayments.NextPay.ApplicationClient();
                Services.OnlinePayments.NextPay.TokenGenerator generator = new Services.OnlinePayments.NextPay.TokenGenerator();
                generator.amount = payment.Amount.ToString();
                generator.api_key = payment.Merchant.MerchantId;
                generator.callback_uri = redirectAddress;
                generator.order_id = payment.ID.ToString();
                Services.OnlinePayments.NextPay.TokenGeneratorResponse result = application.TokenGenerator(generator);
                if (result.TokenGeneratorResult.code == "-1")
                {
                    paying.Link = GetForm(payment.Merchant.Bank.PaymentUrl + result.TokenGeneratorResult.trans_id);
                    paying.Status = ResultStatus.Success;
                    paying.ReturnedCode = "-1";
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

        public static Payment DoCallBack(UnitOfWork _context, Payment payment)
        {
            try
            {
                var transid = HttpContext.Current.Request.Form["trans_id"].ToString();
                var orderid = HttpContext.Current.Request.Form["order_id"].ToString();
                Services.OnlinePayments.NextPay.ApplicationClient application = new Services.OnlinePayments.NextPay.ApplicationClient();
                Services.OnlinePayments.NextPay.PaymentVerification verification = new Services.OnlinePayments.NextPay.PaymentVerification();
                verification.amount = payment.Amount.ToString();
                verification.api_key = payment.Merchant.MerchantId;
                verification.trans_id = transid;
                verification.order_id = orderid;
                payment.RefNumber = transid;
                var resultverify = application.PaymentVerification(verification);
                payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY);
                if (resultverify.PaymentVerificationResult.code == "0")
                {
                    try
                    {
                        payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_SUCCESSFUL);
                        BaseCart.DoPaymentCallBack(_context, payment);
                        _context.Payment.Save();
                    }
                    catch (Exception)
                    {
                        payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_SERVICE_FAILED);
                    }
                }
                else
                    payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY_ERROR);

                payment.ExternalInfo1 = resultverify.PaymentVerificationResult.card_holder;
                payment.ExternalInfo2 = resultverify.PaymentVerificationResult.message;
                payment.ExternalInfo3 = resultverify.PaymentVerificationResult.code;
            }
            catch
            {
                payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY_ERROR);
            }

            _context.Save();
            payment = _context.Payment.GetById(payment.ID);
            return payment;
        }

        private static string GetForm(string url)
        {
            StringBuilder html = new StringBuilder();
            html.AppendLine("<html><head></head>");
            //html.AppendLine(string.Format("<body onload=\"document.formpayment.submit()\">"));
            html.AppendLine(string.Format("<body>"));
            html.AppendLine(string.Format("<form name='formpayment' method='get' action='" + url + "' >"));
            html.AppendLine("</form>");
            html.AppendLine("</body></html>");
            return html.ToString();
        }
    }
}
