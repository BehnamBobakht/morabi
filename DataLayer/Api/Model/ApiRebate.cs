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
    public class ApiRebate : ApiResponse
    {

        //public static ApiResult Get(UnitOfWork _context, int? accountId, string rebateValue,AccountAddress address=null)
        //{
        //    if (string.IsNullOrEmpty(rebateValue))
        //        return CreateErrorResult(Enum_Message.REQUIRED_REBATE_VALUE);

        //    double orderPrice, discount = 0;
        //    if (BaseWebsite.WebsiteSetting.HasStore)
        //    {
        //        BaseBasket.GetAccountBasketStore(_context: _context, price: out orderPrice, discount: out discount, accountId: accountId,address:address);
        //    }
        //    else
        //    {

        //        var basket = BaseBasket.GetAccountBasket(_context: _context, accountId: accountId);
        //        orderPrice = basket.TotalPrice;
        //    }
        //    List<Rebate> rebateList = _context.Rebate.GetFromRebateValue(orderPrice: orderPrice, rebateValue: rebateValue, rebatePrice: out double rebatePrice, rebateMessage: out Enum_Message rebateMessage);
        //    if (Enum_Message.SUCCESSFULL_REBATE_HAS == rebateMessage)
        //        return CreateSuccessResult(rebateMessage, rebatePrice);
        //    else
        //        return CreateErrorResult(rebateMessage);


        //}
        //public static ApiResult Post(UnitOfWork _context, int accountId, int? orderId, string rebateValue)
        //{
        //    if (string.IsNullOrEmpty(rebateValue))
        //        return CreateErrorResult(Enum_Message.REQUIRED_REBATE_VALUE);

        //    AccountOrder order = _context.AccountOrder.GetById(orderId);
        //    if (order.AccountId != accountId)
        //        return CreateInvalidKeyResult();
        //    double orderPrice, discount = 0;
        //    if (BaseWebsite.WebsiteSetting.HasStore)
        //    {
        //        BaseBasket.GetAccountBasketStore(_context: _context, price: out orderPrice, discount: out discount, accountId: accountId);
        //    }
        //    else
        //    {

        //        var basket = BaseBasket.GetAccountBasket(_context: _context, accountId: accountId);
        //        orderPrice = basket.TotalPrice;
        //    }
        //    List<Rebate> rebateList = _context.Rebate.GetFromRebateValue(orderPrice: orderPrice, rebateValue: rebateValue, rebatePrice: out double rebatePrice, rebateMessage: out Enum_Message rebateMessage);
        //    if (rebateMessage != Enum_Message.NONE)
        //        return CreateErrorResult(rebateMessage);
        //    if (rebateList.Count == 0)
        //        return CreateErrorResult(Enum_Message.INVALID_REBATE_VALUE);

        //    order.RebateId = rebateList[0].ID;
        //    order.RebatePrice = rebatePrice;
        //    order.Price = order.Price - rebatePrice;
        //    _context.Save();

        //    return CreateSuccessResult(order);
        //}

        //public static ApiResult SendSms(UnitOfWork _context, string rebateMobile)
        //{
        //    if (string.IsNullOrEmpty(rebateMobile))
        //        return CreateErrorResult(Enum_Message.REQUIRED_ACCOUNT_MOBILE);
        //    else if (BaseSecurity.IsValidInput(rebateMobile, Enum_Validation.MOBILE) == false)
        //        return CreateErrorResult(Enum_Message.INVALID_MOBILE_FORMAT);

        //    Rebate rebate = _context.Rebate.FirstOrDefault(p => p.Mobile == rebateMobile);
        //    if (rebate == null)
        //    {
        //        rebate = _context.Rebate.FirstOrDefault(p => p.Mobile == null);
        //        rebate.Mobile = rebateMobile;
        //        _context.Save();
        //    }

        //    SmsType smsType = _context.SmsType.GetByLabel(Enum_SmsType.REBATE);
        //    if (smsType != null)
        //    {
        //        string token_1 = rebate.Mobile;
        //        string token_2 = rebate.CodeValue;
        //        string token_3 = "";

        //        string body = smsType.Body;
        //        body = body.Replace("%token3", token_3);
        //        body = body.Replace("%token2", token_2);
        //        body = body.Replace("%token", token_1);
        //        _context.Sms.SaveNewSms(rebateMobile, Enum_SmsType.REBATE, body.ToString(), token_1, token_2);
        //        _context.Save();
        //    }

        //    return CreateSuccessResult();
        //}

        //public static ApiResult Delete(UnitOfWork _context, int orderId)
        //{
        //    ViewAccount currentUser = _context.Account.GetCurrentAccount();
        //    if (currentUser == null)
        //        return CreateInvalidKeyResult();
        //    AccountOrder order = _context.AccountOrder.GetById(orderId);
        //    if (order.AccountId != currentUser.Id)
        //        return CreateInvalidKeyResult();

        //    order.RebateId = null;
        //    order.Price = order.Price + order.RebatePrice.GetValueOrDefault();
        //    order.RebatePrice = 0;
        //    _context.Save();

        //    return CreateSuccessResult();
        //}
    }
}
