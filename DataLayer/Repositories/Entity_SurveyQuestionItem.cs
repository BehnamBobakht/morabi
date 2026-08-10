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
    public class Entity_SurveyQuestionItem : BaseRepository<SurveyQuestionItem>
    {
        private DatabaseEntities _context;
        public Entity_SurveyQuestionItem(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public List<SurveyQuestionItem> Search(
            int? notId = 0,
            int? surveyId = 0,
            int? questionId = 0,
            int? pageSize = 10,
            int? index = 1,
            Enum_SurveyQuestion order = Enum_SurveyQuestion.NONE,
            string name = null)
        {
            var MyQuery = GetSearchQuery(notId: notId, questionId: questionId, name: name, surveyId: surveyId);
            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;

            if (order == Enum_SurveyQuestion.NEW)
            {
                return _context
                    .SurveyQuestionItem
                    .OrderByDescending(p => p.ID)
                    .Where(MyQuery)
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList();
            }
            else if (order == Enum_SurveyQuestion.OLD)
            {
                return _context
                    .SurveyQuestionItem
                    .OrderBy(p => p.ID)
                    .Where(MyQuery)
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList();
            }
            else if (order == Enum_SurveyQuestion.NUMBER)
            {
                return _context
                    .SurveyQuestionItem
                    .OrderBy(p => p.ShowNumber)
                    .Where(MyQuery)
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList();
            }
            else
            {
                return _context
                    .SurveyQuestionItem
                    .OrderBy(p => p.ID)
                    .Where(MyQuery)
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList();
            }

        }

        public int SearchCount(int? notId = 0, int? questionId = 0, string name = null)
        {
            var MyQuery = GetSearchQuery(notId: notId, questionId: questionId, name: name);
            return _context.SurveyQuestionItem.Count(MyQuery);
        }

        private Expression<Func<SurveyQuestionItem, bool>> GetSearchQuery(int? notId = 0, int? surveyId = 0, int? questionId = 0, string name = null)
        {
            var MyQuery = PredicateBuilder.True<SurveyQuestionItem>();
            if (surveyId != null && surveyId != 0)
                MyQuery = MyQuery.And(p => p.SurveyQuestion.Survey.ID == surveyId);
            if (questionId != null && questionId != 0)
                MyQuery = MyQuery.And(p => p.QuestionId == questionId);
            if (notId != null && notId != 0)
                MyQuery = MyQuery.And(p => p.ID != notId);
            if (name != null)
                MyQuery = MyQuery.And(p => p.Name.Contains(name));
            return MyQuery;
        }
    }
}
