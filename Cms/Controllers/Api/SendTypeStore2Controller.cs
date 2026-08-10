using DataLayer.Api;
using DataLayer.Api.Model;
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
    public class SendTypeStore2Controller : ApiController
    {
        UnitOfWork _context = new UnitOfWork();

        [HttpGet]
        public ApiResult Get(bool ? isShipping=false, int ?countryId=118)
        {
            var currentAccount = BaseWebsite.CurrentAccount;
            List<ViewState> states =new  List<ViewState>();
            //if (isShipping.Value == false)
            //{
                states = _context.State.Where(s=>s.CountryId== countryId && s.ID!=0).ToApi();
            //}
            //else
            //{

            //     states = _context.State.Search(pageSize:200,isActiveShipping: true,storeId: currentAccount.StoreId,countryId:countryId).ToApi();
            //}
            
            return ApiResponse.CreateSuccessResult(states);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
