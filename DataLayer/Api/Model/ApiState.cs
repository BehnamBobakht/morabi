using DataLayer.Entities;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Api.Model
{
    public class ApiState : ApiResponse
    {
        public static ApiResult Get(UnitOfWork _context, int? countryId = null, int? notId = null, int? index = null, int? pageSize = null, string name = null)
        {
            List<StateVm> list = _context.State.Search(countryId: countryId, withAll: true, index: index, pageSize: pageSize, name: name);
            return CreateSuccessResult(list);
        }
    }
}
