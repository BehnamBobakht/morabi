using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ProductCategoryCompany : BaseRepository<ProductCategoryCompany>
    {
        private DatabaseEntities _context;
        public Entity_ProductCategoryCompany(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

     
        public ProductCategoryCompany GetByLabel(string label)
        {
            _context.Product.Include("ProductCategoryCompany").Where(x => x.ID == 12).ToList();
            return _context.ProductCategoryCompany.FirstOrDefault(p => p.Label == label);
        }



   
    }
}
