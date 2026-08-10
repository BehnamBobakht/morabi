using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewSeoOpenGraph
    {
        public string SiteName { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string Locale { get; set; }
        public string Type { get; set; }
        public string Card { get; set; }
        public string Image { get; set; }
        public string Image_Width { get; set; }
        public string Image_Height { get; set; }
        public string Image_Type { get; set; }
        public string Amount { get; set; }
        public string Currency { get; set; }
    }
}
