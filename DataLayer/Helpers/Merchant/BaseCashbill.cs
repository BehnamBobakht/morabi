using DataLayer.Api;
using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using Newtonsoft.Json;
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
//using static DataLayer.Helpers.Merchant.BaseCashbill;

namespace DataLayer.Helpers.Merchant
{
    public class BaseCashbill : BasePayment
    {
        private class Body
        {
            public string service { get; set; }
            public double amount { get; set; }
            public string desc { get; set; }
            public string userdata { get; set; }
            public string sign { get; set; }
        }
        public class UserInformation
        {

            public string forname { get; set; }
            public string surname { get; set; }
            public string email { get; set; }
            public string tel { get; set; }
            public string street { get; set; }
            public string street_n1 { get; set; }
            public string street_n2 { get; set; }
            public string city { get; set; }
            public string postcode { get; set; }
            public string country { get; set; }
        }

        public class ViewResults
        {
            public string service { get; set; }
            public string orderid { get; set; }
            public string amount { get; set; }
            public string userdata { get; set; }
            public string status { get; set; }
            public string sign { get; set; }
        }
        public class Results
        {
            public ViewResults GET { get; set; }
        }
        public static PayingVm StartPayment(UnitOfWork _context, Payment payment)
        {
            var paying = new PayingVm();
            paying.Status = ResultStatus.Error;
            paying.Message = Resource.Notify.PaymentWasProblematic;
            try
            {
                var service = payment.Merchant.Username;
                var key = payment.Merchant.MerchantId;
                var desc = payment.Description;
                var userdata = payment.ID.ToString();
                var amount = payment.Amount.ToString();
                var currency = payment.CurrencyTypeId.HasValue ? payment.Code1.Name : "";
                var domain = payment.PaymentWebsiteId.HasValue ? payment.PaymentWebsite.Domain : "";
                var sign = GetMd5Hash(service + amount + desc + userdata + key);
                payment.Sign = sign;
                _context.Payment.Save();
                var bankUrl = payment.Merchant.Bank.PaymentUrl;
                paying.Link = GetForm(bankUrl, service, amount, currency, desc, userdata, sign, domain);
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
                var url = HttpContext.Current.Request.Url.AbsoluteUri;
                Uri myUri = new Uri(url);
                string userdata = HttpUtility.ParseQueryString(myUri.Query).Get("userdata");
                string service = HttpUtility.ParseQueryString(myUri.Query).Get("service");
                string orderid = HttpUtility.ParseQueryString(myUri.Query).Get("orderid");
                string amount = HttpUtility.ParseQueryString(myUri.Query).Get("amount");
                string status = HttpUtility.ParseQueryString(myUri.Query).Get("status");
                string sign = HttpUtility.ParseQueryString(myUri.Query).Get("sign");
                ViewResults response = new ViewResults()
                {
                    service=service,
                    orderid=orderid,
                    amount=amount,
                    userdata=userdata,
                    status=status,
                    sign=sign
                };
               
                if (response.service != payment.Merchant.Username)
                {
                    payment.ExternalInfo1 = "Inavlid Service";
                }
                else
                {
                    string Hash = GetMd5Hash(response.service + response.orderid + response.amount + response.userdata + response.status + payment.Merchant.MerchantId);
                    if (Hash == response.sign)
                    {
                        if ((!string.IsNullOrEmpty(response.status)) && response.status.ToLower() == "ok")
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
                            payment.ExternalInfo1 = "Inavlid Service";
                            payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_SERVICE_FAILED);

                        }

                    }
                    else
                    {
                        payment.ExternalInfo1 = "There was an error processing payment";
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



        public static string GetMd5Hash(string input)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash. 
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Create a new Stringbuilder to collect the bytes 
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data  
                // and format each one as a hexadecimal string. 
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string. 
                return sBuilder.ToString();
            }
        }

        // Verify a hash against a string. 
        public static bool VerifyMd5Hash(string input, string hash)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                // Hash the input. 
                string hashOfInput = GetMd5Hash(input);

                // Create a StringComparer an compare the hashes.
                StringComparer comparer = StringComparer.OrdinalIgnoreCase;

                if (0 == comparer.Compare(hashOfInput, hash))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        private static string GetForm(string bankUrl,string service, string amount,string currency, string desc, string userdata, string sign,string domain)
        {
            StringBuilder html = new StringBuilder();
            //html.AppendLine("<html><head><title>PostByCashBill</title></head>");
            //html.AppendLine(string.Format("<body onload=\"document.formpayment.submit()\">"));

            //html.AppendLine("<ul>");
            //html.AppendLine(string.Format("<li><strong>"+ "From : " + domain + "</strong> </li>"));
            //html.AppendLine(string.Format("<li><strong>" + "Amount : " + amount +" "+ currency + "</strong> </li>"));
            //html.AppendLine("</ul>");

            html.AppendLine(string.Format("<form id='formpayment' name='formpayment' action='"+bankUrl+"' method='POST'>"));
            html.AppendLine("<input type='hidden' name='service' value='" + service + "' />");
            html.AppendLine("<input type='hidden' name='amount' value='" + amount + "' />");
            html.AppendLine("<input type='hidden' name='desc' value='" + desc + "' />");
            html.AppendLine("<input type='hidden' name='userdata' value='" + userdata + "' />");
            html.AppendLine("<input type='hidden' name='sign' value='" + sign + "' />");

            html.AppendLine("</form>");
            //html.AppendLine("</body></html>");
            return html.ToString();
        }


    }

}
