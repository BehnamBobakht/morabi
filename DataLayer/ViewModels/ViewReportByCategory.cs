using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewReportByCategory
    {
        public int? CountTrue { get; set; }
        public int? CountFalse { get; set; }
        public int? CountNull { get; set; }
        public int? TotalCount { get; set; }
        public string NumberString { get; set; }

        public string Name { get; set; }

        public ViewReportByCategory() { }


    }
}
