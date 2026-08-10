using DataLayer.Base;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewRebate
    {
        public int Id { get; set; }
        public string RowId { get; set; }
        public string Name { get; set; }
        public string CodeValue { get; set; }
        public ViewCode Type { get; set; }
        public bool Active { get; set; }
        public DateTime? StartDatetime { get; set; }
        public DateTime? EndDatetime { get; set; }

        //public ViewRebate(Rebate rebate, int index, string MaxZero)
        //{
        //    this.Id = rebate.ID;
        //    this.CodeValue = rebate.CodeValue;
        //    this.Type = new ViewCode(rebate.Code);
        //    this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
        //    this.Name = rebate.GetRebateName();
        //    this.Active = rebate.Active;
        //    this.StartDatetime = rebate.StartDatetime.GetValueOrDefault();
        //    this.EndDatetime = rebate.EndDatetime.GetValueOrDefault();
        //}

        //public ViewRebate(Rebate rebate)
        //{
        //    this.Id = rebate.ID;
        //    this.CodeValue = rebate.CodeValue;
        //    this.Type = new ViewCode(rebate.Code);
        //    this.Name = rebate.GetRebateName();
        //    this.Active = rebate.Active;
        //    this.StartDatetime = rebate.StartDatetime.GetValueOrDefault();
        //    this.EndDatetime = rebate.EndDatetime.GetValueOrDefault();
        //}
    }
}
