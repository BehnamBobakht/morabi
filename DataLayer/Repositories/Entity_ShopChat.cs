using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;

namespace DataLayer.Repositories
{
    public class Entity_ShopChat : BaseRepository<ShopChat>
    {
        private DatabaseEntities _context;
        public Entity_ShopChat(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<ShopChat> GetAllByShopIdAndAccountId(int ShopId, int AccountId)
        {
            return _context.ShopChat.Where(p =>
                p.ShopId == ShopId &&
                p.AccountId == AccountId
            ).ToList();
        }

        public List<ShopChat> GetAllUnreadAndAccountId(int AccountId)
        {
            return _context.ShopChat.Where(p => 
                p.AccountId == AccountId && 
                p.IsRead == false
            ).ToList();
        }

        public void DoReadAllChatByAccountForShop(Account account, int shopId)
        {
            foreach (ShopChat item in account.ShopChat.Where(p =>
                p.IsRead == false &&
                p.ShopId == shopId &&
                p.IsAccountSend == true))
            {
                item.IsRead = true;
            }
        }

        public void DoReadAllChatByShopForAccount(Shop shop, int accountId)
        {
            foreach (ShopChat item in shop.ShopChat.Where(p =>
                p.IsRead == false &&
                p.AccountId == accountId &&
                p.IsAccountSend == false))
            {
                item.IsRead = true;
            }
        }

        public List<ShopChat> GetAllByShopId(int shopId)
        {
            return _context.ShopChat.Where(p =>
                p.ShopId == shopId
            ).ToList();
        }

        public void DeleteByShopId(int shopId)
        {
            List<ShopChat> list = GetAllByShopId(shopId);
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }
    }
}
