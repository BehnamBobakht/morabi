using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewProductCategoryCompany
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public Nullable<int> ShowNumber { get; set; }
        public bool Active { get; set; }
        public string Title { get; set; }
        public string SyncName { get; set; }
        public Nullable<int> SubCategoryId { get; set; }
        public ViewProductCategoryCompany(ProductCategoryCompany productCompany, int index, string MaxZero)
        {
            this.ID = productCompany.ID;
            this.Name = productCompany.Name;
            this.Label = productCompany.Label;
            this.Description = productCompany.Description;
            this.Active = productCompany.Active;
            this.ShowNumber = productCompany.ShowNumber;
            this.Title = productCompany.Title;
            this.SyncName = productCompany.SyncName;
            this.SubCategoryId = productCompany.SubCategoryId;
            

        }
    }
}
