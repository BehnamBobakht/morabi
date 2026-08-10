using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_WebsiteForm : BaseRepository<WebsiteForm>
    {
        private DatabaseEntities _context;
        public Entity_WebsiteForm(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public WebsiteForm GetByLabel(string label)
        {
            return _context.WebsiteForm.FirstOrDefault(p => p.Label == label);
        }
    }
}
