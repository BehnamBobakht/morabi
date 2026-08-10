using DataLayer.Entities;
using DataLayer.ViewModels;
using ExpressMapper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_AccountAddress : BaseRepository<AccountAddress>
    {
        private DatabaseEntities _context;
        public Entity_AccountAddress(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public List<AccountAddress> GetAllByAccount(int accountId)
        {
            return _context.AccountAddress.Where(p =>
            p.AccountId == accountId
            ).OrderByDescending(p=>p.ID).ToList();
        }    
        public List<AccountAddressVm> GetAllByAccountVm(int accountId)
        {
            var model= _context.AccountAddress.Where(p =>
            p.AccountId == accountId
            ).OrderByDescending(p=>p.ID).Map<IQueryable<AccountAddress>, List<AccountAddressVm>>();
            return model;
        }
        public ResultVm<AccountAddressVm> Insert(AccountAddressVm accountAddressVm)
        {
            var result = new ResultVm<AccountAddressVm>();
            if(accountAddressVm.CityId==null)
            {

            }
            return result;
        }
    }
}
