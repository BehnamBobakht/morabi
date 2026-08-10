using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_AccountOrderComment : BaseRepository<AccountOrderComment>
    {
        private DatabaseEntities _context;
        public Entity_AccountOrderComment(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<AccountOrderComment> GetAllByOrderId(int orderId)
        {
            return _context.AccountOrderComment.Where(p => 
                p.OrderId == orderId
            ).ToList();
        }
    }
}
