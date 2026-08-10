using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class AccountReport
    {
        public AccountReport()
        {
            Items = new List<AccountReportItem>();
        }
        public string Subject { get; set; }
        public string Date { get; set; }
        public int TotalCount { get; set; }
        public List<AccountReportItem> Items { get; set; }

    }
    public class AccountReportItem
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string BirthDay { get; set; }
        public string RegisterDate { get; set; }

    }
   
}
