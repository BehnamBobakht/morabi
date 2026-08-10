using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewLanguage
    {
        public int Id { get; set; }
        public string RowId { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public string Culture { get; set; }

        public ViewLanguage() { }

        public ViewLanguage(Language language, int index, string MaxZero)
        {
            this.Id = language.ID;
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
            this.Culture = language.Culture;
            this.Label = language.Label;
            this.Name = language.Name;
        }
    }
}
