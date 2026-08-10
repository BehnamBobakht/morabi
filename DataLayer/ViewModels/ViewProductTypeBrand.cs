using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewProductTypeBrand
    {
        public string ProductTypeName { get; set; }
        public int ProductTypeId { get; set; }
        public bool Selected { get; set; }

        public ViewProductTypeBrand() { }
    }
}
