using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ProductOptionItemVm
    {
        public int ID { get; set; }
        public int OptionId { get; set; }
        public string OptionName { get; set; }
        public string Value { get; set; }
        public bool Exist { get; set; }
        public bool Selected { get; set; }


    }
}
