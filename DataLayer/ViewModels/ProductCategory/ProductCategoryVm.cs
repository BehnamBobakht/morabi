using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ProductCategoryVm
    {
        public int ID { get; set; }
        public Nullable<int> Priority { get; set; }
        public int TypeId { get; set; }
        public string SyncId { get; set; }
        public Nullable<int> PictureId { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> UpdateDatetime { get; set; }
        public Nullable<System.DateTime> SyncDatetime { get; set; }
        public Nullable<int> IconId { get; set; }
        public string Url { get; set; }
        public string DescriptionMeta { get; set; }
        public string TitlePage { get; set; }
        public string H1 { get; set; }
        public string Description { get; set; }
        public string CustomLabel { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        public string Href { get; set; }
        public string ProductTypeName { get; set; }
        public bool Select { get; set; }
        public string LandingText { get; set; }

        public PictureVm Picture { get; set; }
        public  PictureVm Icon { get; set; }
        public List<ProductSubCategoryVm> ProductSubCategory { get; set; }
        public List<ProductCustomFieldVm> ProductCustomField { get; set; }




    }
}
