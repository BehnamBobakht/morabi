using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_EmailInbox : BaseRepository<EmailInbox>
    {
        DatabaseEntities _context;
        public Entity_EmailInbox(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public int GetEmailInboxCountByEmailAddress(EmailAddress email)
        {
            return _context.EmailInbox.Count(p => p.EmailId == email.ID);
        }

        public List<EmailInbox> GetAllBySiteUser(int UserId)
        {
            return _context.EmailInbox.Where(p => p.UserId == UserId).OrderByDescending(p => p.EmailDatetime).ToList();
        }
    }
}
