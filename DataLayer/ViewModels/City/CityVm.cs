using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class CityVm
    {
        public int ID { get; set; }
        public Nullable<int> StateId { get; set; }
        public string StateName { get; set; }
        public string Name { get; set; }

    }
}
