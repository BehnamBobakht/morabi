using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ShopPicture : BaseRepository<ShopPicture>
    {
        private DatabaseEntities _context;
        public Entity_ShopPicture(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<ShopPicture> GetAllByShopId(int shopId)
        {
            return _context.ShopPicture.Where(p => p.ShopId == shopId).ToList();
        }
    }
}
