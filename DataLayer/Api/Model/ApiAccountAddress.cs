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
    public class ApiAccountAddress : ApiResponse
    {
        public static ApiResult Get(UnitOfWork _context, int accountId)
        {
            return CreateSuccessResult(_context.AccountAddress.GetAllByAccountVm(accountId));
        }
        public static ApiResult Get(UnitOfWork _context, int accountId,int addressId)
        {
            return CreateSuccessResult(_context.AccountAddress.Where(s=>s.AccountId==accountId&&s.ID==addressId).ToApi());
        }
        public static ApiResult Post(UnitOfWork _context, int accountId, AccountAddress address, bool returnAddresses = true)
        {
            if (string.IsNullOrEmpty(address.NameFamily))
                return CreateErrorResult(Enumarables.Enum_Message.REQUIRED_ACCOUNT_ADDRESS_NAMEFAMILY);
            //else if (string.IsNullOrEmpty(address.NationalCode) && BaseWebsite.WebsiteSetting.NationalCodeIsRequired)
            //    return CreateErrorResult(Enum_Message.REQUIRED_ACCOUNT_NationalCode);
            //else if (BaseWebsite.WebsiteSetting.NationalCodeIsRequired && BaseSecurity.IsValidInput(address.NationalCode.ToEnglishDigit(), Enum_Validation.NationalCode) == false)
            //    return CreateErrorResult(Enum_Message.INVALID_ACCOUNT_NationalCode);
            else if (string.IsNullOrEmpty(address.Mobile))
                return CreateErrorResult(Enumarables.Enum_Message.REQUIRED_ACCOUNT_ADDRESS_MOBILE);

            else if (BaseSecurity.IsValidInput(address.Mobile.ToEnglishDigit(), Enum_Validation.MOBILE) == false)
                return CreateErrorResult(Enum_Message.INVALID_ACCOUNT_MOBILE);

            else if (BaseWebsite.WebsiteSetting.ZipCodeIsRequired && string.IsNullOrEmpty(address.PostalCode))
                return CreateErrorResult(Enumarables.Enum_Message.REQUIRED_ACCOUNT_ADDRESS_ZipCode);
            if (BaseWebsite.WebsiteSetting.ZipCodeIsRequired && BaseSecurity.IsValidInput(address.PostalCode, Enum_Validation.ZipCode) == false)
                return CreateErrorResult(Enum_Message.INVALID_ZipCode_FORMAT);


            if (string.IsNullOrEmpty(address.AddressValue))
                return CreateErrorResult(Enumarables.Enum_Message.REQUIRED_ACCOUNT_ADDRESS_VALUE);

            if (BaseWebsite.WebsiteSetting.HasRequierdState == true)
            {
                if (address.StateId == 0 || address.StateId == null)
                {
                    return CreateErrorResult(Enumarables.Enum_Message.REQUIRED_ACCOUNT_State_VALUE);
                }
                if (address.CityId == 0 || address.CityId == null)
                {
                    return CreateErrorResult(Enumarables.Enum_Message.REQUIRED_ACCOUNT_City_VALUE);
                }
            }
            else
            {
                if (address.StateId == 0)
                {
                    address.StateId = 8;
                }
                if (address.CityId == 0)
                {
                    address.CityId = 87;
                }
            }

            address.AccountId = accountId;
            _context.AccountAddress.Insert(address);
            _context.Save();
            if (returnAddresses)
            {
                List<AccountAddress> list = _context.AccountAddress.GetAllByAccount(accountId);
                return CreateSuccessResult(list.ToApi());
            }
            return CreateSuccessResult();


        }

        public static ApiResult Put(UnitOfWork _context, int accountId, AccountAddress address)
        {
            AccountAddress entity = _context.AccountAddress.GetById(address.ID);
            if (entity.AccountId != accountId)
                return ApiResponse.CreateInvalidKeyResult();

            if (string.IsNullOrEmpty(address.NameFamily))
                return CreateErrorResult(Enumarables.Enum_Message.REQUIRED_ACCOUNT_ADDRESS_NAMEFAMILY);

            else if (string.IsNullOrEmpty(address.Mobile))
                return CreateErrorResult(Enumarables.Enum_Message.REQUIRED_ACCOUNT_ADDRESS_MOBILE);
            else if (BaseSecurity.IsValidInput(address.Mobile, Enum_Validation.MOBILE) == false)
                return CreateErrorResult(Enum_Message.INVALID_ACCOUNT_MOBILE);

            if (!string.IsNullOrEmpty(address.PostalCode))
            {
                if (BaseSecurity.IsValidInput(address.PostalCode, Enum_Validation.ZipCode) == false)
                    return CreateErrorResult(Enum_Message.INVALID_ZipCode_FORMAT);
            }
            if (string.IsNullOrEmpty(address.AddressValue))
                return CreateErrorResult(Enumarables.Enum_Message.REQUIRED_ACCOUNT_ADDRESS_VALUE);

            if (BaseWebsite.WebsiteSetting.HasRequierdState == true)
            {
                if (address.StateId == 0)
                {
                    return CreateErrorResult(Enumarables.Enum_Message.REQUIRED_ACCOUNT_State_VALUE);
                }
                if (address.CityId == 0)
                {
                    return CreateErrorResult(Enumarables.Enum_Message.REQUIRED_ACCOUNT_City_VALUE);
                }
            }
            else
            {
                if (address.StateId == 0)
                {
                    address.StateId = 8;
                }
                if (address.CityId == 0)
                {
                    address.CityId = 87;
                }
            }

            entity.AddressValue = address.AddressValue;
            entity.CityId = address.CityId;
            entity.Latitude = address.Latitude;
            entity.Longitude = address.Longitude;
            entity.Mobile = address.Mobile;
            entity.NameFamily = address.NameFamily;
            entity.Phone = address.Phone;
            entity.StateId = address.StateId;
            entity.PostalCode = address.PostalCode;
            entity.CountryId = address.CountryId;
            entity.NationalCode = address.NationalCode;
            _context.AccountAddress.Update(entity);
            _context.Save();

            List<AccountAddress> list = _context.AccountAddress.GetAllByAccount(accountId);
            return CreateSuccessResult(list.ToApi());
        }

        public static ApiResult Delete(UnitOfWork _context, int accountId, int addressId)
        {
            AccountAddress entity = _context.AccountAddress.GetById(addressId);
            if (entity.AccountId != accountId)
                return ApiResponse.CreateInvalidKeyResult();

            if (entity.AccountOrder.Count > 0)
            {
                entity.AccountId = null;
                _context.AccountAddress.Update(entity);
            }
            else
            {
                if(entity.Cart.Any())
                {
                    entity.Cart.ToList().ForEach(x => x.AddressId = null);
                }
                _context.AccountAddress.Delete(entity);
            }
            _context.Save();

            List<AccountAddress> list = _context.AccountAddress.GetAllByAccount(accountId);
            return CreateSuccessResult(list.ToApi());
        }
    }
}
