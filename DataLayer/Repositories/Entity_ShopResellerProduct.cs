using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ShopResellerProduct : BaseRepository<ShopResellerProduct>
    {
        private DatabaseEntities _context;
        public Entity_ShopResellerProduct(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<ShopResellerProduct> GetAllByReseller(int resellerId)
        {
            return _context.ShopResellerProduct.Where(p => p.ResellerId == resellerId).ToList();
        }

        public List<ShopResellerProduct> GetAllNotApproved()
        {
            string not_checked_status = Enum_Code.STOREPRODUCT_REQUEST_NOT_CHECKED.ToString();
            return _context.ShopResellerProduct.Where(p => p.Code.Label == not_checked_status && p.Product.Active).ToList();
        }
    }
}
