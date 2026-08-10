using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ProductExteraType : BaseRepository<ProductExteraType>
    {
        private DatabaseEntities _context;
        public Entity_ProductExteraType(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public void DeleteByProductId(int productId)
        {
            List<ProductExteraType> list = Where(p=>p.ProductId==productId).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }
    }
}
