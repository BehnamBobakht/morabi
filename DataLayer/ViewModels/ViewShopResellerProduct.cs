using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewShopResellerProduct
    {
        public int Id { get; set; }
        public ViewShopReseller ShopReseller { get; set; }
        public ViewProductType ProductType { get; set; }
        public ViewProductBrand ProductBrand { get; set; }
        public ViewProductCategory ProductCategory { get; set; }
        public ViewProductSubCategory ProductSubCategory { get; set; }
        public ViewProduct Product { get; set; }
        public ViewCode Status { get; set; }

        public ViewShopResellerProduct() { }
    }
}
