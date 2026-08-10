using DataLayer.Entities;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewCalendarEvent
    {
        public int Id { get; set; }
        public string RowId { get; set; }
        public string Type { get; set; }
        public string TypeName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }

        public ViewCalendarEvent(Event entity)
        {
            this.Date = entity.Date;
            this.Description = entity.Description;
            this.Id = entity.ID;
            this.StartTime = entity.StartTime.GetTimeString();
            this.EndTime = entity.EndTime.GetTimeString();
            this.Type = entity.Code.Label;
        }

        public ViewCalendarEvent(Event entity, int index, string MaxZero)
        {
            this.Id = entity.ID;
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
            this.Type = entity.Code.Label;
            this.TypeName = entity.Code.Name;
            this.StartTime = entity.StartTime.GetTimeString();
            this.EndTime = entity.EndTime.GetTimeString();
            this.Description = entity.Description;
            this.Date = entity.Date;
        }
    }
}
