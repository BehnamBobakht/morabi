using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class AccountOrdersReport
    {
        public AccountOrdersReport()
        {
            Items = new List<OrderReportItem>();
        }
        public string Subject { get; set; }
        public string Date { get; set; }
        public int TotalCount { get; set; }
        public List<OrderReportItem> Items { get; set; }

    }
    public class OrderReportItem
    {
        public int Row { get; set; }
        public int Id { get; set; }

        public string FullName { get; set; }
        public double? TotalPrice { get; set; }
        public double? UnitPrice { get; set; }
        public string ProductOption { get; set; }
        public string ProductName { get; set; }

        public string Datetime { get; set; }
        public string Status { get; set; }
        public string StoreName { get; set; }
        public string OptionName { get; set; }
        public double ?SendPrice { get; set; }

        public int Count { get; set; }

        public string  Size { get; set; }
        public string Color { get; set; }
        public double? Discount { get; set; }
        public string Rebate { get; set; }



    }

}
