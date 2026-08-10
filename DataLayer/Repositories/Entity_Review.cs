using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DataLayer.Repositories
{
    public class Entity_Review : BaseRepository<Review>
    {
        DatabaseEntities _context;
        public Entity_Review(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public List<Review> GetAllByProductId(int productId)
        {
            return _context.Review.Where(p =>
                p.ProductId == productId
            ).ToList();
        }
        public void DeleteByProductId(int productId)
        {
            List<Review> list = GetAllByProductId(productId);
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }
    }
}
