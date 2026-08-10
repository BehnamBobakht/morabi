using Binbin.Linq;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_Country : BaseRepository<Country>
    {
        private DatabaseEntities _context;
        public Entity_Country(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<Country> Search(
            int? notId = null,
            int? index = null,
            int? pageSize = null,
            string name = null,
             bool isActiveShipping = false,
            int storeId = 0)
        {
            var MyQuery = PredicateBuilder.True<Country>();
            index = index == null ? 1 : index;
            pageSize = pageSize == null ? 10 : pageSize;

            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;

            if (notId != null && notId != 0)
                MyQuery = MyQuery.And(p => p.ID != notId);
            if (name != null)
                MyQuery = MyQuery.And(p => p.FaName.Contains(name) || p.EnName.Contains(name));
            if (isActiveShipping == true)
            {
                var storeStateList = _context.ShippingCity.Where(s => s.StoreId == storeId).ToList();
                foreach (var item in storeStateList)
                {
                    MyQuery = MyQuery.And(p => p.ID != item.CountryId);
                }

            }
            return _context
                .Country
                .OrderBy(p => p.ID)
                .Where(MyQuery)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList();
        }
    }
}
