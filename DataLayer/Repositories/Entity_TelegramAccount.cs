using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_TelegramAccount : BaseRepository<TelegramAccount>
    {
        private DatabaseEntities _context;
        public Entity_TelegramAccount(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public TelegramAccount GetByBotAndTelegramId(int botId, int telegramId)
        {
            string strTelegramId = telegramId.ToString();
            return _context.TelegramAccount.FirstOrDefault(p =>
                p.ChatId == strTelegramId &&
                p.BotId == botId
            );
        }
    }
}
