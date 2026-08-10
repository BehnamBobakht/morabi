using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewProductCustomItem
    {
        public int Id { get; set; }
        public string RowId { get; set; }
        public ViewProductCustomField Field { get; set; }
        public string Value { get; set; }

        public ViewProductCustomItem() { }

        public ViewProductCustomItem(ProductCustomItem item)
        {
            if (item != null)
            {
                this.Id = item.ID;
                //this.Field = new ViewProductCustomField(item.ProductCustomField);
                this.Value = item.Value;
            }
        }

        public ViewProductCustomItem(ProductCustomItem item, int index, string MaxZero)
        {
            this.Id = item.ID;
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
            //this.Field = new ViewProductCustomField(item.ProductCustomField);
            this.Field = new ViewProductCustomField() {
                Name = item.ProductCustomField.Name
            };
            this.Value = item.Value;
        }
    }
}
