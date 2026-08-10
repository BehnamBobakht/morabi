using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewTelegramKeyBoardItem
    {
        public int Id { get; set; }
        public string RowId { get; set; }
        public ViewTelegramCommand Command { get; set; }
        public ViewTelegramKeyBoard KeyBoard { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int Row { get; set; }
        public int ShowNumber { get; set; }

        public ViewTelegramKeyBoardItem(TelegramKeyBoardItem keyboardItem, int index, string MaxZero)
        {
            this.Id = keyboardItem.ID;
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
            this.Name = keyboardItem.Name;
            this.Url = keyboardItem.Url;
            this.Row = keyboardItem.Row;
            this.ShowNumber = keyboardItem.ShowNumber;
            this.Command = new ViewTelegramCommand(keyboardItem.TelegramCommand);
            this.KeyBoard = new ViewTelegramKeyBoard(keyboardItem.TelegramKeyBoard);
        }
    }
}
