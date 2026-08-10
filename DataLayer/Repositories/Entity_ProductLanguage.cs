using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ProductLanguage : BaseRepository<ProductLanguage>
    {
        private DatabaseEntities _context;
        public Entity_ProductLanguage(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<ProductLanguage> GetAllByProductId(int productId)
        {
            return _context.ProductLanguage.Where(p =>
                p.ProductId == productId
            ).ToList();
        }

        public void DeleteByProductId(int productId)
        {
            List<ProductLanguage> list = GetAllByProductId(productId);
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }
    }
}
