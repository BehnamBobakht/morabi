using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewSurveyAnswer
    {
        public string Id { get; set; }
        public string RowId { get; set; }
        public string Name { get; set; }
        public DateTime? CreateDateTime { get; set; }
        public List<SurveyAnswerDetail> SurveyAnswerDetail { get; set; }
        public ViewSurvey Survey { get; set; }
        public ViewAccount Account { get; set; }

        public ViewSurveyAnswer() { }

        public ViewSurveyAnswer(SurveyAnswer surveyAnswer)
        {
            if (surveyAnswer != null)
            {
                this.Id = surveyAnswer.ID.ToString();
                this.Name = surveyAnswer.Name;
                this.CreateDateTime = surveyAnswer.CreateDateTime;
                this.Survey = new ViewSurvey(surveyAnswer.Survey);
                this.Account = new ViewAccount(surveyAnswer.Account);
            }
        }

        public ViewSurveyAnswer(SurveyAnswer surveyAnswer, int index, string MaxZero)
        {
            this.Id = surveyAnswer.ID.ToString();
            this.Name = surveyAnswer.Name;
            this.CreateDateTime = surveyAnswer.CreateDateTime;
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
            this.Survey = new ViewSurvey(surveyAnswer.Survey);
            this.Account = new ViewAccount(surveyAnswer.Account);
        }
    }
}
