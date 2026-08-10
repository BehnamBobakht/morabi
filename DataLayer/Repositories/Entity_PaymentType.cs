using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_PaymentType : BaseRepository<PaymentType>
    {
        private DatabaseEntities _context;
        public Entity_PaymentType(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public PaymentType GetByLabel(Enum_PaymentType type)
        {
            string typeString = type.ToString();
            return _context.PaymentType.FirstOrDefault(p => p.Label == typeString);
        }

        public List<PaymentType> GetAllActive()
        {
            return _context.PaymentType.Where(p => p.Active == true).ToList();
        }
    }
}
