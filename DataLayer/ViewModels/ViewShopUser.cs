using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewShopUser
    {
        public int Id { get; set; }
        public string RowId { get; set; }
        public string User { get; set; }
        public string Shop { get; set; }

        public ViewShopUser(ShopUser shopuser, int index, string MaxZero)
        {
            this.Id = shopuser.ID;
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
            this.User = shopuser.SiteUser.FullName;
            this.Shop = shopuser.Shop.Name;
        }
    }
}
