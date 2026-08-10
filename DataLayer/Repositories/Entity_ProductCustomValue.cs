using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ProductCustomValue : BaseRepository<ProductCustomValue>
    {
        private DatabaseEntities _context;
        public Entity_ProductCustomValue(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<ProductCustomValue> GetAllByProductId(int productId)
        {
            return _context.ProductCustomValue.Where(p =>
                p.ProductId == productId
            ).ToList();
        }

        public void DeleteByProductId(int productId)
        {
            List<ProductCustomValue> list = GetAllByProductId(productId);
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }
    }
}
