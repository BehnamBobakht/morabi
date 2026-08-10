using Binbin.Linq;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_BookPayment : BaseRepository<BookPayment>
    {
        private DatabaseEntities _context;
        public Entity_BookPayment(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

    }
}
