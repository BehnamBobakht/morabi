using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using ExpressMapper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace DataLayer.Base
{
    public static class BaseCache
    {
        private static List<CacheSettingVm> Caches = null;
        public static CacheSettingVm GetByLabel(Enum_OutputCache enum_OutputCache)
        {
            var result = new CacheSettingVm() { Duration = 1, Active = true };
            if (Caches == null)
            {
                var unitOfWork = new UnitOfWork();
                Caches = unitOfWork.CacheSetting.GetAll().Map<List<CacheSetting>, List<CacheSettingVm>>();        
                unitOfWork.Dispose();
            }
            var exist = Caches.FirstOrDefault(s => s.Label == Enum_OutputCache.Defualt.ToString() && s.Active==false );
            if (exist != null)
            {
                return result;
            }
            exist = Caches.FirstOrDefault(s => s.Label == enum_OutputCache.ToString() && s.Active && s.Duration > 1);
            if (exist!=null)
            {
                result = exist;
            }

            return result;
        }
    }
}
