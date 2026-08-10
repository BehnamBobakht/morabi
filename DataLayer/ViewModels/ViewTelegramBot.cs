using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewTelegramBot
    {
        public int Id { get; set; }
        public string RowId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public string Description { get; set; }

        public ViewTelegramBot(TelegramBot bot, int index, string MaxZero)
        {
            this.Id = bot.ID;
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
            this.Name = bot.Name;
            this.Username = bot.Username;
            this.Token = bot.Token;
            this.Description = bot.Description;
        }
    }
}
