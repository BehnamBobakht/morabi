using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ProductCompareVm
    {
        public ProductCompareVm()
        {
            Products = new List<ProductShowVm>();
            CustomFields = new List<ProductCustomFieldVm>();
        }
        public List<ProductShowVm> Products { get; set; }
        public List<ProductCustomFieldVm> CustomFields { get; set; }
        public int Count { get; set; }
        public int? SubCategoryId { get; set; }
    }
}
