using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewState
    {
        public int Id { get; set; }
        public ViewCountry Country { get; set; }
        public string Name { get; set; }

        public ViewState() { }
        public ViewState(State state)
        {
            if (state != null)
            {
                this.Id = state.ID;
                this.Name = state.Name;
                this.Country = new ViewCountry(state.Country);
            }
        }
    }
}
