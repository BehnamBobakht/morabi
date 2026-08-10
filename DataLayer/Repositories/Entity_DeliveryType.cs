using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.ViewModels;
using ExpressMapper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DataLayer.Repositories
{
    public class Entity_DeliveryType : BaseRepository<DeliveryType>
    {
        DatabaseEntities _context;
        public Entity_DeliveryType(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<DeliveryTypeVm> GetActives()
        {
            return GetAll().Map<List<DeliveryType>, List<DeliveryTypeVm>>();
        }
      
    }
}
