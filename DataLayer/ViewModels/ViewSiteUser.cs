using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewSiteUser
    {
        public int Id { get; set; }
        public string RowId { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<string> UserGroups{ get; set; }

        public bool Active { get; set; }

        public ViewSiteUser() {
            UserGroups = new List<string>();
        }

        public ViewSiteUser(SiteUser user, int index, string MaxZero)
        {
            this.Active = user.Active;
            this.Email = user.Email;
            this.Mobile = user.Mobile;
            this.Id = user.ID;
            this.Name = user.FullName;
            this.UserGroups = user.SiteUserUserGroup.Select(s => s.UserGroup.Name).ToList();
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
        }
    }
}
