using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewPaymentType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }

        public ViewPaymentType() { }
        public ViewPaymentType(PaymentType paymentType)
        {
            if (paymentType != null)
            {
                this.Id = paymentType.ID;
                this.Label = paymentType.Label;
                this.Name = paymentType.Name;
            }
        }
    }
}
