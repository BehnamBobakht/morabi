using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;

namespace DataLayer.Repositories
{
    public class Entity_ProductBrandLanguage : BaseRepository<ProductBrandLanguage>
    {

        private DatabaseEntities _context;
        public Entity_ProductBrandLanguage(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
               
        public void DeleteByBrandId(int BrandID)
        {
            List<ProductBrandLanguage> list = GetAllByBrandId(BrandID);
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }

        public List<ProductBrandLanguage> GetAllByBrandId(int BrandID)
        {
            return _context.ProductBrandLanguage.Where(p =>p.ProductBrandId == BrandID).ToList();
        }

    }
}
