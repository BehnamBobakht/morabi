using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ExpressMapper.Extensions;

namespace DataLayer.Repositories
{
    public class Entity_Cart : BaseRepository<Cart>
    {
        DatabaseEntities _context;
        public Entity_Cart(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public Cart GetCartEntity(int? accountId = null, string syncCode = null,bool insert=false)
        {
            var model = new Cart();
            model = null;
            if (accountId != null)
            {
                model = FirstOrDefault(a => a.AccountId == accountId);
            }
            if (model == null && (!string.IsNullOrEmpty(syncCode)))
            {
                model = FirstOrDefault(a => a.SyncCode == syncCode);
            }
            if (model == null && insert)
            {
                model = Insert(accountId: accountId,syncCode);
            }
            else if(model==null)
            {
                model = new Cart();
            }
            return model;
        }
        public ResultVm<CartVm> GetCart(int? accountId=null, string syncCode=null)
        {
            var result = new ResultVm<CartVm>();
            Cart model = GetCartEntity(accountId, syncCode);
            if (model != null)
            {
                result.Model = model.Map<Cart, CartVm>();
                result.Model.HaveAddress = model.CartStore.Any(s => s.CartStoreItem.Any(x => x.Product.IsService != true));
                if (result.Model.HaveAddress == false)
                {
                    result.Model.AddressId = 0;
                    var delivey = result.Model.DeliveryList.FirstOrDefault(s => s.BasePrice == 0 && s.MaxDays == 0);
                    if (delivey != null)
                        result.Model.DeliveryId = delivey.ID;

                }
                else
                {
                    var delivey = result.Model.DeliveryList.FirstOrDefault(s => s.BasePrice != 0 && s.MaxDays != 0);
                    if (delivey != null)
                        result.Model.DeliveryId = delivey.ID;
                }
                //if (model.CartStore.Any(s => s.CartStoreItem.Any(x => x.Product.ProductTypeId ==1) && s.CartStoreItem.Count()==1))
                //{

                //    var delivey = result.Model.DeliveryList.FirstOrDefault(s => s.BasePrice == 0 && s.MaxDays == 0);
                //    if (delivey != null)
                //        result.Model.DeliveryId = delivey.ID;

                //}
                result.Status = Enumarables.ResultStatus.Success;

            }

            return result;
        }
        public Cart Insert(int? accountId,string syncCode)
        {
            var newItem = new Cart()
            {
                AccountId = accountId,
                SyncCode=syncCode,
                CreateDate = DateTime.Now,
                LastUpdateDate = DateTime.Now
            };
            if(string.IsNullOrEmpty(syncCode))
            {
                newItem.SyncCode = Guid.NewGuid().ToString();
            }
            Insert(newItem);
            Save();
            return GetById(newItem.ID);
        }

        public void DeleteExpired(int dateAgo=5)
        {
            var date = DateTime.Now.AddDays((dateAgo * -1));
            var carts = Where(s => s.LastUpdateDate < date).ToList();
            foreach(var cart in carts)
            {
                foreach(var cartStore in cart.CartStore.ToList())
                {
                    foreach(var cartStoreItem in cartStore.CartStoreItem.ToList())
                    {
                        _context.CartStoreItem.Remove(cartStoreItem);
                    }
                    _context.CartStore.Remove(cartStore);
                }
                _context.Cart.Remove(cart);
                Save();
            }

        }
    }
}
