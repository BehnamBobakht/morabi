using Binbin.Linq;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_AccountCredit : BaseRepository<AccountCredit>
    {
        private DatabaseEntities _context;
        public Entity_AccountCredit(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public void CreateCredit(int accountId, double amount, string description = null, int? paymentId = null)
        {
            double remain = _context.AccountCredit.Where(p => p.AccountId == accountId).Sum(s => s.Amount);
            AccountCredit credit = new AccountCredit {
                AccountId = accountId,
                Amount = amount,
                Datetime = DateTime.Now,
                Description = description,
                PaymentId = paymentId,
                Remain = remain + amount
            };
            Insert(credit);
        }

        public double? GetSum()
        {
            if (_context.AccountCredit.Count() > 0)
                return _context.AccountCredit.Sum(p => p.Amount);
            else
                return 0;
        }

        public List<AccountCredit> Search(int? accountId = null, int? index = null, int? pageSize = null)
        {
            var MyQuery = PredicateBuilder.True<AccountCredit>();
            index = index == null ? 1 : index;
            pageSize = pageSize == null ? 10 : pageSize;

            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;
            
            if (accountId != null && accountId != 0)
                MyQuery = MyQuery.And(p => p.AccountId == accountId);

            return _context
                .AccountCredit
                .OrderBy(p => p.ID)
                .Where(MyQuery)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList();
        }
    }
}
