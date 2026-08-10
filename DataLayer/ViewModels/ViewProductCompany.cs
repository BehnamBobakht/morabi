using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewProductCompany
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public Nullable<int> ShowNumber { get; set; }
        public bool Active { get; set; }
        public string Title { get; set; }
        public ViewProductCompany(ProductCompany productCompany)
        {
            this.ID = productCompany.ID;
           
            this.Name = productCompany.Name;
            this.Label = productCompany.Label;
            this.Description = productCompany.Description;
            this.Active = productCompany.Active;
            this.ShowNumber = productCompany.ShowNumber;
            this.Title = productCompany.Title;

        }
    }
}
