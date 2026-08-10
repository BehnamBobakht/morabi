using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewUserGroupPermissions
    {
        public int PermissionId { get; set; }
        public int? ParentId { get; set; }
        public string PermissionName { get; set; }
        public bool HasPermission { get; set; }

        public ViewUserGroupPermissions() {}
    }
}
