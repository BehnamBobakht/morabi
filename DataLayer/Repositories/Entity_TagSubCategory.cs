using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_TagSubCategory : BaseRepository<TagSubcategory>
    {
        private DatabaseEntities _context;
        public Entity_TagSubCategory(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public void DeleteByProductSubCategoryId(int subCategoryId)
        {
            List<TagSubcategory> list = GetAllByProductSubCategoryId(subCategoryId);
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }
        public List<TagSubcategory> GetAllByProductSubCategoryId(int subCategoryId)
        {
            return _context.TagSubcategory.Where(p => p.SubCategoryId == subCategoryId).ToList();
        }
    }
}
