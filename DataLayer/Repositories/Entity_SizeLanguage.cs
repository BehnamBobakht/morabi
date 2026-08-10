using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_SizeLanguage : BaseRepository<SizeLanguage>
    {
        private DatabaseEntities _context;
        public Entity_SizeLanguage(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<SizeLanguage> GetAllBySizeId(int sizeId)
        {
            return _context.SizeLanguage.Where(p => p.SizeId == sizeId).ToList();
        }

        public void DeleteBySizeId(int sizeId)
        {
            List<SizeLanguage> list = GetAllBySizeId(sizeId);
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }
    }
}
