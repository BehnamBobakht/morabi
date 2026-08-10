using Binbin.Linq;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ExameAccountAnswerDetails : BaseRepository<ExameAccountAnswerDetails>
    {
        private DatabaseEntities _context;
        public Entity_ExameAccountAnswerDetails(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public bool Isvalid(int? ExameAnswerId , int QuestionId )
        {
          
            return _context.ExameAccountAnswerDetails.Any(s => s.ExameAnswerId == ExameAnswerId && s.QuestionId == QuestionId);
        }
        public List<ExameAccountAnswerDetails> Search(int? notId = null, int? categoryId = null, string categoryLabel = null, int index = 1, int pageSize = 10, string name = null)
        {
            var MyQuery = PredicateBuilder.True<ExameAccountAnswerDetails>();
            if (notId != null && notId != 0)
                MyQuery = MyQuery.And(p => p.ID != notId);

            

            int skipValue = pageSize * (index - 1);
            int pageValue = pageSize;

            return _context.ExameAccountAnswerDetails.OrderBy(p => p.ID).Where(MyQuery).Skip(skipValue).Take(pageValue).ToList();
        }

    }
}
