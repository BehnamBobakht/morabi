using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_PaymentWebsite : BaseRepository<PaymentWebsite>
    {
        private DatabaseEntities _context;
        public Entity_PaymentWebsite(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public PaymentWebsite Authenticate(string tokent,string ip,string domain,bool withOutToken)
        {
            //if((withOutToken?true : tokent!= "1212") || ip!= "209.172.34.5"  || domain != "talfighehonar.com" || ip != "104.18.42.80" || domain != "talfighbeta.com")
            //{
            //    return null;
            //}

            var paymentWebsite = FirstOrDefault(s => s.Active && (withOutToken ? s.Domain == domain : s.Token == tokent&& s.Ip == ip ));

            return paymentWebsite;
        }

    }
}
