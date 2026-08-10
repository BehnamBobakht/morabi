using DataLayer.Api;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_NotificationApp : BaseRepository<NotificationApp>
    {
        private DatabaseEntities _context;
        public Entity_NotificationApp(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
    }
}
