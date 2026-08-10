using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewPostModel
    {
      
        public Post Post { get; set; }
        public List<Post> RelatedPost { get; set; }
        public List<DataLayer.Entities.Product> RelatedProduct { get; set; }

        public ViewPostModel()
        {
            Post = new Post();
            RelatedProduct = new List<DataLayer.Entities.Product>();
            RelatedPost = new List<Post>();
        }
    }
}
