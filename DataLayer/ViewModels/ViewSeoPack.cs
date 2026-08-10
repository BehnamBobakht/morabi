using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewSeoPack
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public ViewSeoOpenGraph OpenGraph { get; set; }
    }
}
