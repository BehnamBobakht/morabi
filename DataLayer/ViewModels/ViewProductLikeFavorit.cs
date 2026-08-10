using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewProductLikeFavorit
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int Id { get; set; }
        public List<ProductLike> Results { get; set; }


        public ViewProductLikeFavorit() {
            PageIndex = 1;
            PageSize = 10;
        }
        public class ViewProductLikeDetail
        {
            public int Count { get; set; }

        }
    }
}
