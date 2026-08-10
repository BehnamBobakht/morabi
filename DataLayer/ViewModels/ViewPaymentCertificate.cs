using DataLayer.Base;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewPaymentCertificate
    {
        public int Id { get; set; }

        public ViewAccount Account { get; set; }
        public double Price { get; set; }

        public DateTime Datetime { get; set; }


        public ViewProduct Product { get; set; }

        public ViewPaymentCertificate() { }

        public ViewPaymentCertificate(PaymentCertificate order)
        {
            this.Id = order.ID;
            this.Account = new ViewAccount(order.Account);
            this.Price = order.Amount;
            this.Datetime = order.Datetime;
            this.Product = new ViewProduct(order.Product);
           
            

        }

        public ViewPaymentCertificate(PaymentCertificate order, int index, string MaxZero)
        {
            this.Id = order.ID;
            this.Account = new ViewAccount(order.Account);
            this.Price = order.Amount;
            this.Datetime = order.Datetime;
            this.Product = new ViewProduct(order.Product);

        }
    }
}
