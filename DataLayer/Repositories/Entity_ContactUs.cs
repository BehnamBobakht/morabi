using Binbin.Linq;
using DataLayer.Entities;
using DataLayer.Base;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ContactUs : BaseRepository<ContactUs>
    {
        DatabaseEntities _context;



        public Entity_ContactUs(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<ContactUs> Search(
  int? index = 1,
  int? pageSize = null,
  string title = null,
  string name = null,

  DateTime? fromDatetime = null,
  DateTime? toDatetime = null

  )
        {
            var MyQuery = GetSearchQuery(
 title: title,
                name: name, fromDatetime: fromDatetime,
            toDatetime: toDatetime
                );

            pageSize = pageSize == null ? 10 : pageSize;
            index = index == null ? 1 : index;
            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;

            List<ContactUs> results = _context
                .ContactUs
                .OrderByDescending(p => p.ID)
                .Where(MyQuery)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList();

            return results;
        }
        public int SearchCount(
  string title = null,
  string name = null
            , DateTime? fromDatetime = null, DateTime? toDatetime = null
         )
        {
            var MyQuery = GetSearchQuery(
                title: title,
                name: name,
                fromDatetime: fromDatetime,
                toDatetime: toDatetime
             );
            return _context.ContactUs.Count(MyQuery);
        }
        private Expression<Func<ContactUs, bool>> GetSearchQuery(
  string title = null,
  string name = null, DateTime? fromDatetime = null,
         DateTime? toDatetime = null
         )
        {
            List<ContactUs> results = new List<ContactUs>();
            var MyQuery = PredicateBuilder.True<ContactUs>();

            title = title.ToPersianChar();
            if (!string.IsNullOrEmpty(title))
                MyQuery = MyQuery.And(p => p.Title.Contains(title));
            name = name.ToPersianChar();
            if (!string.IsNullOrEmpty(name))
                MyQuery = MyQuery.And(p => p.Name.Contains(name));

            if (fromDatetime != null && fromDatetime != default(DateTime))
                MyQuery = MyQuery.And(p => p.CreateDate >= fromDatetime);

            if (toDatetime != null && toDatetime != default(DateTime))
            {
                DateTime toDatetimeTemp = toDatetime.Value.AddDays(1);
                MyQuery = MyQuery.And(p => p.CreateDate <= toDatetimeTemp);
            }

            return MyQuery;
        }


    }
}
