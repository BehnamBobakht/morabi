using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewSimpleApiValue
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime? Datetime { get; set; }

        public ViewSimpleApiValue() { }

        public ViewSimpleApiValue(string Id, string Name, DateTime? Datetime) {
            this.Id = Id;
            this.Name = Name;
            this.Datetime = Datetime;
        }
    }
}
