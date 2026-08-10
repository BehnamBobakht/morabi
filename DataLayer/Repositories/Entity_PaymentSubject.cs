using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_PaymentSubject : BaseRepository<PaymentSubject>
    {
        DatabaseEntities _context;
        public Entity_PaymentSubject(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public PaymentSubject GetByLabel(Enum_PaymentSubject subject)
        {
            string label = subject.ToString();
            return _context.PaymentSubject.FirstOrDefault(p => p.Label == label);
        }

        public PaymentSubject GetByLabel(string subject)
        {
            return _context.PaymentSubject.FirstOrDefault(p => p.Label == subject);
        }
    }
}
