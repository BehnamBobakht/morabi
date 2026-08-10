using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_UserGroupDashboard : BaseRepository<UserGroupDashboard>
    {
        private DatabaseEntities _context;
        public Entity_UserGroupDashboard(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<UserGroupDashboard> GetAllByUserGroupId(int userGroupId)
        {
            return _context.UserGroupDashboard.Where(p =>
                p.UserGroupId == userGroupId
            ).ToList();
        }
    }
}
