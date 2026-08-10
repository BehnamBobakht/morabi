using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ProductBrandUser : BaseRepository<ProductBrandUser>
    {
        private DatabaseEntities _context;
        public Entity_ProductBrandUser(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<ProductBrandUser> GetAllByBrandId(int BrandId)
        {
            return _context.ProductBrandUser.Where(p => p.ProductBrandId == BrandId).ToList();
        }

        public void DeleteByBrandId(int userId)
        {
            List<ProductBrandUser> list = GetAllByBrandId(userId);
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }
    }
}
