using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewProductBarcode
    {
        public ViewProduct Product { get; set; }
        public ViewColor Color { get; set; }
        public ViewSize Size { get; set; }
        public string Value { get; set; }

        public ViewProductBarcode() { }
    }
}
