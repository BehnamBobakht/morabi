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
    public class Entity_SurveyAnswerDetailItem : BaseRepository<SurveyAnswerDetailItem>
    {
        private DatabaseEntities _context;
        public Entity_SurveyAnswerDetailItem(DatabaseEntities context) : base(context)
        {
            _context = context;
        }


    }
}
