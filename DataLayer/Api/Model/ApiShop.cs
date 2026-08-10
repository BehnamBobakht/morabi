using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Api.Model
{
    public class ApiShop : ApiResponse
    {
        public static ApiResult Search(
            UnitOfWork _context,
            int? index = null,
            int? notId = null,
            int? pageSize = null,
            int? typeId = null,
            string name = null)
        {
            pageSize = pageSize == null ? 10 : pageSize;
            index = index == null ? 1 : index;
            List<Shop> list = _context.Shop.Search(
                typeId: typeId,
                index: index.Value,
                notId: notId,
                name: name,
                pageSize: pageSize.Value);
            return ApiResponse.CreateSuccessResult(list);
        }
    }
}
