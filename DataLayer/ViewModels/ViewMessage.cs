using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewMessage
    {
        public Enum_MessageType Type { get; set; }
        public string Body { get; set; }

        public ViewMessage() {
            this.Type = Enum_MessageType.SUCCESS;
        }
    }
}
