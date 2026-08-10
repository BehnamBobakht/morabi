using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewSurvey
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RowId { get; set; }
        public List<ViewSurveyQuestion> SurveyQuestions { get; set; }

        public ViewSurvey() { }
        public ViewSurvey(Survey survey, int index, string MaxZero)
        {
            if (survey != null)
            {
                this.Id = survey.ID;
                this.Name = survey.Name;
                this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
            }
        }
        public ViewSurvey(Survey survey)
        {
            if (survey != null)
            {
                this.Id = survey.ID;
                this.Name = survey.Name;
            }
        }
    }
}
