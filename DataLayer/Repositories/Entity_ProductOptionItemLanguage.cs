using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ProductOptionItemLanguage : BaseRepository<ProductOptionItemLanguage>
    {
        DatabaseEntities _context;
        public Entity_ProductOptionItemLanguage(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<ProductOptionItemLanguage> GetAllByProductOptionItemId(int ProductOptionItemId)
        {
            return _context.ProductOptionItemLanguage.Where(p => p.ProductOptionItemId == ProductOptionItemId).ToList();
        }

        public void DeleteByProductOptionItemId(int subCategoryId)
        {
            List<ProductOptionItemLanguage> list = GetAllByProductOptionItemId(subCategoryId);
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }
    }
}
