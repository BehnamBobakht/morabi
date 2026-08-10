using DataLayer.Base;
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
    public class ApiAccountReagent : ApiResponse
    {
        public static ApiResult Post_RegisterAccountReagent(UnitOfWork _context, ViewAccount account, string fullName, string mobile, string email)
        {
            mobile = mobile != null ? mobile.Trim() : mobile;

            if (string.IsNullOrEmpty(mobile))
                return CreateErrorResult(Enum_Message.REQUIRED_REAGENT_MOBILE_VALUES);

            mobile = mobile.GetEnglish();
            email = email.GetEnglish();
            
            AccountReagent reagent = new AccountReagent() {
                AccountId = account.Id,
                Email = email,
                Mobile = mobile,
                FullName = fullName
            };
            _context.AccountReagent.Insert(reagent);
            _context.Save();
            return CreateSuccessResult();
        }
    }
}
