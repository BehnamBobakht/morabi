using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewStore
    {
        public int ID { get; set; }
        public Nullable<int> PictureId { get; set; }
        public Nullable<int> AccountId { get; set; }
        public string Name { get; set; }
        public int BenefitPercent { get; set; }
        public string AddressValue { get; set; }
        public string Phone1 { get; set; }
        public double ? Rate { get; set; }
        public string Phone2 { get; set; }
        public string Fax { get; set; }
        public string Website { get; set; }
        public string Description { get; set; }
        public bool Approve { get; set; }
        public bool Active { get; set; }
        public ViewAccount Account { get; set; }
        public ViewPicture Picture { get; set; }
        public ViewSendType SendType { get; set; }
        public List<ViewStoreCompetitiveFeature> StoreCompetitiveFeatures { get; set; }
        public ViewSendTypeStores SendTypeStores { get; set; }
        public ViewShippingCity ShippingCity { get; set; }
        //public ICollection<StoreComment> Comments { get; set; }
        public ViewStore()
        { }
        public ViewStore(DataLayer.Entities.Store store, bool complate = true,AccountAddress address=null)
        {

            
            this.StoreCompetitiveFeatures = new List<ViewStoreCompetitiveFeature>();
            foreach (var item in store.StoreCompetitiveFeature)
            {
                StoreCompetitiveFeatures.Add(new ViewStoreCompetitiveFeature(item));
            }
            this.ID = store.ID;
            this.PictureId = store.PictureId;
            this.Name = store.Name;
            this.Picture = new ViewPicture(store.Picture);
            this.Phone1 = store.Phone1;
            this.Approve = store.Approve;
            this.Active = store.Active;
            this.Rate = store.StoreComment!=null? store.StoreComment.Where(s => s.Rate.HasValue).Average(s => s.Rate):null;
            //this.Comments = store.SendType != null ? new ViewSendType(store.SendType.First()) : null;  store.StoreComment;
            if (complate)
            {
                this.AccountId = store.AccountId;
                this.BenefitPercent = store.BenefitPercent;
                this.AddressValue = store.AddressValue;
                this.Phone2 = store.Phone2;
                this.Fax = store.Fax;
                this.Website = store.Website;
                this.Description = store.Description;
                this.Account = new ViewAccount(store.Account,address);
            }
        }
       
    }
}
