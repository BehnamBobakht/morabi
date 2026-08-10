using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ProductExteraSubCategory : BaseRepository<ProductExteraSubCategory>
    {
        private DatabaseEntities _context;
        public Entity_ProductExteraSubCategory(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public void DeleteByProductId(int productId)
        {
            List<ProductExteraSubCategory> list = Where(p=>p.ProductId==productId).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }
    }
}
