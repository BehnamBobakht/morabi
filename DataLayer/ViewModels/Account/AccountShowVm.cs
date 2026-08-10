using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class AccountShowVm
    {
        public int ID { get; set; }
        public string FullName { get; set; } 
        public string Email { get; set; }
        public string NationalCode { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }      
        public Nullable<bool> IsMale { get; set; }         
        public Nullable<System.DateTime> BirthDay { get; set; }      
        public string Fax { get; set; }
        public string Instagram { get; set; }
        public string Telegram { get; set; }
        public string WhatsApp { get; set; }
        public string ProfileUrl { get; set; }
      
      

          }
}
