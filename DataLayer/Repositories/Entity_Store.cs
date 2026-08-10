using Binbin.Linq;
using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using ExpressMapper.Extensions;

namespace DataLayer.Repositories
{
    public class Entity_Store : BaseRepository<Store>
    {
        private DatabaseEntities _context;
        public Entity_Store(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public void EditByStore(Store store)
        {
            var model = GetById(store.ID);
            if (model != null)
            {
                model.Name = store.Name;
                model.Phone1 = store.Phone1;
                model.Phone2 = store.Phone2;
                model.Fax = store.Fax;
                model.Description = store.Description;
                model.Active = store.Active;
                model.AddressValue = store.AddressValue;
                model.BankName = store.BankName;
                model.BankAccountNo = store.BankAccountNo;
                model.BankAccountCardNo = store.BankAccountCardNo;
                model.BankAccountShaba = store.BankAccountShaba;
                if (store.PictureId != null)
                    model.PictureId = store.PictureId;
                Save();
                Base.BaseStore.UpdateProductQuantity(model);

            }
        }


        public List<Store> Search(string stId = null, int? notId = null, int? shopId = null, int? typeId = null, int index = 1, int pageSize = 10, string name = null, Boolean? active = null, int? categoryId = null, int? subcategoryId = null)
        {
            var MyQuery = GetSearchQuery(stId: stId, notId: notId, typeId: typeId, name: name, active: active, categoryId: categoryId, subcategoryId: subcategoryId);
            int skipValue = pageSize * (index - 1);
            int pageValue = pageSize;
            List<Store> results = new List<Store>();

            results = _context
                .Store
                .OrderBy(p => p.ID)
                .Where(MyQuery)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList();

            return results;
        }

        public List<StoreShowVm> GetActives(bool withNull=false)
        {
            var result = new List<StoreShowVm>();
            if(withNull)
            {
                result.Add(new StoreShowVm() { ID = null, Name = Resource.Lang.All });
            }
            var stores = Search(active: true);
            result.AddRange(stores.Map<List<Store>, List<StoreShowVm>>());
            return result;
        }
        private Expression<Func<Store, bool>> GetSearchQuery(string stId = null, int? notId = null, int? typeId = null, string name = null, Boolean? active = null, int? categoryId = null, int? subcategoryId = null)
        {
            var MyQuery = PredicateBuilder.True<Store>();
            name= name.ToPersianChar();
            if (stId != null)
            {
                var QueryStr = PredicateBuilder.False<Store>();
                string[] Split = stId.Split('-');
                foreach (var item in Split)
                {
                    int itemId = item.GetInteger();
                    if (itemId != 0)
                    {
                        QueryStr = QueryStr.Or(p =>
                            p.ID == itemId
                        );
                    }
                }
                MyQuery = MyQuery.And(QueryStr);
            }
            if (typeId != null)
                MyQuery = MyQuery.And(p => p.StoreProduct.Any(q => q.Product.ProductType.ID == typeId));
            if (notId != null)
                MyQuery = MyQuery.And(p => p.ID != notId);
            if (!string.IsNullOrEmpty(name))
                MyQuery = MyQuery.And(p => p.Name.Contains(name));
            if (categoryId != null)
                MyQuery = MyQuery.And(p => p.StoreProduct.Any(q => q.Product.ProductCategoryId== categoryId));
            if (subcategoryId != null)
                MyQuery = MyQuery.And(p => p.StoreProduct.Any(q => q.Product.ProductSubCategoryId == subcategoryId));
            if (active != null)
                MyQuery = MyQuery.And(p => p.Active == active && p.Approve == active);
           
           
                MyQuery = MyQuery.And(p => p.StoreProduct.Any(q => q.StoreProductQuantity.Any(s=>s.Count>0) ));
            return MyQuery;
        }
        public bool StoreIsActive(int storeId)
        {
            var model = GetById(storeId);
            if (model!=null && (model.Deleted!=true) && model.Active && model.Approve)
            {
                return true;
            }
            return false;
        }


    }
}
