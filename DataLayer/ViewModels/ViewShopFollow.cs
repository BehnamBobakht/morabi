using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewShopFollow
    {
        public int Id { get; set; }

        public string RowId { get; set; }
        public ViewShop Shop { get; set; }
        public ViewAccount Account { get; set; }
        public DateTime Datetime { get; set; }

        public ViewShopFollow() { }

        public ViewShopFollow(ShopFollow follow, int index, string MaxZero)
        {
            this.Id = follow.ID;
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
            this.Account = new ViewAccount(follow.Account);
            this.Shop = new ViewShop(follow.Shop);
            this.Datetime = follow.Datetime;
        }
    }
}
