using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_SurveyLanguage : BaseRepository<SurveyLanguage>
    {

        private DatabaseEntities _context;
        public Entity_SurveyLanguage(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<SurveyLanguage> GetAllBySurveyId(int surveyId)
        {
            return _context.SurveyLanguage.Where(g =>
                g.SurveyId == surveyId
            ).ToList();
        }

        public void DeleteBySurveyId(int surveyId)
        {
            List<SurveyLanguage> list = GetAllBySurveyId(surveyId);
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }
    }




}
