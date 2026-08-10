using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_AccountReagent : BaseRepository<AccountReagent>
    {
        private DatabaseEntities _context;
        public Entity_AccountReagent(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
    }
}
