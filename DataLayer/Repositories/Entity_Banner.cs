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
    public class Entity_Banner : BaseRepository<Banner>
    {
        private DatabaseEntities _context;
        public Entity_Banner(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<Banner> Search(int? notId = null, int? categoryId = null, string categoryLabel = null, int index = 1, int pageSize = 10, string name = null, Enum_BannerOrder order = Enum_BannerOrder.NONE)
        {
            var MyQuery = PredicateBuilder.True<Banner>();
            if (notId != null && notId != 0)
                MyQuery = MyQuery.And(p => p.ID != notId);
            if (categoryId != null && categoryId != 0)
                MyQuery = MyQuery.And(p => p.CategoryId == categoryId);
            if (categoryLabel != null)
                MyQuery = MyQuery.And(p => p.Category.Label == categoryLabel);
            if (name != null)
                MyQuery = MyQuery.And(p => p.Name.Contains(name));

            int skipValue = pageSize * (index - 1);
            int pageValue = pageSize;

            if (order == Enum_BannerOrder.NEW)
                return _context.Banner.OrderByDescending(p => p.ID).Where(MyQuery).Skip(skipValue).Take(pageValue).ToList();
            else if (order == Enum_BannerOrder.OLD)
                return _context.Banner.OrderBy(p => p.ID).Where(MyQuery).Skip(skipValue).Take(pageValue).ToList();
            else if (order == Enum_BannerOrder.SHOWNUMBER)
                return _context.Banner.OrderBy(p => p.ShowNumber).Where(MyQuery).Skip(skipValue).Take(pageValue).ToList();
            else
                return _context.Banner.OrderBy(p => p.ID).Where(MyQuery).Skip(skipValue).Take(pageValue).ToList();
        }
        
    }
}
