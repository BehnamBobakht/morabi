using Binbin.Linq;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ProductBarcode : BaseRepository<ProductBarcode>
    {
        private DatabaseEntities _context;

        public Entity_ProductBarcode(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public ProductBarcode GetByBarcodeValue(string barcodeValue)
        {
            return _context.ProductBarcode.FirstOrDefault(p => p.Value == barcodeValue);
        }
    }
}
