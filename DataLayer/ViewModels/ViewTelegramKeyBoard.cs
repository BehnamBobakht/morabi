using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewTelegramKeyBoard
    {
        public int Id { get; set; }
        public string RowId { get; set; }
        public string Name { get; set; }
        public ViewCode Type { get; set; }

        public ViewTelegramKeyBoard() { }

        public ViewTelegramKeyBoard(TelegramKeyBoard keyboard)
        {
            this.Id = keyboard.ID;
            this.Name = keyboard.Name;
            this.Type = new ViewCode(keyboard.Code);
        }

        public ViewTelegramKeyBoard(TelegramKeyBoard keyboard, int index, string MaxZero)
        {
            this.Id = keyboard.ID;
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
            this.Name = keyboard.Name;
            this.Type = new ViewCode(keyboard.Code);
        }
    }
}
