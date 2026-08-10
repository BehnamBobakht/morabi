using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewWebsite
    {
        public int Id { get; set; }
        public string RowId { get; set; }
        public string Title { get; set; }
        public string Domain { get; set; }
        public ViewCode Type { get; set; }

        public ViewWebsite() { }

        public ViewWebsite(Website website)
        {
            if (website != null)
            {
                this.Id = website.ID;
                this.Title = website.Title;
                this.Type = new ViewCode(website.Code);
            }
        }

        public ViewWebsite(Website website, int index, string MaxZero)
        {
            this.Id = website.ID;
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
            this.Title = website.Title;
            this.Type = new ViewCode(website.Code);
        }
    }
}
