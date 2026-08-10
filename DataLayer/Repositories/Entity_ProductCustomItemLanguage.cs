using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ProductCustomItemLanguage : BaseRepository<ProductCustomItemLanguage>
    {
        private DatabaseEntities _context;
        public Entity_ProductCustomItemLanguage(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<ProductCustomItemLanguage> GetAllByItemId(int itemId)
        {
            return _context.ProductCustomItemLanguage.Where(p => p.FieldItemId == itemId).ToList();
        }

        public void DeleteByProductCustomItemId(int itemId)
        {
            List<ProductCustomItemLanguage> list = GetAllByItemId(itemId);
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }
    }
}
