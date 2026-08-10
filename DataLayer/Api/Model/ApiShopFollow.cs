using DataLayer.Entities;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Api.Model
{
    public class ApiShopFollow
    {
        public static ApiResult Post(UnitOfWork _context, int shopId)
        {
            ViewAccount CurrentUser = _context.Account.GetCurrentAccount();
            if (CurrentUser != null)
            {
                bool isRepeated = _context.ShopFollow.IsAny(shopId, CurrentUser.Id);
                if (isRepeated == false)
                {
                    ShopFollow follow = new ShopFollow()
                    {
                        AccountId = CurrentUser.Id,
                        ShopId = shopId,
                        Datetime = DateTime.Now
                    };
                    _context.ShopFollow.Insert(follow);
                    _context.Save();
                }
                return ApiResponse.CreateSuccessResult();
            }
            else
            {
                return ApiResponse.CreateInvalidKeyResult();
            }
        }

        public static ApiResult Delete(UnitOfWork _context, int shopId)
        {
            ViewAccount CurrentUser = _context.Account.GetCurrentAccount();
            if (CurrentUser != null)
            {
                ShopFollow follow = _context.ShopFollow.GetByShopIdAndAccountId(shopId, CurrentUser.Id);
                if (follow != null)
                {
                    _context.ShopFollow.Delete(follow);
                    _context.Save();
                }
                return ApiResponse.CreateSuccessResult();
            }
            else
            {
                return ApiResponse.CreateInvalidKeyResult();
            }
        }
    }
}
