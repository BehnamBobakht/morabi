using Binbin.Linq;
using DataLayer.Entities;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpressMapper.Extensions;

namespace DataLayer.Repositories
{
    public class Entity_Color : BaseRepository<Color>
    {
        private DatabaseEntities _context;
        public Entity_Color(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<ColorVm> Search(
            int? notId = null,
            int? index = 1,
            int? pageSize = null,
            int? typeId = null,
            string name = null)
        {
            List<ColorVm> results = new List<ColorVm>();
            var MyQuery = PredicateBuilder.True<Color>();
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
                .Color
                .OrderBy(p => p.ID)
                .Where(MyQuery)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList().Map<List<Color>, List<ColorVm>>();

            return results;
        }

        public List<Color> GetAllProductTypeId(int ProductTypeId)
        {
            return _context.Color.Where(p =>
                p.ProductTypeId == ProductTypeId ||
                p.ProductTypeId == null
            )
            .OrderBy(p => p.HexValue)
            .ThenBy(p => p.Name)
            .ToList();
        }
        public List<ColorVm> GetAllByProductTypeForProduct(int productTypeId, int? productId = null, List<int> selectedColors = null)
        {
            var result = new List<ColorVm>();
            var list = GetAllProductTypeId(productTypeId);
            foreach (var item in list)
            {
                var listItem = item.Map<Color, ColorVm>();
                if (item.ProductColor.Any(s => s.ProductId == productId))
                {
                    listItem.Exist = true;
                    listItem.Selected = true;
                }
                if (selectedColors != null && selectedColors.Any())
                {
                    if (selectedColors.Any(s => s.Equals(item.ID)))
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
