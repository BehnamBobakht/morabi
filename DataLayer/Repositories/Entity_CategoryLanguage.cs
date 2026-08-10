using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;

namespace DataLayer.Repositories
{
    public class Entity_CategoryLanguage : BaseRepository<CategoryLanguage>
    {

        private DatabaseEntities _context;
        public Entity_CategoryLanguage(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
               
        public void DeleteByCategoryId(int categoryID)
        {
            List<CategoryLanguage> list = GetAllByCategoryId(categoryID);
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }

        public List<CategoryLanguage> GetAllByCategoryId(int categoryID)
        {
            return _context.CategoryLanguage.Where(p =>p.CategoryId == categoryID).ToList();
        }

    }
}
