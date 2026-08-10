using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ProductTypeBrand : BaseRepository<ProductTypeBrand>
    {
        private DatabaseEntities _context;
        public Entity_ProductTypeBrand(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<ProductTypeBrand> GetAllByBrandId(int brandId)
        {
            return _context.ProductTypeBrand.Where(p => p.ProductBrandId == brandId).ToList();
        }

        public List<ProductTypeBrand> GetAllByTypeId(int typeId)
        {
            return _context.ProductTypeBrand.Where(p => p.ProductTypeId == typeId).ToList();
        }

        public void DeleteByBrandId(int BrandId)
        {
            List<ProductTypeBrand> list = GetAllByBrandId(BrandId);
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }
    }
}
