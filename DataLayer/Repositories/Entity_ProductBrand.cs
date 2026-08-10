using Binbin.Linq;
using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.ViewModels;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ExpressMapper.Extensions;

namespace DataLayer.Repositories
{
    public class Entity_ProductBrand : BaseRepository<ProductBrand>
    {
        private DatabaseEntities _context;
        public Entity_ProductBrand(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public ProductBrandVm GetBrandVm(int id)
        {
            var product = GetById(id);
            if (product != null)
            {
                return product.Map<ProductBrand, ProductBrandVm>();
            }
            else { return null; }
        }


        public List<ProductBrand> GetAllHasSyncId()
        {

            return _context.ProductBrand.Where(p =>
                p.SyncId != null &&
                p.SyncId != ""
            ).ToList();
        }

        public List<ProductBrandVm> Search(
            bool withAll = false,
            string pbId = null,
            int? notId = null,
            int? shopId = null,
            int? typeId = null,
            int? categoryId = null,
            int? subcategoryId = null,
            int index = 1, int pageSize = 10, string name = null, int? userId = null, Enum_ProductBrandOrder order = Enum_ProductBrandOrder.NONE)
        {
            var MyQuery = GetSearchQuery(withAll: withAll, pbId: pbId, notId: notId, shopId: shopId, typeId: typeId, categoryId: categoryId, subcategoryId: subcategoryId, name: name, userId: userId);
            int skipValue = pageSize * (index - 1);
            int pageValue = pageSize;
            List<ProductBrand> results = new List<ProductBrand>();
            if (order == Enum_ProductBrandOrder.MORE_PRODUCT)
            {
                results = _context
                    .ProductBrand
                    .OrderByDescending(p => p.Product.Count)
                    .Where(MyQuery)
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList();
            }
            else if (order == Enum_ProductBrandOrder.NAME)
            {
                results = _context
                    .ProductBrand
                    .OrderBy(p => p.Name)
                    .Where(MyQuery)
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList();
            }
            else
            {
                results = _context
                    .ProductBrand
                    .OrderBy(p => p.ShowNumber)
                    .Where(MyQuery)
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList();
            }

            return results.Map<List<ProductBrand>, List<ProductBrandVm>>();
        }

        public int SearchCount(bool withAll = false,
            string pbId = null,
            int? notId = null,
            int? shopId = null,
            int? typeId = null,
            int? categoryId = null,
            int? subcategoryId = null,
            string name = null,
            int? userId = null)
        {
            var MyQuery = GetSearchQuery(withAll:withAll, pbId: pbId, notId: notId, shopId: shopId, typeId: typeId, categoryId: categoryId, subcategoryId: subcategoryId, name: name, userId: userId);
            return _context.ProductBrand.Count(MyQuery);
        }

        private Expression<Func<ProductBrand, bool>> GetSearchQuery(
            bool withAll=false,
            string pbId = null,
            int? notId = null,
            int? shopId = null,
            int? typeId = null,
            int? categoryId = null,
            int? subcategoryId = null,
            string name = null,
            int? userId = null)
        {
            var MyQuery = PredicateBuilder.True<ProductBrand>();
            if (name != null)
                MyQuery = MyQuery.And(p => p.Name.Contains(name));
            if (pbId != null)
            {
                var QueryStr = PredicateBuilder.False<ProductBrand>();
                string[] Split = pbId.Split('-');
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
                MyQuery = MyQuery.And(p =>(withAll && p.ProductTypeId==null) || p.ProductTypeId == typeId || ( p.ProductTypeBrand.Any(q => q.ProductTypeId == typeId)));
            if (categoryId != null)
                MyQuery = MyQuery.And(p => p.Product.Any(s => s.ProductCategoryId == categoryId));
            if (subcategoryId != null)
                MyQuery = MyQuery.And(p => p.Product.Any(s => s.ProductSubCategoryId == subcategoryId));
            if (notId != null)
                MyQuery = MyQuery.And(p => p.ID != notId);

            return MyQuery;
        }
        public List<ProductBrand> SearchBox(string name = null, int size = 5)
        {
            return Where(GetSearchQuery(name: name)).OrderByDescending(s => s.ID).OrderByDescending(s => s.ID).Take(size).ToList();
        }
    }
}
