using Binbin.Linq;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using ExpressMapper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_Survey : BaseRepository<Survey>
    {
        DatabaseEntities _context;

        public Entity_Survey(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<Survey> Search(
            int? notId = 0,
            int? pageSize = 10,
            int? index = 1,
            string name = null

            )
        {
            var MyQuery = PredicateBuilder.True<Survey>();
            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;

            if (notId != null && notId != 0)
                MyQuery = MyQuery.And(p => p.ID != notId);
            if (name != null)
                MyQuery = MyQuery.And(p => p.Name.Contains(name));

            return _context
                .Survey
                .OrderBy(p => p.ID)
                .Where(MyQuery)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList();
        }
        public SurveyAnswerVm GetSurveyVm(int surveyId)
        {
            var survey = GetById(surveyId);
            if(survey!=null)
            {
                var result =new SurveyAnswerVm();
                result.SurveyId = surveyId;
                result.SurveyVm = survey.Map<Survey, SurveyVm>();
                result.SurveyAnswerDetailsVm = result.SurveyVm.SurveyQuestionsVm.Where(s=>s.Active!=false).OrderBy(o=>o.ShowNumber).ToList().Map<List<SurveyQuestionVm>, List<SurveyAnswerDetailVm>>();
                return result;
            }
            return null;
        }
    }
}
