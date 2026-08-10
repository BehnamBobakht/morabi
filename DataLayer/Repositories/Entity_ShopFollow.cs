using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ShopFollow : BaseRepository<ShopFollow>
    {
        private DatabaseEntities _context;
        public Entity_ShopFollow(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<ShopFollow> GetAllByAccountId(int AccountId)
        {
            return _context.ShopFollow.Where(p => p.AccountId == AccountId).ToList();
        }

        public ShopFollow GetByShopIdAndAccountId(int ShopId, int AccountId)
        {
            return _context.ShopFollow.FirstOrDefault(p =>
                p.ShopId == ShopId &&
                p.AccountId == AccountId
            );
        }

        public bool IsAny(int ShopId, int AccountId)
        {
            return _context.ShopFollow.Any(p =>
                p.ShopId == ShopId &&
                p.AccountId == AccountId
            );
        }

        public List<ShopFollow> GetAllByShopId(int ShopId)
        {
            return _context.ShopFollow.Where(p => p.ShopId == ShopId).ToList();
        }

        public void DeleteByShopId(int shopId)
        {
            List<ShopFollow> list = GetAllByShopId(shopId);
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }
    }
}
