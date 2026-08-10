using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class PostSearchVm
    {
        public int TotalCount { get; set; }        
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Name { get; set; }
        public int? notId { get; set; }
        public int? categoryId { get; set; }
        public string categoryLable { get; set; }
        public int? typeId { get; set; }
        public string keyword { get; set; }
        public string ViewName { get; set; }
        public string Title { get; set; }
        public string H1 { get; set; }
        public string MetaDescription { get; set; }
        public string PageUrl { get; set; }
        public string PageCanonical { get; set; }           
        public Enum_PostOrder poorder  { get; set; }
        public List<PostVm> Posts { get; set; }
        public CategoryVm Category { get; set; }
        public PostSearchVm()
        {
            Posts = new List<PostVm>();
        }

    }
}
