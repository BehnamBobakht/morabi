using Binbin.Linq;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_CompetitiveFeature : BaseRepository<CompetitiveFeature>
    {
        private DatabaseEntities _context;
        public Entity_CompetitiveFeature(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public List<CompetitiveFeature> Search(int index = 1, int pageSize = 10, string name = null)
        {
            List<CompetitiveFeature> results = new List<CompetitiveFeature>();
            int skipValue = pageSize * (index - 1);
            int pageValue = pageSize;

            var MyQuery = GetSearchQuery(name: name);
            return _context.CompetitiveFeature.Where(MyQuery)
                .OrderByDescending(p => p.ID)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList();
        }
        public int SearchCount(string name = null)
        {
            var MyQuery = GetSearchQuery(
                name: name);
            return _context.CompetitiveFeature.Count(MyQuery);
        }

        private Expression<Func<CompetitiveFeature, bool>> GetSearchQuery(
            string name = null)
        {
            var MyQuery = PredicateBuilder.True<CompetitiveFeature>();
            if (IsNullOrEmptyId(name) == false)
            {
                MyQuery = MyQuery.And(p => p.TexCompetitiveFeature.Contains(name));
            }
           
            return MyQuery;
        }
    }
}
