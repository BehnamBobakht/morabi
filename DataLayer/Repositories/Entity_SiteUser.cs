using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DataLayer.Repositories
{
    public class Entity_SiteUser : BaseRepository<SiteUser>
    {
        DatabaseEntities _context;
        public Entity_SiteUser(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public void Logout()
        {
            BaseWebsite.CurrentSiteUser = null;
            HttpContext.Current.Session.Remove("SITEUSER");
            HttpContext.Current.Session.Remove("PERMISSION");
            HttpContext.Current.Response.Cookies["USER"].Expires = DateTime.Now.AddDays(-1);
        }

        public void SetCurrentUser(SiteUser user)
        {
            HttpContext.Current.Session["SITEUSER"] = user;
            Entity_Permission.SetCurrentPermission(user);
        }

        public void SetCurrentUser(SiteUser user, bool remember)
        {
            HttpContext.Current.Session["SITEUSER"] = user;
            Entity_Permission.SetCurrentPermission(user);
            HttpCookie loginCookie = new HttpCookie("USER");
            if (remember)
            {
                loginCookie.Values.Add("EMAIL", BaseSecurity.EncryptText(user.Email));
                loginCookie.Values.Add("PASSWORD", BaseSecurity.EncryptText(user.Password));
                loginCookie.Expires = DateTime.Now.AddMonths(1);
                HttpContext.Current.Response.Cookies.Add(loginCookie);
            }
            else
            {
                loginCookie.Expires = DateTime.Now.AddMonths(-1);
            }
        }
        
        public SiteUser GetCurrentUser()
        {
            var CurrentUser = HttpContext.Current.Session["SITEUSER"];
            if (CurrentUser != null)
                return (SiteUser)HttpContext.Current.Session["SITEUSER"];

            HttpCookie loginCookie = HttpContext.Current.Request.Cookies["USER"];
            if (loginCookie != null)
            {
                if (!string.IsNullOrEmpty(loginCookie.Values["EMAIL"]) && !string.IsNullOrEmpty(loginCookie.Values["PASSWORD"]))
                {
                    string Email = BaseSecurity.DecryptText(loginCookie.Values["EMAIL"]);
                    string Password = BaseSecurity.DecryptText(loginCookie.Values["PASSWORD"]);
                    SiteUser RememberUser = GetFromEmailAndPassword(Email, Password);
                    if (RememberUser != null)
                    {
                        SetCurrentUser(RememberUser, true);
                        return RememberUser;
                    }
                }
            }

            return null;
        }

        public SiteUser GetFromEmailAndPassword(string email, string password)
        {
            return _context.SiteUser.FirstOrDefault(p => 
                p.Email == email &&
                p.Password == password
            );
        }

        public bool IsUniqueEmail(SiteUser user)
        {
            return _context.SiteUser.Any(p => (p.Email == user.Email && user.ID != p.ID));
        }

        public bool IsUniqueMobile(SiteUser user)
        {
            return _context.SiteUser.Any(p =>
                (p.Mobile == user.Mobile && 
                user.ID != p.ID && 
                user.Mobile != null && 
                user.Mobile != "")
                );
        }

        public SiteUser GetByEmail(string email, bool IsActive)
        {
            return _context.SiteUser.FirstOrDefault(p => p.Email == email && p.Active == IsActive);
        }

        public SiteUser GetByMobile(string mobile, bool IsActive)
        {
            return _context.SiteUser.FirstOrDefault(p => p.Mobile == mobile && p.Active == IsActive);
        }

        public SiteUser GetByUniqueCode(Guid unique)
        {
            return _context.SiteUser.FirstOrDefault(p => p.UniqueIdentifier == unique);
        }

        public List<SiteUser> GetAllByUserRole(Enum_UserRole role)
        {
            string roleString = role.ToString();
            return _context.SiteUser.Where(p => 
                p.SiteUserUserGroup.Any(s => 
                    s.UserGroup.UserRole.Label == roleString
                )
            ).ToList();
        }
    }
}
