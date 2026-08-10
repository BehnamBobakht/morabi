using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ProductCategoryLanguage : BaseRepository<ProductCategoryLanguage>
    {
        private DatabaseEntities _context;
        public Entity_ProductCategoryLanguage(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<ProductCategoryLanguage> GetAllByProductCategoryId(int categoryId)
        {
            return _context.ProductCategoryLanguage.Where(p => p.ProductCategoryId == categoryId).ToList();
        }

        public void DeleteByProductCategoryId(int categoryId)
        {
            List<ProductCategoryLanguage> list = GetAllByProductCategoryId(categoryId);
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }
    }
}
