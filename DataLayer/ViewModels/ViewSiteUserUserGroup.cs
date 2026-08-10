using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewSiteUserUserGroup
    {
        public int Id { get; set; }
        public string RowId { get; set; }
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public bool IsSelected { get; set; }
        public string UserGroupName { get; set; }
        public string SiteUserName { get; set; }

        public ViewSiteUserUserGroup() { } 

        public ViewSiteUserUserGroup(SiteUserUserGroup user, int index, string MaxZero)
        {
            this.Id = user.ID;
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
            this.UserGroupName = user.UserGroup.Name;
            this.SiteUserName = user.SiteUser.FullName;
        }
    }
}
