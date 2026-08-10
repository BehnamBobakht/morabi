using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ShopAddress : BaseRepository<ShopAddress>
    {
        private DatabaseEntities _context;
        public Entity_ShopAddress(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<ShopAddress> GetAllByShopId(int shopId)
        {
            return _context.ShopAddress.Where(p =>
                p.ShopId == shopId
            ).ToList();
        }

        public void DeleteByShopId(int shopId)
        {
            List<ShopAddress> list = GetAllByShopId(shopId);
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }
    }
}
