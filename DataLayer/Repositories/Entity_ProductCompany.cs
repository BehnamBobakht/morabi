using Binbin.Linq;
using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ProductCompany : BaseRepository<ProductCompany>
    {
        private DatabaseEntities _context;
        public Entity_ProductCompany(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        

    }
}

