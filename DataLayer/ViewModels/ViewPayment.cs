using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewPayment
    {
        public int Id { get; set; }
        public ViewAccount Account { get; set; }
        public ViewMerchant Merchant { get; set; }
        public ViewPaymentSubject PaymentSubject { get; set; }
        public ViewCode Status { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public int OrderId { get; set; }
        public string Sign { get; set; }
    }
}
