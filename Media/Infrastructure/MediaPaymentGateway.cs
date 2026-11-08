using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Script.Serialization;

namespace Media.Infrastructure
{
    public static class MediaPaymentGateway
    {
        private const double MinimumPaymentAmountInTomans = 5000d;
        private const string RegistrationEndedMessage = "اطلاعیه: این دوره جزو دوره‌های خاص است و ثبت‌نام دوره و دسترسی به فایل‌ها تنها از طریق مدیر دوره امکان‌پذیر است.";
        public static PayingVm Start(UnitOfWork context, Payment payment, string callbackUrl)
        {
            var result = new PayingVm { Status = ResultStatus.Error, Message = Resource.Notify.PaymentWasProblematic };
            string technicalError = null;
            try
            {
                if (payment.Merchant == null || payment.Merchant.Bank == null || payment.Merchant.Bank.Label != Enum_Bank.ZARINPAL.ToString())
                {
                    result.Message = "درگاه انتخاب‌شده برای زرین‌پال پیکربندی نشده است.";
                    return result;
                }

                if (payment.Amount < MinimumPaymentAmountInTomans)
                {
                    technicalError = "Payment amount is below the Media minimum: " + payment.Amount + " Toman.";
                    result.Message = RegistrationEndedMessage;
                    payment.ExternalInfo7 = technicalError;
                    payment.StatusId = context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_INSERTED);
                    context.Payment.Update(payment);
                    context.Save();
                    return result;
                }

                var serializer = new JavaScriptSerializer();
                var payload = serializer.Serialize(new
                {
                    merchant_id = payment.Merchant.MerchantId,
                    amount = AmountInRials(payment.Amount),
                    callback_url = callbackUrl,
                    description = payment.Description,
                    metadata = new { mobile = payment.Account == null ? "" : payment.Account.Mobile }
                });
                var responseText = PostJson(RequestUrl(), payload);
                var response = serializer.Deserialize<ZarinResponse>(responseText);
                if (DataCode(response) == 100 && !string.IsNullOrWhiteSpace(DataString(response, "authority")))
                {
                    payment.ExternalInfo5 = DataString(response, "authority");
                    payment.ExternalInfo7 = null;
                    payment.StatusId = context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_STARTED);
                    context.Payment.Update(payment);
                    context.Save();
                    result.Link = StartPayUrl() + payment.ExternalInfo5;
                    result.IsLink = true;
                    result.Status = ResultStatus.Success;
                    return result;
                }
                technicalError = ErrorMessage(response);
                result.Message = IsAmountError(technicalError) ? RegistrationEndedMessage : technicalError;
            }
            catch (Exception ex)
            {
                result.Status = ResultStatus.Exception;
                technicalError = ex.Message;
                result.Message = IsAmountError(technicalError) ? RegistrationEndedMessage : technicalError;
            }

