using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Api.Model
{
    public class ApiProductNotify
    {
        public static ApiResult Post(UnitOfWork _context, int productId, int? colorId, int? sizeId = null , ViewAccount account = null)
        {
            ViewAccount CurrentUser = new ViewAccount();
            if (account == null)
            {
                CurrentUser = _context.Account.GetCurrentAccount();
                
            }
            else
            {
                CurrentUser = account;
            }
            if (CurrentUser == null)
                return ApiResponse.CreateInvalidKeyResult();
            bool isRepeated = _context.ProductNotify.IsAny(productId, CurrentUser.Id);
            if (isRepeated)
                return ApiResponse.CreateSuccessResult(Enum_Message.SUCCESSFULL_PRODUCT_NOTIFY);

            ProductNotify notify = new ProductNotify()
            {
                AccountId = CurrentUser.Id,
                ProductId = productId,
                ColorId = colorId,
                SizeId = sizeId,
                IsEmail = true,
                IsSms = true,
                Datetime = DateTime.Now
            };
            _context.ProductNotify.Insert(notify);
            _context.Save();

            return ApiResponse.CreateSuccessResult(Enum_Message.SUCCESSFULL_PRODUCT_NOTIFY);
        }
    }
}
