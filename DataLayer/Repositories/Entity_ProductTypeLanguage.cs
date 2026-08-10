using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ProductTypeLanguage : BaseRepository<ProductTypeLanguage>
    {
        private DatabaseEntities _context;
        public Entity_ProductTypeLanguage(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<ProductTypeLanguage> GetAllByProductTypeId(int typeId)
        {
            return _context.ProductTypeLanguage.Where(p => p.ProductTypeId == typeId).ToList();
        }

        public void DeleteByProductTypeId(int typeId)
        {
            List<ProductTypeLanguage> list = GetAllByProductTypeId(typeId);
            
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
            Save();
        }
    }
}
