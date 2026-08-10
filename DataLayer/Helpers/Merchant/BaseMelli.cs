using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace DataLayer.Helpers.Merchant
{
    public class BaseMelli : BasePayment
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
                long Amount = (long)payment.Amount * 10;
                var dataBytes = Encoding.UTF8.GetBytes(string.Format("{0};{1};{2}", payment.Merchant.MerchantId, payment.ID, Amount));
                var symmetric = SymmetricAlgorithm.Create("TripleDes");
                symmetric.Mode = CipherMode.ECB;
                symmetric.Padding = PaddingMode.PKCS7;
                var encryptor = symmetric.CreateEncryptor(Convert.FromBase64String(payment.Merchant.TerminalKey), new byte[8]);

                var requestObject = new
                {
                    MerchantId = payment.Merchant.Username,
                    MerchantKey = payment.Merchant.TerminalKey,
                    TerminalId = payment.Merchant.MerchantId,
                    PurchasePage = payment.Merchant.Bank.PaymentUrl,
                    Amount = Amount,
                    OrderId = payment.ID,
                    LocalDateTime = DateTime.Now,
                    ReturnUrl = redirectAddress,
                    AdditionalData = payment.Description,
                    SignData = Convert.ToBase64String(encryptor.TransformFinalBlock(dataBytes, 0, dataBytes.Length))
                };

                System.Web.HttpCookie merchantTerminalKeyCookie = new System.Web.HttpCookie("Data", JsonConvert.SerializeObject(requestObject));
                HttpContext.Current.Response.Cookies.Add(merchantTerminalKeyCookie);

                var client = new RestClient(payment.Merchant.Bank.PaymentUrl);
                var request = new RestRequest("api/v0/Request/PaymentRequest", RestSharp.Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddJsonBody(requestObject);
                IRestResponse response = client.Execute(request);
                dynamic result = JsonConvert.DeserializeObject(response.Content);
                if (result != null && result.ResCode == 0)
                {
                    if (result.ResCode == "0")
                    {
                        paying.Link = string.Format("{0}/Purchase/Index?token={1}", payment.Merchant.Bank.PaymentUrl, result.Token);
                        paying.IsLink = true;
                        paying.Status = ResultStatus.Success;
                        paying.ReturnedCode = "0";
                    }

                    else
                    {
                        paying.ReturnedCode = result.ResCode;
                        paying.Message = result.ResCode;

                    }
                }
                else
                    paying.ReturnedCode = result.ResCode;
                    paying.Message = result.ResCode;
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
            var Token = HttpContext.Current.Request["Token"];
            try
            {
                payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY);
                _context.Save();

                var cookie = HttpContext.Current.Request.Cookies["Data"].Value;
                dynamic model = JsonConvert.DeserializeObject(cookie);
                var dataBytes = Encoding.UTF8.GetBytes(Token);
                var symmetric = SymmetricAlgorithm.Create("TripleDes");
                symmetric.Mode = CipherMode.ECB;
                symmetric.Padding = PaddingMode.PKCS7;
                var encryptor = symmetric.CreateEncryptor(Convert.FromBase64String(model.MerchantKey), new byte[8]);
                var signedData = Convert.ToBase64String(encryptor.TransformFinalBlock(dataBytes, 0, dataBytes.Length));
                var data = new
                {
                    Token = Token,
                    SignData = signedData
                };

                var client = new RestClient(payment.Merchant.Bank.PaymentUrl);
                var request = new RestRequest("api/v0/Advice/Verify", RestSharp.Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddJsonBody(data);
                IRestResponse response = client.Execute(request);
                dynamic result = JsonConvert.DeserializeObject(response.Content);
                if (result != null)
                {
                    if (result.ResCode == 0)
                    {
                        try
                        {
                            payment.ExternalInfo1 = "پرداخت با موفقیت انجام شد";
                            payment.ExternalInfo2 = result.SystemTraceNo;
                            payment.RefNumber = result.RetrivalRefNo;
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

        private static string GetForm(string url, string refId)
        {
            StringBuilder html = new StringBuilder();
            html.AppendLine("<html><head></head>");
            html.AppendLine(string.Format("<body onload=\"document.formpayment.submit()\">"));
            html.AppendLine(string.Format("<form name='formpayment' method='get' action='" + url + "' >"));
            html.AppendLine("<input type='hidden' name='token' value='" + refId + "' />");
            html.AppendLine("</form>");
            html.AppendLine("</body></html>");
            return html.ToString();
        }
    }
}
