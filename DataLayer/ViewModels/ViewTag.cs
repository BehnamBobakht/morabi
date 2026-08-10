using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewTag
    {
        
        public List<Tag> AllTags { get; set; }
        public List<Tag> SourceTags { get; set; }

        public ViewTag() {
            AllTags = new List<Tag>();
            SourceTags = new List<Tag>();
        }

        
    }
}
