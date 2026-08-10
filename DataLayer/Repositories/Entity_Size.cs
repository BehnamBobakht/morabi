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
    public class Entity_Size : BaseRepository<Size>
    {
        private DatabaseEntities _context;
        public Entity_Size(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<Size> GetAllProductTypeId(int ProductTypeId)
        {
            return _context.Size.Where(p =>
                p.ProductTypeId == ProductTypeId ||
                p.ProductTypeId == null
            ).OrderBy(p => p.Name).ToList();
        }

        public List<Size> Search(
            int? notId = null,
            int? index = 1,
            int? pageSize = null,
            int? typeId = null,
            string name = null)
        {
            List<Size> results = new List<Size>();
            var MyQuery = PredicateBuilder.True<Size>();
            pageSize = pageSize == null ? 10 : pageSize;
            index = index == null ? 1 : index;
            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;

            if (notId != null)
                MyQuery = MyQuery.And(p => p.ID != notId);
            if (string.IsNullOrEmpty(name) == false)
                MyQuery = MyQuery.And(p => p.Name.Contains(name));
            if (typeId != null)
                MyQuery = MyQuery.And(p => p.ProductTypeId == typeId);

            results = _context
                .Size
                .OrderBy(p => p.ID)
                .Where(MyQuery)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList();

            return results;
        }
        public List<SizeVm> GetAllByProductTypeForProduct(int productTypeId, int? productId = null, List<int> selectedSizes = null)
        {
            var result = new List<SizeVm>();
            var list = GetAllProductTypeId(productTypeId);
            foreach (var item in list)
            {
                var listItem = item.Map<Size, SizeVm>();
                if (item.ProductSize.Any(s => s.ProductId == productId))
                {
                    listItem.Exist = true;
                    listItem.Selected = true;
                }
                if (selectedSizes != null && selectedSizes.Any())
                {
                    if (selectedSizes.Any(s => s.Equals(item.ID)))
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
