using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
   public class SurveyAnswerVm
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int SurveyId { get; set; }
        public SurveyVm SurveyVm { get; set; }
        public List<SurveyAnswerDetailVm> SurveyAnswerDetailsVm { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public SurveyAnswerVm()
        {
            SurveyAnswerDetailsVm = new List<SurveyAnswerDetailVm>();
        }
    }
}
