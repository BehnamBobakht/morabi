using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewSurveyAnswerModel
    {
        public string Answer { get; set; }
        public string Answerchecked { get; set; }
        public int QuestionId { get; set; }
        public int FileId { get; set; }
    }
}
