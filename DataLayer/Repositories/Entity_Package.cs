using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_Package : BaseRepository<Package>
    {
        private DatabaseEntities _context;
        public Entity_Package(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public Package GetByShopId(int shopId)
        {
            return _context.Package.FirstOrDefault(p =>
                p.ShopPackage.Any(s =>
                    s.ShopId == shopId
                )
            );
        }
    }
}
