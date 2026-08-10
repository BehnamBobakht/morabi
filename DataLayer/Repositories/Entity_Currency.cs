using DataLayer.Api;
using DataLayer.Api.Model;
using DataLayer.Entities;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace DataLayer.Repositories
{
   public  class Entity_Currency : BaseRepository<Currency>
    {
        private DatabaseEntities _context;
        public Entity_Currency(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public int GetChangedPrice(int Id, int price)
        {
            int ChangedPrice = 0;
            var currency = _context.Currency.FirstOrDefault(c => c.CurrencyId == Id);
            if (currency != null)
            {
                ChangedPrice = currency.UnitPrice.Value * price;
            }
            return ChangedPrice;
        }

    }
  
}

