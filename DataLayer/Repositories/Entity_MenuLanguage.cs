using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_MenuLanguage : BaseRepository<MenuLanguage>
    {
        private DatabaseEntities _context;

        public Entity_MenuLanguage(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<MenuLanguage> GetAllByMenuId(int menuId)
        {
            return _context.MenuLanguage.Where(p => p.MenuId == menuId).ToList();
        }

        public void DeleteByMenuId(int menuId)
        {
            List<MenuLanguage> list = GetAllByMenuId(menuId);
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }
    }
}
