using Binbin.Linq;
using DataLayer.Entities;
using DataLayer.ViewModels;
using ExpressMapper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ProductOptionItem : BaseRepository<ProductOptionItem>
    {
        private DatabaseEntities _context;
        public Entity_ProductOptionItem(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public List<ProductOptionItem> Search(int productOptionId , int? pageSize = null, int? index = null)
        {
            List<ProductOptionItem> results = new List<ProductOptionItem>();
            var MyQuery = PredicateBuilder.True<ProductOptionItem>();
            pageSize = pageSize == null ? 10 : pageSize;
            index = index == null ? 1 : index;
            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;

            MyQuery = MyQuery.And(p =>p.OptionId==productOptionId);

            results = _context
                .ProductOptionItem                
                .Where(MyQuery)
                .OrderBy(p=>p.ID)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList();

            return results;
        }

        public List<ProductOptionItem> GetAllByProductId(int productId)
        {
            var list = _context.ProductOptionItem.Where(p => p.ProductOption.Product.Any(s => s.ID == productId)).ToList();
            return list;
        }
        public List<ProductOptionItem> GetAllOptionId(int productOptionId)
        {
            var list = _context.ProductOptionItem.Where(p => p.OptionId==productOptionId).ToList();
            return list;
        }
        public List<ProductOptionItemVm> GetAllByOptionForProduct(int productOptionId, int? productId = null, List<int> selectedOptions = null)
        {
            var result = new List<ProductOptionItemVm>();
            var list = GetAllOptionId(productOptionId);
            foreach (var item in list)
            {
                var listItem = item.Map<ProductOptionItem, ProductOptionItemVm>();
                if (item.ProductProductOptionItem.Any(s => s.ProductId == productId))
                {
                    listItem.Exist = true;
                    listItem.Selected = true;
                }
                if (selectedOptions != null && selectedOptions.Any())
                {
                    if (selectedOptions.Any(s => s.Equals(item.ID)))
                    {
                        listItem.Selected = true;
                    }
                }
                result.Add(listItem);
            }
            return result;
        }
    }
}
