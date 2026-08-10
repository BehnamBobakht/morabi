using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_EmailAddress : BaseRepository<EmailAddress>
    {
        DatabaseEntities _context;
        public Entity_EmailAddress(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<EmailAddress> GetEmailsForSync()
        {
            return _context.EmailAddress.Where(p => 
                p.AutoSync == true && 
                p.EmailHost.AutoSync == true && 
                p.EmailHost.Active == true).ToList();
        }
    }
}
