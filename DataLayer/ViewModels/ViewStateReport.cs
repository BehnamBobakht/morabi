
using System.Collections.Generic;

namespace DataLayer.ViewModels
{
    public class ViewStateReport
    {
        public ViewStateReport()
        {
            this.Items = new List<ProductsReportItem>();
        }

        public string State { get; set; }

        public string Date { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }
        public List<ProductsReportItem> Items { get; set; }

        public int TotalCount { get; set; }
        public class ProductsReportItem
        {
            public int Id { get; set; }

            public string Datetime { get; set; }

            public string Price { get; set; }

            public string ShippingPrice { get; set; }

            public int Count { get; set; }
        }
    }
}
