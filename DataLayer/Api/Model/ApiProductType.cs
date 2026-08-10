using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Api.Model
{
    public class ApiProductType : ApiResponse
    {
        public static ApiResult Post(UnitOfWork _context, ProductType productType)
        {
            if (_context.Permission.HasPermission(Enum_Permission.ProductType_New) == false)
                return CreateInvalidKeyResult();

            List<ProductTypeLanguage> listLanguage = productType.ProductTypeLanguage.ToList();
            productType.ProductTypeLanguage.Clear();

            if (productType.Name == null)
                return ApiResponse.CreateErrorResult(Enum_Message.REQUIRED_PRODUCT_TYPE_NAME);
            else
                _context.ProductType.Insert(productType);
                _context.Save();

                foreach (ProductTypeLanguage item in listLanguage)
                {
                    item.ProductTypeId = productType.ID;
                    _context.ProductTypeLanguage.Insert(item);
                    _context.Save();
                }

            return ApiResponse.CreateSuccessResult();
        }
    }
}
