using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewSurveyQuestionItem
    {
        public string Id { get; set; }
        public string RowId { get; set; }
        public int? ShowNumber { get; set; }
        public int? QuestionId { get; set; }
        public Boolean? Active { get; set; }
        public ViewSurveyQuestion SurveyQuestion { get; set; }
        public string Name { get; set; }

        public ViewSurveyQuestionItem() { }

        public ViewSurveyQuestionItem(SurveyQuestionItem surveyQuestionItem, bool? extra = true)
        {
            if (surveyQuestionItem != null)
            {
                this.Id = surveyQuestionItem.ID.ToString();
                this.Name = surveyQuestionItem.Name;
                this.ShowNumber= surveyQuestionItem.ShowNumber;
                this.QuestionId = surveyQuestionItem.QuestionId;
                this.Active = surveyQuestionItem.Active;
                if (extra == true)
                {
                    this.SurveyQuestion = new ViewSurveyQuestion(surveyQuestionItem.SurveyQuestion);
                }
            }
        }

        public ViewSurveyQuestionItem(SurveyQuestionItem surveyQuestinItem, int index, string MaxZero)
        {
            this.Id = surveyQuestinItem.ID.ToString();
            this.ShowNumber = surveyQuestinItem.ShowNumber;
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
            this.Name = surveyQuestinItem.Name;
            this.Active = surveyQuestinItem.Active;
            this.QuestionId = surveyQuestinItem.QuestionId;
            this.SurveyQuestion = new ViewSurveyQuestion(surveyQuestinItem.SurveyQuestion);
        }
    }
}
