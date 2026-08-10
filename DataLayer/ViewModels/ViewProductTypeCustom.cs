using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    /// <summary>
    /// نوع محصول
    /// </summary>
    public class ViewProductTypeCustom
    {
        public string TypeId { get; set; }
        public string CategoryId { get; set; }
        public string SubCategoryId { get; set; }

        public List<ProductTypeVm> ProductTypes { get; set; }

        public ViewProductTypeCustom() {
            ProductTypes = new List<ProductTypeVm>();
        }

       
    }
}
