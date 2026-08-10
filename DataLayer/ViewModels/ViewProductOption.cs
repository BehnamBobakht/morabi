using DataLayer.Api;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewProductOption
    {
        public int Id { get; set; }
        public int? ProductTypeId { get; set; }
        public string Name { get; set; }
        public int? ShowNumber { get; set; }
        public string SyncId { get; set; }
        public string SyncName { get; set; }
        public DateTime? SyncDatetime { get; set; }
        public List<ViewProductOptionItem> OptionItem { get; set; }
        public ViewProductOption() { }
        public ViewProductOption(ProductOption option,bool extera=true)
        {
            this.Id = option.ID;
            this.Name = option.Name;
            this.OptionItem =option.ProductOptionItem.ToApi(extra:false);
          
        }
      
    }
}
