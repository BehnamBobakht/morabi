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
    public class Entity_Merchant : BaseRepository<Merchant>
    {
        private DatabaseEntities _context;
        public Entity_Merchant(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<Merchant> GetAllActive()
        {
            return _context.Merchant.Where(p => p.Active).ToList();
        }   
        public List<MerchantVm> GetActives()
        {
            return _context.Merchant.Where(p => p.Active).ToList().Map<List<Merchant>, List<MerchantVm>>();
        }
        public bool IsActive(int merchantId)
        {
           var item= GetById(merchantId);
            if(item.Active)
            {
                return true;
            }
            return false;
        }
    }
}
