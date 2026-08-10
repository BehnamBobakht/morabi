using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
   public class SurveyAnswerDetailVm
    {
        public int ID { get; set; }
        public int AnswerId { get; set; }
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public string QuestionCodeLabel { get; set; }
        public List<int> SelectedItemIds { get; set; }
        public List<SurveyQuestionItemVm> ItemIdsVm { get; set; }
        public string AnswerText { get; set; }
        public SurveyAnswerDetailVm()
        {
            SelectedItemIds = new List<int>();
        }
    }
}
