using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DataLayer.Repositories
{
    public class Entity_CartStore : BaseRepository<CartStore>
    {
        DatabaseEntities _context;
        public Entity_CartStore(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
      
    }
}
