using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_WebsiteFormField : BaseRepository<WebsiteFormField>
    {
        private DatabaseEntities _context;
        public Entity_WebsiteFormField(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<WebsiteFormField> GetFormFieldByLabel(string label)
        {
            return _context.WebsiteFormField.Where( x=>x.WebsiteForm.Label == label).ToList();
        }



    }
}
