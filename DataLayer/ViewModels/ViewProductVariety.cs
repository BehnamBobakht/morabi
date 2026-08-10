using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
   public class ViewProductVariety
    {
        public int? SizeId { get; set; }
        public int? ColorId { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
        public int? OptionItemId { get; set; }
        public string OptionItemValue { get; set; }
        public string ColorHex { get; set; }
        public string OptionName { get; set; }
        public List<ViewColor> Colors { get; set; }
        public List<ViewSize> Sizes { get; set; }
        public List<ViewProductOptionItem> OptionItems { get; set; }
        public object ProductQuantity { get; set; }

        public ViewProductVariety()
        {
            Colors = new List<ViewColor>();
            Sizes = new List<ViewSize>();
            OptionItems = new List<ViewProductOptionItem>();

            //ProductQuantity = new List<ViewStoreProductQuantity>();
        }

    }
}
