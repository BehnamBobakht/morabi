using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_UserGroup : BaseRepository<UserGroup>
    {
        DatabaseEntities _context;
        public Entity_UserGroup(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public UserGroup GetFirstByRole(Enum_UserRole role)
        {
            string roleString = role.ToString();
            return _context.UserGroup.FirstOrDefault(p => p.UserRole.Label == roleString);
        }
    }
}
