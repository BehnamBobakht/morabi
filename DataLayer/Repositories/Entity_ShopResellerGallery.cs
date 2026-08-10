using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ShopResellerGallery : BaseRepository<ShopResellerGallery>
    {
        private DatabaseEntities _context;
        public Entity_ShopResellerGallery(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<ShopResellerGallery> GetAllByResellerId(int resellerId)
        {
            return _context.ShopResellerGallery.Where(p => p.ResellerId == resellerId).ToList();
        }
    }
}
