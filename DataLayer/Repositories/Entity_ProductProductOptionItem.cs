using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ProductProductOptionItem : BaseRepository<ProductProductOptionItem>
    {
        private DatabaseEntities _context;
        public Entity_ProductProductOptionItem(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public List<ProductProductOptionItem> GetAllByProductId(int productId)
        {

            return Where(p =>
                p.ProductId == productId
            ).ToList();
        }


    }
}
