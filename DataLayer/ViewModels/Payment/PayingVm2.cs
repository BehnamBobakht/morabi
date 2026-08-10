using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class PayingVm2
    {
        public string Link { get; set; }
        public string BankName { get; set; }
        public bool IsLink { get; set; }
        public ResultStatus Status { get; set; }
        public string ReturnedCode { get; set; }
        public string Message { get; set; }
        public PaymentVm2 Payment { get; set; }
    }
}
