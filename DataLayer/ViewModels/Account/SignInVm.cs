using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataLayer.ViewModels
{
    public class SignInVm:StatusVm
    {
        public string Mobile { get; set; }
        public string Password { get; set; }
        public string BackUrl { get; set; }
        public bool Remember { get; set; }
    }
}
