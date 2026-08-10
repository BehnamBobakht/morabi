using Binbin.Linq;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.Base;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using DataLayer.Api;
using System.Device.Location;

namespace DataLayer.Repositories
{
    public class Entity_StoreProduct : BaseRepository<StoreProduct>
    {
        private DatabaseEntities _context;
        public Entity_StoreProduct(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public List<StoreProduct> GetAllNotApproved()
        {
            string not_checked_status = Enum_Code.STOREPRODUCT_REQUEST_NOT_CHECKED.ToString();
            return _context.StoreProduct.Where(p => p.Code.Label == not_checked_status && p.Product.Active).ToList();
        }
        public void MultipleInsert(List<StoreProduct> storeProducts)
        {
            foreach (var item in storeProducts)
            {
                if (ProductNotDuplicate(item))
                {
                    Insert(item);
                    Save();
                }
            }
        }
        public bool ProductNotDuplicate(StoreProduct storeProductId)
        {
            return !_context.StoreProduct.Any(p => p.StoreId == storeProductId.StoreId && p.ProductId == storeProductId.ProductId);

        }
        public List<StoreProduct> Search(
                       int? statusId = null,
                       int? index = 1,
                       int? pageSize = null,
                       string name = null,
                       int? accountId = null,
                       int? storeId = null)
        {
            var results = new List<StoreProduct>();
            pageSize = pageSize == null ? 10 : pageSize;
            index = index == null ? 1 : index;
            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;

            var MyQuery = GetSearchQuery(statusId: statusId, name: name, accountId: accountId, storeId: storeId);
            results = _context
           .StoreProduct
           .OrderBy(o => o.StatusId)
           .ThenByDescending(p => p.ID)
           .Where(MyQuery)
           .Skip(skipValue)
           .Take(pageValue)
           .ToList();
            return results;
        }
        public int SearchCount(
    int? statusId = null,
    string name = null,
    int? accountId = null,
    int? storeId = null)
        {
            var MyQuery = GetSearchQuery(statusId: statusId, name: name, accountId: accountId, storeId: storeId);

            var count = _context
                .StoreProduct
                .Where(MyQuery).Count();

            return count;
        }
        public Expression<Func<StoreProduct, bool>> GetSearchQuery(
            int? statusId = null,
            string name = null,
            int? accountId = null,
            int? storeId = null)
        {
            var MyQuery = PredicateBuilder.True<StoreProduct>();
            name = name.ToPersianChar();
            if (accountId != null)
                MyQuery = MyQuery.And(p => p.Store.AccountId == accountId);
            if (statusId != null)
                MyQuery = MyQuery.And(p => p.StatusId == statusId);
            if (name != null && name != string.Empty)
                MyQuery = MyQuery.And(p => p.Product.Name.Contains(name));
            if (storeId != null)
                MyQuery = MyQuery.And(p => p.StoreId == storeId);

            return MyQuery;
        }

    }
}
