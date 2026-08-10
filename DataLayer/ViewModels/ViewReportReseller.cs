using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewReportReseller
    {
        public string Name { get; set; }
        public int ProductCount { get; set; }
        public double SumPrice { get; set; }
        public double BenefitPercent { get; set; }
        public double SumCheckOut { get; set; }
        public double AmountOfDept { get; set; }

        public ViewReportReseller() { }
    }
}
