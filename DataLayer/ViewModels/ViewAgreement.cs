using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewAgreement
    {
        public int Id { get; set; }
        public string RowId { get; set; }
        public string Name { get; set; }
        public string Employer { get; set; }
        public ViewCode Type { get; set; }
        public int? TotalPrice { get; set; }

        public ViewAgreement(Agreement agreement, int index, string MaxZero)
        {
            this.Id = agreement.ID;
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
            this.Name = agreement.Name;
            this.Employer = agreement.Employer;
            this.Type = new ViewCode(agreement.Code);
            this.TotalPrice = agreement.TotalPrice;
        }
    }
}
