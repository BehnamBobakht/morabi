using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewError
    {
        public int Code { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string BackUrl { get; set; }
    }
}
