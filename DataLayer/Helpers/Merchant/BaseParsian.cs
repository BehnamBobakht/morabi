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
    public class BaseParsian : BasePayment
    {
        public const short Successful = 0;
        public const short OrderIdDuplicated = -112;
        public const short InvalidLoginAccount = -126;
        public const short InvalidCallerIP = -127;
        public const short BatchBillPaymentRequestWasValidForSomeOfItems = -1554;

        public static PayingVm StartPayment(UnitOfWork _context, Payment payment)
        {

            var paying = new PayingVm();
            paying.Status = ResultStatus.Error;
            paying.Message = Resource.Notify.PaymentWasProblematic;
            try
            {
                string host = string.IsNullOrEmpty(payment.Merchant.ExternalInfo2) ? BaseWebsite.ShopUrl : payment.Merchant.ExternalInfo2;
                string redirectAddress = host + "payment/callback?id=" + payment.ID;
                long token = 0;
                short paymentStatus = Int16.MinValue;
                using (var service = new Services.OnlinePayments.Parsian.SaleService.SaleServiceSoapClient())
                {
                    System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback((o, xc, xch, sslP) => true);
                    var billRequest = new Services.OnlinePayments.Parsian.SaleService.ClientSaleRequestData
                    {
                        LoginAccount = payment.Merchant.Username,
                        CallBackUrl = redirectAddress,
                        Amount = (long)payment.Amount * 10,
                        OrderId = payment.ID,
                        AdditionalData = ""
                    };

                    var response = service.SalePaymentRequest(billRequest);
                    paymentStatus = response.Status;

                    if (response.Status == Successful)
                        token = response.Token;
                    else
                    {
                        paying.Message= "ERROR-" + response.Status;
                    }
                }

                if (paymentStatus == Successful && token != 0L)
                {
                    paying.Link = GetForm(payment.Merchant.Bank.PaymentUrl + token.ToString());
                    paying.Status = ResultStatus.Success;
                }
                else
                {
                    paying.Message = "ERROR" ;

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
            var Request = HttpContext.Current.Request;
            string status_started = Enum_Code.PAYMENT_STATUS_STARTED.ToString();

            if (payment != null)
            {
                var token = Convert.ToInt64(Request.Form["Token"]);
                var orderId = Convert.ToInt64(Request.Form["OrderId"]);
                var terminalNumber = Convert.ToInt32(Request.Form["TerminalNo"]);
                var rrn = Convert.ToInt64(Request.Form["RRN"]);
                var status = Convert.ToInt16(Request.Form["status"]);

                payment.RefNumber = rrn.ToString();
                payment.ExternalInfo2 = token.ToString();
                payment.ExternalInfo3 = terminalNumber.ToString();
                payment.ExternalInfo4 = orderId.ToString();
                payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY);
                _context.Save();

                if (status == Successful)
                {
                    using (var confirmSvc = new Services.OnlinePayments.Parsian.ConfirmService.ConfirmServiceSoapClient())
                    {
                        var confirmRequestData = new Services.OnlinePayments.Parsian.ConfirmService.ClientConfirmRequestData
                        {
                            Token = token,
                            LoginAccount = payment.Merchant.Username
                        };
                        var confirmResponse = confirmSvc.ConfirmPayment(confirmRequestData);
                        if (confirmResponse.Status == Successful)
                        {
                            try
                            {
                                payment.ExternalInfo1 = "پرداخت با موفقیت انجام شد";
                                payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_SUCCESSFUL);
                                BaseCart.DoPaymentCallBack(_context, payment);
                                _context.Save();
                            }
                            catch (Exception)
                            {
                                payment.ExternalInfo1 = "خطا در هنگام سرویس دهی";
                                payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_SERVICE_FAILED);
                                _context.Save();
                            }
                        }
                        else
                        {
                            payment.ExternalInfo1 = "خطا در هنگام بازگشت";
                            payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY_ERROR);
                            _context.Save();
                        }
                    }

                    payment = _context.Payment.GetById(payment.ID);
                    return payment;
                }
                else
                {
                    payment.ExternalInfo1 = "خطا در هنگام بازگشت";
                    payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY_ERROR);
                    _context.Save();
                    return payment;
                }
            }
            else
                return null;
        }

        private static string GetForm(string url)
        {
            StringBuilder html = new StringBuilder();
            html.AppendLine("<html><head></head>");
            //html.AppendLine(string.Format("<body onload=\"document.formpayment.submit()\">"));
            html.AppendLine(string.Format("<body>"));
            html.AppendLine(string.Format("<form name='formpayment' method='post' action='" + url + "' >"));
            html.AppendLine("</form>");
            html.AppendLine("</body></html>");
            return html.ToString();
        }
    }
}
