using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DataLayer.Repositories
{
    public class Entity_Permission : BaseRepository<Permission>
    {
        DatabaseEntities _context;
        public Entity_Permission(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<ViewPermission> GetAllView()
        {
            List<Permission> list = _context.Permission.Where(p => p.Module.IsSelected == true).OrderBy(p => p.ParentId).ToList();
            List<ViewPermission> Output = new List<ViewPermission>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewPermission(list[i], i, MaxZero));
            }
            return Output;
        }

        public List<Permission> GetAllActiveForWebsite()
        {
            return _context.Permission.Where(p => p.Module.IsSelected == true).ToList();
        }

        public static void SetCurrentPermission(SiteUser CurrentUser)
        {
            List<string> list = new List<string>();
            HttpContext.Current.Session["PERMISSION"] = null;
            foreach (SiteUserUserGroup siteUserUserGroup in CurrentUser.SiteUserUserGroup)
            {
                foreach (UserGroupPermission usergroupPermission in siteUserUserGroup.UserGroup.UserGroupPermission.Where(p => p.Permission.Module.IsSelected == true))
                {
                    bool IsAny = list.Any(p => p == usergroupPermission.Permission.Label);
                    if (IsAny == false)
                    {
                        list.Add(usergroupPermission.Permission.Label);
                    }
                }
            }
            HttpContext.Current.Session["PERMISSION"] = list;
        }

        public List<string> GetCurrentPermission()
        {
            var CurrentPermissions = HttpContext.Current.Session["PERMISSION"];
            if (CurrentPermissions != null)
                return (List<string>)HttpContext.Current.Session["PERMISSION"];
            return new List<string>();
        }

        public void CheckPagePermission(SiteUser CurrentUser, Enum_Permission Permission)
        {
            if (CurrentUser != null)
            {
                if (HasPermission(Permission) == false)
                {
                    HttpContext.Current.Response.Redirect("~/error/403");
                }
            }
            else
                HttpContext.Current.Response.Redirect("~/profile/login");
        }

        public bool HasPermission(Enum_Permission Permission)
        {
            List<string> list = GetCurrentPermission();
            return list.Any(p => p == Permission.ToString());
        }
    }
}
