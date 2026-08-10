using DataLayer.Entities;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ShopUser : BaseRepository<ShopUser>
    {
        private DatabaseEntities _context;
        public Entity_ShopUser(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<ShopUser> GetAllByShopId(int ShopId)
        {
            return _context.ShopUser.Where(p => p.ShopId == ShopId).ToList();
        }

        public List<ShopUser> GetAllByUserId(int UserId)
        {
            return _context.ShopUser.Where(p => p.UserId == UserId).ToList();
        }

        public void DeleteByUserId(int userId)
        {
            List<ShopUser> list = GetAllByUserId(userId);
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }

        public void DeleteByShopId(int shopId)
        {
            List<ShopUser> list = GetAllByShopId(shopId);
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }

        public bool HasShopUser(int ShopId, int UserId)
        {
            return _context.ShopUser.Any(p =>
                p.ShopId == ShopId &&
                p.UserId == UserId
            );
        }
    }
}
