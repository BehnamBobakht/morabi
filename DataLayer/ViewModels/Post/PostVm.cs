using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class PostVm
    {
        public int ID { get; set; }
        public int WebsiteId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryBody { get; set; }

        public Nullable<int> PictureId { get; set; }
        public Nullable<int> FileId { get; set; }
        public string Name { get; set; }
        public string Keywords { get; set; }
        public string Summary { get; set; }
        public string Body { get; set; }
        public Nullable<System.DateTime> ShowDateTime { get; set; }
        public System.DateTime UpdateDateTime { get; set; }
        public System.DateTime CreateDateTime { get; set; }
        public Nullable<int> ShowNumber { get; set; }
        public int VisitCount { get; set; }
        public bool IsSlider { get; set; }
        public bool Active { get; set; }
        public string URL { get; set; }
        public string H1 { get; set; }
        public string Writer { get; set; }
        public string WriterPostion { get; set; }
        public Nullable<int> WriterPictureId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Aparat { get; set; }
        public Nullable<int> StateId { get; set; }
        public string Canonical { get; set; }
        public bool NoFollow { get; set; }
        public bool NoIndex { get; set; }
        public string PostLabel { get; set; }
        public string Href { get; set; }
        public  CategoryVm Category { get; set; }
        public  PictureVm Picture { get; set; }
        public  PictureVm Picture1 { get; set; }
        public StateVm State { get; set; }
        public  WebsiteDocumentVm WebsiteDocument { get; set; }
        public  List<TagPostVm> TagPost { get; set; }
        public  List<PostQuestionVm> PostQuestion { get; set; }


    }
}
