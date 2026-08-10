using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class TagPostVm
    {
        public int Id { get; set; }
        public Nullable<int> PostId { get; set; }
        public Nullable<int> TagId { get; set; }

    }
}
