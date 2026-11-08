using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Panel.Areas.Store.Models
{
    public class CourseVideoSessionModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "انتخاب دوره الزامی است.")]
        [Display(Name = "دوره")]
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        [Required(ErrorMessage = "عنوان الزامی است.")]
        [StringLength(250)]
        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Required(ErrorMessage = "آدرس ویدئو الزامی است.")]
        [StringLength(2000)]
        [Display(Name = "آدرس ویدئو")]
        public string VideoUrl { get; set; }

        [Display(Name = "تصویر کاور")]
        public int? PictureId { get; set; }
        public string PictureUrl { get; set; }

        [Display(Name = "ترتیب")]
        public int SortOrder { get; set; }

        [Display(Name = "دمو")]
        public bool IsDemo { get; set; }

        [Display(Name = "فعال")]
        public bool Active { get; set; }

        public int VisitCount { get; set; }
    }

    public class VideoCollectionModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "عنوان الزامی است.")]
        [StringLength(250)]
        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "تصویر کاور")]
        public int? PictureId { get; set; }
        public string PictureUrl { get; set; }

        [Display(Name = "فعال")]
        public bool Active { get; set; }

        public int SessionCount { get; set; }
        public int AccountCount { get; set; }
    }

    public class VideoCollectionSessionModel
    {
        public int ID { get; set; }
        public int VideoCollectionId { get; set; }
        public string CollectionTitle { get; set; }

        [Required(ErrorMessage = "عنوان الزامی است.")]
        [StringLength(250)]
        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Required(ErrorMessage = "آدرس ویدئو الزامی است.")]
        [StringLength(2000)]
        [Display(Name = "آدرس ویدئو")]
        public string VideoUrl { get; set; }

        [Display(Name = "تصویر کاور")]
        public int? PictureId { get; set; }
        public string PictureUrl { get; set; }

        [Display(Name = "ترتیب")]
        public int SortOrder { get; set; }

        [Display(Name = "فعال")]
        public bool Active { get; set; }
    }

    public class MediaVideoIndexModel
    {
        public IList<CourseVideoSessionModel> CourseVideos { get; set; }
        public IList<VideoCollectionModel> Collections { get; set; }
        public IEnumerable<SelectListItem> Products { get; set; }
        public int? ProductId { get; set; }
        public string Search { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }

    public class MediaCollectionIndexModel
    {
        public IList<VideoCollectionModel> Collections { get; set; }
        public string Search { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }

    public class CollectionAccountModel
    {
        public long AccountId { get; set; }
        public string FullName { get; set; }
        public string Mobile { get; set; }
        public string NationalCode { get; set; }
        public bool Assigned { get; set; }
    }

    public class CollectionAccountsModel
    {
        public VideoCollectionModel Collection { get; set; }
        public string Search { get; set; }
        public IList<CollectionAccountModel> Accounts { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
    public class CollectionSessionsIndexModel
    {
        public VideoCollectionModel Collection { get; set; }
        public string Search { get; set; }
        public IList<VideoCollectionSessionModel> Sessions { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }

    public class MediaVisitModel
    {
        public long AccountId { get; set; }
        public string FullName { get; set; }
        public string Mobile { get; set; }
        public string NationalCode { get; set; }
        public DateTime LastVisitDateTime { get; set; }
    }

    public class MediaVisitIndexModel
    {
        public string Source { get; set; }
        public int MediaId { get; set; }
        public int ParentId { get; set; }
        public string MediaTitle { get; set; }
        public string Search { get; set; }
        public IList<MediaVisitModel> Visits { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
}
