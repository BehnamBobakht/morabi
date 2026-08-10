using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_SurveyQuestionLanguage : BaseRepository<SurveyQuestionLanguage>
    {
        private DatabaseEntities _context;
        public Entity_SurveyQuestionLanguage(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<SurveyQuestionLanguage> GetAllBySurveyQuestionId(int SurveyQuestionId)
        {
            return _context.SurveyQuestionLanguage.Where(p => p.QuestionId == SurveyQuestionId).ToList();
        }

        public void DeleteBySurveyQuestionId(int SurveyQuestionId)
        {
            List<SurveyQuestionLanguage> list = GetAllBySurveyQuestionId(SurveyQuestionId);
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }
    }
}
