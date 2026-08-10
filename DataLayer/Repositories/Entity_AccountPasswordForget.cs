using DataLayer.Base;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_AccountPasswordForget : BaseRepository<AccountPasswordForget>
    {
        private DatabaseEntities _context;
        public Entity_AccountPasswordForget(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public AccountPasswordForget GetByEmailKeyAndAccountKey(string emailKey, Guid accountKey)
        {
            return _context.AccountPasswordForget.FirstOrDefault(p => 
                p.EmailKey == emailKey &&
                p.Account.UniqueId == accountKey &&
                p.IsUsed == false
            );
        }
        public AccountPasswordForget GetByEmailOrMobile(string email, string mobile,string code)
        {
            return _context.AccountPasswordForget.FirstOrDefault(p =>
               ( p.Account.Email == email ||p.Account.Mobile == mobile) &&
                p.IsUsed == false && p.MobileKey==code);
        }
    }
}
