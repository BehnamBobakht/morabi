using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ColorVm
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string HexValue { get; set; }
        public bool Exist { get; set; }
        public bool Selected { get; set; }
    }
}
