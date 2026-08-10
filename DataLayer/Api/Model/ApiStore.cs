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
    public class ApiStore : ApiResponse
    {

        public static ApiResult Post_DoRegister(UnitOfWork _context, DataLayer.Entities.Store store)
        {
            if (BaseWebsite.CurrentAccount == null)
                return CreateErrorResult(Enum_Message.REQUIRED_STORE_LOGIN);
            else if (_context.Store.FirstOrDefault(s => s.AccountId == BaseWebsite.CurrentAccount.Id) != null)
                return CreateErrorResult(Enum_Message.INVALID_STORE_MULTIPLE_STORE);
            else if (store.Name == null)
                return CreateErrorResult(Enum_Message.REQUIRED_STORE_NAME);
            else if (store.AddressValue == null)
                return CreateErrorResult(Enum_Message.REQUIRED_STORE_ADDRESS_VALUE);
            else if (store.Description == null)
                return CreateErrorResult(Enum_Message.REQUIRED_STORE_DESCRIPTION);
            else
            {
                store.Active = false;
                store.Approve = false;
                store.CrateDate = DateTime.Now;
                store.AccountId = BaseWebsite.CurrentAccount.Id;
                _context.Store.Insert(store);
                _context.Save();
                var storeId = _context.Store.FirstOrDefault(s => s.AccountId == BaseWebsite.CurrentAccount.Id).ID;
                ShippingCity shippingCity = new ShippingCity();
                shippingCity.StoreId = storeId;
                shippingCity.CityId = 0;
                shippingCity.StateId = 0;
                shippingCity.CountryId = 118;
                shippingCity.SendTime = 1;
                shippingCity.SendPrice = 10000;
                _context.ShippingCity.Insert(shippingCity);
                _context.Save();
                SendType shipping = new SendType();
                shipping.StoreId = storeId;
                _context.SendType.Insert(shipping);
                _context.Save();
                return CreateSuccessResult(Enum_Message.SUCCESSFULL_REGISTER_STORE);

            }
        }
        public static ApiResult Search(
          UnitOfWork _context,
          int? index = null,
          int? pageSize = null,
          string name = null,
          bool active = true
          )
        {
            pageSize = pageSize == null ? 10 : pageSize;
            index = index == null ? 1 : index;
            List<Store> list = _context.Store.Search(
                index: index.Value,
                pageSize: pageSize.Value,
                name: name,
                active: active);
            return ApiResponse.CreateSuccessResult(list.ToApi());
        }

    }
}
