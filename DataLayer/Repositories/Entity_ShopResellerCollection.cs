using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;
using Binbin.Linq;

namespace DataLayer.Repositories
{
    public class Entity_ShopResellerCollection : BaseRepository<ShopResellerCollection>
    {
        private DatabaseEntities _context;
        public Entity_ShopResellerCollection(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<ShopResellerCollection> Search(
            int? notId = null,
            int? index = 1,
            int? pageSize = null,
            string name = null,
            int? resellerId = null)
        {
            List<ShopResellerCollection> results = new List<ShopResellerCollection>();
            var MyQuery = PredicateBuilder.True<ShopResellerCollection>();
            pageSize = pageSize == null ? 10 : pageSize;
            index = index == null ? 1 : index;
            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;

            if (notId != null)
                MyQuery = MyQuery.And(p => p.ID != notId);
            if (name != null)
                MyQuery = MyQuery.And(p => p.Name.Contains(name));
            if (resellerId != null)
                MyQuery = MyQuery.And(p => p.ResellerId == resellerId);

            results = _context
                .ShopResellerCollection
                .OrderBy(p => p.ID)
                .Where(MyQuery)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList();

            return results;
        }
    }
}
