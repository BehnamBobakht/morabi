using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.ViewModels;
using ExpressMapper.Extensions;

namespace DataLayer.Repositories
{
    public class Entity_Payment : BaseRepository<Payment>
    {
        private DatabaseEntities _context;
        public Entity_Payment(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public PaymentVm GetPaymentVm(int id)
        {
            var model = GetById(id);
            if (model != null)
            {
                return model.Map<Payment, PaymentVm>();
            }
            return null;
        }
        public string CreateExternal(PaymentWebsite paymentWebsite, string merchant, int amount, string currency, string orderId, string sign)
        {
            var merchantId = _context.Merchant.FirstOrDefault(s => s.Bank.Label.ToLower() == merchant.ToLower());
            if (merchantId == null)
            {
                return null;
            }
            var currencyId = _context.Code.FirstOrDefault(s => s.Label.ToLower() == currency.ToLower());
            if (currencyId == null)
            {
                return null;
            }
            Code statusEntity = _context.Code.FirstOrDefault(p => p.Label == "PAYMENT_STATUS_INSERTED");
            PaymentSubject paymentSubject = _context.PaymentSubject.FirstOrDefault(s => s.Label == "EXTERNAL");
            var payment = new Payment()
            {
                Datetime = DateTime.Now,
                Amount = amount,
                CurrencyTypeId = currencyId.ID,
                MerchantId = merchantId.ID,
                PaymentWebsiteId = paymentWebsite.ID,
                SubjectId = paymentSubject.ID,
                IpAddress = paymentWebsite.Ip,
                StatusId = statusEntity.ID,
                ExternalInfo5 = Guid.NewGuid().ToString(),
                ExternalInfo6 = orderId,
                ExternalInfo7 = sign,
            };
            Insert(payment);
            return payment.ExternalInfo5;
        }
        public Payment GetByTokenAndSign(string token, string sign)
        {
            return _context.Payment.FirstOrDefault(s => s.ExternalInfo5 == token && s.ExternalInfo7 == sign);

        }
        public Payment GetByReferenceNumber(string refNumber)
        {
            return _context.Payment.FirstOrDefault(p => p.RefNumber == refNumber);
        }

        public Payment CreatePayment(int? accountId, int? userId, int subjectId, double price, int? merchantId,
            string description = null, string payment_status = "PAYMENT_STATUS_STARTED")
        {
            Code statusEntity = _context.Code.FirstOrDefault(p => p.Label == payment_status);
            Payment payment = new Payment()
            {
                StatusId = statusEntity.ID,
                MerchantId = merchantId,
                SubjectId = subjectId,
                Amount = price,
                IpAddress = BaseSecurity.GetClientIPAddress(),
                Datetime = DateTime.Now,
                Description = description
            };

            if (accountId != null)
                payment.AccountId = accountId.GetValueOrDefault();
            if (userId != null)
                payment.SiteUserId = userId.GetValueOrDefault();
            Insert(payment);
            Save();
            return payment;
        }

        public void DoPaymentServices(UnitOfWork mainContext, Payment payment)
        {
            if (Base.BaseWebsite.WebsiteSetting.HasSendByPost)
            {
                mainContext.Account.RemoveSendTypeStore();
            }

            if (payment.PaymentSubject.Label == Enum_PaymentSubject.ORDER.ToString())
            {
                int orderId = 0;
                double price = 0;
                PaymentProductOrder productOrder = payment.PaymentProductOrder.FirstOrDefault();
                AccountOrder order = null;

                if (productOrder != null)
                {

                    order = productOrder.AccountOrder;
                    order.StatusId = mainContext.Code.GetIdByLabel(Enum_Code.ORDER_STATUS_SUCCESS);
                    order.AccountOrder1.ToList().ForEach(s => s.StatusId = order.StatusId);
                    order.Datetime = DateTime.Now;
                    //change abak
                    var accountOrderSub = order.AccountOrder1.FirstOrDefault();
                    orderId = accountOrderSub != null ? accountOrderSub.ID : order.ID;
                    price = order.Price;
                    Save();
                }

                mainContext.AccountBasket.ClearBasket(payment.AccountId.Value);
                if (order.AccountOrder1.Any())
                {
                    foreach (var storeOrder in order.AccountOrder1)
                    {
                        foreach (AccountOrderProduct orderProduct in storeOrder.AccountOrderProduct)
                        {
                            var quantity = mainContext.StoreProductQuantity.FirstOrDefault(p =>
                                p.ProductQuantity.ProductId == orderProduct.ProductId &&

                                p.StoreProduct.StoreId == orderProduct.AccountOrder.StoreId);
                            if (quantity != null)
                            {
                                quantity.Count = quantity.Count - orderProduct.Count;
                                if (quantity.Count < 0)
                                    quantity.Count = 0;
                                mainContext.StoreProductQuantity.Update(quantity);
                                Save();
                                Base.BaseStore.UpdateProductQuantity(quantity.ProductQuantity.ProductId);

                            }

                        }
                    }
                }

                else
                {
                    foreach (AccountOrderProduct orderProduct in order.AccountOrderProduct)
                    {
                        ProductQuantity quantity = mainContext.ProductQuantity.FirstOrDefault(p =>
                            p.ProductId == orderProduct.ProductId

                            );

                        if (quantity != null)
                        {
                            quantity.Count = quantity.Count - orderProduct.Count;
                            mainContext.ProductQuantity.Update(quantity);
                            Save();
                            Base.BaseStore.UpdateProductQuantity(quantity.ProductId);
                        }

                    }

                }


                ViewWebsite website = BaseWebsite.ShopWebsite;
                string emailBody = getOrderEmail(order);
                try
                {
                    StringBuilder str_1 = new StringBuilder();
                    string token_1 = payment.RefNumber;
                    string token_2 = "";
                    string token_3 = "";
                    str_1.AppendLine("پرداخت شما در سایت " + website.Title + " با موفقیت انجام شد");
                    str_1.AppendLine("کد رهگیری پرداخت: " + token_1);
                    mainContext.Sms.SaveNewSms(payment.Account.Mobile, Enum_SmsType.PAYMENT_SUCCESS, str_1.ToString(), token_1, token_2, token_3);
                    foreach (var item in payment.PaymentProductOrder)
                    {
                        string mobileNo = item.AccountOrder.Store.Account.Mobile;
                        mainContext.Sms.SaveNewSms(mobileNo, Enum_SmsType.PAYMENT_SUCCESS, str_1.ToString(), token_1, token_2, token_3);
                    }

                    mainContext.Email.SaveNewEmail(payment.Account.Email, Enum_EmailType.PAYMENT_SUCCESS, emailBody, "پرداخت موفقیت آمیز");
                    Save();
                }
                catch (Exception) { }

                try
                {
                    StringBuilder str_2 = new StringBuilder();
                    string token_1 = orderId.ToPersian();
                    string token_2 = price.GetCurrencyFormat().ToPersian();
                    string token_3 = "";
                    str_2.AppendLine("سفارش جدید با کد " + token_1 + /*" مبلغ " + token_2 +*/ " در سایت " + website.Title + " ثبت شد");
                    List<SiteUser> listUser = mainContext.SiteUser.GetAllByUserRole(Enum_UserRole.ADMIN);
                    foreach (SiteUser item in listUser.Where(p => p.Mobile != null))
                    {
                        //if (shop.ShopUser.Any(p => p.UserId == item.ID))
                        //{
                        mainContext.Sms.SaveNewSms(item.Mobile, Enum_SmsType.PAYMENT_SUCCESS_ADMIN, str_2.ToString(), token_1, token_2, token_3);
                        mainContext.Email.SaveNewEmail(item.Email, Enum_EmailType.PAYMENT_SUCCESS_ADMIN, emailBody, "خرید موفقیت آمیز");
                        //}
                    }
                }
                catch (Exception) { }

                try
                {
                    StringBuilder str_2 = new StringBuilder();
                    string token_1 = orderId.ToPersian();
                    string token_2 = price.GetCurrencyFormat().ToPersian();
                    string token_3 = "";
                    str_2.AppendLine("سفارش جدید با کد " + token_1 + /*" مبلغ " + token_2 +*/ " در سایت " + website.Title + " ثبت شد");
                    foreach (var storeOrder in order.AccountOrder1)
                    {

                        mainContext.Sms.SaveNewSms(storeOrder.Store.Account.Mobile, Enum_SmsType.PAYMENT_SUCCESS_Store, str_2.ToString(), token_1, token_2, token_3);


                    }

                }
                catch (Exception) { }

                Save();
            }
            else if (payment.PaymentSubject.Label == Enum_PaymentSubject.CREDIT.ToString())
            {
                mainContext.AccountCredit.CreateCredit(payment.AccountId.Value, payment.Amount, payment.Description, payment.ID);
                Save();
            }
        }

        private string getOrderEmail(AccountOrder order)
        {
            StringBuilder html = new StringBuilder();
            try
            {
                html.Append("<b>شماره فاکتور: </b>" + order.ID + "<br />");
                html.Append("<b>تاریخ فاکتور: </b>" + order.Datetime.ToPersianComplete() + order.Datetime.ToString("HH:mm") + "<br />");
                html.Append("<b>وضعیت فاکتور: </b>" + order.Code.Name + "<br />");
                html.Append("<hr />");

                html.Append("<b>خریدار: </b>" + order.Account.FullName + "&nbsp;&nbsp;&nbsp;");
                html.Append("<b>شماره تماس: </b>" + order.Account.Mobile + "&nbsp;&nbsp;&nbsp;");
                html.Append("<b>آدرس: </b>" + order.AccountAddress.AddressValue + "<br />");
                html.Append("<b>شماره تلفن: </b>" + order.AccountAddress.Phone + "&nbsp;&nbsp;&nbsp;");
                html.Append("<b>تلفن همراه: </b>" + order.AccountAddress.Mobile + "&nbsp;&nbsp;&nbsp;");
                html.Append("<b>کدپستی: </b>" + order.AccountAddress.PostalCode + "&nbsp;&nbsp;&nbsp;");
                html.Append("<b>نام تحویل گیرنده: </b>" + order.AccountAddress.NameFamily);
                html.Append("<hr />");

                string payment_status = Enum_Code.PAYMENT_STATUS_SUCCESSFUL.ToString();
                PaymentProductOrder paymentOrder = order.PaymentProductOrder.FirstOrDefault(p =>
                    p.Payment.Code.Label == payment_status
                );
                if (paymentOrder != null)
                {
                    Payment payment = paymentOrder.Payment;
                    html.Append("<b>وضعیت پرداخت: </b>" + payment.Code.Name + "&nbsp;&nbsp;&nbsp;");
                    html.Append("<b>بانک مرجع: </b>" + payment.Merchant.Bank.Name + "&nbsp;&nbsp;&nbsp;");
                    html.Append("<b>کد رهگیری پرداخت: </b>" + payment.RefNumber);
                    html.Append("<hr />");
                }

                html.Append("<table cellpadding='5' cellspacing='1' style='width: 100%; border:1px solid black; text-align: center; font-family:\'b nazanin\''>");
                html.Append("<thead>");
                html.Append("<tr>");
                html.Append("<th style='border: 1px solid black;'>#</th>");
                html.Append("<th style='border: 1px solid black;'>نام کالا</th>");
                html.Append("<th style='border: 1px solid black;'>تعداد</th>");
                html.Append("<th style='border: 1px solid black;'>مبلغ واحد (تومان)</th>");
                html.Append("<th style='border: 1px solid black;'>درصد تخفیف</th>");
                html.Append("<th style='border: 1px solid black;'>مبلغ با تخفیف</th>");
                html.Append("<th style='border: 1px solid black;'>مبلغ نهایی (تومان)</th>");
                html.Append("</tr>");
                html.Append("</thead>");

                html.Append("<tbody>");

                int index = 1;
                foreach (AccountOrderProduct product in order.AccountOrderProduct)
                {
                    html.Append("<tr>");
                    html.Append("<td style='border: 1px solid black;'>" + index.ToPersian() + "</td>");
                    html.Append("<td style='border: 1px solid black;'>");
                    html.Append(product.Product.Name);

                    html.Append("</td>");
                    html.Append("<td style='border: 1px solid black;'>" + product.Count.ToPersian() + " </td>");
                    html.Append("<td style='border: 1px solid black;'>" + product.ProductPrice.GetCurrencyFormat().ToPersian() + " تومان</td>");

                    if (product.ProductDiscount != 0)
                    {
                        html.Append("<td style='border: 1px solid black;'>" + ((product.ProductDiscount * 100) / product.ProductPrice) + "%</td>");
                    }
                    else
                    {
                        html.Append("<td style='border: 1px solid black;'>-</td>");
                    }

                    html.Append("<td style='border: 1px solid black;'>" + ((product.ProductPrice - product.ProductDiscount).GetCurrencyFormat().ToPersian()) + "تومان</td>");
                    html.Append("<td style='border: 1px solid black;'>" + ((product.Price).GetCurrencyFormat().ToPersian()) + "تومان </td>");
                    html.Append("<tr>");
                    index++;
                }
                html.Append("</tbody>");
                html.Append("<tfoot>");

                if (order.DiscountPrice != 0)
                {
                    html.Append("<tr>");
                    html.Append("<td style='border: 1px solid black;' colspan='6'>میزان تخفیف سایت</td>");
                    html.Append("<td style='border: 1px solid black;'>" + order.DiscountPrice.GetCurrencyFormat().ToPersian() + " تومان</td>");
                    html.Append("</tr>");
                }
                if (order.RebateId != null)
                {
                    html.Append("<tr>");
                    html.Append("<td style='border: 1px solid black;' colspan='6'>کد تخفیف (" + order.Rebate.Name + ")</td>");
                    html.Append("<td style='border: 1px solid black;'>" + (order.RebatePrice.GetCurrencyFormat().ToPersian()) + " تومان</td>");
                    html.Append("</tr>");
                }
                html.Append("<tr>");
                html.Append("<td style='border: 1px solid black;' colspan='6'>جمع</td>");
                html.Append("<td style='border: 1px solid black;'>" + order.Price.GetCurrencyFormat().ToPersian() + " تومان</td>");
                html.Append("</tr>");
                html.Append("</tfoot>");
                html.Append("</table>");
            }
            catch (Exception) { }
            return html.ToString();
        }
    }
}

