using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewProductFilter
    {
        public List<ProductBrandVm> Brands { get; set; }
        public List<ViewColor> Colors { get; set; }
        public List<ViewSize> Sizes { get; set; }

        public ViewProductFilter() {
            this.Brands = new List<ProductBrandVm>();
            this.Colors = new List<ViewColor>();
            this.Sizes = new List<ViewSize>();
        }
    }
}
