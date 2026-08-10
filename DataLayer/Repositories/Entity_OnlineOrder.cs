using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_OnlineOrder : BaseRepository<OnlineOrder>
    {
        private DatabaseEntities _context;
        public Entity_OnlineOrder(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public Boolean CheckNotExistRefrenceCode(string refrenceCode)
        {
            return _context.OnlineOrder.Any(x => x.RefrenceCode == refrenceCode);
        }

        public List<OnlineOrder> GetAllOnlineOrder()
        {
            return _context.OnlineOrder.OrderByDescending(x => x.ID).ToList();
        }

    }
}
