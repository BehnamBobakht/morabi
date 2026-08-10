using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class IId
    {
        public int Id { get; set; }
        public IId() { }
        public IId(int Id)
        {
            this.Id = Id;
        }
    }
}
