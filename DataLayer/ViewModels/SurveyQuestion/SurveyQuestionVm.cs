using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
   public class SurveyQuestionVm
    {
        public int ID { get; set; }
        public string Question { get; set; }
        public Nullable<int> QuestionType { get; set; }
        public Nullable<int> SurveyId { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> ShowNumber { get; set; }
        public List<SurveyQuestionItemVm> SurveyQuestionItemsVm { get; set; }
        public  CodeVm Code { get; set; }

    }
}
