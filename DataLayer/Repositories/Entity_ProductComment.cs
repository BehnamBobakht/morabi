using Binbin.Linq;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ProductComment : BaseRepository<ProductComment>
    {
        private DatabaseEntities _context;
        public Entity_ProductComment(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<ProductComment> Search(
            int? notId = null,
            int? accountId = null,
            int? productId = null,
            int? index = 1,
            int? pageSize = null,
            string body = null,
            bool? approved = null,
            bool notSee = false,
            int? id = null)
        {
            List<ProductComment> results = new List<ProductComment>();
            var MyQuery = PredicateBuilder.True<ProductComment>();
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
            if (productId != null)
                MyQuery = MyQuery.And(p => p.ProductId == productId);
            if (body != null)
                MyQuery = MyQuery.And(p => p.Body.Contains(body));
            if (approved != null)
                MyQuery = MyQuery.And(p => (p.Approved != null ? p.Approved : false) == approved);

            if (notSee == true)
                MyQuery = MyQuery.And(p => p.Approved == null);

            results = _context
                .ProductComment
                .OrderByDescending(p => p.ID)
                .Where(MyQuery)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList();

            return results;
        }
    }
}
