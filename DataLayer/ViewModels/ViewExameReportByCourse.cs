using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewExameReportByCourse
    {
        public string Name { get; set; }
        public string Type { get; set; }


        public int? CountTrue { get; set; }
        public int? CountFalse { get; set; }
        public int? TotalRate { get; set; }
        public double? RatePercent { get; set; }
        public int? CountNull { get; set; }

        public double? TotalRateWithoutFalse { get; set; }
        public double? TotalLevel { get; set; }
        public double? RateInCountry { get; set; }

        public ViewExameReportByCourse() { }


    }
}
