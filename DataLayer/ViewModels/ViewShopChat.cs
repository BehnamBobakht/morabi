using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewShopChat
    {
        public int Id { get; set; }
        public string RowId { get; set; }
        public ViewShop Shop { get; set; }
        public ViewAccount Account { get; set; }
        public string Body { get; set; }
        public DateTime Datetime { get; set; }
        public bool IsAccountSend { get; set; }
        public bool IsRead { get; set; }
        public int UnreadCount { get; set; }

        public ViewShopChat() { }

        public ViewShopChat(ShopChat shopchat, int index, string MaxZero)
        {
            this.Id = shopchat.ID;
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
            this.Account = new ViewAccount(shopchat.Account);
            this.Shop = new ViewShop(shopchat.Shop);
        }
    }
}
