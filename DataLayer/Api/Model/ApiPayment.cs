using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Api.Model
{
    public class ApiPayment : ApiResponse
    {
        public static ApiResult Post(UnitOfWork _context, int? accountId, int? siteuserId, ViewPayment payment)
        {
            if (payment.PaymentSubject == null || payment.PaymentSubject.Label == null)
                return CreateErrorResult(Enum_Message.REQUIRED_PAYMENT_SUBJECT);

            int? merchantId = payment.Merchant == null ? default(int?) : payment.Merchant.Id;
            Merchant merchant = merchantId == null ? _context.Merchant.FirstOrDefault() : new Merchant() { ID = merchantId.Value };
            if (merchant == null)
                return CreateErrorResult(Enum_Message.REQUIRED_PAYMENT_MERCHANT);

            PaymentSubject subjectObj = _context.PaymentSubject.GetByLabel(payment.PaymentSubject.Label);
            if (subjectObj.Label == Enum_PaymentSubject.ORDER.ToString())
            {
                var order = _context.AccountOrder.GetById(payment.OrderId);
                payment.Price = order.Price;
                if(order.PaymentType.Label==Enum_PaymentType.PLACE.ToString())
                {
                    _context.AccountBasket.ClearBasket(accountId);

                }
            }
            payment.Status = payment.Status == null ?
            payment.Status = new ViewCode()
            {
                Label = Enum_Code.PAYMENT_STATUS_INSERTED.ToString()
            } : payment.Status;

            Payment paymentObj = _context.Payment.CreatePayment(accountId, siteuserId, subjectObj.ID, payment.Price, merchant.ID, payment.Description, payment.Status.Label);
            if (subjectObj.Label == Enum_PaymentSubject.ORDER.ToString())
            {
                _context.PaymentProductOrder.Insert(new PaymentProductOrder()
                {
                    PaymentId = paymentObj.ID,
                    OrderId = payment.OrderId,
                });
                _context.Save();
            }
            ViewPayment output = paymentObj.ToApi();
            return output != null ? CreateSuccessResult(output) : CreateErrorResult(Enum_Message.INVALID_PAYMENT_OBJECT);
        }


    }
}
