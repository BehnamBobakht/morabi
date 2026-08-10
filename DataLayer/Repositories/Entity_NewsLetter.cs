using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;
using DataLayer.Enumarables;

namespace DataLayer.Repositories
{
    public class Entity_NewsLetter : BaseRepository<Newsletter>
    {
        private DatabaseEntities _context;
        public Entity_NewsLetter(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public bool IsRepeated(Newsletter newsletter)
        {
            return _context.Newsletter.Any(p =>
                (
                    (p.Email != null && p.Email != "" && p.Email == newsletter.Email) ||
                    (p.Mobile != null && p.Mobile != "" && p.Mobile == newsletter.Mobile))
                );
        }
        public List<Newsletter> Search(string Email = null, string Mobile = null, Enum_Lang lang = Enum_Lang.NONE)
        {
            return _context.Newsletter.Where(p =>
                (
                    (p.Email != null && p.Email != "" && p.Email == Email) ||
                    (p.Mobile != null && p.Mobile != "" && p.Mobile == Mobile))
                ).ToList();
        }
        public List<Newsletter> ReadyToSend(bool type) // t= email f=mobile
        {
            if(type)
            {
               return Where(x => x.Active && (!string.IsNullOrEmpty(x.Email))).ToList();
            }
            else
            {
                return Where(x => x.Active && (!string.IsNullOrEmpty(x.Mobile))).ToList();
            }
        }
    }
}
