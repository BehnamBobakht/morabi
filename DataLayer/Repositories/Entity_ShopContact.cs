using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ShopContact : BaseRepository<ShopContact>
    {
        private DatabaseEntities _context;
        public Entity_ShopContact(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<ShopContact> GetAllByShopId(int ShopId)
        {
            return _context.ShopContact.Where(p => p.ShopId == ShopId).ToList();
        }

        public void DeleteByShopId(int shopId)
        {
            List<ShopContact> list = GetAllByShopId(shopId);
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }
    }
}
