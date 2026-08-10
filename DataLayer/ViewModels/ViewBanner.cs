using DataLayer.Entities;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewBanner
    {
        public int Id { get; set; }
        public string RowId { get; set; }
        public ViewCategory Category { get; set; }
        public ViewPicture Picture { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Url { get; set; }
        public int ShowNumber { get; set; }

        public ViewBanner() { }

        public ViewBanner(Banner banner)
        {
            this.Id = banner.ID;
            this.Name = banner.Name;
            this.Summary = banner.Summary;
            this.Url = banner.Url;
            this.Category = new ViewCategory(banner.Category);
            this.Picture = new ViewPicture(banner.Picture);
        }

        public ViewBanner(Banner banner, int index, string MaxZero)
        {
            this.Id = banner.ID;
            this.Name = banner.Name;
            this.Summary = banner.Summary;
            this.Url = banner.Url;
            this.Category = new ViewCategory(banner.Category);
            this.Picture = new ViewPicture(banner.Picture);
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
        }
    }
}
