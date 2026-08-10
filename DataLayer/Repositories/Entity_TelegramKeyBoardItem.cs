using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;

namespace DataLayer.Repositories
{
    public class Entity_TelegramKeyBoardItem : BaseRepository<TelegramKeyBoardItem>
    {
        private DatabaseEntities _context;
        public Entity_TelegramKeyBoardItem(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<TelegramKeyBoardItem> GetAllByKeyBoardId(int keyboardId)
        {
            return _context.TelegramKeyBoardItem.Where(p => 
                p.KeyboardId == keyboardId)
                .ToList();
        }
    }
}
