using System;
using System.Collections.Generic;
namespace DataLayer.ViewModels
{
   public class SurveyVm
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime DateTime { get; set; }
        public string Label { get; set; }
        public List<SurveyQuestionVm> SurveyQuestionsVm { get; set; }

    }
}
