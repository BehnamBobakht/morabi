using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewUserGroup
    {
        public int Id { get; set; }
        public string RowId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }

        public ViewUserGroup(UserGroup group, int index, string MaxZero)
        {
            this.Active = group.Active;
            this.Description = group.Description;
            this.Id = group.ID;
            this.Name = group.Name;
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
        }
    }
}
