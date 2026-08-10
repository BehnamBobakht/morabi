using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Api.Model
{
    public class ApiShopReseller : ApiResponse
    {
        public static ApiResult Get(UnitOfWork _context, ShopReseller reseller)
        {
            return CreateSuccessResult(reseller.ToApi());
        }

        public static ApiResult Put(UnitOfWork _context, ShopReseller reseller)
        {
            if (string.IsNullOrEmpty(reseller.Name))
                return CreateErrorResult(Enum_Message.REQUIRED_RESELLER_NAME);

            ShopReseller entity = _context.ShopReseller.GetById(reseller.ID);
            entity.Name = reseller.Name;
            entity.Website = reseller.Website;
            entity.PictureId = reseller.PictureId;
            _context.Save();

            ShopReseller tempEntity = _context.ShopReseller.GetById(entity.ID);
            return CreateSuccessResult(tempEntity.ToApi());
        }

        public static ApiResult Search(
            UnitOfWork _context,
            int? index = null,
            int? notId = null,
            int? pageSize = null,
            string name = null)
        {
            pageSize = pageSize == null ? 10 : pageSize;
            index = index == null ? 1 : index;
            List<ShopReseller> list = _context.ShopReseller.Search(
                index: index.Value,
                notId: notId,
                name: name,
                pageSize: pageSize.Value);
            return ApiResponse.CreateSuccessResult(list);
        }
    }
}
