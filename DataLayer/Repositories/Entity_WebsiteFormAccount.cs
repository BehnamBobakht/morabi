using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_WebsiteFormAccount : BaseRepository<WebsiteFormAccount>
    {
        private DatabaseEntities _context;
        public Entity_WebsiteFormAccount(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<WebsiteFormAccount> GetAllByFormId(int formId)
        {
            return _context.WebsiteFormAccount.Where(p =>
                p.FormId == formId
            ).ToList();
        }
    }
}
