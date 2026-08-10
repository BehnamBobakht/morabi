using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Api
{
    public class ApiPost : ApiResponse
    {
        public static ApiResult Search(
            UnitOfWork _context,
            int? notId = null,
            int? categoryId = null,
            string categoryLabel = null,
            int? typeId = null,
            int index = 1,
            int pageSize = 10, string keyword = null, string name = null, Enum_PostOrder order = Enum_PostOrder.NONE, bool? active = true)
        {

            var list = _context.Post.SearchVm(
                index:index,
                pageSize:pageSize,
                              notId: notId,
                              categoryId: categoryId,
                              categoryLabel: categoryLabel,
                              typeId: typeId,
                              keyword: keyword,
                              name: name, 
                              active: active

                );

            return ApiResponse.CreateSuccessResult(list);
        }  
        public static ApiResult Get(
            UnitOfWork _context,
            int id
)
        {

            var item = _context.Post.GetPostVm(
               id:id
                );

            return ApiResponse.CreateSuccessResult(item);
        }



    }
}
