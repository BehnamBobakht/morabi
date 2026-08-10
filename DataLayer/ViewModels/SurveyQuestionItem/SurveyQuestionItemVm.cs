using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
   public class SurveyQuestionItemVm
    {
        public int ID { get; set; }
        public int QuestionId { get; set; }
        public string Name { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> ShowNumber { get; set; }
    }
}
