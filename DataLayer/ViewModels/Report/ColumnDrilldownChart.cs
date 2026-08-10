using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels.Report
{
    public class ColumnDrilldownChart
    {
        public string ColumnName { get; set; }
        public string yTitle { get; set; }

        public SubColumnDrilldownChart[] List { get; set; }
    }
    public class SubColumnDrilldownChart
    {
        public string name { get; set; }
        public double y { get; set; }
    }
}
