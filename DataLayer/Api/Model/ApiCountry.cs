using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Api.Model
{
    public class ApiCountry : ApiResponse
    {
        public static ApiResult Get(UnitOfWork _context, int? notId = null, int? index = null, int? pageSize = null, string name = null)
        {
            List<Country> list = _context.Country.Search(notId: notId, index: index, pageSize: pageSize, name: name);
            return CreateSuccessResult(list);
        }
    }
}
