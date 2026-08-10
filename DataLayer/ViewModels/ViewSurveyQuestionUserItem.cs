using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewSurveyQuestionUserItem
    {
        public int QuestionId { get; set; }
        public string Question { get; set; }
        public string QuestionType { get; set; }
        public string PictureUrl { get; set; }

        public int ? RelatedId { get; set; }
        public bool HaseRelated { get; set; }

        public int FileId { get; set; }
        public string Answer { get; set; }
        public List<SurveyQuestionItem> SurveyQuestionItems { get; set; }


    }
}
