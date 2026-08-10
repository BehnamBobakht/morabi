using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewReport2
    {
        public ViewReportModel ExameAccountAnswer { get; set; }
        public List<ViewReportExameReportByCourse>  ExameReportByCourseGeneral { get; set; }
        public List<ExameReportByCourse> ExameReportByCourseProfessional { get; set; }
        public int ? TotalCountG { get; set; }
        public int? PassValue { get; set; }

        public int? TotalCountP { get; set; }

        public double RateInCountryG { get; set; }
        public double RateInCountryP { get; set; }
        public double? AvrageLevelCountry { get; set; }
        public double? TopAvrageLevelCountry { get; set; }

        public double? TotalLastLevel { get; set; }
        public List<ViewReportByLastAvrage> LastAvrageLevel { get; set; }


        public List<ViewReportByCategory> ReportByCategory { get; set; }


        public ViewReport2() { }

       
    }
}
