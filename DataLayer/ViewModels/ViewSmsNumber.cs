using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewSmsNumber
    {
        public int Id { get; set; }
        public string RowId { get; set; }
        public ViewSmsProvider Provider { get; set; }
        public string Number { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ApiKey { get; set; }

        public ViewSmsNumber() { }

        public ViewSmsNumber(SmsNumber number)
        {
            this.Id = number.ID;
            this.ApiKey = number.ApiKey;
            this.Number = number.Number;
            this.Provider = new ViewSmsProvider(number.SmsProvider);
            this.Username = number.Username;
        }
    }
}
