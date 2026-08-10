using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ProductOptionLanguage : BaseRepository<ProductOptionLanguage>
    {
        DatabaseEntities _context;
        public Entity_ProductOptionLanguage(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<ProductOptionLanguage> GetAllByProductOptionId(int ProductOptionId)
        {
            return _context.ProductOptionLanguage.Where(p => p.ProductOptionId == ProductOptionId).ToList();
        }

        public void DeleteByProductOptionId(int subCategoryId)
        {
            List<ProductOptionLanguage> list = GetAllByProductOptionId(subCategoryId);
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }
    }
}
