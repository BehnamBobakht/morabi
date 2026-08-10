using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewShopResellerCollection
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ViewPicture Picture { get; set; }

        public ViewShopResellerCollection() { }
    }
}
