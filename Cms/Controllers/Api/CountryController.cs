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
    public class CountryController : ApiController
    {
        UnitOfWork _context = new UnitOfWork();

        [HttpGet]
        public ApiResult Get(bool ? isShipping=false)
        {
            var currentAccount = BaseWebsite.CurrentAccount;
            List<ViewCountry> states =new  List<ViewCountry>();
            //if (isShipping.Value == false)
            //{
                states = _context.Country.Where(s=>s.ID!=0).ToApi();
            //}
            //else
            //{

            //     states = _context.Country.Search(pageSize:200,isActiveShipping: true,storeId: currentAccount.StoreId).ToApi();
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
