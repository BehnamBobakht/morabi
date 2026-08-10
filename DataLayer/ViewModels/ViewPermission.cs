using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewPermission
    {
        public int Id { get; set; }
        public string RowId { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public string Parent { get; set; }
        public string Module { get; set; }

        public ViewPermission(Permission permission, int index, string MaxZero)
        {
            this.Id = permission.ID;
            this.Name = permission.Name;
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
            this.Label = permission.Label;
            this.Parent = permission.ParentId == null ? "" : permission.Permission2.Name;
            this.Module = permission.Module.Name;
        }
    }
}
