using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewTelegramAccount
    {
        public int Id { get; set; }
        public string RowId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }

        public ViewTelegramAccount(TelegramAccount account, int index, string MaxZero)
        {
            this.Id = account.ID;
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
            this.Username = account.Username;
            this.FirstName = account.FirstName;
            this.LastName = account.LastName;
            this.Mobile = account.Mobile;
        }
    }
}
