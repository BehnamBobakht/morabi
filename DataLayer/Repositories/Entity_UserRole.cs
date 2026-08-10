using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;
using DataLayer.Enumarables;

namespace DataLayer.Repositories
{
    public class Entity_UserRole : BaseRepository<UserRole>
    {
        private DatabaseEntities _context;
        public Entity_UserRole(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public bool HasUserRole(List<SiteUserUserGroup> list, SiteUser user, Enum_UserRole role)
        {
            string roleString = role.ToString();
            return list.Any(p =>
                p.UserGroup.UserRole.Label == roleString
            );
        }
    }
}
