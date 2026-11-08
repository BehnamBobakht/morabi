using System.Collections.Generic;

namespace Media.Models
{
    public class LearningCourseViewModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool IsOwned { get; set; }
        public bool HasVideos { get; set; }
        public bool HasDemoVideos { get; set; }
        public bool HasDocuments { get; set; }
        public bool HasExam { get; set; }
        public int VideoCount { get; set; }
        public int ViewedVideoCount { get; set; }
        public int DocumentCount { get; set; }
        public int ViewedDocumentCount { get; set; }
        public bool ExamUnlocked { get; set; }
    }

    public class CourseDocumentViewModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PdfUrl { get; set; }
        public int SortOrder { get; set; }
        public bool IsViewed { get; set; }
    }

    public class CourseDocumentListViewModel
    {
        public int ProductId { get; set; }
        public string ProductTitle { get; set; }
        public IList<CourseDocumentViewModel> Documents { get; set; }
    }

    public class CourseDocumentReadViewModel
    {
        public int ProductId { get; set; }
        public string ProductTitle { get; set; }
        public CourseDocumentViewModel Document { get; set; }
        public IList<CourseDocumentViewModel> Documents { get; set; }
    }
}
