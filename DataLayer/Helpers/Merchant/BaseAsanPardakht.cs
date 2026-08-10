
using DataLayer.Entities;
using System;
using System.Text;
using DataLayer.Services.OnlinePayments.AsanPardakht;
using System.Web;
using System.Net;
using System.Security.Cryptography;
using System.IO;
using DataLayer.Base;
using RestSharp;
using DataLayer.Enumarables;
using DataLayer.ViewModels;

namespace DataLayer.Helpers.Merchant
{
    public class BaseAsanPardakht : BasePayment
    {
        public static PayingVm StartPayment(UnitOfWork _context, Payment payment)
        {
            var paying = new PayingVm();
            paying.Status = ResultStatus.Error;
            try
            {
                paying.Message = Resource.Notify.PaymentWasProblematic;
                string host = string.IsNullOrEmpty(payment.Merchant.ExternalInfo2) ? BaseWebsite.ShopUrl : payment.Merchant.ExternalInfo2;
                string redirectAddress = host + "payment/callback?id=" + payment.ID;
                string mobile = payment.AccountId != null ? payment.Account.Mobile : "";
                string restClient = payment.Merchant.Bank.PaymentUrl;
                var client = new RestClient(restClient);
                var request = new RestRequest("", RestSharp.Method.POST);
                request.Parameters.Clear();
                string localdate = DateTime.Now.Year.ToString("0000") +
                                   DateTime.Now.Month.ToString("00") +
                                   DateTime.Now.Day.ToString("00") + " " +
                                   DateTime.Now.Hour.ToString("00") +
                                   DateTime.Now.Minute.ToString("00") +
                                   DateTime.Now.Second.ToString("00");
                var entity = new
                {
                    merchantConfigurationId = payment.Merchant.ExternalInfo1,
                    serviceTypeId = 1,
                    localInvoiceId = payment.ID,
                    amountInRials = payment.Amount * 10,
                    localDate = localdate,
                    additionalData = payment.Description,
                    callbackURL = redirectAddress,
                    paymentId = payment.ID
                };
                request.AddHeader("usr", payment.Merchant.Username);
                request.AddHeader("pwd", payment.Merchant.Password);
                request.RequestFormat = DataFormat.Json;
                request.AddBody(entity);
                IRestResponse response = client.Execute(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string token = response.Content;
                    paying.Link = GetForm(token, mobile);
                    paying.Status = ResultStatus.Success;
                    return paying;

                }
                else
                {
                    paying.Status = ResultStatus.Error;
                    paying.ReturnedCode = response.Content;
                    paying.Message = Resource.Notify.PaymentWasProblematic;
                    payment.ExternalInfo7 = response.Content;
                    _context.Payment.Update(payment);
                    _context.Save();
                    return paying;
                }
            }
            catch (Exception ex)
            {
                paying.Status = ResultStatus.Exception;
                paying.Message = Resource.Notify.PaymentWasProblematic;
                payment.ExternalInfo7 = ex.Message;
                _context.Payment.Update(payment);
                _context.Save();

            }
            return paying;
        }

        private static string GetForm(string token, string mobile)
        {
            StringBuilder html = new StringBuilder();
            html.AppendLine("<html><head></head>");
            html.AppendLine(string.Format("<body onload=\"document.formpayment.submit()\">"));
            html.AppendLine(string.Format("<form name='formpayment' method='post' action='https://asan.shaparak.ir' >"));
            html.AppendLine("<input type='hidden' name='RefId' value=" + token + " />");
            html.AppendLine("<input type='hidden' name='mobileap' value=" + mobile + " />");
            html.AppendLine("</form>");
            html.AppendLine("</body></html>");
            return html.ToString();
        }

        public static Payment DoCallBack(UnitOfWork _context, Payment payment)
        {
            try
            {
                String test = HttpContext.Current.Request.QueryString["PayGateTranID"];
                payment.ExternalInfo2 = test;
                var form = HttpContext.Current.Request.Form;

                string payGateTranID = form["PayGateTranID"];
                payment.ExternalInfo1 = payGateTranID;
                payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY);
                string restClient = payment.Merchant.Bank.VerficationUrl;
                var client = new RestClient(restClient);
                var request = new RestRequest("", RestSharp.Method.POST);
                var entity = new
                {
                    merchantConfigurationId = payment.Merchant.ExternalInfo1,
                    payGateTranId = payGateTranID
                };
                request.AddHeader("usr", payment.Merchant.Username);
                request.AddHeader("pwd", payment.Merchant.Password);
                request.RequestFormat = DataFormat.Json;
                request.AddBody(entity);
                IRestResponse response = client.Execute(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    try
                    {
                        payment.RefNumber = payGateTranID;
                        payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_SUCCESSFUL);
                        _context.Payment.DoPaymentServices(_context, payment);
                        _context.Payment.Save();
                    }
                    catch (Exception ex)
                    {
                        payment.ExternalInfo1 = ex.ToString();
                        payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_SERVICE_FAILED);
                    }
                }
            }
            catch (Exception ex)
            {
                payment.ExternalInfo1 = ex.ToString();
                payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY_ERROR);
            }

            _context.Save();
            payment = _context.Payment.GetById(payment.ID);
            return payment;
        }

    }
}
