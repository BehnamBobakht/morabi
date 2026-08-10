using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ProductTypeVm
    {
        public int ID { get; set; }
        public Nullable<int> Priority { get; set; }
        public Nullable<int> PictureId { get; set; }
        public string SyncId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> SyncDatetime { get; set; }
        public Nullable<int> IconId { get; set; }
        public bool Active { get; set; }
        public string H1 { get; set; }
        public string Url { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public bool IsService { get; set; }
        public string CustomLabel { get; set; }
        public Nullable<int> AttachFileId1 { get; set; }
        public Nullable<int> AttachFileId2 { get; set; }
        public Nullable<bool> Deleted { get; set; }
        public bool Select { get; set; }
        public string Href { get; set; }
        public string LandingText { get; set; }

        public List<ProductCategoryVm> ProductCategory { get; set; }
        public List<ProductCustomFieldVm> ProductCustomField { get; set; }
        public PictureVm Picture { get; set; }
        public PictureVm Icon { get; set; }

    }
}
