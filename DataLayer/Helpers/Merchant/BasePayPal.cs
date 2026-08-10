using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace DataLayer.Helpers.Merchant
{
    public class BasePayPal : BasePayment
    {
        private class Paypal
        {
            public string cmd { get; set; }
            public string business { get; set; }
            public string no_shipping { get; set; }
            public string @return { get; set; }
            public string cancel_return { get; set; }
            public string notify_url { get; set; }
            public string currency_code { get; set; }
            public string item_name { get; set; }
            public string amount { get; set; }
            public string redirectUrl { get; set; }

        }
        public static PayingVm StartPayment(UnitOfWork _context, Payment payment)
        {
            var paying = new PayingVm();
            paying.Status = ResultStatus.Error;
            paying.Message = Resource.Notify.PaymentWasProblematic;
            try
            {
                string host = string.IsNullOrEmpty(payment.Merchant.ExternalInfo2) ? BaseWebsite.ShopUrl : payment.Merchant.ExternalInfo2;
                string redirectAddress = host + "payment/callback?id=" + payment.ID; string amount = (payment.Amount * 10).ToString();
                Paypal p = new Paypal();
               
                p.cmd = "_xclick"; ;
                p.business = payment.Merchant.Username;
                p.redirectUrl = payment.Merchant.Bank.PaymentUrl;
                p.cancel_return = "http://localhost/PaypalTest/Home/CancelFromPaypal";
                p.@return = "http://localhost/PaypalTest/Home/RedirectFromPaypal";
                p.notify_url = "http://localhost/PaypalTest/Home/NotifyFromPaypal";
                p.currency_code = "USD";
                //this.item_name = item;
                p.amount = payment.Amount.ToString();
                paying.Link = GetForm(p.cmd, p.business, p.no_shipping, p.@return, p.cancel_return, p.notify_url, p.currency_code, p.amount, p.redirectUrl, payment);
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

        public static Payment DoCallBack(UnitOfWork _context, Payment payment)
        {
            try
            {
                var formVals = new Dictionary<string, string>();
                formVals.Add("cmd", "_notify-synch"); //notify-synch_notify-validate
                formVals.Add("at", "90029006d0c0f23e98c71562df9b3ff6"); // this has to be adjusted
                formVals.Add("tx", HttpContext.Current.Request["tx"]);
                string paymentResponse = GetPayPalResponse(formVals, false);
                string transactionID = GetPDTValue(paymentResponse, "txn_id");


                if (paymentResponse.Contains("SUCCESS"))
                {
                    try
                    {
                        payment.ExternalInfo1 = "پرداخت با موفقیت انجام شد";
                        payment.ExternalInfo2 = "";
                        payment.RefNumber = transactionID;
                        payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_SUCCESSFUL);
                        BaseCart.DoPaymentCallBack(_context, payment);
                        _context.Payment.Save();
                    }
                    catch (Exception)
                    {
                        payment.ExternalInfo1 = "خطا در زمان سرویس دهی";
                        payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_SERVICE_FAILED);
                    }


                }
                else
                {
                    payment.ExternalInfo1 = "تراکنش بازگشت داده شد";
                    payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY_ERROR);
                    _context.Save();
                }

            }


            catch (Exception)
            {
                payment.ExternalInfo1 = "خطا در هنگام اعتبارسنجی درخواست";
                payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY_ERROR);
            }
            _context.Save();
            payment = _context.Payment.GetById(payment.ID);
            return payment;
        }

        private static string GetPayPalResponse(Dictionary<string, string> formVals, bool useSandbox)
        {

            string paypalUrl = useSandbox ? "https://www.sandbox.paypal.com/cgi-bin/webscr"
                : "https://www.paypal.com/cgi-bin/webscr";

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(paypalUrl);

            // Set values for the request back
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";

            byte[] param = HttpContext.Current.Request.BinaryRead(HttpContext.Current.Request.ContentLength);
            string strRequest = Encoding.ASCII.GetString(param);

            StringBuilder sb = new StringBuilder();
            sb.Append(strRequest);

            foreach (string key in formVals.Keys)
            {
                sb.AppendFormat("&{0}={1}", key, formVals[key]);
            }
            strRequest += sb.ToString();
            req.ContentLength = strRequest.Length;

            //for proxy
            //WebProxy proxy = new WebProxy(new Uri("http://urlort#");
            //req.Proxy = proxy;
            //Send the request to PayPal and get the response
            string response = "";
            using (StreamWriter streamOut = new StreamWriter(req.GetRequestStream(), System.Text.Encoding.ASCII))
            {

                streamOut.Write(strRequest);
                streamOut.Close();
                using (StreamReader streamIn = new StreamReader(req.GetResponse().GetResponseStream()))
                {
                    response = streamIn.ReadToEnd();
                }
            }

            return response;
        }
        private static string GetPDTValue(string pdt, string key)
        {

            string[] keys = pdt.Split('\n');
            string thisVal = "";
            string thisKey = "";
            foreach (string s in keys)
            {
                string[] bits = s.Split('=');
                if (bits.Length > 1)
                {
                    thisVal = bits[1];
                    thisKey = bits[0];
                    if (thisKey.Equals(key, StringComparison.InvariantCultureIgnoreCase))
                        break;
                }
            }
            return thisVal;

        }


        private static string GetForm(string cmd, string business, string no_shipping, string @return, string cancel_return, string notify_url,
            string currency_code, string amount, string redirectUrl,Payment payment)
        {
            StringBuilder html = new StringBuilder();
            html.AppendLine("<html><head><title>PostByPaypal</title></head>");
            //html.AppendLine(string.Format("<body onload=\"document.formpayment.submit()\">"));
            html.AppendLine(string.Format("<body>"));
            html.AppendLine(string.Format("<form id='formpayment' name='formpayment' action='https://www.paypal.com/cgi-bin/websrc'>"));
            html.AppendLine("<div><h4>PayPal</h4><hr/>");
            html.AppendLine("<dl class='dl-horizontal'>");
            html.AppendLine("<dt>");
            html.AppendLine("<input type='text' name='cmd' value='" + cmd + "' />");
            html.AppendLine("</dt>");
            html.AppendLine("<dt>");
            html.AppendLine("<input type='text' name='business' value='" + business + "' />");
            html.AppendLine("</dt>");
            html.AppendLine("<dt>");
            html.AppendLine("<input type='text' name='no_shipping' value='" + no_shipping + "' />");
            html.AppendLine("</dt>");
            html.AppendLine("<dt>");
            html.AppendLine("<input type='text' name='@return' value='" + @return + "' />");
            html.AppendLine("</dt>");
            html.AppendLine("<dt>");
            html.AppendLine("<input type='text' name='cancel_return' value='" + cancel_return + "' />");
            html.AppendLine("</dt>");
            html.AppendLine("<dt>");
            html.AppendLine("<input type='text' name='notify_url' value='" + notify_url + "' />");
            html.AppendLine("</dt>");
            html.AppendLine("<dt>");
            html.AppendLine("<input type='text' name='currency_code' value='" + currency_code + "' />");
            html.AppendLine("</dt>");
            html.AppendLine("<dt>");
            html.AppendLine("<input type='text' name='amount' value='" + amount + "' />");
            html.AppendLine("</dt>");
            html.AppendLine("<dt>");
            html.AppendLine("<input type='text' name='redirectUrl' value='" + redirectUrl + "' />");
            html.AppendLine("</dt>");
            html.AppendLine("</dl>");
            html.AppendLine("</div>");
            html.AppendLine("</form>");
            html.AppendLine("</body></html>");
            return html.ToString();
        }
    }

}
