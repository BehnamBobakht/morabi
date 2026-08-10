using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ShopProductType : BaseRepository<ShopProductType>
    {
        DatabaseEntities _context;
        public Entity_ShopProductType(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<ShopProductType> GetAllByShopId(int shopId)
        {
            return _context.ShopProductType.Where(p => p.ShopId == shopId).ToList();
        }

        public void DeleteByShopId(int ShopId)
        {
            List<ShopProductType> list = GetAllByShopId(ShopId);
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }
    }
}
