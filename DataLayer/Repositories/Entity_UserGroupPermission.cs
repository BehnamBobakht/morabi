using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_UserGroupPermission : BaseRepository<UserGroupPermission>
    {
        DatabaseEntities _context;
        public Entity_UserGroupPermission(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<UserGroupPermission> GetAllByUserGroupId(int UserGroupId)
        {
            return _context.UserGroupPermission.Where(p => 
                p.UserGroupId == UserGroupId
            ).ToList();
        }

        public void UpdateByUserGroupId(int UserGroupId, int[] SelectedPermissions)
        {
            List<UserGroupPermission> list = _context.UserGroupPermission.Where(p => p.UserGroupId == UserGroupId).ToList();
            foreach (UserGroupPermission item in list)
            {
                Delete(item);
            }

            foreach (int item in SelectedPermissions)
            {
                UserGroupPermission entity = new UserGroupPermission() {
                    UserGroupId = UserGroupId,
                    PermissionId = item
                };
                Insert(entity);
            }
            Save();
        }
    }
}
