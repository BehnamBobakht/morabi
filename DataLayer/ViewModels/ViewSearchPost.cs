using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewSearchPost
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public string Name { get; set; }
        public DateTime Datetime { get; set; }
        public Enum_SearchType Type { get; set; }
        public Category Category { get; set; }
        public List<Post> Results { get; set; }

        public ViewSearchPost()
        {
            this.PageIndex = 1;
            this.PageSize = 10;
        }
    }
}
