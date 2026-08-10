using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_Dashboard : BaseRepository<Dashboard>
    {
        private DatabaseEntities _context;
        public Entity_Dashboard(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<Dashboard> GetAllBySiteUserId(int UserId)
        {
            return _context.Dashboard.Where(p =>
                p.UserGroupDashboard.Any(s =>
                    s.UserGroup.SiteUserUserGroup.Any(q =>
                        q.SiteUserId == UserId
                    )
                )
            ).OrderBy(p => p.ID).ToList();
        }
    }
}
