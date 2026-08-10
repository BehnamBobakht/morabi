using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ProductsReport
    {
        public ProductsReport()
        {
            Items = new List<ProductsSaleReportItem>();
        }
        public string OrderType { get; set; }
        public string Date { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int TotalCount { get; set; }
        public List<ProductsSaleReportItem> Items { get; set; }

    }
    public class ProductsSaleReportItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double TotalPrice { get; set; }
        public int Count { get; set; }


    }
   
}
