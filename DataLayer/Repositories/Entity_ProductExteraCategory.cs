using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ProductExteraCategory : BaseRepository<ProductExteraCategory>
    {
        private DatabaseEntities _context;
        public Entity_ProductExteraCategory(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public void DeleteByProductId(int productId)
        {
            List<ProductExteraCategory> list = Where(p=>p.ProductId==productId).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }
    }
}
