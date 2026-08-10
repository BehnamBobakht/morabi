using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;
using Binbin.Linq;
using DataLayer.ViewModels;
using ExpressMapper.Extensions;
namespace DataLayer.Repositories
{
    public class Entity_Slider : BaseRepository<Slider>
    {
        private DatabaseEntities _context;
        public Entity_Slider(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<SliderVm> Search(
            int? notId = null,
            int? index = null,
            int? pageSize = null,
            int? websiteId = null)
        {
            var MyQuery = PredicateBuilder.True<Slider>();
            index = index == null ? 1 : index;
            pageSize = pageSize == null ? 10 : pageSize;
            
            List<Slider> results = new List<Slider>();
            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;

            if (notId != null && notId != 0)
                MyQuery = MyQuery.And(p => p.ID != notId);

            if (websiteId != null)
                MyQuery = MyQuery.And(p => p.WebsiteId == websiteId);

            return _context
                .Slider
                .OrderBy(p => p.ShowNumber)
                .Where(MyQuery)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList().Map<List<Slider>, List<SliderVm>>();
        }
    }
}
