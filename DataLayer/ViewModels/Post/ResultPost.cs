using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ResultPost
    {
        public List<ResultPostItem> List { get; set; }
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public string categoryLable { get; set; }
        public string ViewName { get; set; }
        public string Name { get; set; }


    }
}
