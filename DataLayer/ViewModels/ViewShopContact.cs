using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewShopContact
    {
        public int Id { get; set; }
        public string RowId { get; set; }
        public ViewShop Shop { get; set; }
        public ViewCode Type { get; set; }
        public string Value { get; set; }

        public ViewShopContact() { }

        public ViewShopContact(ShopContact contact, int index, string MaxZero)
        {
            this.Id = contact.ID;
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
            this.Shop = new ViewShop(contact.Shop);
            this.Type = new ViewCode(contact.Code);
            this.Value = contact.Value;
        }
    }
}
