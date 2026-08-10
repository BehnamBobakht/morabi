using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_SyncLog : BaseRepository<SyncLog>
    {
        DatabaseEntities _context;
        public Entity_SyncLog(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<SyncLog> GetLog(int Count)
        {
            return _context.SyncLog.OrderByDescending(p => p.ID).Take(Count).ToList();
        }
    }
}
