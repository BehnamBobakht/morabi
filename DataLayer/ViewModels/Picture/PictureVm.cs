using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class PictureVm
    {
        public int ID { get; set; }
        public string Url { get; set; }
        public string PathUrl { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public string ContentType { get; set; }
        public string DirectUrl { get; set; }


    }
}
