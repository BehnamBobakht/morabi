using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ProductCustomValueVm
    {
        public int ID { get; set; }
        public int FieldId { get; set; }
        public int ProductId { get; set; }
        public Nullable<int> ItemId { get; set; }
        public Nullable<int> FileId { get; set; }
        public string SyncId { get; set; }
        public string Value { get; set; }
        public Nullable<System.DateTime> SyncDatetime { get; set; }

        public  ProductCustomFieldVm ProductCustomField { get; set; }
        public  ProductCustomItemVm ProductCustomItem { get; set; }

    }
}
