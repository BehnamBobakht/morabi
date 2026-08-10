using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewProductOptionItem
    {
        public int Id { get; set; }
        public int OptionId { get; set; }
        public string Value { get; set; }
        public ViewProductOption ProductOption { get; set; }
        public ViewProductOptionItem() { }
        public ViewProductOptionItem(ProductOptionItem item, bool extra)
        {
            if(item!=null)
            {           
            this.Id = item.ID;
            this.OptionId = item.OptionId;
            this.Value = item.Value;
            if (extra == true)
            {
                this.ProductOption = new ViewProductOption()
                {
                    Id = item.ProductOption.ID,
                    Name = item.ProductOption.Name,
                };
            }
            }
        }
      
    }
}
