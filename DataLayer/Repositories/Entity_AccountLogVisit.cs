using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_AccountLogVisit : BaseRepository<AccountLogVisit>
    {
        private DatabaseEntities _context;
        public Entity_AccountLogVisit(DatabaseEntities context) : base(context)
        {
            _context = new DatabaseEntities();
        }
        public List<AccountLogVisit> GetLogVisitSearch(int? accountId, string ip, int? sourceId, int pageName)
        {
            List<AccountLogVisit> accSearch = null;
            List<AccountLogVisit> ipSearch = null;
            if (accountId != null)
            {
                accSearch = _context.AccountLogVisit.Where(x => x.AccountId == accountId && x.SourceId == sourceId && x.PageName == pageName).ToList();
            }
            if (accSearch == null)
            {
                ipSearch = _context.AccountLogVisit.Where(x => x.IP == ip && x.SourceId == sourceId && x.PageName == pageName).ToList();
                return ipSearch;
            }
            else
                return accSearch;

        }

        //public List<AccountLog> GetAllByAccountId(int AccountId)
        //{
        //    return _context.AccountLog.Where(p =>
        //        p.AccountId == AccountId
        //    ).ToList();
        //}

        //public void DeleteByAccountId(int AccountId)
        //{
        //    List<AccountLog> list = GetAllByAccountId(AccountId);
        //    foreach (AccountLog item in list)
        //    {
        //        Delete(item);
        //    }
        //}

        //public AccountLog NewLog(int accountId, bool isWeb)
        //{
        //    return new AccountLog() {
        //        AccountId = accountId,
        //        AccountKey = Guid.NewGuid(),
        //        Datetime = DateTime.Now,
        //        IsExpired = false,
        //        IsWeb = isWeb
        //    };
        //}

        //public AccountLog GetByKeyAndAccountUniqueId(Guid key, Guid uniqueId)
        //{
        //    return _context.AccountLog.FirstOrDefault(p =>
        //        p.AccountKey == key &&
        //        p.Account.UniqueId == uniqueId &&
        //        p.IsExpired == false
        //    );
        //}
    }
}
