using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ProductSize : BaseRepository<ProductSize>
    {
        private DatabaseEntities _context;
        public Entity_ProductSize(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<ProductSize> GetAllByProductId(int productId)
        {
            return Where(p =>
                p.ProductId == productId
            ).ToList();
        }

        public void DeleteByProductId(int productId)
        {
            List<ProductSize> list = GetAllByProductId(productId);
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }
    }
}
