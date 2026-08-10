using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Api.Model
{
    public class ApiSendType : ApiResponse
    {
        public static ApiResult Get(UnitOfWork _context)
        {
            List<SendType> list = _context.SendType.Search();
            return CreateSuccessResult(list);
        }


        public static ApiResult GetOneRcord(UnitOfWork _context)
        {
            SendType FirstReCord = _context.SendType.FirstOrDefault();
            return CreateSuccessResult(FirstReCord);
        }
    }
}
