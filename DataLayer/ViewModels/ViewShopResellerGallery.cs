using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewShopResellerGallery
    {
        public int Id { get; set; }
        public ViewShopReseller Reseller { get; set; }
        public ViewPicture Picture { get; set; }
        public ViewWebsiteDocument Video { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ViewShopResellerGallery() { }
    }
}
