using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ProductColor : BaseRepository<ProductColor>
    {
        private DatabaseEntities _context;
        public Entity_ProductColor(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<ProductColor> GetAllByProductId(int productId)
        {
            return _context.ProductColor.Where(p =>
                p.ProductId == productId
            ).ToList();
        }

        public void DeleteByProductId(int productId)
        {
            List<ProductColor> list = GetAllByProductId(productId);
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }
    }
}
