using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ProductPicture : BaseRepository<ProductPicture>
    {
        private DatabaseEntities _context;
        public Entity_ProductPicture(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<ProductPicture> GetAllByProductId(int productId)
        {
            return _context.ProductPicture.Where(p =>
                p.ProductId == productId
            ).ToList();
        }

        public void DeleteByProductId(int productId, bool hasColor = false)
        {
            List<ProductPicture> list = GetAllByProductId(productId);
            if (hasColor == true)
                list = list.Where(p => p.ColorId == null).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }

        public List<ProductPicture> GetAllHasSyncId()
        {
            return _context.ProductPicture.Where(p =>
                p.SyncId != null
            ).ToList();
        }

    }
}
