using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewDiscountGroup
    {
        public int Id { get; set; }
        public string RowId { get; set; }
        public ViewCode Type { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public double? PriceValue { get; set; }
        public int? PercentValue { get; set; }
        public DateTime? StartDatetime { get; set; }
        public DateTime? EndDatetime { get; set; }
        public int? PictureId { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        public ViewDiscountGroup() { }

        public ViewDiscountGroup(DiscountGroup group)
        {
            if (group != null)
            {
                this.Id = group.ID;
                this.Type = new ViewCode(group.Code);
                this.Name = group.Name;
                this.Label = group.Label;
                this.PriceValue = group.PriceValue;
                this.PercentValue = group.PercentValue;
                this.StartDatetime = group.StartDatetime;
                this.EndDatetime = group.EndDatetime;
                this.PictureId = group.PictureId;
                this.Active = group.Active;
                this.Deleted = group.Deleted;
            }

        }

        public ViewDiscountGroup(DiscountGroup group, int index, string MaxZero)
        {
            if (group != null)
            {
                this.Id = group.ID;
                this.Type = new ViewCode(group.Code);
                this.Name = group.Name;
                this.Label = group.Label;
                this.PriceValue = group.PriceValue;
                this.PercentValue = group.PercentValue;
                this.StartDatetime = group.StartDatetime;
                this.EndDatetime = group.EndDatetime;
                this.PictureId = group.PictureId;
                this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
            }
        }
    }
}
