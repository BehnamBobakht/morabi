using DataLayer.Entities;
using DataLayer.ViewModels;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_SiteUserUserGroup : BaseRepository<SiteUserUserGroup>
    {
        DatabaseEntities _context;
        public Entity_SiteUserUserGroup(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<SiteUserUserGroup> GetAllByUserGroupId(int UserGroupId)
        {
            return _context.SiteUserUserGroup.Where(p => p.UserGroupId == UserGroupId).ToList();
        }

        public List<SiteUserUserGroup> GetAllBySiteUserId(int SiteUserId)
        {
            return _context.SiteUserUserGroup.Where(p => p.SiteUserId == SiteUserId).ToList();
        }

        public void DeleteBySiteUserId(int SiteUserId)
        {
            List<SiteUserUserGroup> list = GetAllBySiteUserId(SiteUserId);
            foreach (SiteUserUserGroup item in list)
            {
                Delete(item);
            }
        }

        public List<ViewSiteUserUserGroup> GetAllView(int UserGroupId)
        {
            List<SiteUserUserGroup> list = GetAllByUserGroupId(UserGroupId);
            List<ViewSiteUserUserGroup> Output = new List<ViewSiteUserUserGroup>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewSiteUserUserGroup(list[i], i, MaxZero));
            }
            return Output;
        }
    }
}
