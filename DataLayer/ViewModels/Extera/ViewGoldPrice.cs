using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLayer.ViewModels
{
    public class ViewGoldPrice {
        public gold gold { get; set; }
    }
    public class gold
    {
        public gold_18k gold_18k { get; set; }
    }

    public class gold_18k
    {
        public string v { get; set; }
    }
}