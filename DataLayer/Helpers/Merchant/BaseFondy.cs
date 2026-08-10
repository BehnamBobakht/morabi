using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DataLayer.Helpers.Merchant
{
    public class BaseFondy : BasePayment
    {
        public static PayingVm StartPayment(UnitOfWork _context, Payment payment)
        {
            var paying = new PayingVm();
            paying.Status = ResultStatus.Error;
            paying.Message = Resource.Notify.PaymentWasProblematic;
            try
            {
                var fondy = payment.Merchant;
                var password = fondy.TerminalKey;
                var merchantId = fondy.MerchantId;
                var callbackUrl = Base.BaseWebsite.ShopUrl + "payment/CallBack/?id=" + payment.ID;
                var responseUrl = Base.BaseWebsite.ShopUrl + "payment/FondyResponse/?id=" + payment.ID;
                var orderId = payment.ID.ToString();
                var orderDesc = "payment " + payment.ID;
                var currency = "EUR";
                var domain = payment.PaymentWebsiteId.HasValue ? payment.PaymentWebsite.Domain : "";
                var amount = payment.Amount * 100 /*payment.Amount*/;
                var signature = BaseSecurity.GetSha1(password + "|" + amount + "|" + currency + "|" + merchantId + "|" + orderDesc + "|" + orderId + "|" + responseUrl + "|" + callbackUrl).ToLower();
                var bankUrl = payment.Merchant.Bank.PaymentUrl;
                //add abak
                payment.Sign = signature;
                payment.ExternalInfo3 = orderDesc;
                _context.Payment.Save();
                paying.Link = GetForm(bankUrl, callbackUrl, responseUrl, merchantId, orderId, orderDesc, amount, currency, signature, domain);
                paying.Status = ResultStatus.Success;
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

        private static string GetForm(string bankUrl, string callbackUrl, string responseUrl, string merchantId, string orderId, string orderDesc, double amount, string currency, string signature, string domain)
        {
            StringBuilder html = new StringBuilder();
            //html.AppendLine("<html><head><title>PostByFondy</title></head>");
            //html.AppendLine(string.Format("<body onload=\"document.tocheckout.submit()\">"));

            //html.AppendLine("<ul>");
            //html.AppendLine(string.Format("<li><strong>" + "From : " + domain + "</strong> </li>"));
            //html.AppendLine(string.Format("<li><strong>" + "Amount : " + amount + " " + currency + "</strong> </li>"));
            //html.AppendLine("</ul>");

            html.AppendLine(string.Format("<form id='tocheckout' name='tocheckout' action='" + bankUrl + "' method='POST'>"));
            html.AppendLine("<input type='hidden' name='server_callback_url' value='" + callbackUrl + "' />");
            html.AppendLine("<input type='hidden' name='response_url' value='" + responseUrl + "' />");
            html.AppendLine("<input type='hidden' name='order_id' value='" + orderId + "' />");
            html.AppendLine("<input type='hidden' name='order_desc' value='" + orderDesc + "' />");
            html.AppendLine("<input type='hidden' name='currency' value='" + currency + "' />");
            html.AppendLine("<input type='hidden' name='amount' value='" + amount + "' />");
            html.AppendLine("<input type='hidden' name='signature' value='" + signature + "' />");
            html.AppendLine("<input type='hidden' name='merchant_id' value='" + merchantId + "' />");
            html.AppendLine("</form>");
            //html.AppendLine("</body></html>");
            return html.ToString();
        }
        //add abak
        public static Payment DoCallBack(UnitOfWork _context, Payment payment, string status, string amount, string merchantId, string currency)
        {
            try
            {


                var fondy = payment.Merchant;
                var password = fondy.TerminalKey;
                var callbackUrl = Base.BaseWebsite.ShopUrl + "payment/CallBack/?id=" + payment.ID;
                var responseUrl = Base.BaseWebsite.ShopUrl + "payment/FondyResponse/?id=" + payment.ID;
                var orderId = payment.ID.ToString();
                var orderDesc = "payment " + payment.ID;
                var signature = BaseSecurity.GetSha1(password + "|" + amount + "|" + currency + "|" + merchantId + "|" + orderDesc + "|" + orderId + "|" + responseUrl + "|" + callbackUrl).ToLower();

                if (status != "success")
                {
                    payment.ExternalInfo1 = "Inavlid Service";
                }
                else
                {
                    if (signature == payment.Sign)
                    {
                        payment.ExternalInfo1 = "Thank you for payment";
                        payment.ExternalInfo2 = "";
                        payment.RefNumber = payment.Sign;
                        // payment.RefNumber = transactionID;
                        payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_SUCCESSFUL);
                        BaseCart.DoPaymentCallBack(_context, payment);
                        _context.Payment.Save();
                    }
                    else
                    {
                        payment.ExternalInfo1 = "sign is invalid";
                        payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY_ERROR);
                        _context.Save();
                    }
                }
            }
            catch
            {
                payment.ExternalInfo1 = "There was an error processing payment";
                payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY_ERROR);
            }
            _context.Save();
            return payment;
        }
    }
}
