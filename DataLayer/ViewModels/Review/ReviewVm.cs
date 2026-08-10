using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ReviewVm
    {
        public int ID { get; set; }
        public Nullable<int> ProductId { get; set; }
        public string Contect { get; set; }
        public Nullable<int> PictureId { get; set; }
        public string PictureUrl { get; set; }

        public  PictureVm Picture { get; set; }

    }
}
