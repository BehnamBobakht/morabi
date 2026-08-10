using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.ViewModels;
using DataLayer.Entities;
using DataLayer.Enumarables;
using ExpressMapper.Extensions;

namespace DataLayer.Base
{
    public static class BaseMerchant
    {
        public static List<MerchantVm> GetActives(UnitOfWork _context=null)
        {
            if (_context == null)
                _context = new UnitOfWork();
            var list= _context.Merchant.GetActives().Map<List<MerchantVm>, List<MerchantVm>>();
            _context.Dispose();
            return list;
        }

    }
}
