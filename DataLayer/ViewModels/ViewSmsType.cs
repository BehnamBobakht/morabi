using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewSmsType
    {
        public int Id { get; set; }
        public string RowId { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public string Body { get; set; }

        public ViewSmsType() { }

        public ViewSmsType(SmsType type)
        {
            this.Id = type.ID;
            this.Name = type.Name;
            this.Label = type.Label;
            this.Body = type.Body;
        }

        public ViewSmsType(SmsType type, int index, string MaxZero)
        {
            this.Id = type.ID;
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
            this.Name = type.Name;
            this.Label = type.Label;
            this.Body = type.Body;
        }
    }
}
