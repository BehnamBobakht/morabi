using Binbin.Linq;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_Gallery : BaseRepository<Gallery>
    {
        DatabaseEntities _context;
        public Entity_Gallery(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<Gallery> GetAll(bool HasDefault)
        {
            List<Gallery> list = new List<Gallery>();
            if (HasDefault == true)
                list.Add(new Gallery() { ID = 0, Name = "انتخاب" });
            list.AddRange(_context.Gallery.OrderByDescending(p => p.ID));
            return list;
        }

        public List<Gallery> Search(
            int? websiteId = null,
            int? notId = null,
            int? index = null,
            int? pageSize = null,
            string name = null,
            bool? showInHome = null,
            string categoryLabel = null)
        {
            var MyQuery = PredicateBuilder.True<Gallery>();
            index = index == null ? 1 : index;
            pageSize = pageSize == null ? 10 : pageSize;

            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;

            if (notId != null && notId != 0)
                MyQuery = MyQuery.And(p => p.ID != notId);
            if (websiteId != null && websiteId != 0)
                MyQuery = MyQuery.And(p => p.WebsiteId == websiteId);
            if (showInHome != null)
                MyQuery = MyQuery.And(p => p.ShowInHome == showInHome);
            if (name != null)
                MyQuery = MyQuery.And(p => p.Name.Contains(name));
            if (categoryLabel != null)
                MyQuery = MyQuery.And(p => p.Category.Label == categoryLabel);


            return _context
                .Gallery
                .OrderBy(p => p.ID)
                .Where(MyQuery)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList();
        }
    }
}
