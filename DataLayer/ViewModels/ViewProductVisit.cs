using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewProductVisit
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int Id { get; set; }
        public List<ProductVisit> Results { get; set; }


        public ViewProductVisit() {
            PageIndex = 1;
            PageSize = 10;
        }
        public class ViewProductVisitDetail
        {
            public int Count { get; set; }

        }
    }
}
