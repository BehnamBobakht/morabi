using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ShopReportBlock : BaseRepository<ShopReportBlock>
    {
        private DatabaseEntities _context;
        public Entity_ShopReportBlock(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public ShopReportBlock GetByShopIdAndAccountId(int ShopId, int AccountId)
        {
            return _context.ShopReportBlock.FirstOrDefault(p =>
                p.ShopId == ShopId &&
                p.AccountId == AccountId
            );
        }
    }
}
