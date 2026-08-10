using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewShopReseller
    {
        public int Id { get; set; }
        public ViewShop Shop { get; set; }
        public ViewCity City { get; set; }
        public ViewPicture Picture { get; set; }
        public ViewPicture CoverPicture { get; set; }
        public ViewPicture PersonalPicture { get; set; }

        public string Name { get; set; }
        public string Website { get; set; }
        public string AddressValue { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }

        public ViewShopReseller() { }
    }
}
