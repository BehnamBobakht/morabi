using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewShopProductType
    {
        public string ProductTypeName { get; set; }
        public int ProductTypeId { get; set; }
        public bool Selected { get; set; }

        public ViewShopProductType() { }
    }
}
