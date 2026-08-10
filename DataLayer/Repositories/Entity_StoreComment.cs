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
    public class Entity_StoreComment : BaseRepository<StoreComment>
    {
        private DatabaseEntities _context;
        public Entity_StoreComment(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<StoreComment> Search(
            int? notId = null,
            int? accountId = null,
            int? storeId = null,
            int? index = 1,
            int? pageSize = null,
            string body = null,
            bool? approved = null,
            bool notSee = false,
            int? id = null)
        {
            List<StoreComment> results = new List<StoreComment>();
            var MyQuery = PredicateBuilder.True<StoreComment>();
            pageSize = pageSize == null ? 10 : pageSize;
            index = index == null ? 1 : index;
            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;

            if (id != null)
                MyQuery = MyQuery.And(p => p.ID == id);
            if (notId != null)
                MyQuery = MyQuery.And(p => p.ID != notId);
            if (accountId != null)
                MyQuery = MyQuery.And(p => p.AccountId == accountId);
            if (storeId != null)
                MyQuery = MyQuery.And(p => p.StoreId == storeId);
            if (body != null)
                MyQuery = MyQuery.And(p => p.Body.Contains(body));
            if (approved != null)
                MyQuery = MyQuery.And(p => (p.Approved != null ? p.Approved : false) == approved);

            if (notSee == true)
                MyQuery = MyQuery.And(p => p.Approved == null);

            results = _context
                .StoreComment
                .OrderByDescending(p => p.ID)
                .Where(MyQuery)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList();

            return results;
        }

        public int SearchCount(int? storeId = null, int? accountId = null)
        {
            var MyQuery = GetSearchQuery(accountId: accountId, storeId: storeId);
            var count = _context.StoreComment.Where(MyQuery).Count();
            return count;
        }
        public Expression<Func<StoreComment, bool>> GetSearchQuery(int? storeId = null, int? accountId = null)
        {
            var MyQuery = PredicateBuilder.True<StoreComment>();
            if (accountId != null)
                MyQuery = MyQuery.And(p => p.Store.AccountId == accountId);
            if (storeId != null)
                MyQuery = MyQuery.And(p => p.StoreId == storeId);

            return MyQuery;
        }
    }
}
