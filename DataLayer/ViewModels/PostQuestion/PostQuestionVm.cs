using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class PostQuestionVm
    {
        public int ID { get; set; }
        public string QuestionText { get; set; }
        public string AnswerText { get; set; }
        public int PostId { get; set; }


    }
}
