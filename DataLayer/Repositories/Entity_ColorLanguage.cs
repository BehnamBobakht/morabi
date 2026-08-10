using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ColorLanguage : BaseRepository<ColorLanguage>
    {
        DatabaseEntities _context;
        public Entity_ColorLanguage(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<ColorLanguage> GetAllByColorId(int colorId)
        {
            return _context.ColorLanguage.Where(p => p.ColorId == colorId).ToList();
        }

        public void DeleteByColorId(int subCategoryId)
        {
            List<ColorLanguage> list = GetAllByColorId(subCategoryId);
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }
    }
}
