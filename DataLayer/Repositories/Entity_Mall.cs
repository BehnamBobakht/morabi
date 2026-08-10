using Binbin.Linq;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_Mall : BaseRepository<Mall>
    {
        private DatabaseEntities _context;
        public Entity_Mall(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<Mall> Search(int? cityId = null, string name = null, int pageSize = 10, int pageIndex = 1)
        {
            pageIndex = pageSize * (pageIndex - 1);
            var MyQuery = PredicateBuilder.True<Mall>();
            if (cityId != null)
                MyQuery = MyQuery.And(p => p.CityId == cityId);
            if (name != null)
                MyQuery = MyQuery.And(p => p.Name.Contains(name));
            return 
                _context.Mall.
                OrderBy(p => p.ID)
                .Where(MyQuery)
                .Skip(pageIndex)
                .Take(pageSize)
                .ToList();
        }
    }
}
