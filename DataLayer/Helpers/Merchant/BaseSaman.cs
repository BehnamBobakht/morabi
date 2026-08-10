using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DataLayer.Helpers.Merchant
{
    public class BaseSaman : BasePayment
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
                //NameValueCollection datacollection = new NameValueCollection();
                //datacollection.Add("ResNum", CreateResnum().ToString());
                //datacollection.Add("MID", payment.Merchant.MerchantId);
                //datacollection.Add("Amount", (payment.Amount * 10).ToString());
                //datacollection.Add("RedirectURL", redirectAddress);
                //paying.Link = PreparePOSTForm(payment.Merchant.Bank.PaymentUrl, datacollection);

                Services.OnlinePayments.Saman.InitPayment.PaymentIFBindingSoapClient initPayment = new Services.OnlinePayments.Saman.InitPayment.PaymentIFBindingSoapClient();
                string token = initPayment.RequestToken(payment.Merchant.MerchantId, payment.Merchant.Password, Convert.ToInt64(amount), 0, 0, 0, 0, 0, 0, payment.Description, "", 0);


                NameValueCollection datacollection = new NameValueCollection();
                datacollection.Add("Token", token);
                datacollection.Add("RedirectURL", redirectAddress);
                paying.Link = PreparePOSTForm(payment.Merchant.Bank.PaymentUrl, datacollection);
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
            if (payment != null)
            {
                if (RequestFieldIsEmpty(payment))
                {
                    var Request = HttpContext.Current.Request;
                    string referenceNumber = Request.Form["RefNum"].ToString();
                    string reservationNumber = Request.Form["ResNum"].ToString();
                    string transactionState = Request.Form["state"].ToString();
                    string traceNumber = Request.Form["TraceNo"].ToString();
                    payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY);

                    if (transactionState.Equals("OK"))
                    {
                        if (_context.Payment.GetByReferenceNumber(referenceNumber) == null)
                        {
                            ServicePointManager.ServerCertificateValidationCallback =
                                delegate (object s,
                                    X509Certificate certificate,
                                    X509Chain chain,
                                    SslPolicyErrors sslPolicyErrors)
                                { return true; };

                            var srv = new Services.OnlinePayments.Saman.ReferecePayment.PaymentIFBindingSoapClient(); // new Sep.RefrencePayment.PaymentIFBinding();
                            var result = srv.verifyTransaction(Request.Form["RefNum"], Request.Form["MID"]);

                            if (result > 0)
                            {
                                if (result == (payment.Amount / 10))
                                {
                                    try
                                    {
                                        payment.ExternalInfo1 = "تراکنش موفقیت آمیز انجام شد";
                                        payment.RefNumber = referenceNumber;
                                        payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_SUCCESSFUL);
                                        BaseCart.DoPaymentCallBack(_context, payment);
                                        _context.Payment.Save();
                                    }
                                    catch (Exception)
                                    {
                                        payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_SERVICE_FAILED);
                                        _context.Save();
                                    }
                                }
                                else
                                {
                                    srv.reverseTransaction(Request.Form["RefNum"], Request.Form["MID"], payment.Merchant.MerchantId, payment.Merchant.Password);
                                    payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY_ERROR);
                                    _context.Save();
                                }
                            }
                            else
                            {
                                payment.ExternalInfo1 = TransactionChecking((int)result);
                                payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY_ERROR);
                                _context.Save();
                            }
                        }
                        else
                            return null;
                    }
                    else
                    {
                        payment.ExternalInfo1 = GetMessage(transactionState);
                        payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY);
                        _context.Save();
                    }
                }
                else
                {
                    payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY);
                    _context.Save();
                }
            }
            else
                return null;

            payment = _context.Payment.GetById(payment.ID);
            return payment;
        }

        private static string GetMessage(string transactionState)
        {
            if (transactionState.Equals("Canceled By User") || transactionState.Equals(string.Empty))
                return "تراكنش توسط خريدار كنسل شد";
            else if (transactionState.Equals("Invalid Amount"))
                return "مبلغ نامعتبر";
            else if (transactionState.Equals("Invalid Transaction"))
                return "تراکنش مورد نظر یافت نشد";
            else if (transactionState.Equals("Invalid Card Number"))
                return "شماره کارت صحیح وارد نشده است";
            else if (transactionState.Equals("No Such Issuer"))
                return "صادر کننده کارت بانکی معتبر نمی باشد";
            else if (transactionState.Equals("Expired Card Pick Up"))
                return "تاریخ انقضای کارت گذشته است";
            else if (transactionState.Equals("Allowable PIN Tries Exceeded Pick Up"))
                return "رمز بیش از 3 بار غلط وارد شده است";
            else if (transactionState.Equals("Incorrect PIN"))
                return "رمز صحیح وارد نشده است";
            else if (transactionState.Equals("Exceeds Withdrawal Amount Limit"))
                return "موجودی کافی نیست";
            else if (transactionState.Equals("Transaction Cannot Be Completed"))
                return "تراکنش تکمیل نشده است";
            else if (transactionState.Equals("Response Received Too Late"))
                return "تایم اوت";
            else if (transactionState.Equals("Suspected Fraud Pick Up"))
                return "مقادیر CVV2 و تاریخ انقضاء نادرست وارد شده اند";
            else if (transactionState.Equals("No Sufficient Funds"))
                return "موجودی حساب کافی نمی باشد";
            else if (transactionState.Equals("Issuer Down Slm"))
                return "سرور بانک با مشکل مواجه شده است";
            else if (transactionState.Equals("TME Error"))
                return "خطای نامشخص";
            else
                return "";
        }

        private static bool RequestFieldIsEmpty(Payment payment)
        {
            var Request = HttpContext.Current.Request;
            bool isError = false;
            if (Request.Form["state"].ToString().Equals(string.Empty))
            {
                isError = true;
                payment.ExternalInfo1 = "خريد شما توسط بانک تاييد شده است اما رسيد ديجيتالي شما تاييد نگشت! مشکلي در فرايند رزرو خريد شما پيش آمده است";
            }

            if (Request.Form["RefNum"].ToString().Equals(string.Empty) &&
                Request.Form["state"].ToString().Equals(string.Empty))
            {
                isError = true;
                payment.ExternalInfo1 = "فرايند انتقال وجه با موفقيت انجام شده است اما فرايند تاييد رسيد ديجيتالي با خطا مواجه گشت";
            }

            if (Request.Form["ResNum"].ToString().Equals(string.Empty) &&
                Request.Form["state"].ToString().Equals(string.Empty))
            {
                isError = true;
                payment.ExternalInfo1 = "خطا در برقرار ارتباط با بانک";
            }
            return isError;
        }

        private static string TransactionChecking(int i)
        {
            string errorMsg = "";
            switch (i)
            {
                case -1:		//TP_ERROR
                    errorMsg = "بروز خطا درهنگام بررسي صحت رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-1";
                    break;
                case -2:		//ACCOUNTS_DONT_MATCH
                    errorMsg = "بروز خطا در هنگام تاييد رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-2";
                    break;
                case -3:		//BAD_INPUT
                    errorMsg = "خطا در پردازش رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-3";
                    break;
                case -4:		//INVALID_PASSWORD_OR_ACCOUNT
                    errorMsg = "خطاي دروني سيستم درهنگام بررسي صحت رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-4";
                    break;
                case -5:		//DATABASE_EXCEPTION
                    errorMsg = "خطاي دروني سيستم درهنگام بررسي صحت رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-5";
                    break;
                case -7:		//ERROR_STR_NULL
                    errorMsg = "خطا در پردازش رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-7";
                    break;
                case -8:		//ERROR_STR_TOO_LONG
                    errorMsg = "خطا در پردازش رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-8";
                    break;
                case -9:		//ERROR_STR_NOT_AL_NUM
                    errorMsg = "خطا در پردازش رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-9";
                    break;
                case -10:	    //ERROR_STR_NOT_BASE64
                    errorMsg = "خطا در پردازش رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-10";
                    break;
                case -11:	    //ERROR_STR_TOO_SHORT
                    errorMsg = "خطا در پردازش رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-11";
                    break;
                case -12:		//ERROR_STR_NULL
                    errorMsg = "خطا در پردازش رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-12";
                    break;
                case -13:		//ERROR IN AMOUNT OF REVERS TRANSACTION AMOUNT
                    errorMsg = "خطا در پردازش رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-13";
                    break;
                case -14:	    //INVALID TRANSACTION
                    errorMsg = "خطا در پردازش رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-14";
                    break;
                case -15:	    //RETERNED AMOUNT IS WRONG
                    errorMsg = "خطا در پردازش رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-15";
                    break;
                case -16:	    //INTERNAL ERROR
                    errorMsg = "خطا در پردازش رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-16";
                    break;
                case -17:	    // REVERS TRANSACTIN FROM OTHER BANK
                    errorMsg = "خطا در پردازش رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-17";
                    break;
                case -18:	    //INVALID IP
                    errorMsg = "خطا در پردازش رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-18";
                    break;

            }
            return errorMsg;
        }

        private static String PreparePOSTForm(string url, NameValueCollection data)
        {
            string formID = "PostForm";
            StringBuilder strForm = new StringBuilder();
            strForm.Append("<form id=\"" + formID + "\" name=\"" + formID + "\" action=\"" + url + "\" method=\"POST\">");
            foreach (string key in data)
            {
                strForm.Append("<input type=\"hidden\" name=\"" + key + "\" value=\"" + data[key] + "\">");
            }
            strForm.Append("</form>");

            //StringBuilder strScript = new StringBuilder();
            //strScript.Append("<script language='javascript'>");
            //strScript.Append("var v" + formID + " = document." + formID + ";");
            //strScript.Append("v" + formID + ".submit();");
            //strScript.Append("</script>");
            //return strForm.ToString() + strScript.ToString();
            return strForm.ToString();
        }

        private static int CreateResnum()
        {
            return (int)(DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds;
        }
    }
}
