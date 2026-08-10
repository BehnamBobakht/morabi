using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewCity
    {
        public int Id { get; set; }
        public ViewState State { get; set; }
        public string Name { get; set; }

        public ViewCity() { }

        public ViewCity(City city)
        {
            if (city != null)
            {
                this.Id = city.ID;
                this.Name = city.Name;
                this.State = new ViewState(city.State);
            }
        }
    }
}
