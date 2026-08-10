using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ProductCustomLabelVm
    {
        public  ProductCategoryVm ProductCategory { get; set; }
        public  ProductTypeVm ProductType { get; set; }

    }
}
