using Binbin.Linq;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_QuestionRegisterItem : BaseRepository<QuestionRegisterItem>
    {
        private DatabaseEntities _context;
        public Entity_QuestionRegisterItem(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<QuestionRegisterItem> Search(
            int? typeId = null,
            int? fieldId = null,
            int? notId = null,
            string fieldName = null,
            string value = null,
            int? index = 1,
            int? pageSize = null)
        {
            List<QuestionRegisterItem> results = new List<QuestionRegisterItem>();
            var MyQuery = PredicateBuilder.True<QuestionRegisterItem>();
            pageSize = pageSize == null ? 10 : pageSize;
            index = index == null ? 1 : index;
            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;
            
          
            if (fieldId != null)
                MyQuery = MyQuery.And(p => p.FieldId == fieldId);
           
            if (notId != null)
                MyQuery = MyQuery.And(p => p.ID != notId);
            if (value != null)
                MyQuery = MyQuery.And(p => p.Value.Contains(value));

            results = _context
                .QuestionRegisterItem
                .OrderBy(p => p.ID)
                .Where(MyQuery)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList();

            return results;
        }
    }
}
