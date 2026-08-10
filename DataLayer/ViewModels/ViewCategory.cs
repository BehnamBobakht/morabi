using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Base;
namespace DataLayer.ViewModels
{
    public class ViewCategory
    {
        public int Id { get; set; }
        public string RowId { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public string Picture { get; set; }
        public string Body { get; set; }
        public ViewCode Type { get; set; }
        public ViewWebsite Website { get; set; }

        public ViewCategory() { }

        public ViewCategory(Category category)
        {
            this.Picture = category.Picture.GetUrl();
            this.Body = category.Body;
            this.Id = category.ID;
            this.Name = category.Name;
            this.Label = category.Label;
            this.Type = new ViewCode(category.Code);
        }

        public ViewCategory(Category category, int index, string MaxZero)
        {
            this.Id = category.ID;
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
            this.Name = category.GetName();
            this.Label = category.Label;
            this.Type = new ViewCode(category.Code);
            this.Website = new ViewWebsite(category.Website);
        }
    }
}
