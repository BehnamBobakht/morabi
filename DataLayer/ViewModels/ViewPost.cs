using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewPost
    {
        public int Id { get; set; }
        public string RowId { get; set; }
        public ViewCategory Category { get; set; }
        public ViewPicture Picture { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Keywords { get; set; }
        public string Body { get; set; }
        public string Url { get; set; }
        public DateTime? Datetime { get; set; }
        public string DatetimePersian { get; set; }
        public bool Active { get; set; }
        public string PostLabel { get; set; }

        public ViewPost() { }

        public ViewPost(Post post, int index, string MaxZero)
        {
            if (post != null)
            {
                this.Id = post.ID;
                this.Url = post.URL;
                this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
                this.Category = new ViewCategory(post.Category);
                this.Active = post.Active;
                this.Body = post.Body;
                this.Keywords = post.Keywords;
                this.Name = post.Name;
                this.Picture = new ViewPicture(post.Picture);
                this.Summary = post.Summary;
                this.PostLabel = post.PostLabel;
            }
        }
    }
}
