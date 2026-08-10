using DataLayer.Api;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Api.Model
{
    public class ApiOrderTracking : ApiResponse
    {
        public static ApiResult Post(UnitOfWork _context, string code)
        {
            if (string.IsNullOrEmpty(code))
                return CreateErrorResult(Enum_Message.REQUIRED_TRACKING_CODE);
            AccountOrder order = _context.AccountOrder.Search(refId: code).FirstOrDefault();
            return (order == null) ? CreateErrorResult(Enum_Message.INVALID_ORDER_TRACKING_CODE) : CreateSuccessResult(order.ToApi());
        }
    }
}
