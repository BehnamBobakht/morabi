using DataLayer.Api;
using DataLayer.Api.Model;
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
    public class CurrencyController : ApiController
    {
        UnitOfWork _context = new UnitOfWork();

        [HttpGet]
        public ApiResult Get()
        {
            List<ViewCurrency> currencies = _context.Currency.GetAll().ToApi();
            return ApiResponse.CreateSuccessResult(currencies);
        }

        [HttpGet]
        public ApiResult Get(int currencyId,int price)
        {
            int ChangedPrice = 0;
            ChangedPrice= _context.Currency.GetChangedPrice(currencyId, price);
            return ApiResponse.CreateSuccessResult(ChangedPrice);
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