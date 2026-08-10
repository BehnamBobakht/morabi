using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_Shipping : BaseRepository<Shipping>
    {
        private DatabaseEntities _context;
        public Entity_Shipping(DatabaseEntities context) : base(context)
        {
            _context = context;
        } 
        public Shipping GetByStoreId(int? storeId)
        {
            var shipping = _context.Shipping.FirstOrDefault(s => s.StoreId == storeId);
            if(shipping==null)
            {
                shipping = new Shipping()
                {
                    StoreId = storeId,
                    MaxDays=1,                   
                };
                _context.Shipping.Add(shipping);
                Save();

            }
            return shipping;
        }
        public void EditByStore(Shipping shipping)
        {
            var model = _context.Shipping.FirstOrDefault(s => s.StoreId == shipping.StoreId);
            if (model != null)
            {
                model.MaxDays = shipping.MaxDays;
                model.MinPriceForFree = shipping.MinPriceForFree;
                model.Price = shipping.Price;
                Save();
            }
        }
        public int GetShippingPrice(int? storeId,int orderPrice)
        {
            var model = GetByStoreId(storeId);
            if(model.Price > 0)
            {
                if(model.MinPriceForFree !=0 && model.MinPriceForFree != null)
                {
                    if(orderPrice >= model.MinPriceForFree)
                    {
                        return 0;
                    }
                }
                return model.Price;
            }
            return 0;
        }

    }
}