            payment.ExternalInfo7 = string.IsNullOrWhiteSpace(technicalError) ? result.Message : technicalError;
            payment.StatusId = context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_INSERTED);
            context.Payment.Update(payment);
            context.Save();
            return result;
        }

        public static Payment Verify(UnitOfWork context, Payment payment, string status, string authority)
        {
            try
            {
                if (payment.Merchant == null || payment.Merchant.Bank == null || payment.Merchant.Bank.Label != Enum_Bank.ZARINPAL.ToString()) return payment;
                if (!string.Equals(status, "OK", StringComparison.OrdinalIgnoreCase) || string.IsNullOrWhiteSpace(authority) ||
                    (!string.IsNullOrWhiteSpace(payment.ExternalInfo5) && !string.Equals(payment.ExternalInfo5, authority, StringComparison.Ordinal)))
                {
                    payment.StatusId = context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY_ERROR);
                    context.Save();
                    return payment;
                }

                var serializer = new JavaScriptSerializer();
                var payload = serializer.Serialize(new
                {
                    merchant_id = payment.Merchant.MerchantId,
                    amount = AmountInRials(payment.Amount),
                    authority = authority
                });
                var responseText = PostJson(VerifyUrl(), payload);
                var response = serializer.Deserialize<ZarinResponse>(responseText);
                if (DataCode(response) == 100 || DataCode(response) == 101)
                {
                    payment.RefNumber = DataString(response, "ref_id");
                    payment.StatusId = context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_SUCCESSFUL);
                    CompleteOrderWithoutCart(context, payment);
                    context.Payment.Update(payment);
                    context.Save();
                }
                else
                {
                    payment.ExternalInfo7 = ErrorMessage(response);
                    payment.StatusId = context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY_ERROR);
                    context.Save();
                }
                return context.Payment.GetById(payment.ID);
            }
            catch (Exception ex)
            {
                payment.ExternalInfo7 = ex.Message;
                payment.StatusId = context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY_ERROR);
                context.Save();
                return payment;
            }
        }

        private static string PostJson(string url, string json)
        {
            using (var client = new HttpClient())
            using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
            {
                client.Timeout = TimeSpan.FromSeconds(30);
                var response = client.PostAsync(url, content).GetAwaiter().GetResult();
                var body = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                if (!response.IsSuccessStatusCode) throw new InvalidOperationException("ZarinPal HTTP " + (int)response.StatusCode + ": " + body);
                return body;
            }
        }

        private static string RequestUrl() { return "https://api.zarinpal.com/pg/v4/payment/request.json"; }
        private static string VerifyUrl() { return "https://api.zarinpal.com/pg/v4/payment/verify.json"; }
        private static string StartPayUrl() { return "https://www.zarinpal.com/pg/StartPay/"; }

        private static long AmountInRials(double amountInTomans)
        {
            return checked(Convert.ToInt64(amountInTomans * 10d));
        }

        private static bool IsAmountError(string error)
        {
            if (string.IsNullOrWhiteSpace(error)) return false;
            return error.IndexOf("amount", StringComparison.OrdinalIgnoreCase) >= 0 ||
                   error.IndexOf("مبلغ", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static int DataCode(ZarinResponse response)
        {
            var data = response == null ? null : response.data as IDictionary<string, object>;
            object value;
            return data != null && data.TryGetValue("code", out value) ? Convert.ToInt32(value) : 0;
        }

        private static string DataString(ZarinResponse response, string key)
        {
            var data = response == null ? null : response.data as IDictionary<string, object>;
            object value;
            return data != null && data.TryGetValue(key, out value) && value != null ? Convert.ToString(value) : null;
        }

        private static string ErrorMessage(ZarinResponse response)
        {
            var errors = response == null ? null : response.errors as IDictionary<string, object>;
            object code;
            object message;
            if (errors != null)
            {
                errors.TryGetValue("code", out code);
                errors.TryGetValue("message", out message);
                return "ZarinPal " + Convert.ToString(code) + ": " + Convert.ToString(message);
            }
            return "پاسخ معتبر از زرین‌پال دریافت نشد.";
        }

        private sealed class ZarinResponse
        {
            public object data { get; set; }
            public object errors { get; set; }
        }
        public static void CompleteOrderWithoutCart(UnitOfWork context, Payment payment)
        {
            var productOrder = payment.PaymentProductOrder.FirstOrDefault();
            if (productOrder == null) return;
            var parentOrder = productOrder.AccountOrder;
            parentOrder.StatusId = context.Code.GetIdByLabel(Enum_Code.ORDER_STATUS_SUCCESS);
            parentOrder.Datetime = DateTime.Now;
            foreach (var storeOrder in parentOrder.AccountOrder1)
            {
                storeOrder.StatusId = parentOrder.StatusId;
                foreach (var orderProduct in storeOrder.AccountOrderProduct)
                {
                    var quantity = context.StoreProductQuantity.GetById(orderProduct.StoreProductQuantityId);
                    if (quantity == null) continue;
                    quantity.Count = Math.Max(0, quantity.Count - orderProduct.Count);
                    context.StoreProductQuantity.Update(quantity);
                    context.Save();
                    BaseStore.UpdateProductQuantity(quantity.ProductQuantity.ProductId);
                }
            }
            context.AccountOrder.Update(parentOrder);
            context.Save();
        }
    }
}