using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using ExpressMapper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_CacheSetting : BaseRepository<CacheSetting>
    {
        private DatabaseEntities _context;
        public Entity_CacheSetting(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public CacheSettingVm GetByLabel(Enum_OutputCache enum_OutputCache)
        {
            var item = FirstOrDefault(s => s.Label == enum_OutputCache.ToString());
            if(item!=null)
            {
                return item.Map<CacheSetting, CacheSettingVm>();
            }
            return null;
        }
    }
}
