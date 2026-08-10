using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ProductSubCategoryLanguage : BaseRepository<ProductSubCategoryLanguage>
    {
        private DatabaseEntities _context;
        public Entity_ProductSubCategoryLanguage(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<ProductSubCategoryLanguage> GetAllByProductSubCategoryId(int subCategoryId)
        {
            return _context.ProductSubCategoryLanguage.Where(p => p.ProductSubCategoryId == subCategoryId).ToList();
        }

        public void DeleteByProductSubCategoryId(int subCategoryId)
        {
            List<ProductSubCategoryLanguage> list = GetAllByProductSubCategoryId(subCategoryId);
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }
    }
}
