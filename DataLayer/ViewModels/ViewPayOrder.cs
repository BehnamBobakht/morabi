using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewPayOrder
    {
     
        public string  Name { get; set; }
        public  double Price { get; set; }
        public double SendPrice { get; set; }

        public double Count { get; set; }

        public ViewPayOrder() { }

        
      
    }
}
