using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_SmsType : BaseRepository<SmsType>
    {
        private DatabaseEntities _context;

        public SmsType GetByLabel(Enum_SmsType smsType)
        {
            string typeStr = smsType.ToString();
            return _context.SmsType.FirstOrDefault(p => p.Label == typeStr);
        }

        public Entity_SmsType(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
    }
}
