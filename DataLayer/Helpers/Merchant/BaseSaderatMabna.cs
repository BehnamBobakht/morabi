using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using Newtonsoft.Json;
using RestSharp;
//using DataLayer.Services.OnlinePayments.MabnaIPG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DataLayer.Helpers.Merchant
{
    public class BaseSaderatMabna : BasePayment
    {
        public static PayingVm StartPayment(UnitOfWork _context, Payment payment)
        {
            var paying = new PayingVm();
            paying.Status = ResultStatus.Error;
            paying.Message = Resource.Notify.PaymentWasProblematic;
            try
            {
                string host = string.IsNullOrEmpty(payment.Merchant.ExternalInfo2) ? BaseWebsite.ShopUrl : payment.Merchant.ExternalInfo2;
                string redirectAddress = host + "payment/callback?id=" + payment.ID; string amount = (payment.Amount * 10).ToString();

                string restClient = payment.Merchant.Bank.VerficationUrl;
                var client = new RestClient(restClient);
                var request = new RestRequest("", RestSharp.Method.POST);
                request.AddHeader("content-type", "application/x-www-form-urlencoded");

                request.AddParameter("Amount", payment.Amount * 10);
                request.AddParameter("callbackURL", redirectAddress);
                request.AddParameter("invoiceID", payment.ID);
                request.AddParameter("terminalID", payment.Merchant.MerchantId);

                IRestResponse response = client.Execute(request);
                dynamic result = JsonConvert.DeserializeObject(response.Content);

                int status = result.Status;
                if (status == 0)
                {
                    string token = result.Accesstoken;
                    payment.ExternalInfo1 = result.Accesstoken;
                    _context.Save();
                    paying.Link = GetForm(payment, token);
                    paying.Status = ResultStatus.Success;
                }

                paying.Message= status.ToString();
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
            var Request = HttpContext.Current.Request;
            string respCode = Request["respcode"];
            string respmsg = Request["respmsg"];
            string digitalReceipt = Request["digitalreceipt"];
            string tracenumber = Request["tracenumber"];

            payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY);
            payment.ExternalInfo2 = respCode + "-" + respmsg;
            payment.ExternalInfo3 = digitalReceipt;
            _context.Save();

            if (respCode != "0")
            {
                payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY_ERROR);
                _context.Save();
            }
            else
            {
                string restClient = payment.Merchant.Bank.Verfication2Url;
                var client = new RestClient(restClient);
                var request = new RestRequest("", RestSharp.Method.POST);
                request.AddHeader("content-type", "application/x-www-form-urlencoded");

                request.AddParameter("digitalreceipt", digitalReceipt);
                request.AddParameter("Tid", payment.Merchant.MerchantId);

                IRestResponse response = client.Execute(request);
                dynamic result = JsonConvert.DeserializeObject(response.Content);

                string verifyStatus = result.Status;
                if (verifyStatus != null && verifyStatus.ToUpper() != "OK")
                {
                    payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY_ERROR);
                    payment.ExternalInfo4 = result.ReturnId + "-" + result.Message;
                    _context.Save();
                }
                else
                {
                    try
                    {
                        payment.RefNumber = tracenumber;
                        payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_SUCCESSFUL);
                        _context.Payment.DoPaymentServices(_context, payment);
                        _context.Payment.Save();
                    }
                    catch (Exception)
                    {
                        payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_SERVICE_FAILED);
                        _context.Save();
                    }
                }
            }

            payment = _context.Payment.GetById(payment.ID);
            return payment;
        }


        private static string GetForm(Payment payment, string token)
        {
            StringBuilder html = new StringBuilder();
            html.AppendLine("<html><head></head>");
            html.AppendLine(string.Format("<body onload=\"document.formpayment.submit()\">"));
            html.AppendLine("<form name='formpayment' method='post' action='" + payment.Merchant.Bank.PaymentUrl + "'>");
            html.AppendLine("<input type='hidden' name='TerminalID' value='" + payment.Merchant.MerchantId + "' />");
            html.AppendLine("<input type='hidden' name='token' value='" + token + "' />");
            html.AppendLine("</form>");
            html.AppendLine("</body></html>");
            return html.ToString();
        }
    }
}
