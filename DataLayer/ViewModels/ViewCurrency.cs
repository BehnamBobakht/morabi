using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewCurrency
    {
        public int CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySign { get; set; }
        public int? UnitPrice { get; set; }


        public ViewCurrency() { }


        public ViewCurrency(Currency currency)
        {
            this.CurrencyId = currency.CurrencyId;
            this.CurrencyName = currency.CurrencyName;
            this.CurrencySign = currency.CurrencySign;
            this.UnitPrice = currency.UnitPrice;
        }
    }
}
