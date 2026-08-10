using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class BannerVm
    {
        public int ID { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public Nullable<int> PictureId { get; set; }
        public Nullable<int> ClickCount { get; set; }
        public string Summary { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public Nullable<int> ShowNumber { get; set; }

        public  PictureVm Picture { get; set; }


    }
}
