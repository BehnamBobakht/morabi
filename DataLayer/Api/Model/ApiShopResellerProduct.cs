using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Api.Model
{
    public class ApiShopResellerProduct : ApiResponse
    {
        public static ApiResult Get(UnitOfWork _context, int resellerId)
        {
            List<ShopResellerProduct> listReseller = _context.ShopResellerProduct.GetAllByReseller(resellerId);
            return CreateSuccessResult(listReseller.ToApi());
        }

        public static ApiResult Post(UnitOfWork _context, List<ShopResellerProduct> list, int resellerId)
        {
            Code status_not_checked = _context.Code.GetByLabel(Enum_Code.STOREPRODUCT_REQUEST_NOT_CHECKED);
            List<ShopResellerProduct> listReseller = _context.ShopResellerProduct.GetAllByReseller(resellerId);
            foreach (ShopResellerProduct item in list)
            {
                ShopResellerProduct entity = listReseller.FirstOrDefault(p => 
                    p.ProductId == item.ProductId);
                if (entity == null)
                {
                    entity = new ShopResellerProduct()
                    {
                        ResellerId = resellerId,
                        ProductId = item.ProductId,
                        StatusId = status_not_checked.ID
                    };
                    _context.ShopResellerProduct.Insert(entity);
                    _context.Save();

                    Product product = _context.Product.GetById(item.ProductId);
                    _context.ShopResellerStory.InsertNewStory(Enum_StoryType.PRODUCT, resellerId, item.ProductId, product.PictureId, "محصول جدید - " + product.Name);
                }
            }
            _context = new UnitOfWork();
            
            List<ShopResellerProduct> listProducts = _context.ShopResellerProduct.GetAllByReseller(resellerId);
            return CreateSuccessResult(listProducts.ToApi());
        }
    }
}
