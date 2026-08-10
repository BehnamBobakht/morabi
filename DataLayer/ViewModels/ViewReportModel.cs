using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewReportModel
    {
        public int? AccountId { get; set; }
        public int ID { get; set; }

        public string Exame { get; set; }
        public string FullName { get; set; }
        public string NationalCode { get; set; }

        public string Date { get; set; }

        public int? TotalRateInCountry { get; set; }
        public int? CountQuestion { get; set; }
        public int? CountTrue { get; set; }
        public int? CountFalse { get; set; }
        public int? CountNull { get; set; }
        public int? ExameId { get; set; }

        public double? TotalRate { get; set; }
        public double? TotalRateWithoutFalse { get; set; }
        public double? TotalRatePercent { get; set; }
        public double? TotalLevel { get; set; }
        public double? TotalRateInCity { get; set; }
        public double? TotalLastLevel { get; set; }
        public string QuestionAnswer { get; set; }

        public List<ViewExameReportByCourse> ExameReportByCourse { get; set; }

        public List<ViewExameAccountAnswerDetails> AccountAnswerDetails { get; set; }
        public ViewReportModel() { }


    }
}
