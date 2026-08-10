using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewShop
    {
        public int Id { get; set; }
        public string RowId { get; set; }
        public string Website { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public int LikeCount { get; set; }
        public ViewPicture Picture { get; set; }
        public List<ViewProduct> Products { get; set; }
        public List<ViewShopChat> Chats { get; set; }
        public List<ViewShopFollow> Follows { get; set; }
        public List<ViewShopContact> ShopContacts { get; set; }
        public string Description { get; set; }

        public ViewShop() { }

        public ViewShop(Shop shop)
        {
            if (shop != null)
            {
                this.Id = shop.ID;
                this.Name = shop.Name;
                this.Label = shop.Label;
            }
        }

        public ViewShop(Shop shop, int index, string MaxZero)
        {
            this.Id = shop.ID;
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
            this.Name = shop.Name;
            this.Label = shop.Label;
            this.Website = shop.Website.Title;
        }
    }
}
