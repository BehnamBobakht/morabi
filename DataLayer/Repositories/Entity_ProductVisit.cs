using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using DataLayer.Entities;
using System.Linq.Expressions;
using Binbin.Linq;
using DataLayer.ViewModels;

namespace DataLayer.Repositories
{
    public class Entity_ProductVisit : BaseRepository<ProductVisit>
    {
        private DatabaseEntities _context;
        public List<ProductVisit> GetAllByAccountId( int accountId, int prindex = 1, int prpageSize = 10)
        {
            var query = GetSearchQuery(accountId: accountId);
            int skipValue = prpageSize * (prindex - 1);
            int pageValue = prpageSize;
            return _context.ProductVisit.Where(query).OrderByDescending(o => o.DateVisit).Skip(skipValue)
                    .Take(pageValue)
                    .ToList();
        }
        public Expression<Func<ProductVisit, bool>> GetSearchQuery(int accountId)
        {

            var MyQuery = PredicateBuilder.True<ProductVisit>();
            MyQuery = MyQuery.And(p => p.AccountId == accountId);
            MyQuery = MyQuery.And(p => p.Product.Deleted !=true);
            MyQuery = MyQuery.And(p => p.Product.Active == true);
            return MyQuery;
        }
        public ViewProductVisit SearchDetail(int accountId)
        {
            var MyQuery = GetSearchQuery(accountId);

            var result = new ViewProductVisit();

            IEnumerable<ProductVisit> q = _context.ProductVisit.Where(MyQuery);

            if (q.Any())
            {
                result.TotalCount = q.Count();

            }
            return result;
        }

        public Entity_ProductVisit(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
    
    }
}
