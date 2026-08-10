using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewEmailHost
    {
        public int Id { get; set; }
        public string RowId { get; set; }
        public string Name { get; set; }
        public string Pop3 { get; set; }
        public string Smtp { get; set; }
        public bool Active { get; set; }

        public ViewEmailHost(EmailHost emailHost, int index, string MaxZero)
        {
            this.Id = emailHost.ID;
            this.Name = emailHost.Name;
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
            this.Pop3 = emailHost.Pop3 + ":" + emailHost.Pop3Port;
            this.Smtp = emailHost.Smtp + ":" + emailHost.SmtpPort;
            this.Active = emailHost.Active;
        }
    }
}
