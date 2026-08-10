using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;
using DataLayer.Enumarables;

namespace DataLayer.Repositories
{
    public class Entity_SetupLevel : BaseRepository<SetupLevel>
    {
        DatabaseEntities _context;
        public Entity_SetupLevel(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public void SetCurrnetLevel(Enum_SetupLevel newLevel)
        {
            SetupLevel activeLevel = _context.SetupLevel.FirstOrDefault(p => p.IsCurrent == "active");
            if (activeLevel != null)
            {
                activeLevel.IsCurrent = "";
            }

            string action = newLevel.ToString();
            SetupLevel level = _context.SetupLevel.FirstOrDefault(p => p.ActionName == action);
            if (level != null)
            {
                level.IsCurrent = "active";
            }
            Save();
        }
    }
}
