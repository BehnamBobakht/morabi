using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewGallery
    {
        public int Id { get; set; }
        public string RowId { get; set; }
        public ViewWebsite Website { get; set; }
        public Category Category { get; set; }
        public string Name { get; set; }

        public ViewGallery(Gallery gallery, int index, string MaxZero)
        {
            this.Id = gallery.ID;
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
            this.Name = gallery.Name;
            this.Website = new ViewWebsite(gallery.Website);
            this.Category = gallery.Category;
        }

        public ViewGallery(Gallery gallery)
        {
            this.Id = gallery.ID;
            this.Name = gallery.Name;
            this.Website = new ViewWebsite(gallery.Website);
            this.Category = gallery.Category;
        }
    }
}
