using Binbin.Linq;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_SurveyAnswerDetail : BaseRepository<SurveyAnswerDetail>
    {
        private DatabaseEntities _context;
        public Entity_SurveyAnswerDetail(DatabaseEntities context) : base(context)
        {
            _context = context;
        }


    }
}
