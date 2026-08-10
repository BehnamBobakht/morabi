using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewProductCustomValue
    {
        public int Id { get; set; }
        public ViewProductCustomField Field { get; set; }
        public ViewProductCustomItem Item { get; set; }
        public ViewProduct Product { get; set; }
        public string Value { get; set; }
    }
}
