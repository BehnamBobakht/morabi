using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewPay
    {
     
        public Payment Payment { get; set; }
        public bool? IsPictureId { get; set; }
        public bool? IsNationalId { get; set; }
        public bool? IsParport { get; set; }

        public List<string> Links { get; set; }
        public List<string> Names { get; set; }


        public ViewPay() { }

        
      
    }
}
