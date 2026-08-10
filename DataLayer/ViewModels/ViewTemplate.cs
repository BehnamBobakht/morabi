using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewTemplate
    {
        public int Id { get; set; }
        public string RowId { get; set; }
        public ViewCode Type { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public int? Version { get; set; }
        public string Author { get; set; }

        public ViewTemplate(Template template, int index, string MaxZero)
        {
            this.Id = template.ID;
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
            this.Type = new ViewCode(template.Code);
            this.Name = template.Name;
            this.Label = template.Label;
            this.Version = template.Version;
            this.Author = template.Author;
        }
    }
}
