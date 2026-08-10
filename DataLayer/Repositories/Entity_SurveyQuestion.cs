using Binbin.Linq;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_SurveyQuestion : BaseRepository<SurveyQuestion>
    {
        private DatabaseEntities _context;
        public Entity_SurveyQuestion(DatabaseEntities context) : base(context)
        {
            _context = context;
        }



        public List<SurveyQuestion> Search(
            int? notId = 0,
            int? surveyId = 0,
            int? pageSize = 10,
            int? index = 1,
            Enum_SurveyQuestion order = Enum_SurveyQuestion.NONE,
            string question = null,
            string surveyLabel = null,
            Boolean? active = null)
        {
            var MyQuery = GetSearchQuery(notId: notId, surveyId: surveyId, question: question, surveyLabel: surveyLabel, active: active);
            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;

            if (order == Enum_SurveyQuestion.NEW)
            {
                return _context
                    .SurveyQuestion
                    .OrderByDescending(p => p.ID)
                    .Where(MyQuery)
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList();
            }
            else if (order == Enum_SurveyQuestion.OLD)
            {
                return _context
                    .SurveyQuestion
                    .OrderBy(p => p.ID)
                    .Where(MyQuery)
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList();
            }
            else if (order == Enum_SurveyQuestion.NUMBER)
            {
                return _context
                    .SurveyQuestion
                    .OrderBy(p => p.ShowNumber).ThenBy(x => x.ID)
                    .Where(MyQuery)
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList();
            }
            else
            {
                return _context
                    .SurveyQuestion
                    .OrderBy(p => p.ID)
                    .Where(MyQuery)
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList();
            }

        }

        public int SearchCount(int? notId = 0, int? surveyId = 0, string question = null, Boolean? active = null)
        {
            var MyQuery = GetSearchQuery(notId: notId, surveyId: surveyId, question: question, active: active);
            return _context.SurveyQuestion.Count(MyQuery);
        }

        private Expression<Func<SurveyQuestion, bool>> GetSearchQuery(int? notId = 0, int? surveyId = 0, string question = null, string surveyLabel = null, Boolean? active = null)
        {
            var MyQuery = PredicateBuilder.True<SurveyQuestion>();
            if (surveyId != null && surveyId != 0)
                MyQuery = MyQuery.And(p => p.SurveyId == surveyId);
            if (surveyLabel != null)
                MyQuery = MyQuery.And(p => p.Survey.Label == surveyLabel);
            if (notId != null && notId != 0)
                MyQuery = MyQuery.And(p => p.ID != notId);
            if (question != null)
                MyQuery = MyQuery.And(p => p.Question.Contains(question));
            if (active != null)
                MyQuery = MyQuery.And(p => p.Active == active);
            return MyQuery;
        }
    }
}
