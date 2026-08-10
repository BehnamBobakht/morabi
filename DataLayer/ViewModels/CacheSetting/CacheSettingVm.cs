using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class CacheSettingVm
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public int Duration { get; set; }
        public string Parameters { get; set; }
        public bool Active { get; set; }
    }
}
