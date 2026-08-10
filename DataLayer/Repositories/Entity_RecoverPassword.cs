using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_RecoverPassword : BaseRepository<RecoverPassword>
    {
        DatabaseEntities _context;
        public Entity_RecoverPassword(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public RecoverPassword GetByUniqueId(Guid UniqueId)
        {
            return _context.RecoverPassword.FirstOrDefault(p => 
                p.UniqueId == UniqueId && 
                p.IsUsed == false);
        }
    }
}
