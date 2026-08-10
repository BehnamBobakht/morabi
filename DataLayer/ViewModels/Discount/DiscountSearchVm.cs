using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class DiscountSearchVm
    {
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Name { get; set; }
        public int? StoreId { get; set; }
        public bool? Active { get; set; }
        public int? TypeId { get; set; }

        public List<DiscountVm>Discounts { get; set; }
        public DiscountSearchVm()
        {
            Discounts = new List<DiscountVm>();
        }

    }
}
