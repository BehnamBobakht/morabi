using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DataLayer.Repositories
{
    public class Entity_Language : BaseRepository<Language>
    {
        DatabaseEntities _context;
        public Entity_Language(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public Language GetByLabel(string lbl)
        {
            return FirstOrDefault(s => s.Label == lbl);
        }
    }
}
