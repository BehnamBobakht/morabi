using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewTelegramCommand
    {
        public int Id { get; set; }
        public string RowId { get; set; }
        public string Name { get; set; }
        public string Command { get; set; }
        public ViewCode Type { get; set; }

        public ViewTelegramCommand(TelegramCommand command)
        {
            if (command != null)
            {
                this.Id = command.ID;
                this.Name = command.Name;
                this.Command = command.Command;
                this.Type = new ViewCode(command.Code);
            }
        }

        public ViewTelegramCommand(TelegramCommand command, int index, string MaxZero)
        {
            this.Id = command.ID;
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
            this.Name = command.Name;
            this.Command = command.Command;
            this.Type = new ViewCode(command.Code);
        }
    }
}
