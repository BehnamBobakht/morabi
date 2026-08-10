using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewShopResellerStory
    {
        public int ResellerId { get; set; }
        public ViewPicture Picture { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public List<ViewShopResellerStoryItem> Items { get; set; }
    }
}
