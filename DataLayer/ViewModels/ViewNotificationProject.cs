using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewNotificationProject
    {
        public int Id { get; set; }
        public string RowId { get; set; }
        public ViewCode Type { get; set; }
        public string Name { get; set; }
        public string ProjectName { get; set; }
        public string AuthorizationValue { get; set; }

        public ViewNotificationProject(NotificationProject project, int index, string MaxZero)
        {
            this.Id = project.ID;
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
            this.Name = project.Name;
            this.ProjectName = project.ProjectName;
            this.Type = new ViewCode(project.Code);
            this.AuthorizationValue = project.AuthorizationValue;
        }
    }
}
