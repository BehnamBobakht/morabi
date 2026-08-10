using Binbin.Linq;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_SendType : BaseRepository<SendType>
    {
        private DatabaseEntities _context;
        public Entity_SendType(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<SendType> Search(int? pageSize = 10, int? index = 1)
        {
            var MyQuery = PredicateBuilder.True<SendType>();
            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;

            return _context
                .SendType
                .OrderBy(p => p.ID)
                .Where(MyQuery)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList();
        }
        public SendType GetByStoreId(int? storeId)
        {
            if (storeId == null)
            {
                return null;
            }
            var shipping = _context.SendType.FirstOrDefault(s => s.StoreId == storeId);
            try
            {
                if (shipping == null)
                {
                    shipping = new SendType()
                    {
                        StoreId = storeId,
                        MaxDays = 1,
                    };
                    _context.SendType.Add(shipping);
                    Save();

                }
            }
            catch (Exception ex)
            {

            }
           
            return shipping;
        }
        public void EditByStore(SendType sendType)
        {
            var model = _context.SendType.FirstOrDefault(s => s.StoreId == sendType.StoreId);
            if (model != null)
            {
                model.MaxDays = sendType.MaxDays;
                model.MinPriceForFree = sendType.MinPriceForFree;
                model.BasePrice = sendType.BasePrice;
                model.MaxDays = sendType.MaxDays;
                model.MaxPrice = sendType.MaxPrice;
                model.MinPriceForFree = sendType.MinPriceForFree;
                Save();
            }
        }
        public double GetShippingPrice(int? storeId, double orderPrice)
        {
            var model = GetByStoreId(storeId);
            if (model == null)
            {
                return 0;
            }
            if (model.BasePrice.HasValue && model.BasePrice > 0)
            {
                if (model.MinPriceForFree != 0 && model.MinPriceForFree != null)
                {
                    if (orderPrice >= model.MinPriceForFree)
                    {
                        return 0;
                    }
                }
                return model.BasePrice.Value;
            }
            return 0;
        }
    }
}
