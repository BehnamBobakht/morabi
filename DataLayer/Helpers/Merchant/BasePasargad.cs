using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace DataLayer.Helpers.Merchant
{

    public class BasePasargad : BasePayment
    {
        public bool Result { get; set; }
        public string Action { get; set; }
        public string TransactionReferenceID { get; set; }
        public int InvoiceNumber { get; set; }
        public string InvoiceDate { get; set; }
        public string MerchantCode { get; set; }
        public string TerminalCode { get; set; }
        public int Amount { get; set; }
        public int TraceNumber { get; set; }
        public int ReferenceNumber { get; set; }
        public DateTime TransactionDate { get; set; }

        public BasePasargad(string xmlResponse)
        {
            XDocument xmlDoc = XDocument.Parse(xmlResponse);
            if (xmlDoc != null)
            {
                this.Result = PasargadHelper.getBooleanValue(xmlDoc, "resultObj", "result");
                this.TransactionReferenceID = PasargadHelper.getStringValue(xmlDoc, "resultObj", "transactionReferenceID");
                this.InvoiceNumber = PasargadHelper.getIntegerValue(xmlDoc, "resultObj", "invoiceNumber");
                this.MerchantCode = PasargadHelper.getStringValue(xmlDoc, "resultObj", "merchantCode");
                this.TerminalCode = PasargadHelper.getStringValue(xmlDoc, "resultObj", "terminalCode");
                this.InvoiceDate = PasargadHelper.getStringValue(xmlDoc, "resultObj", "invoiceDate");
                this.Amount = PasargadHelper.getIntegerValue(xmlDoc, "resultObj", "amount");
                this.Action = PasargadHelper.getStringValue(xmlDoc, "resultObj", "action");
                this.TraceNumber = PasargadHelper.getIntegerValue(xmlDoc, "resultObj", "traceNumber");
                this.ReferenceNumber = PasargadHelper.getIntegerValue(xmlDoc, "resultObj", "referenceNumber");
                this.TransactionDate = PasargadHelper.getDatetimeValue(xmlDoc, "resultObj", "transactionDate");
            }
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
                string timeStamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                string invoiceDate = payment.Datetime.ToString("yyyy/MM/dd HH:mm:ss");
                string invoiceNumber = payment.ID.ToString();
                string ActionIs = "1003";

                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(payment.Merchant.PrivateKey);
                string data = "#" + payment.Merchant.MerchantId + "#" + payment.Merchant.TerminalKey + "#" + invoiceNumber + "#" + invoiceDate + "#" + amount + "#" + redirectAddress + "#" + ActionIs + "#" + timeStamp + "#";
                byte[] signedData = rsa.SignData(Encoding.UTF8.GetBytes(data), new SHA1CryptoServiceProvider());
                string signedString = Convert.ToBase64String(signedData);

                BasePayment dp = new BasePayment
                {
                    Url = payment.Merchant.Bank.PaymentUrl,
                    FormName = "formpayment",
                    Method = "post"
                };
                dp.AddKey("merchantCode", payment.Merchant.MerchantId);
                dp.AddKey("terminalCode", payment.Merchant.TerminalKey);
                dp.AddKey("amount", amount);
                dp.AddKey("redirectAddress", redirectAddress);
                dp.AddKey("invoiceNumber", invoiceNumber);
                dp.AddKey("invoiceDate", invoiceDate);
                dp.AddKey("action", ActionIs);
                dp.AddKey("sign", signedString);

                dp.AddKey("timeStamp", timeStamp);

               
                paying.Link = dp.Post();
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

        private static BasePasargad VerficationStep1(Payment payment)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(payment.Merchant.Bank.VerficationUrl);
                string text = "invoiceUID=" + HttpContext.Current.Request.QueryString["tref"];
                byte[] textArray = Encoding.UTF8.GetBytes(text);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = textArray.Length;
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(RemoteCertificateValidation);
                request.GetRequestStream().Write(textArray, 0, textArray.Length);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string xmlResponse = reader.ReadToEnd();
                return new BasePasargad(xmlResponse);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static PasargadResult VerificationStep2(Payment payment, BasePasargad entity)
        {
            try
            {
                string timeStamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(payment.Merchant.PrivateKey);
                string data = "#" + payment.Merchant.MerchantId + "#" + payment.Merchant.TerminalKey + "#" + entity.InvoiceNumber + "#" + entity.InvoiceDate + "#" + entity.Amount + "#" + timeStamp + "#";
                byte[] signedData = rsa.SignData(Encoding.UTF8.GetBytes(data), new SHA1CryptoServiceProvider());
                string signedString = Convert.ToBase64String(signedData);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://pep.shaparak.ir/VerifyPayment.aspx");
                string text = "InvoiceNumber=" + entity.InvoiceNumber + "&InvoiceDate=" + entity.InvoiceDate + "&MerchantCode=" + payment.Merchant.MerchantId + "&TerminalCode=" + payment.Merchant.TerminalKey + "&Amount=" + entity.Amount + "&TimeStamp=" + timeStamp + "&Sign=" + signedString;
                byte[] textArray = Encoding.UTF8.GetBytes(text);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = textArray.Length;
                request.GetRequestStream().Write(textArray, 0, textArray.Length);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string result = reader.ReadToEnd();
                return new PasargadResult(result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static bool RemoteCertificateValidation(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;
            return false;
        }

        public static Payment DoCallBack(UnitOfWork _context, Payment payment)
        {
            string msg = "";
            BasePasargad step1 = VerficationStep1(payment);
            if (step1 != null)
            {
                string status_started = Enum_Code.PAYMENT_STATUS_STARTED.ToString();
                int amount = (step1.Amount / 10);
                
                if (payment != null)
                {
                    payment.RefNumber = step1.TransactionReferenceID.ToString();
                    if (step1.Result == true)
                    {
                        payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY);
                        payment.ExternalInfo1 = step1.TraceNumber.ToString();
                        payment.ExternalInfo2 = step1.ReferenceNumber.ToString();
                        payment.ExternalInfo3 = step1.TransactionDate.ToString();
                        _context.Save();

                        PasargadResult step2 = VerificationStep2(payment, step1);
                        if (step2 != null)
                        {
                            if (step2.Result == true)
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
                            {
                                payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY_ERROR);
                            }

                            msg = step2.ResultMessage;
                            payment.ExternalInfo4 = step2.ResultMessage;
                        }
                    }
                    else
                    {
                        payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY_ERROR);
                        payment.ExternalInfo4 = "خطا در هنگام پرداخت";
                    }

                    _context.Save();
                    payment = _context.Payment.GetById(payment.ID);
                    return payment;
                }
                else
                    msg = "اطلاعات فاکتور صحیح نمی باشد";
            }
            return null;
        }
    }
    public class PasargadResult
    {
        public bool Result { get; set; }
        public string ResultMessage { get; set; }
        public PasargadResult(String xmlResponse)
        {
            XDocument xmlDoc = XDocument.Parse(xmlResponse);
            if (xmlDoc != null)
            {
                this.Result = PasargadHelper.getBooleanValue(xmlDoc, "actionResult", "result");
                this.ResultMessage = PasargadHelper.getStringValue(xmlDoc, "actionResult", "resultMessage");
            }
        }
    }

    public class PasargadHelper
    {
        public static DateTime getDatetimeValue(XDocument xmlDocument, string parent, string elementName)
        {
            var element = xmlDocument.Descendants(parent).Elements().Where(x => x.Name == elementName).FirstOrDefault();
            if (element != null)
            {
                /*
                if (element.Value.IsDatetime())
                {
                    return BaseDate.CastDate(element.Value);
                }
                */
            }
            return default(DateTime);
        }

        public static String getStringValue(XDocument xmlDocument, string parent, string elementName)
        {
            var element = xmlDocument.Descendants(parent).Elements().Where(x => x.Name == elementName).FirstOrDefault();
            return element != null ? element.Value : null;
        }

        public static long getLongValue(XDocument xmlDocument, string parent, string elementName)
        {
            var element = xmlDocument.Descendants(parent).Elements().Where(x => x.Name == elementName).FirstOrDefault();
            if (element != null)
            {
                if (element.Value.IsLong())
                {
                    return long.Parse(element.Value);
                }
            }
            return 0;
        }

        public static Boolean getBooleanValue(XDocument xmlDocument, string parent, string elementName)
        {
            var element = xmlDocument.Descendants(parent).Elements().Where(x => x.Name == elementName).FirstOrDefault();
            if (element != null)
            {
                if (element.Value.IsBoolean())
                {
                    return bool.Parse(element.Value);
                }
            }
            return false;
        }

        public static Int32 getIntegerValue(XDocument xmlDocument, string parent, string elementName)
        {
            var element = xmlDocument.Descendants(parent).Elements().Where(x => x.Name == elementName).FirstOrDefault();
            if (element != null)
            {
                if (element.Value.IsInteger())
                {
                    return int.Parse(element.Value);
                }
            }
            return 0;
        }
    }
}
