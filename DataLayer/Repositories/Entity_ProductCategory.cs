using Binbin.Linq;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ExpressMapper.Extensions;

namespace DataLayer.Repositories
{
    public class Entity_ProductCategory : BaseRepository<ProductCategory>
    {
        private DatabaseEntities _context;
        public Entity_ProductCategory(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public ProductCategoryVm GetByIdVm(int id)
        {
            return FirstOrDefault(s => s.ID == id && s.Active && s.Deleted != true).Map<ProductCategory, ProductCategoryVm>();
        }
        public ProductCategory GetByCustomLabel(string customLabel)
        {
            return FirstOrDefault(p => p.CustomLabel.Trim().ToLower().Replace(" ", "-") == customLabel.Trim().ToLower().Replace(" ", "-"));
        }
        public ProductCategory GetBySyncId(string SyncId)
        {
            return _context.ProductCategory.FirstOrDefault(p => p.SyncId == SyncId);
        }

        public List<ProductCategory> GetAllHasSyncId()
        {
            return _context.ProductCategory.Where(p =>
                p.SyncId != null &&
                p.SyncId != ""
            ).ToList();
        }

        public List<ProductCategoryVm> Search(
            int? notId = 0,
            int? typeId = 0,
            int? pageSize = 10,
            int? index = 1,
            Enum_ProductCategoryOrder order = Enum_ProductCategoryOrder.NONE,
            string name = null,
            string customLabel = null,
            bool? active=null)
        {
            var MyQuery = GetSearchQuery(notId: notId, typeId: typeId, name: name, customLabel: customLabel,active:active);
            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;

            if (order == Enum_ProductCategoryOrder.NEW)
            {
                return _context
                    .ProductCategory
                    .OrderByDescending(p => p.ID)
                    .Where(MyQuery)
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList().Map<List<ProductCategory>, List<ProductCategoryVm>>();
            }
            else
            {
                return _context
                    .ProductCategory
                    .OrderBy(p => p.ID)
                    .Where(MyQuery)
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList().Map<List<ProductCategory>, List<ProductCategoryVm>>();
            }

        }

        public int SearchCount(int? notId = 0, int? typeId = 0, string name = null)
        {
            var MyQuery = GetSearchQuery(notId: notId, typeId: typeId, name: name);
            return _context.ProductCategory.Count(MyQuery);
        }

        private Expression<Func<ProductCategory, bool>> GetSearchQuery(int? notId = 0, int? typeId = 0, string name = null, string customLabel = null, bool? active = null)
        {
            var MyQuery = PredicateBuilder.True<ProductCategory>();           
            MyQuery= MyQuery.And(p => p.Deleted != true);
            if (active != null)
            {
                MyQuery = MyQuery.And(p => p.Active == active.Value);
            }
        
            if (name != null)
                MyQuery = MyQuery.And(p => p.Name.Contains(name));
            if (customLabel != null)
                MyQuery = MyQuery.And(p => p.CustomLabel == customLabel);
            if (typeId != null && typeId != 0)
                MyQuery = MyQuery.And(p => p.TypeId == typeId);
            if (notId != null && notId != 0)
                MyQuery = MyQuery.And(p => p.ID != notId);
           
            return MyQuery;
        }
        public List<ProductCategory> SearchBox(string name = null, int size = 5)
        {
            return Where(GetSearchQuery(name: name, active: true)).OrderByDescending(s => s.ID).Take(size).ToList();
        }
        public List<ProductCategory> GetStoreProductCategories(int StoreId)
        {
            var model = _context.StoreProduct.Where(s => s.StoreId == StoreId && s.Product.ProductCategory != null).Select(s => s.Product.ProductCategory).Distinct().ToList();
            return model;
        }
    }
}
