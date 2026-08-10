using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class MerchantVm
    {
        public int ID { get; set; }
        public int BankId { get; set; }      
        public string BankName { get; set; }      
        public string Picture { get; set; }      
        public bool Active { get; set; }

    }
}
