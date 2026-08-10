using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_SurveyQuestionItemLanguage : BaseRepository<SurveyQuestionItemLanguage>
    {
        private DatabaseEntities _context;
        public Entity_SurveyQuestionItemLanguage(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<SurveyQuestionItemLanguage> GetAllBySurveyQuestionItemId(int SurveyQuestionItemId)
        {
            return _context.SurveyQuestionItemLanguage.Where(p => p.QuestionItemID == SurveyQuestionItemId).ToList();
        }

        public void DeleteBySurveyQuestionItemId(int SurveyQuestionItemId)
        {
            List<SurveyQuestionItemLanguage> list = GetAllBySurveyQuestionItemId(SurveyQuestionItemId);
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }
    }
}
