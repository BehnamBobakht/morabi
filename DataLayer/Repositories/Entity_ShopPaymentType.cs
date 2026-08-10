using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ShopPaymentType : BaseRepository<ShopPaymentType>
    {
        private DatabaseEntities _context;
        public Entity_ShopPaymentType(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<ShopPaymentType> GetAllByShopId(int shopId)
        {
            return _context.ShopPaymentType.Where(p => p.ShopId == shopId).ToList();
        }

        public void DeleteByShopId(int ShopId)
        {
            List<ShopPaymentType> list = GetAllByShopId(ShopId);
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }
    }
}
