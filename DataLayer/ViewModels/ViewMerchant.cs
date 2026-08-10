using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewMerchant
    {
        public int Id { get; set; }
        public string RowId { get; set; }
        public ViewBank Bank { get; set; }
        public bool Active { get; set; }

        public ViewMerchant() { }

        public ViewMerchant(Merchant merchant, int index, string MaxZero)
        {
            this.Id = merchant.ID;
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
            this.Bank = new ViewBank(merchant.Bank);
            this.Active = merchant.Active;
        }
    }
}
