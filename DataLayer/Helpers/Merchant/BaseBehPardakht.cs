using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

namespace DataLayer.Helpers.Merchant
{
    public class BaseBehPardakht : BasePayment
    {
        public static PayingVm StartPayment(UnitOfWork _context, Payment payment)
        {
            var paying = new PayingVm();
            paying.Status = ResultStatus.Error;
            paying.Message = Resource.Notify.PaymentWasProblematic;
            try
            {
                string host = string.IsNullOrEmpty(payment.Merchant.ExternalInfo2)?BaseWebsite.ShopUrl: payment.Merchant.ExternalInfo2;
                string redirectAddress = host + "payment/callback?id=" + payment.ID;
                var client = new Services.OnlinePayments.BehPardakht.PaymentGatewayClient();
                string result = client.bpPayRequest(
                    Int64.Parse(payment.Merchant.MerchantId),
                    payment.Merchant.Username,
                    payment.Merchant.Password,
                    payment.ID,
                    (long)payment.Amount * 10,
                    GetDate(payment.Datetime),
                    GetTime(payment.Datetime),
                    payment.Description,
                    redirectAddress,
                    Int64.Parse("0")
                );

                if (result != null)
                {
                    string[] ResultArray = result.Split(',');
                    if (ResultArray[0].ToString() == "0")
                    {
                        paying.Link= GetForm(payment.Merchant.Bank.PaymentUrl, ResultArray[1]);
                        paying.Status = ResultStatus.Success;
                        paying.ReturnedCode = "0";
                    }
                    else
                    {
                        paying.Status = ResultStatus.Error;
                        paying.ReturnedCode = ResultArray[0].ToString();
                        paying.Message = Resource.Notify.PaymentWasProblematic;
                        payment.ExternalInfo7 = result;
                        _context.Payment.Update(payment);
                        _context.Save();
                    }
                }
               
            }
            catch(Exception ex)
            {
                paying.Status = ResultStatus.Exception;
                paying.Message = Resource.Notify.PaymentWasProblematic;
                payment.ExternalInfo7 = ex.Message;
                _context.Payment.Update(payment);
                _context.Save();

            }
            return paying;
        }

