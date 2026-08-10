using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class SliderVm
    {
        public int ID { get; set; }
        public Nullable<int> PictureId { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public Nullable<int> ShowNumber { get; set; }
        public string SecondName { get; set; }

        public virtual PictureVm Picture { get; set; }


    }
}
