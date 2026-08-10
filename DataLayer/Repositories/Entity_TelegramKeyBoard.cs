using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;

namespace DataLayer.Repositories
{
    public class Entity_TelegramKeyBoard : BaseRepository<TelegramKeyBoard>
    {
        private DatabaseEntities _context;
        public Entity_TelegramKeyBoard(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<TelegramKeyBoard> GetAllByBotId(int botId)
        {
            return _context.TelegramKeyBoard.Where(p =>
                p.BotId == botId
            ).ToList();
        }
    }
}
