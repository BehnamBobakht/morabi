using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_SmsSetting : BaseRepository<SmsSetting>
    {
        private DatabaseEntities _context;
        public Entity_SmsSetting(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public SmsSetting GetBySmsType(Enum_SmsType smsType)
        {
            string smsTypeString = smsType.ToString();
            return _context.SmsSetting.FirstOrDefault(p =>
                p.SmsType.Label == smsTypeString
            );
        }
    }
}
