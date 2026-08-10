using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ProductCustomFieldLanguage : BaseRepository<ProductCustomFieldLanguage>
    {
        private DatabaseEntities _context;
        public Entity_ProductCustomFieldLanguage(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<ProductCustomFieldLanguage> GetAllByFieldId(int fieldId)
        {
            return _context.ProductCustomFieldLanguage.Where(p => p.FieldId == fieldId).ToList();
        }

        public void DeleteByProductCustomFieldId(int typeId)
        {
            List<ProductCustomFieldLanguage> list = GetAllByFieldId(typeId);
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }
    }
}
