using Binbin.Linq;
using DataLayer.Entities;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_Shop : BaseRepository<Shop>
    {
        private DatabaseEntities _context;
        public Entity_Shop(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<Shop> GetAllShopForUserId(int UserId)
        {
            return _context.Shop.Where(p =>
                p.ShopUser.Any(s => s.UserId == UserId) ||
                p.UserCreatorId == UserId
            ).ToList();
        }

        public Shop GetFirstOrDefaultForUserId(int UserId)
        {
            return _context.Shop.FirstOrDefault(p =>
                p.ShopUser.Any(s => s.UserId == UserId)
            );
        }

        public List<Shop> Search(
            int? notId = null,
            int? typeId = null,
            int? index = 1,
            int? pageSize = null,
            string name = null)
        {
            List<Shop> results = new List<Shop>();
            var MyQuery = PredicateBuilder.True<Shop>();
            pageSize = pageSize == null ? 10 : pageSize;
            index = index == null ? 1 : index;
            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;
            
            if (typeId != null)
                MyQuery = MyQuery.And(p => p.ShopProductType.Any(s => s.ProductTypeId == typeId));
            if (notId != null)
                MyQuery = MyQuery.And(p => p.ID != notId);
            if (name != null)
                MyQuery = MyQuery.And(p => p.Name.Contains(name));

            results = _context
                .Shop
                .OrderBy(p => p.ID)
                .Where(MyQuery)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList();

            return results;
        }

        public Shop GetByLabel(string label)
        {
            return _context.Shop.FirstOrDefault(p => p.Label == label);
        }
    }
}
