using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class SignUpVm:StatusVm
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string NationalCode { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public bool IsMale { get; set; }
        public string BackUrl { get; set; }
    }
}
