using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;

namespace DataLayer.Repositories
{
    public class Entity_BannerLanguage : BaseRepository<BannerLanguage>
    {

        private DatabaseEntities _context;
        public Entity_BannerLanguage(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
               
        public void DeleteByBannerId(int bannerID)
        {
            List<BannerLanguage> list = GetAllByBannerId(bannerID);
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }

        public List<BannerLanguage> GetAllByBannerId(int bannerID)
        {
            return Where(p =>p.BannerId == bannerID).ToList();
        }

    }
}
