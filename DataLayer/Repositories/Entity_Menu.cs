using DataLayer.Entities;
using DataLayer.Base;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Binbin.Linq;

namespace DataLayer.Repositories
{
    public class Entity_Menu : BaseRepository<Menu>
    {
        DatabaseEntities _context;
        public Entity_Menu(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<Menu> Search(int? notId = null, int? parentId = null, string name = null, int index = 1, int pageSize = 10)
        {
            var MyQuery = PredicateBuilder.True<Menu>();
            if (notId != null)
                MyQuery = MyQuery.And(p => p.ID != notId);
            if (parentId != null && parentId != 0)
                MyQuery = MyQuery.And(p => p.ParentId == parentId);
            if (parentId == 0)
                MyQuery = MyQuery.And(p => p.ParentId == null);
            if (name != null)
                MyQuery = MyQuery.And(p => p.Name.Contains(name));

            int skipValue = pageSize * (index - 1);
            int pageValue = pageSize;
            List<Menu> results = _context
                .Menu
                .OrderBy(p => p.ID)
                .Where(MyQuery)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList();

            return results;
        }
    }
}
