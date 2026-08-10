using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewContactUs
    {
        public Nullable<int> AccountId { get; set; }

        public string Subject { get; set; }
        public string FullName { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public string Body { get; set; }

        public DateTime Datetime { get; set; }


    }
}
