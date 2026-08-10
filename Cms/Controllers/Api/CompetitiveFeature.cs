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
    public class CompetitiveFeatureController : ApiController
    {
        UnitOfWork _context = new UnitOfWork();

        [HttpGet]
        public ApiResult Get()
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
        [HttpPost]
        public ApiResult Put(List<ViewStoreCompetitiveFeature> competitive)
        {
            var states=false;
            var currentAccount = BaseWebsite.CurrentAccount;
            states = _context.StoreCompetitiveFeature.UpdateStoreCompetitive(competitive, currentAccount.StoreId);
            if (states == true)
            {
                return ApiResponse.CreateSuccessResult();
            }
            else
            {
                return ApiResponse.CreateErrorResult(DataLayer.Enumarables.Enum_Message.NONE);
            }
            
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
