using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels.Product
{
    public class EmallsProductsVm
    {
        public EmallsProductsVm()
        {
            EmallsProducts = new List<EmallsProductVm>();
        }
       public List<EmallsProductVm> EmallsProducts { get; set; }
        //public bool Success { get; set; }
    }
    public class EmallsProductVm
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double DiscountValue { get; set; }
        public double OldPrice { get; set; }
        public double Price { get; set; }
        public int DiscountPercent { get; set; }
        public bool InStock { get; set; }
    }
}
