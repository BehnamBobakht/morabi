using DataLayer.Api;
using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cms.Controllers.Api
{
    public class CityController : ApiController
    {
        // GET: City
        UnitOfWork _context = new UnitOfWork();

        [HttpGet]
        public ApiResult Get(int? id)
        {
            var currentAccount = BaseWebsite.CurrentAccount;
            List<ViewCity> cities = new List<ViewCity>();
            //if (isShipping.Value == false)
            //{
                cities= _context.City.Where(x => x.StateId == id &&  x.ID != 0).ToList().ToApi();
            //}
            //else
            //{
            //    cities= _context.City.Search(stateId:id,pageSize: 200, isActiveShipping: true, storeId: currentAccount.StoreId).ToApi();
            //}
            return ApiResponse.CreateSuccessResult(cities);
        }
    }
}