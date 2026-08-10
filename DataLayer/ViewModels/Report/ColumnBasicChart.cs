using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels.Report
{
    public class ColumnBasicChart
    {
        public string ColumnName { get; set; }
        public string yTitle { get; set; }

        public List<string> Categories { get; set; }
        public List<SubColumnBasicChart> List { get; set; }
    }
    public class SubColumnBasicChart
    {
        public string name { get; set; }
        public List<double> data { get; set; }
    }
}
