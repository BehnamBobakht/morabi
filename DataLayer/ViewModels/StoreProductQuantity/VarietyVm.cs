using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class VarietyVm
    {
        public VarietyVm()
        {
            Colors = new List<ColorVm>();
            Sizes = new List<SizeVm>();
            OptionItems = new List<ProductOptionItemVm>();
            storeProductQuantities = new List<StoreProductQuantityVm>();
        }
        public int? ColorId { get; set; }
        public int? SizeId { get; set; }
        public int? OptionItemId { get; set; }  
        public ColorVm Color { get; set; }
        public SizeVm Size { get; set; }
        public ProductOptionItemVm OptionItem { get; set; }
        public string OptionName { get; set; }
        public List<ColorVm> Colors { get; set; }
        public List<SizeVm> Sizes { get; set; }
        public List<ProductOptionItemVm> OptionItems { get; set; }
        public List<StoreProductQuantityVm> storeProductQuantities { get; set; }
    }
}
