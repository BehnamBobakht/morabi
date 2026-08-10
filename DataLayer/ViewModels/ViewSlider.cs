using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewSlider
    {
        public int Id { get; set; }
        public string RowId { get; set; }
        public string Name { get; set; }

        public string SecondName { get; set; }
        public ViewWebsite Website { get; set; }
        public ViewPicture Picture { get; set; }

        public ViewSlider() { }
        public ViewSlider(Slider slider, int index, string MaxZero)
        {
            this.Id = slider.ID;
            this.Name = slider.Name;
            this.SecondName = slider.SecondName;
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
            this.Website = new ViewWebsite(slider.Website);
            this.Picture = new ViewPicture(slider.Picture);
        }
    }
}
