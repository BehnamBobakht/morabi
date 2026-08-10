using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewSurveyQuestionUser
    {
        public int SurveyId { get; set; }
        public string SurveyName { get; set; }

        public List<ViewSurveyQuestionUserItem> Questions { get; set; }
    }
}
