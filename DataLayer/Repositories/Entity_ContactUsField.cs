using Binbin.Linq;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ContactUsField : BaseRepository<ContactUsField>
    {
        DatabaseEntities _context;



        public Entity_ContactUsField(DatabaseEntities context) : base(context)
        {
            _context = context;
        }




    }
}
