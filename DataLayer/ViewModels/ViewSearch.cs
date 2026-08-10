using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewSearchProduct
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public string Name { get; set; }
        public string GroupIds { get; set; }
        public double? MaxPrice { get; set; }
        public DateTime Datetime { get; set; }
        public Enum_SearchType Type { get; set; }
        public ProductType ProductType { get; set; }
        public ProductBrand ProductBrand { get; set; }
        public Store Store { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public ProductSubCategory ProductSubCategory { get; set; }
        public List<DataLayer.Entities.Product> Results { get; set; }

        public ViewSearchProduct() {
           PageIndex = 1;
           PageSize = 10;
        }
    }



  
}
