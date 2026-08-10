using Binbin.Linq;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using ExpressMapper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_PriceRange : BaseRepository<PriceRange>
    {
        DatabaseEntities _context;



        public Entity_PriceRange(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public ResultVm<PriceRangeVm> AddRange(PriceRangeVm priceRange)
        {
            var result = new ResultVm<PriceRangeVm>();
            if (priceRange.StartCount<1)
            {
                result.Message = Resource.Notify.InvalidPriceRange;
                result.Status = ResultStatus.Error;
                return result;
            }
            if (priceRange.StartCount>priceRange.EndCount)
            {
                result.Message = Resource.Notify.InvalidPriceRange;
                result.Status = ResultStatus.Error;
                return result;
            }

            if (priceRange.Price<1)
            {
                result.Message = Resource.Notify.PleaseInsertThePrice;
                result.Status = ResultStatus.Error;
                return result;
            }
            var list = Where(p => p.StoreProductQuantityId == priceRange.StoreProductQuantityId);
            if (list.Any(s => s.StartCount <= priceRange.StartCount && (!s.EndCount.HasValue)))
            {
                result.Message = Resource.Notify.InvalidPriceRange;
                result.Status = ResultStatus.Error;
                return result;
            }     
            if (list.Any(s => s.StartCount <= priceRange.StartCount && s.EndCount >= priceRange.StartCount))
            {
                result.Message = Resource.Notify.InvalidPriceRange;
                result.Status = ResultStatus.Error;
                return result;
            }     
            if (list.Any(s => s.StartCount > priceRange.StartCount &&  (!priceRange.EndCount.HasValue)))
            {
                result.Message = Resource.Notify.InvalidPriceRange;
                result.Status = ResultStatus.Error;
                return result;
            }      
            if (list.Any(s => s.StartCount > priceRange.StartCount && s.StartCount <=  priceRange.EndCount))
            {
                result.Message = Resource.Notify.InvalidPriceRange;
                result.Status = ResultStatus.Error;
                return result;
            }
            var model = priceRange.Map<PriceRangeVm, PriceRange>();
            Insert(model);
            Save();
            result.Status = ResultStatus.Success;
            return result;
        }
        public List<PriceRange> GetByStoreproductQuantityId(int id)
        {
            return Where(s => s.StoreProductQuantityId == id).ToList();
        }
        public PriceRangeVm GetPriceRangeVm(int id)
        {
            return GetById(id).Map<PriceRange, PriceRangeVm>();
        }
        public ResultVm<PriceRangeVm> Update(PriceRangeVm priceRangeVm)
        {
            var result = new ResultVm<PriceRangeVm>();
            if (priceRangeVm.Price < 1)
            {
                result.Message = Resource.Notify.PleaseInsertThePrice;
                result.Status = ResultStatus.Error;
                return result;
            }
            var model = GetById(priceRangeVm.ID);
            model.Price = priceRangeVm.Price;
            Update(model);
            Save();
            result.Status = ResultStatus.Success;
            return result;
        }
    }
}
