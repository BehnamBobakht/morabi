using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Api
{
    public class ApiStoreProductQuantity : ApiResponse
    {

        public static ApiResult Search(UnitOfWork _context, int productId, int? colorId = null, int? sizeId = null, bool available = true)
        {
            var result = _context.StoreProductQuantity.Search(productId, colorId, sizeId, available);
            return CreateSuccessResult(result.ToApi());
        }
    }
}
