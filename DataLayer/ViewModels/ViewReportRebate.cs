using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewReportRebate
    {
        public string Name { get; set; }
        public int OrderCount { get; set; }
        public double SumPrice { get; set; }
        public double? RebatePrice { get; set; }

        public ViewReportRebate() { }
    }
}
