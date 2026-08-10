using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_File : BaseRepository<File>
    {
        private DatabaseEntities _context;
        public Entity_File(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
    }
}
