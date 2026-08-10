using DataLayer.Entities;
using DataLayer.ViewModels;


using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Api.Model
{
    public class ApiAccountCredit : ApiResponse
    {
        public static ApiResult Search(UnitOfWork _context, int? accountId = null, int? index = null, int? pageSize = null)
        {
            if (accountId == null && index == null && pageSize == null)
                return CreateSuccessResult(_context.AccountCredit.GetSum());
            else
                return CreateSuccessResult(_context.AccountCredit.Search(accountId: accountId, index: index, pageSize: pageSize));
        }
    }
}
