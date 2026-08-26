using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Panel.Areas.Store.Models
{
    public class CourseDocumentModel
    {
        public int ID { get; set; }
        [Required(ErrorMessage="انتخاب دوره الزامی است.")][Display(Name="دوره")] public int ProductId { get; set; }
        public string ProductTitle { get; set; }
        [Required(ErrorMessage="عنوان الزامی است.")][StringLength(250)][Display(Name="عنوان PDF")] public string Title { get; set; }
        [Display(Name="توضیحات")] public string Description { get; set; }
        [Required(ErrorMessage="لینک PDF الزامی است.")][StringLength(2000)][Display(Name="لینک PDF")] public string PdfUrl { get; set; }
        [Display(Name="ترتیب")] public int SortOrder { get; set; }
        [Display(Name="فعال")] public bool Active { get; set; }
    }

    public class CourseDocumentIndexModel
    {
        public IList<CourseDocumentModel> Items { get; set; }
        public IEnumerable<SelectListItem> Products { get; set; }
        public int? ProductId { get; set; }
        public string Search { get; set; }
    }

    public class CourseDocumentVisitModel
    {
        public long AccountId { get; set; }
        public string FullName { get; set; }
        public string Mobile { get; set; }
        public string NationalCode { get; set; }
        public DateTime FirstVisitDateTime { get; set; }
        public DateTime LastVisitDateTime { get; set; }
    }

    public class CourseDocumentVisitsModel
    {
        public int DocumentId { get; set; }
        public string DocumentTitle { get; set; }
        public string Search { get; set; }
        public IList<CourseDocumentVisitModel> Visits { get; set; }
    }
}
