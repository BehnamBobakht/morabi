using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ResultPostItem
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public PictureResult Picture { get; set; }
        public CategoryResult category { get; set; }

        public int CategoryId { get; set; }

        public int VisitCount { get; set; }
        public bool SeoNoIndex { get; set; }
        public bool SeoNoFollow { get; set; }
        public string SeoCanonical { get; set; }
        public string SeoH1 { get; set; }
        public string SeoMinDescription { get; set; }
        public string SeoDescription { get; set; }
        public string SeoTitle { get; set; }
        public string SeoPictureAlt { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }


        public List<TagPostResult> Hashtags { get; set; }

        public class PictureResult
        {
            public int Id { get; set; }
            public string Url { get; set; }
        }
        public class TagPostResult
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
        public class CategoryResult
        {
            public ParentResult Parent { get; set; }
          
        }
        public class ParentResult
        {
            
            public string Label { get; set; }
        }
    }
    
}
