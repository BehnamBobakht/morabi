using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewMenu
    {
        public int Id { get; set; }
        public string RowId { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public ViewCode Type { get; set; }
        public ViewMenu Parent { get; set; }
        public ViewPicture Picture { get; set; }
        public List<ViewMenu> Childs { get; set; }

        public ViewMenu() { }

        public ViewMenu(Menu menu)
        {
            this.Id = menu.ID;
            this.Name = menu.Name;
            this.Type = new ViewCode(menu.Code);
            if (menu.ParentId != null)
                this.Parent = new ViewMenu(menu.Menu2);
        }

        public ViewMenu(Menu menu, int index, string MaxZero)
        {
            this.Id = menu.ID;
            this.Name = menu.Name;
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
            this.Type = new ViewCode(menu.Code);
            if (menu.ParentId != null)
                this.Parent = new ViewMenu(menu.Menu2);
        }
    }
}
