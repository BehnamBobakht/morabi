using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class CreateDiscountVm
    {
        public int ID { get; set; }
        public Nullable<int> GroupId { get; set; }
        public int TypeId { get; set; }
        public Nullable<int> ProductTypeId { get; set; }
      
        public Nullable<int> ProductCategoryId { get; set; }
        public Nullable<int> ProductSubCategoryId { get; set; }
        public Nullable<int> ProductId { get; set; }
        public Nullable<double> PriceValue { get; set; }
        public Nullable<int> PercentValue { get; set; }
        public Nullable<System.DateTime> StartDatetime { get; set; }
        public Nullable<System.DateTime> EndDatetime { get; set; }
        public bool Active { get; set; }
        public string Description { get; set; }
        public Nullable<int> ProductBrandId { get; set; }
        public Nullable<int> StoreId { get; set; }
        public Nullable<int> StoreProductQuantityId { get; set; }
        public Nullable<int> ValueTypeId { get; set; }

        public string GroupName { get; set; }
        public string StoreName { get; set; }

        public string ProductTypeName { get; set; }
        public string ProductCategoryName { get; set; }
        public string ProductSubCategoryName { get; set; }
        public string ProductName { get; set; }
        public string VarityName { get; set; }
        public string ProductBrandName { get; set; }

    }
}
