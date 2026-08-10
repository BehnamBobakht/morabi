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

    public class BaseCashOnDelivery : BasePayment
    {



        public static PayingVm StartPayment(UnitOfWork _context, Payment payment)
        {

            //payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_SUCCESSFUL);
            //BaseCart.DoPaymentCallBack(_context, payment);
            //_context.Save();
            var paying = new PayingVm();
            paying.Status = ResultStatus.Success;
            paying.Link = "/payment/callback/" + payment.ID;
            paying.IsLink = true;
            return paying;
        }





        public static Payment DoCallBack(UnitOfWork _context, Payment payment)
        {
            try
            {
                if (payment.PaymentSubject.Label == Enum_PaymentSubject.ORDER.ToString())
                {
                    PaymentProductOrder orderPayment = payment.PaymentProductOrder.OrderByDescending(p => p.ID).FirstOrDefault();
                    if (orderPayment.AccountOrder.PaymentType.Label == Enum_PaymentType.PLACE.ToString())
                    {
                        payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_SUCCESSFUL);

                        BaseCart.DoPaymentCallBack(_context, payment);
                        payment.IsInPlace = true;
                        _context.Payment.Save();
                    }
                    else
                    {
                        payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_START_ERROR);
                        _context.Payment.Save();
                    }
                }
                else
                {
                    payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_START_ERROR);
                    _context.Payment.Save();
                }

            }
            catch (Exception)
            {
                payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_SERVICE_FAILED);
                _context.Payment.Save();

            }
            return payment;
        }
    }

}
