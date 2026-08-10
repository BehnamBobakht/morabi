using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewPaymentSubject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }

        public ViewPaymentSubject() { }

        public ViewPaymentSubject(PaymentSubject subject)
        {
            if (subject != null)
            {
                this.Id = subject.ID;
                this.Name = subject.Name;
                this.Label = subject.Label;
            }
        }
    }
}
