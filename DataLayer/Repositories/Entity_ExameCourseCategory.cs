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
    public class Entity_ExameCourseCategory : BaseRepository<ExameCourseCategory>
    {
        private DatabaseEntities _context;
        public Entity_ExameCourseCategory(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<ExameCourseCategory> Search(int? notId = null, int? categoryId = null, string categoryLabel = null, int index = 1, int pageSize = 10, string name = null)
        {
            var MyQuery = PredicateBuilder.True<ExameCourseCategory>();
            if (notId != null && notId != 0)
                MyQuery = MyQuery.And(p => p.ID != notId);

            

            int skipValue = pageSize * (index - 1);
            int pageValue = pageSize;

            return _context.ExameCourseCategory.OrderBy(p => p.ID).Where(MyQuery).Skip(skipValue).Take(pageValue).ToList();
        }

    }
}
