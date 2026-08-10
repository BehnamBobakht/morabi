using Binbin.Linq;
using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ExpressMapper.Extensions;
using DataLayer.Enumarables;

namespace DataLayer.Repositories
{
    public class Entity_ProductSubCategory : BaseRepository<ProductSubCategory>
    {
        private DatabaseEntities _context;
        public Entity_ProductSubCategory(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public ProductSubCategoryVm GetByIdVm(int id)
        {
            return FirstOrDefault(s => s.ID == id && s.Active && s.Deleted != true).Map<ProductSubCategory, ProductSubCategoryVm>();
        }
        public List<ProductSubCategory> GetAllHasSyncId()
        {
            return _context.ProductSubCategory.Where(p =>
                p.SyncId != null &&
                p.SyncId != ""
            ).ToList();
        }

        public ProductSubCategory GetBySyncId(string SyncId)
        {
            return _context.ProductSubCategory.FirstOrDefault(p => p.SyncId == SyncId);
        }

        public List<ProductSubCategoryVm> Search(string Id = null, int? notId = 0, string typeId = null, string categoryId = null, int? pageSize = 10, int? index = 1, string name = null, string customLabel = null, Enum_ProductCategoryOrder order = Enum_ProductCategoryOrder.NEW, bool? active = null)
        {
            var MyQuery = GetSearchQuery(Id: Id, notId: notId, typeId: typeId, categoryId: categoryId, name: name, customLabel: customLabel, active: active);
            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;
            if (order == Enum_ProductCategoryOrder.NEW)
            {
                return _context
                .ProductSubCategory
                .OrderByDescending(p => p.ID)
                .Where(MyQuery)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList().Map<List<ProductSubCategory>, List<ProductSubCategoryVm>>();
            }
            else
            {
                return _context
                         .ProductSubCategory
                         .OrderBy(p => p.ID)
                         .Where(MyQuery)
                         .Skip(skipValue)
                         .Take(pageValue)
                         .ToList().Map<List<ProductSubCategory>, List<ProductSubCategoryVm>>();
            }
        }

        public int SearchCount(string pbId = null, int? notId = 0, string typeId = null, string categoryId = null, string name = null)
        {
            var MyQuery = GetSearchQuery(Id: pbId, notId: notId, typeId: typeId, categoryId: categoryId, name: name);
            return _context.ProductSubCategory.Count(MyQuery);
        }

        private Expression<Func<ProductSubCategory, bool>> GetSearchQuery(string Id = null, int? notId = 0, string typeId = null, string categoryId = null, string name = null, string customLabel = null, bool? active = null)
        {
            var MyQuery = PredicateBuilder.True<ProductSubCategory>();
            MyQuery = MyQuery.And(p => p.Deleted != true);
            if (active != null)
            {
                MyQuery = MyQuery.And(p => p.Active == active.Value);
            }
   
            if (name != null)
                MyQuery = MyQuery.And(p => p.Name.Contains(name));
            if (customLabel != null)
                MyQuery = MyQuery.And(p => p.CustomLabel == customLabel);
            if (Id != null && Id != "0")
            {
                var QueryStr = PredicateBuilder.False<ProductSubCategory>();
                foreach (int item in SplitIds(Id))
                {
                    QueryStr = QueryStr.Or(p => p.ID == item);
                }
                MyQuery = MyQuery.And(QueryStr);
            }
            if (typeId != null && typeId != "0")
            {
                var QueryStr = PredicateBuilder.False<ProductSubCategory>();
                foreach (int item in SplitIds(typeId))
                {
                    QueryStr = QueryStr.Or(p => p.ProductCategory.TypeId == item);
                }
                MyQuery = MyQuery.And(QueryStr);
            }
            if (categoryId != null && categoryId != "0")
            {
                var QueryStr = PredicateBuilder.False<ProductSubCategory>();
                foreach (int item in SplitIds(categoryId))
                {
                    QueryStr = QueryStr.Or(p => p.CategoryId == item);
                }
                MyQuery = MyQuery.And(QueryStr);
            }

            if (notId != null && notId != 0)
                MyQuery = MyQuery.And(p => p.ID != notId);

            return MyQuery;
        }
        public List<ProductSubCategory> SearchBox(string name = null, int size = 5)
        {
            return Where(GetSearchQuery(name: name, active: true)).Take(size).ToList();
        }
        public List<ProductSubCategory> GetStoreProductSubCategories(int StoreId)
        {
            var model = _context.StoreProduct.Where(s => s.StoreId == StoreId && s.Product.ProductSubCategory != null).Select(s => s.Product.ProductSubCategory).Distinct().ToList();
            return model;
        }

    }
}
