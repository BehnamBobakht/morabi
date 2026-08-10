using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewSurveyQuestion
    {
        public string Id { get; set; }
        public string RowId { get; set; }
        public ViewSurvey Survey { get; set; }
        public ViewSurveyQuestion ReletedQuestion { get; set; }

        public ViewPicture Picture { get; set; }

        public string Question { get; set; }
        public ViewCode Code { get; set; }
        public List<ViewSurveyQuestionItem> SurveyQuestionItems { get; set; }

        public ViewSurveyQuestion() { }

        public ViewSurveyQuestion(SurveyQuestion surveyQuestion)
        {
            if (surveyQuestion != null)
            {
                this.Id = surveyQuestion.ID.ToString();
                this.Question = surveyQuestion.Question;
                this.Survey = new ViewSurvey(surveyQuestion.Survey);
                this.Code = new ViewCode(surveyQuestion.Code);
                this.ReletedQuestion = new ViewSurveyQuestion(surveyQuestion.SurveyQuestion2);

                this.Picture = new ViewPicture(surveyQuestion.Picture);

            }
        }

        public ViewSurveyQuestion(SurveyQuestion surveyQuestion, int index, string MaxZero)
        {
            this.Id = surveyQuestion.ID.ToString();
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
            this.Question = surveyQuestion.Question;
            this.Survey = new ViewSurvey(surveyQuestion.Survey);
            this.Code = new ViewCode(surveyQuestion.Code);
            this.ReletedQuestion = new ViewSurveyQuestion(surveyQuestion.SurveyQuestion2);

            this.Picture = new ViewPicture(surveyQuestion.Picture);
        }
    }
}