        public static Payment DoCallBack(UnitOfWork _context, Payment payment)
        {
            var Request = HttpContext.Current.Request;
            BypassCertificateError();

            string status_started = Enum_Code.PAYMENT_STATUS_STARTED.ToString();

            if (payment != null)
            {
                if (string.IsNullOrEmpty(Request.Params["SaleReferenceId"]))
                {
                    if (!string.IsNullOrEmpty(Request.Params["ResCode"]))
                    {
                        payment.ExternalInfo1 = MellatResult(Request.Params["ResCode"]);
                        payment.ExternalInfo2 = Request.Params["ResCode"];
                    }
                    else
                    {
                        payment.ExternalInfo1 = "رسید قابل قبول نیست";
                    }
                }
                else
                {
                    try
                    {
                        string TerminalId = payment.Merchant.MerchantId;
                        string UserName = payment.Merchant.Username;
                        string UserPassword = payment.Merchant.Password;
                        long SaleOrderId = 0;  //PaymentID 
                        long SaleReferenceId = 0;
                        string RefId = null;
                        try
                        {
                            SaleOrderId = long.Parse(Request.Params["SaleOrderId"].TrimEnd());
                            SaleReferenceId = long.Parse(Request.Params["SaleReferenceId"].TrimEnd());
                            RefId = Request.Params["RefId"].TrimEnd();
                        }
                        catch (Exception)
                        {
                            payment.ExternalInfo1 = "وضعیت:مشکلی در پرداخت بوجود آمده ، در صورتی که وجه پرداختی از حساب بانکی شما کسر شده است آن مبلغ به صورت خودکار برگشت داده خواهد شد";
                        }

                        payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY);
                        _context.Save();
                        var bpService = new Services.OnlinePayments.BehPardakht.PaymentGatewayClient();

                        string Result;
                        Result = bpService.bpVerifyRequest(
                                    long.Parse(TerminalId),
                                    UserName,
                                    UserPassword,
                                    SaleOrderId,
                                    SaleOrderId,
                                    SaleReferenceId);

                        if (!string.IsNullOrEmpty(Result))
                        {
                            if (Result == "0")
                            {
                                string IQresult;
                                IQresult = bpService.bpInquiryRequest(long.Parse(TerminalId), UserName, UserPassword, SaleOrderId, SaleOrderId, SaleReferenceId);

                                if (IQresult == "0")
                                {
                                    long paymentID = Convert.ToInt64(SaleOrderId);
                                    payment.RefNumber = SaleReferenceId.ToString();
                                    payment.ExternalInfo1 = "پرداخت با موفقیت انجام شد.";
                                    payment.ExternalInfo2 = Request.Params["ResCode"];
                                    payment.ExternalInfo3 = RefId;
                                    string Sresult = bpService.bpSettleRequest(long.Parse(TerminalId), UserName, UserPassword, SaleOrderId, SaleOrderId, SaleReferenceId);
                                    if (Sresult != null)
                                    {
                                        if (Sresult == "0" || Sresult == "45")
                                        {
                                            //تراکنش تایید و ستل شده است 
                                        }
                                        else
                                        {
                                            //تراکنش تایید شده ولی ستل نشده است
                                        }
                                        payment.ExternalInfo4 = Sresult;
                                    }

                                    try
                                    {
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
                                    string Rvresult = bpService.bpReversalRequest(long.Parse(TerminalId), UserName, UserPassword, SaleOrderId, SaleOrderId, SaleReferenceId);
                                    payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY_ERROR);
                                    payment.ExternalInfo1 = "تراکنش بازگشت داده شد";
                                    long paymentId = Convert.ToInt64(SaleOrderId);
                                    _context.Save();
                                }
                            }
                            else
                            {
                                // TODO SAVE
                                long paymentId = Convert.ToInt64(SaleOrderId);
                                payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY_ERROR);
                                payment.ExternalInfo1 = MellatResult(Result);
                            }
                        }
                        else
                        {
                            payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY_ERROR);
                            payment.ExternalInfo1 = "شماره رسید قابل قبول نیست";
                        }

                    }
                    catch (Exception)
                    {
                        payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY_ERROR);
                        payment.ExternalInfo1 = "مشکلی در پرداخت به وجود آمده است ، در صورتیکه وجه پرداختی از حساب بانکی شما کسر شده است آن مبلغ به صورت خودکار برگشت داده خواهد شد";
                    }
                }

                _context.Save();
                payment = _context.Payment.GetById(payment.ID);
                return payment;
            }
            else
                return null;
        }

        static void BypassCertificateError()
        {
            ServicePointManager.ServerCertificateValidationCallback +=
                delegate (
                    Object sender1,
                    X509Certificate certificate,
                    X509Chain chain,
                    SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };
        }

        private static string GetForm(string url, string refId)
        {
            StringBuilder html = new StringBuilder();
            html.AppendLine("<html><head></head>");
            //html.AppendLine(string.Format("<body onload=\"document.formpayment.submit()\">"));
            html.AppendLine(string.Format("<body>"));
            html.AppendLine(string.Format("<form name='formpayment' method='post' action='" + url + "' >"));
            html.AppendLine("<input type='hidden' name='RefId' value='" + refId + "' />");
            html.AppendLine("</form>");
            html.AppendLine("</body></html>");
            return html.ToString();
        }

        private static string GetDate(DateTime datetime)
        {
            return 
                datetime.Year.ToString() + 
                datetime.Month.ToString().PadLeft(2, '0') + 
                datetime.Day.ToString().PadLeft(2, '0');
        }

        private static string GetTime(DateTime datetime)
        {
            return 
                datetime.Hour.ToString().PadLeft(2, '0') + 
                datetime.Minute.ToString().PadLeft(2, '0') + 
                datetime.Second.ToString().PadLeft(2, '0');
        }

        public static string MellatResult(string ID)
        {
            string result = "";
            switch (ID)
            {
                case "-100":
                    result = "پرداخت لغو شده";
                    break;
                case "0":
                    result = "تراكنش با موفقيت انجام شد";
                    break;
                case "11":
                    result = "شماره كارت نامعتبر است ";
                    break;
                case "12":
                    result = "موجودي كافي نيست ";
                    break;
                case "13":
                    result = "رمز نادرست است ";
                    break;
                case "14":
                    result = "تعداد دفعات وارد كردن رمز بيش از حد مجاز است ";
                    break;
                case "15":
                    result = "كارت نامعتبر است ";
                    break;
                case "16":
                    result = "دفعات برداشت وجه بيش از حد مجاز است ";
                    break;
                case "17":
                    result = "كاربر از انجام تراكنش منصرف شده است ";
                    break;
                case "18":
                    result = "تاريخ انقضاي كارت گذشته است ";
                    break;
                case "19":
                    result = "مبلغ برداشت وجه بيش از حد مجاز است ";
                    break;
                case "111":
                    result = "صادر كننده كارت نامعتبر است ";
                    break;
                case "112":
                    result = "خطاي سوييچ صادر كننده كارت ";
                    break;
                case "113":
                    result = "پاسخي از صادر كننده كارت دريافت نشد ";
                    break;
                case "114":
                    result = "دارنده كارت مجاز به انجام اين تراكنش نيست";
                    break;
                case "21":
                    result = "پذيرنده نامعتبر است ";
                    break;
                case "23":
                    result = "خطاي امنيتي رخ داده است ";
                    break;
                case "24":
                    result = "اطلاعات كاربري پذيرنده نامعتبر است ";
                    break;
                case "25":
                    result = "مبلغ نامعتبر است ";
                    break;
                case "31":
                    result = "پاسخ نامعتبر است ";
                    break;
                case "32":
                    result = "فرمت اطلاعات وارد شده صحيح نمي باشد ";
                    break;
                case "33":
                    result = "حساب نامعتبر است ";
                    break;
                case "34":
                    result = "خطاي سيستمي ";
                    break;
                case "35":
                    result = "تاريخ نامعتبر است ";
                    break;
                case "41":
                    result = "شماره درخواست تكراري است ، دوباره تلاش کنید";
                    break;
                case "42":
                    result = "يافت نشد  Sale تراكنش";
                    break;
                case "43":
                    result = "داده شده است  Verify قبلا درخواست";
                    break;
                case "44":
                    result = "يافت نشد  Verfiy درخواست";
                    break;
                case "45":
                    result = "شده است  Settle تراكنش";
                    break;
                case "46":
                    result = "نشده است  Settle تراكنش";
                    break;
                case "47":
                    result = "يافت نشد  Settle تراكنش";
                    break;
                case "48":
                    result = "شده است  Reverse تراكنش";
                    break;
                case "49":
                    result = "يافت نشد  Refund تراكنش";
                    break;
                case "412":
                    result = "شناسه قبض نادرست است ";
                    break;
                case "413":
                    result = "شناسه پرداخت نادرست است ";
                    break;
                case "414":
                    result = "سازمان صادر كننده قبض نامعتبر است ";
                    break;
                case "415":
                    result = "زمان جلسه كاري به پايان رسيده است ";
                    break;
                case "416":
                    result = "خطا در ثبت اطلاعات ";
                    break;
                case "417":
                    result = "شناسه پرداخت كننده نامعتبر است ";
                    break;
                case "418":
                    result = "اشكال در تعريف اطلاعات مشتري ";
                    break;
                case "419":
                    result = "تعداد دفعات ورود اطلاعات از حد مجاز گذشته است ";
                    break;
                case "421":
                    result = "نامعتبر است  IP";
                    break;
                case "51":
                    result = "تراكنش تكراري است ";
                    break;
                case "54":
                    result = "تراكنش مرجع موجود نيست ";
                    break;
                case "55":
                    result = "تراكنش نامعتبر است ";
                    break;
                case "61":
                    result = "خطا در واريز ";
                    break;
                default:
                    result = string.Empty;
                    break;
            }
            return result;
        }
    }
}
