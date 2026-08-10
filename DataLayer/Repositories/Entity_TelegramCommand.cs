using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_TelegramCommand : BaseRepository<TelegramCommand>
    {
        private DatabaseEntities _context;
        public Entity_TelegramCommand(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<TelegramCommand> GetAllByBotId(int botId)
        {
            return _context.TelegramCommand.Where(p =>
                p.BotId == botId
            ).ToList();
        }
    }
}
