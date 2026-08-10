using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ProductBrandVm
    {
        public ProductBrandVm()
        {
            Picture = new PictureVm();
        }
        public int ID { get; set; }
        public Nullable<int> ProductTypeId { get; set; }
        public Nullable<int> PictureId { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public Nullable<System.DateTime> SyncDatetime { get; set; }
        public bool IsPublic { get; set; }
        public string ThemeColor { get; set; }
        public string Description { get; set; }
        public Nullable<int> ShowNumber { get; set; }
        public bool Active { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Href { get; set; }
        public PictureVm Picture { get; set; }

    }
}
