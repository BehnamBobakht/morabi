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
    public class ApiAccountOrder : ApiResponse
    {
        public static ApiResult Post(UnitOfWork _context, int accountId, int? sendTypeId = null, int? paymentTypeId = null, int? addressId = null, string rebateValue = null, int? currencyId = null, bool IsGift = false, string GiftDescription = null, bool HasFactor = false, string Description = null, bool store = false, AccountAddress address = null)
        {
            double sendPrice = 0;
            double SumsendPrice = 0;
            List<AccountBasket> list = _context.AccountBasket.GetAllByAccount(accountId);
            var storeList = list.ToApiStore(_context, address);
            if (list.Count == 0)
                return CreateErrorResult(Enum_Message.REQUIRED_BASKET_ITEM);

            double currencyValue = 1;
            if (currencyId != null)
            {
                var currency = _context.Currency.GetById(currencyId);
                if (currency != null && currency.UnitPrice.HasValue)
                {
                    currencyValue = currency.UnitPrice.Value;
                }
            }

            PaymentType paymentType =
                (paymentTypeId != null) ?
                _context.PaymentType.GetById(paymentTypeId) :
                _context.PaymentType.GetByLabel(Enum_PaymentType.ONLINE);
            Code status = null;
            if (paymentType.Label == Enum_PaymentType.PLACE.ToString())
            {
                status = _context.Code.GetByLabel(Enum_Code.ORDER_STATUS_SUCCESS);
            }
            else
            {
                status = _context.Code.GetByLabel(Enum_Code.ORDER_STATUS_INSERTED);
            }
            AccountOrder order = new AccountOrder();
            order.PaymentTypeId = paymentType.ID;
            order.AccountId = accountId;
            order.Datetime = DateTime.Now;
            order.StatusId = status.ID;
            order.RebatePrice = 0;
            order.IsGift = IsGift;
            order.GiftDescription = GiftDescription;
            order.HasFactor = HasFactor;
            order.Description = Description;
            if (addressId != null)
                order.AddressId = addressId;

            if (store == false)
            {
                order.ParentId = null;
#pragma warning disable CS0219 // The variable 'sendType' is assigned but its value is never used
                SendType sendType = null;
#pragma warning restore CS0219 // The variable 'sendType' is assigned but its value is never used
                //double sendPrice = 0;
                //if (sendTypeId != null)
                //{
                //    sendType = _context.SendType.GetById(sendTypeId);
                //    int count = list.Sum(p => p.Count);
                //    sendPrice = sendType.GetPrice(count);
                //}
                if (address == null)
                {
                    var location = BaseWebsite.CurrentLocation;
                    var stateId = Base.BaseWebsite.CurrentLocation.StateId;
                    var countyId = Base.BaseWebsite.CurrentLocation.CountryId;
                    var cityId = Base.BaseWebsite.CurrentLocation.CityId;
                   
                    foreach (var item in storeList)
                    {
                        //SumsendPrice = 0;
                        sendPrice = 0;
                        var shippingCity = _context.ShippingCity.Where(s => s.StoreId == item.Store.ID && ((s.CountryId == countyId || s.CountryId == 0)
                 && (s.StateId == stateId || s.StateId == 0) && (s.CityId == cityId || s.CityId == 0))).FirstOrDefault();
                        order.DiscountPrice = _context.AccountBasket.GetFinalDiscountPrice(list);
                        if (shippingCity != null)
                        {
                            if (Base.BaseWebsite.WebsiteSetting.HasSendByPost)
                            {
                                var sendTypeStore = _context.Account.GetSendType(item.Store.ID);
                                if (sendTypeStore == 2)
                                {
                                    sendPrice += double.Parse(shippingCity.SendPricePost.ToString());
                                }
                                else if (sendTypeStore == 3)
                                {
                                    sendPrice += double.Parse(shippingCity.SendPricePostP.ToString());
                                }
                                else
                                {
                                    sendPrice += double.Parse(shippingCity.SendPrice.ToString());
                                }
                            }
                            else
                            {
                                sendPrice += double.Parse(shippingCity.SendPrice.ToString());
                            }

                            SumsendPrice += sendPrice;
                        }
                    }
                }
                else
                {

                    var stateId = address.StateId;
                    var countyId = address.CountryId;
                    var cityId = address.CityId;
                  
                    foreach (var item in storeList)
                    {
                        //SumsendPrice = 0;
                        sendPrice = 0;
                        var shippingCity = _context.ShippingCity.Where(s => s.StoreId == item.Store.ID && ((s.CountryId == countyId || s.CountryId == 0)
                 && (s.StateId == stateId || s.StateId == 0) && (s.CityId == cityId || s.CityId == 0))).FirstOrDefault();
                        order.DiscountPrice = _context.AccountBasket.GetFinalDiscountPrice(list);
                        if (shippingCity != null)
                        {
                            if (Base.BaseWebsite.WebsiteSetting.HasSendByPost)
                            {
                                var sendTypeStore = _context.Account.GetSendType(item.Store.ID);
                                if (sendTypeStore == 2)
                                {
                                    sendPrice += double.Parse(shippingCity.SendPricePost.ToString());
                                }
                                else if (sendTypeStore == 3)
                                {
                                    sendPrice += double.Parse(shippingCity.SendPricePostP.ToString());
                                }
                                else
                                {
                                    sendPrice += double.Parse(shippingCity.SendPrice.ToString());
                                }
                            }
                            else
                            {
                                sendPrice += double.Parse(shippingCity.SendPrice.ToString());
                            }

                            SumsendPrice += sendPrice;
                        }
                    }
                }



              
                var orderPrice = _context.AccountBasket.GetFinalBasketPrice(list) * currencyValue;
                order.Price = orderPrice + sendPrice;
            }
            _context.AccountOrder.Insert(order);
            _context.Save();
            if (store == false)
            {
                foreach (AccountBasket item in list)
                {
                    AccountOrderProduct orderproduct = new AccountOrderProduct()
                    {
                        OrderId = order.ID,
                        Count = item.Count,
                        Datetime = item.Datetime,
                        Price = item.Price,
                        ProductId = item.ProductId,
                       
                        ProductPrice = item.ProductPrice,
                        ProductDiscount = item.ProductDiscount
                    };
                    _context.AccountOrderProduct.Insert(orderproduct);
                }
                _context.Save();
                foreach (AccountBasket item in list)
                {
                    _context.Product.IncreaseSellProduct(item.ProductId);

                }

            }
            else
            {


                foreach (var item in storeList)
                {
                    var currentStore = _context.Store.GetById(item.Store.ID);
                    var orderPrice = item.Products.Sum(s => s.Price);
                    var sendtype = _context.SendType.GetByStoreId(item.Store.ID);
                    //SumsendPrice = 0;
                    sendPrice = 0;
                    if (address == null)
                    {
                        var shippingCity = _context.ShippingCity.Where(s => s.StoreId == item.Store.ID && ((s.CountryId == Base.BaseWebsite.CurrentLocation.CountryId || s.CountryId == 0)
           && (s.StateId == Base.BaseWebsite.CurrentLocation.StateId || s.StateId == 0) && (s.CityId == Base.BaseWebsite.CurrentLocation.CityId || s.CityId == 0))).FirstOrDefault();
                        order.DiscountPrice = _context.AccountBasket.GetFinalDiscountPrice(list);

                        if (shippingCity != null)
                        {
                            if (Base.BaseWebsite.WebsiteSetting.HasSendByPost)
                            {
                                var sendTypeStore = _context.Account.GetSendType(item.Store.ID);
                                if (sendTypeStore == 2)
                                {
                                    sendPrice += double.Parse(shippingCity.SendPricePost.ToString());
                                }
                                else if (sendTypeStore == 3)
                                {
                                    sendPrice += double.Parse(shippingCity.SendPricePostP.ToString());
                                }
                                else
                                {
                                    sendPrice += double.Parse(shippingCity.SendPrice.ToString());
                                }
                            }
                            else
                            {
                                sendPrice += double.Parse(shippingCity.SendPrice.ToString());
                            }

                            SumsendPrice += sendPrice;
                        }
                    }
                    else
                    {
                        var shippingCity = _context.ShippingCity.Where(s => s.StoreId == item.Store.ID && ((s.CountryId == address.CountryId || s.CountryId == 0)
           && (s.StateId == address.StateId || s.StateId == 0) && (s.CityId == address.CityId || s.CityId == 0))).FirstOrDefault();
                        order.DiscountPrice = _context.AccountBasket.GetFinalDiscountPrice(list);

                        if (shippingCity != null)
                        {
                            if (Base.BaseWebsite.WebsiteSetting.HasSendByPost)
                            {
                                var sendTypeStore = _context.Account.GetSendType(item.Store.ID);
                                if (sendTypeStore == 2)
                                {
                                    sendPrice += double.Parse(shippingCity.SendPricePost.ToString());
                                }
                                else if (sendTypeStore == 3)
                                {
                                    sendPrice += double.Parse(shippingCity.SendPricePostP.ToString());
                                }
                                else
                                {
                                    sendPrice += double.Parse(shippingCity.SendPrice.ToString());
                                }
                            }
                            else
                            {
                                sendPrice = double.Parse(shippingCity.SendPrice.ToString());
                            }
                            SumsendPrice += sendPrice;
                        }
                    }

                    AccountOrder subOrder = new AccountOrder();
                    subOrder.ParentId = order.ID;
                    subOrder.PaymentTypeId = paymentTypeId;
                    subOrder.AccountId = accountId;
                    subOrder.Datetime = order.Datetime;
                    subOrder.Price = orderPrice + sendPrice;
                    subOrder.DiscountPrice = _context.AccountBasket.GetFinalDiscountPrice(list);
                    order.Price += subOrder.Price;
                    order.DiscountPrice += subOrder.DiscountPrice;
                    subOrder.StatusId = status.ID;
                   
                    subOrder.StoreId = item.Store.ID;
                    subOrder.BenefitPercent = currentStore.BenefitPercent;
                    subOrder.IsGift = IsGift;
                    subOrder.GiftDescription = GiftDescription;
                    subOrder.HasFactor = HasFactor;
                    subOrder.Description = Description;
                    if (addressId != null)
                        subOrder.AddressId = addressId;
                    _context.AccountOrder.Insert(subOrder);
                    _context.Save();

                    foreach (var orderItem in item.Products)
                    {
                        AccountOrderProduct orderproduct = new AccountOrderProduct()
                        {
                            OrderId = subOrder.ID,
                            Count = orderItem.Count,
                            Datetime = orderItem.Datetime,
                            Price = orderItem.Price,
                            ProductId = Convert.ToInt32(orderItem.Product.Id),
                           
                            ProductPrice = orderItem.ProductPrice,
                            ProductDiscount = orderItem.Discount
                        };
                        _context.AccountOrderProduct.Insert(orderproduct);
                    }
                }
                _context.Save();

            }
            if (string.IsNullOrEmpty(rebateValue) == false)
            {

                //List<Rebate> rebateList = _context.Rebate.GetFromRebateValue(orderPrice: order.Price - SumsendPrice, rebateValue: rebateValue, rebatePrice: out double rebatePrice, rebateMessage: out Enum_Message rebateMessage);
                //if (Enum_Message.SUCCESSFULL_REBATE_HAS == rebateMessage)
                //{
                //    order.RebateId = rebateList.First().ID;
                //    order.RebatePrice = rebatePrice;
                //    order.Price = order.Price - rebatePrice;
                //    _context.Save();
                //}
            }

            return CreateSuccessResult(new ViewAccountOrder(order));
        }


        public static ApiResult Get(UnitOfWork _context, int orderId, int accountId)
        {
            AccountOrder order = _context.AccountOrder.GetById(orderId);
            if (order != null)
            {
                if (order.AccountId == accountId)
                    return CreateSuccessResult(new ViewAccountOrder(order));
                else
                    return CreateInvalidKeyResult();
            }
            else
                return CreateErrorResult(Enum_Message.INVALID_DATA);
        }

        public static ApiResult Get_Search(UnitOfWork _context, int? accountId, int? index = null, int? pageSize = null, bool exteraResult = false, AccountAddress address = null, Enum_Code aoStatus = Enum_Code.ORDER_STATUS_NONE)
        {
            List<string> listNewStatus = new List<string>();

            listNewStatus.Add(aoStatus.ToString());
                       List<AccountOrder> list = _context.AccountOrder.Search(index: index, pageSize: pageSize, accountId: accountId, statusStr: listNewStatus);
            if (exteraResult)
            {
                var exteraModel = new ViewModels.ViewApiExtera();

                exteraModel.List = list.ToApi(address);

                exteraModel.Count = _context.AccountOrder.SearchCount(accountId: accountId, statusStr: listNewStatus);
                return ApiResponse.CreateSuccessResult(exteraModel);
            }
            return CreateSuccessResult(list.ToApi());
        }
    }
}
