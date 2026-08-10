using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class CategoryVm
    {
        public int ID { get; set; }
        public int WebsiteId { get; set; }
        public int TypeId { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public Nullable<int> PictureId { get; set; }
        public string URL { get; set; }
        public string Body { get; set; }
        public PictureVm Picture { get; set; }
        public  CodeVm Code { get; set; }

    }
}
