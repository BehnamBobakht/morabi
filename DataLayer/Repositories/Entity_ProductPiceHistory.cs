using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ProductPiceHistory : BaseRepository<ProductPiceHistory>
    {
        private DatabaseEntities _context;
        public Entity_ProductPiceHistory(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public void Insert(int productId, double price)
        {
            if (price > 0)
            {
               var items = Where(s=>s.ProductId==productId).OrderByDescending(s=>s.ID);
                if (items.Any())
                {
                    if (items.First().Price != price)
                    {
                        var newItem = new ProductPiceHistory()
                        {
                            Date = DateTime.Now,
                            Price = price,
                            ProductId = productId
                        };
                        Insert(newItem);
                        Save();
                    }
                }
                else
                {
                    var newItem = new ProductPiceHistory()
                    {
                        Date = DateTime.Now,
                        Price = price,
                        ProductId = productId
                    };
                    Insert(newItem);
                    Save();
                }
            }
        }
    }
}
