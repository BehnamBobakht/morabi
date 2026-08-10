using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewClearing
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public string Detail { get; set; }
        public int StoreId { get; set; }
        public Nullable<int> PictureId { get; set; }
        public int CodeId { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string FromAccount { get; set; }
        public string ToAccount { get; set; }

        public List<ViewAccountOrder> AccountOrder { get; set; }
        public ViewCode Code { get; set; }
        public ViewStore Store { get; set; }
        public ViewPicture Picture { get; set; }

        public ViewClearing() { }

        public ViewClearing(Clearing clearing)
        {
            if (clearing != null)
            {
                this.Id = clearing.ID;
                this.Price = clearing.Price;                
                this.Detail = clearing.Detail;
                this.StoreId = clearing.StoreId;
                this.PictureId = clearing.PictureId;
                this.CreateDate = clearing.CreateDate;
                this.CodeId = clearing.CodeId;
                this.FromAccount = clearing.FromAccount;
                this.ToAccount = clearing.ToAccount;

                this.Code = new ViewCode(clearing.Code);
                this.Picture = new ViewPicture(clearing.Picture);
                this.Store = new ViewStore(clearing.Store);

            }
        }
    }
}
