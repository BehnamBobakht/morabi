using DataLayer.Entities;
using DataLayer.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.ViewModels;
using DataLayer.Api.Model;
using DataLayer.Enumarables;
using ExpressMapper.Extensions;
using System.Web;

namespace DataLayer.Base
{
    public class BaseCart
    {
        public static ResultVm<CartVm> GetCurrentCard(UnitOfWork _context, int? accountId = null, string cartSyncCode = null)
        {
            var result = _context.Cart.GetCart(accountId, cartSyncCode);
            return result;
        }
        public static ResultVm<CartVm> Update(UnitOfWork _context, CartUpdateVm cartUpdate, string description = null)
        {
            var result = new ResultVm<CartVm>();
            result.Model = new CartVm();
            result.Model.AccountId = cartUpdate.AccountId;
            result.Model.SyncCode = cartUpdate.SyncCode;
            var currentCart = _context.Cart.GetCartEntity(cartUpdate.AccountId, cartUpdate.SyncCode, insert: true);
            currentCart.MerchantId = 2;
            //if (cartUpdate.MerchantId != null)
            //{
            //    currentCart.MerchantId = cartUpdate.MerchantId;
            //}
            if (cartUpdate.UpdateType != CartUpdateType.None)
            {
                switch (cartUpdate.UpdateType)
                {

                    case Enumarables.CartUpdateType.SetAddress:
                        {
                            currentCart.AddressId = cartUpdate.AddressId;
                            var address = _context.AccountAddress.GetById(cartUpdate.AddressId);
                            if (address != null)
                            {
                                if (BaseWebsite.WebsiteSetting.HasCentralizedDelivery)
                                {
                                    var deliveries = BaseDelivery.GetPossibleDeliveries(storeId: null, price: (currentCart.TotalPrice + currentCart.EarnestRemainingPrice) - currentCart.RebatePrice, cityId: address.CityId.Value, stateId: address.StateId.Value);
                                    if (deliveries.Model.Any())
                                    {
                                        result.Model.HaveAddress = currentCart.CartStore.Any(s => s.CartStoreItem.Any(x => x.Product.IsService != true));
                                        if (result.Model.HaveAddress == false)
                                        {
                                            var currentDelivery = deliveries.Model.FirstOrDefault(s => s.BasePrice == 0 && s.MaxDays == 0);
                                            currentCart.DeliveryId = currentDelivery.ID;
                                            currentCart.TotalDeliveryPrice = currentDelivery.PayementPrice;
                                            currentCart.CashOnDelivery = currentDelivery.CashOnDelivery;
                                        }
                                        else
                                        {
                                            var currentDelivery = deliveries.Model.FirstOrDefault(s => s.BasePrice != 0 && s.MaxDays != 0);
                                            currentCart.DeliveryId = currentDelivery.ID;
                                            currentCart.TotalDeliveryPrice = currentDelivery.PayementPrice;
                                            currentCart.CashOnDelivery = currentDelivery.CashOnDelivery;

                                        }
                                    }
                                }
                                else
                                {
                                    foreach (var item in currentCart.CartStore)
                                    {
                                        var deliveries = BaseDelivery.GetPossibleDeliveries(storeId: item.StoreId, price: (item.TotalPrice + item.EarnestRemainingPrice) - item.RebatePrice, cityId: address.CityId.Value, stateId: address.StateId.Value);
                                        if (deliveries.Model.Any())
                                        {
                                            result.Model.HaveAddress = currentCart.CartStore.Any(s => s.CartStoreItem.Any(x => x.Product.IsService != true));
                                            if (result.Model.HaveAddress == false)
                                            {
                                                item.DeliveryId = deliveries.Model.FirstOrDefault(s => s.BasePrice == 0 && s.MaxDays == 0).ID;
                                                item.DeliveryPrice = deliveries.Model.FirstOrDefault(s => s.BasePrice == 0 && s.MaxDays == 0).PayementPrice;
                                            }
                                            else
                                            {
                                                item.DeliveryId = deliveries.Model.FirstOrDefault(s => s.BasePrice != 0 && s.MaxDays != 0).ID;
                                                item.DeliveryPrice = deliveries.Model.FirstOrDefault(s => s.BasePrice != 0 && s.MaxDays != 0).PayementPrice;

                                            }
                                        }
                                    }

                                }

                            }

                            break;
                        }
                    case Enumarables.CartUpdateType.SetDelivery:
                        {
                            if (BaseWebsite.WebsiteSetting.HasCentralizedDelivery)
                            {
                                var address = _context.AccountAddress.GetById(currentCart.AddressId);
                                if (address != null)
                                {
                                    var deliveries = BaseDelivery.GetPossibleDeliveries(storeId: null, price: currentCart.TotalPrice - currentCart.RebatePrice, cityId: address.CityId.Value, stateId: address.StateId.Value);
                                    if (deliveries.Model.Any())
                                    {
                                        var delivery = deliveries.Model.FirstOrDefault(s => s.ID == cartUpdate.DeliveryId);
                                        if (delivery != null)
                                        {
                                            currentCart.DeliveryId = delivery.ID;
                                            currentCart.TotalDeliveryPrice = delivery.PayementPrice;
                                            currentCart.CashOnDelivery = delivery.CashOnDelivery;
                                            if (delivery.CashOnDelivery)
                                            {
                                                currentCart.MerchantId = null;
                                            }
                                        }
                                        else
                                        {
                                            currentCart.DeliveryId = null;
                                            currentCart.CashOnDelivery = false;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var storeCart = currentCart.CartStore.FirstOrDefault(s => s.StoreId == cartUpdate.StoreId);
                                if (storeCart == null)
                                {
                                    result.Status = Enumarables.ResultStatus.Error;
                                    result.Message = Resource.Notify.UnspecifiedError;
                                }
                                else
                                {
                                    var address = _context.AccountAddress.GetById(currentCart.AddressId);
                                    if (address != null)
                                    {
                                        var deliveries = BaseDelivery.GetPossibleDeliveries(storeId: storeCart.StoreId, price: storeCart.TotalPrice - storeCart.RebatePrice, cityId: address.CityId.Value, stateId: address.StateId.Value);
                                        if (deliveries.Model.Any())
                                        {
                                            var delivery = deliveries.Model.FirstOrDefault(s => s.ID == cartUpdate.DeliveryId);
                                            if (delivery != null)
                                            {
                                                storeCart.DeliveryId = delivery.ID;
                                                storeCart.DeliveryPrice = delivery.PayementPrice;
                                            }
                                            else
                                            {
                                                storeCart.DeliveryId = null;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        result.Status = Enumarables.ResultStatus.Error;
                                        result.Message = Resource.Notify.UnspecifiedError;
                                    }

                                }
                            }
                            break;
                        }
                    case Enumarables.CartUpdateType.SetRebate:
                        {
                            var rebateVm = BaseRebate.GetRebate(_context: _context, cart: currentCart, cartUpdate.RebateCode);
                            if (rebateVm.Status == Enumarables.ResultStatus.Success)
                            {
                                currentCart.RebateId = null;
                                currentCart.CartStore.ToList().ForEach(x => x.RebateId = null);
                                if (rebateVm.Model.StoreId == null)
                                {
                                    currentCart.RebateId = rebateVm.Model.ID;

                                }
                                else
                                {
                                    var storeCart = currentCart.CartStore.FirstOrDefault(s => s.StoreId == rebateVm.Model.StoreId);
                                    storeCart.RebateId = rebateVm.Model.ID;
                                    storeCart.RebatePrice = rebateVm.Model.RebateFinalValue;
                                    //storeCart.DeliveryId = null;
                                    //storeCart.DeliveryPrice = 0;
                                }
                                currentCart.RebatePrice = rebateVm.Model.RebateFinalValue;
                                //currentCart.DeliveryId = null;
                                //currentCart.TotalDeliveryPrice = 0;
                            }

                            result.Status = rebateVm.Status;
                            result.Message = rebateVm.Message;

                            break;
                        }
                    case Enumarables.CartUpdateType.SetMerchant:
                        {
                            if (!currentCart.CashOnDelivery)
                            {
                                if (cartUpdate.MerchantId != null)
                                {
                                    var active = _context.Merchant.IsActive(merchantId: cartUpdate.MerchantId.Value);
                                    if (!active)
                                    {
                                        result.Status = Enumarables.ResultStatus.Error;
                                        result.Message = Resource.Notify.MerchantIsNotActive;
                                        break;
                                    }
                                }
                                currentCart.MerchantId = cartUpdate.MerchantId;
                            }
                            else
                            {
                                currentCart.MerchantId = null;
                            }

                            break;
                        }
                    case Enumarables.CartUpdateType.SetOther:
                        {

                            currentCart.CustomerMessage = cartUpdate.CustomerMessage;


                            if (cartUpdate.GiftMessage != null && cartUpdate.GiftMessage != "")
                            {
                                try
                                {
                                    var code = cartUpdate.GiftMessage.Replace("10", "");
                                    var id = int.Parse(code);
                                    if (cartUpdate.AccountId != id)
                                    {
                                        var account = _context.Account.GetById(id);
                                        if (account == null)
                                        {

                                            result.Status = Enumarables.ResultStatus.Error;
                                            result.Message = Resource.Notify.CodeInvalid;
                                            currentCart.GiftMessage = "";
                                            break;
                                        }
                                        else
                                        {
                                            currentCart.GiftMessage = cartUpdate.GiftMessage;
                                            result.Status = Enumarables.ResultStatus.Success;

                                            result.Message = Resource.Notify.CodeValid;

                                        }
                                    }
                                    else
                                    {

                                        result.Status = Enumarables.ResultStatus.Error;
                                        result.Message = Resource.Notify.CodeInvalid;
                                        currentCart.GiftMessage = "";
                                        break;
                                    }

                                }
                                catch
                                {
                                    currentCart.GiftMessage = "";
                                    result.Status = Enumarables.ResultStatus.Error;
                                    result.Message = Resource.Notify.CodeInvalid;
                                    break;
                                }
                            }
                            else
                            {
                                currentCart.GiftMessage = "";
                            }
                            break;
                        }
                    default:
                        {
                            bool isEarnest = false;
                            var storeProductQuantity = _context.StoreProductQuantity.GetById(cartUpdate.StoreProductQuantityId);
                            if (storeProductQuantity != null)
                            {
                                isEarnest = BaseStore.StoreProductItemIsEarnest(storeProductQuantity);
                            }
                            if (storeProductQuantity == null)
                            {
                                result.Status = Enumarables.ResultStatus.Error;
                                result.Message = Resource.Notify.ProductNotFound;
                            }
                            else if (storeProductQuantity.Count < 1 && isEarnest == false)
                            {
                                result.Status = Enumarables.ResultStatus.Error;
                                result.Message = Resource.Notify.TheProductIsNotAvailable;
                            }
                            else if (storeProductQuantity.Price < 1)
                            {
                                result.Status = Enumarables.ResultStatus.Error;
                                result.Message = Resource.Notify.TheProductIsNotAvailable;
                            }
                            else if (_context.Product.ProductIsSalable(productId: storeProductQuantity.StoreProduct.ProductId, isEarnest: isEarnest) == false)
                            {
                                result.Status = Enumarables.ResultStatus.Error;
                                result.Message = Resource.Notify.ProductNotFound;
                            }
                            else if (_context.Store.StoreIsActive(storeProductQuantity.StoreProduct.StoreId) == false)
                            {
                                result.Status = Enumarables.ResultStatus.Error;
                                result.Message = Resource.Notify.ProductNotFound;
                            }
                            else if (BaseWebsite.WebsiteSetting.ProductMaxOrderCount)
                            {
                                if (storeProductQuantity.ProductQuantity.Product.MaxOrderCount != null && storeProductQuantity.ProductQuantity.Product.MaxOrderCount > 0)
                                {
                                    if (cartUpdate.Count > storeProductQuantity.ProductQuantity.Product.MaxOrderCount.Value)
                                    {
                                        result.Status = Enumarables.ResultStatus.Error;
                                        result.Message = Resource.Notify.TheCountOfYourOrderIsMoreThanTheMaximumOrderCount;

                                    }
                                }
                            }
                            if (cartUpdate.UpdateType != CartUpdateType.Delete && cartUpdate.AccountId != null && storeProductQuantity.ProductQuantity.Product.InstallmentCount == null && storeProductQuantity.ProductQuantity.Product.ProductTypeId == 2)
                            {
                                var accountProduct = _context.AccountOrder.FirstOrDefault(s => s.AccountId == cartUpdate.AccountId && s.AccountOrderProduct.Any(x => x.ProductId == storeProductQuantity.ProductQuantity.Product.ID || x.Product.ProductsId.Contains(storeProductQuantity.ProductQuantity.Product.ID.ToString())) && (s.StatusId == 67 || s.StatusId == 2068 || s.StatusId == 2073));
                                var course = _context.CoursePayment.FirstOrDefault(s => s.UserId == cartUpdate.AccountId && s.CourseId == storeProductQuantity.ProductQuantity.Product.ID);
                                if (accountProduct != null || course != null)
                                {
                                    result.Status = Enumarables.ResultStatus.Error;
                                    result.Message = Resource.Notify.OrderDuplicate;
                                }
                            }


                            if (result.Status == Enumarables.ResultStatus.Error)
                            {
                                return result;
                            }

                            var cartStore = currentCart.CartStore.FirstOrDefault(a => a.StoreId == storeProductQuantity.StoreProduct.StoreId);
                            if (cartStore == null)
                            {
                                cartStore = new CartStore() { StoreId = storeProductQuantity.StoreProduct.StoreId };
                                currentCart.CartStore.Add(cartStore);
                            }
                            switch (cartUpdate.UpdateType)
                            {
                                case Enumarables.CartUpdateType.Add:
                                    {
                                        if (cartUpdate.Count < 1)
                                        {
                                            result.Status = Enumarables.ResultStatus.Error;
                                            result.Message = Resource.Notify.TheCountOfOrderShouldNotBeLessThanOne;
                                        }
                                        else if (cartUpdate.Count > storeProductQuantity.Count && isEarnest == false)
                                        {
                                            result.Status = Enumarables.ResultStatus.Error;
                                            result.Message = Resource.Notify.TheCountOfYourOrdersIsMoreThanTheInventoryOfThisStore;
                                        }
                                        else if (cartUpdate.UpdateType != CartUpdateType.Delete && cartUpdate.AccountId != null && storeProductQuantity.ProductQuantity.Product.InstallmentCount == null && storeProductQuantity.ProductQuantity.Product.ProductTypeId == 2)
                                        {
                                            var accountProduct = _context.AccountOrder.FirstOrDefault(s => s.AccountId == cartUpdate.AccountId && s.AccountOrderProduct.Any(x => x.ProductId == storeProductQuantity.ProductQuantity.Product.ID || x.Product.ProductsId.Contains(storeProductQuantity.ProductQuantity.Product.ID.ToString())) && (s.StatusId == 2068 || s.StatusId == 2073 || s.StatusId == 67));
                                            var course = _context.CoursePayment.FirstOrDefault(s => s.UserId == cartUpdate.AccountId && s.CourseId == storeProductQuantity.ProductQuantity.Product.ID);
                                            if (accountProduct != null || course != null)
                                            {
                                                result.Status = Enumarables.ResultStatus.Error;
                                                result.Message = Resource.Notify.OrderDuplicate;
                                            }
                                        }
                                        if (result.Status == Enumarables.ResultStatus.Error)
                                        {
                                            return result;
                                        }
                                        var cartStoreItem = cartStore.CartStoreItem.FirstOrDefault(s => s.StoreProductQuantityId == cartUpdate.StoreProductQuantityId);

                                        if (cartStoreItem != null)
                                        {
                                            cartStoreItem.Count = cartUpdate.Count;
                                            cartStoreItem.Description = description;
                                        }
                                        else
                                        {
                                            cartStore.CartStoreItem.Add(new CartStoreItem() { Description = description, Count = cartUpdate.Count, StoreProductQuantityId = cartUpdate.StoreProductQuantityId, ProductId = storeProductQuantity.StoreProduct.ProductId });
                                        }

                                        break;
                                    }
                                case Enumarables.CartUpdateType.Increase:
                                    {

                                        var cartStoreItem = cartStore.CartStoreItem.FirstOrDefault(s => s.StoreProductQuantityId == cartUpdate.StoreProductQuantityId);
                                        if (cartStoreItem != null)
                                        {
                                            if (BaseWebsite.WebsiteSetting.ProductMaxOrderCount)
                                            {
                                                if (storeProductQuantity.ProductQuantity.Product.MaxOrderCount != null && storeProductQuantity.ProductQuantity.Product.MaxOrderCount > 0)
                                                {
                                                    if (cartStoreItem.Count + 1 > storeProductQuantity.ProductQuantity.Product.MaxOrderCount.Value)
                                                    {
                                                        result.Status = Enumarables.ResultStatus.Error;
                                                        result.Message = Resource.Notify.TheCountOfYourOrderIsMoreThanTheMaximumOrderCount;

                                                    }
                                                }
                                            }
                                            if (cartUpdate.UpdateType != CartUpdateType.Delete && cartUpdate.AccountId != null && storeProductQuantity.ProductQuantity.Product.InstallmentCount == null && storeProductQuantity.ProductQuantity.Product.ProductTypeId == 2)
                                            {
                                                var accountProduct = _context.AccountOrder.FirstOrDefault(s => s.AccountId == cartUpdate.AccountId && s.AccountOrderProduct.Any(x => x.ProductId == storeProductQuantity.ProductQuantity.Product.ID || x.Product.ProductsId.Contains(storeProductQuantity.ProductQuantity.Product.ID.ToString())) && (s.StatusId == 2068 || s.StatusId == 2073));
                                                var course = _context.CoursePayment.FirstOrDefault(s => s.UserId == cartUpdate.AccountId && s.CourseId == storeProductQuantity.ProductQuantity.Product.ID);
                                                if (accountProduct != null || course != null)
                                                {
                                                    result.Status = Enumarables.ResultStatus.Error;
                                                    result.Message = Resource.Notify.OrderDuplicate;
                                                }
                                            }

                                            if (cartStoreItem.Count + 1 > storeProductQuantity.Count && isEarnest == false)
                                            {
                                                result.Status = Enumarables.ResultStatus.Error;
                                                result.Message = Resource.Notify.TheCountOfYourOrdersIsMoreThanTheInventoryOfThisStore;

                                            }
                                            if (result.Status != Enumarables.ResultStatus.Error)
                                            {
                                                cartStoreItem.Count++;
                                            }
                                        }
                                        else
                                        {
                                            cartStore.CartStoreItem.Add(new CartStoreItem() { Count = 1, StoreProductQuantityId = cartUpdate.StoreProductQuantityId, ProductId = storeProductQuantity.StoreProduct.ProductId });
                                        }
                                        break;
                                    }
                                case Enumarables.CartUpdateType.Decrease:
                                    {
                                        var cartStoreItem = cartStore.CartStoreItem.FirstOrDefault(s => s.StoreProductQuantityId == cartUpdate.StoreProductQuantityId);
                                        if (cartStoreItem != null)
                                        {
                                            if (cartStoreItem.Count - 1 < 1)
                                            {
                                                _context.CartStoreItem.Delete(cartStoreItem);
                                            }
                                            else
                                            {
                                                cartStoreItem.Count--;
                                            }
                                        }
                                        else
                                        {
                                            result.Status = Enumarables.ResultStatus.Error;
                                            result.Message = Resource.Notify.ProductNotFound;
                                        }
                                        break;
                                    }
                                case Enumarables.CartUpdateType.Delete:
                                    {
                                        var cartStoreItem = cartStore.CartStoreItem.FirstOrDefault(s => s.StoreProductQuantityId == cartUpdate.StoreProductQuantityId);
                                        if (cartStoreItem != null)
                                        {
                                            _context.CartStoreItem.Delete(cartStoreItem);
                                            _context.Save();
                                        }
                                        else
                                        {
                                            result.Status = Enumarables.ResultStatus.Error;
                                            result.Message = Resource.Notify.ProductNotFound;
                                        }
                                        break;
                                    }
                            }
                            if (result.Status != Enumarables.ResultStatus.Error)
                            {
                                SetToDefault(cart: currentCart, storeId: cartStore.StoreId);
                            }
                            break;
                        }
                }

            }
            if (result.Status != Enumarables.ResultStatus.Error)
            {
                var message = result.Message;
                result = Update(_context, currentCart);
                result.Message = message;
            }
            result.Model = GetCurrentCard(_context: _context, accountId: result.Model.AccountId, cartSyncCode: result.Model.SyncCode).Model;
            return result;
        }
        public static ResultVm<CartVm> Update(UnitOfWork _context, Cart currentCart)
        {
            if (currentCart == null)
                currentCart = _context.Cart.GetCartEntity(insert: true);
            foreach (var cartStore in currentCart.CartStore.ToList())
            {
                if (_context.Store.StoreIsActive(cartStore.StoreId) && cartStore.CartStoreItem.Count > 0)
                {
                    foreach (var cartStoreItem in cartStore.CartStoreItem.ToList())
                    {
                        var storeProductQuantity = _context.StoreProductQuantity.GetById(cartStoreItem.StoreProductQuantityId);
                        if (_context.StoreProductQuantity.IsSalable(storeProductQuantity))
                        {
                            var quantity = storeProductQuantity.Map<StoreProductQuantity, StoreProductQuantityVm>();
                            if (BaseWebsite.WebsiteSetting.HasPriceRange)
                            {
                                if (quantity.PriceRange.Any())
                                {
                                    var priceRange = quantity.PriceRange.FirstOrDefault(s => s.StartCount <= cartStoreItem.Count && (s.EndCount >= cartStoreItem.Count || s.EndCount == null));
                                    if (priceRange != null)
                                    {
                                        quantity.BasePrice = priceRange.Price;
                                        quantity.Price = priceRange.Price;
                                        quantity.DiscountValue = 0;
                                        quantity.DiscountPercent = 0;
                                    }
                                }
                            }
                            cartStoreItem.SingleBasePrice = quantity.BasePrice;
                            cartStoreItem.SingleDiscount = quantity.DiscountValue;
                            cartStoreItem.SingleDiscountPercent = quantity.DiscountPercent;
                            cartStoreItem.SinglePrice = quantity.Price;
                            if (quantity.EarnestPrice > 0)
                            {
                                cartStoreItem.SingleEarnestRemainingPrice = cartStoreItem.SinglePrice - quantity.EarnestPrice;
                                cartStoreItem.TotalEarnestRemainingPrice = cartStoreItem.SingleEarnestRemainingPrice * cartStoreItem.Count;
                                cartStoreItem.SinglePrice = quantity.EarnestPrice;
                            }
                            cartStoreItem.TotalBasePrice = cartStoreItem.SingleBasePrice * cartStoreItem.Count;
                            cartStoreItem.TotalDiscount = cartStoreItem.SingleDiscount * cartStoreItem.Count;
                            cartStoreItem.TotalPrice = cartStoreItem.SinglePrice * cartStoreItem.Count;
                        }
                        else
                        {
                            _context.CartStoreItem.Delete(cartStoreItem);
                            _context.Save();
                        }
                    }
                    cartStore.ItemCount = cartStore.CartStoreItem.Count();
                    if (cartStore.ItemCount < 1)
                    {
                        _context.CartStore.Delete(cartStore);
                        _context.Save();
                    }
                    else
                    {
                        cartStore.EarnestRemainingPrice = cartStore.CartStoreItem.Sum(s => s.TotalEarnestRemainingPrice);
                        cartStore.BasePrice = cartStore.CartStoreItem.Sum(s => s.TotalBasePrice);
                        cartStore.Discount = cartStore.CartStoreItem.Sum(s => s.SingleDiscount);
                        cartStore.TotalPrice = cartStore.CartStoreItem.Sum(s => s.TotalPrice);
                        cartStore.PayementPrice = cartStore.TotalPrice;
                        if (cartStore.DeliveryPrice > 0)
                        {
                            cartStore.PayementPrice += cartStore.DeliveryPrice;
                        }
                        if (cartStore.RebatePrice > 0)
                        {
                            cartStore.PayementPrice -= cartStore.RebatePrice;
                        }
                    }
                }
                else
                {
                    foreach (var item in cartStore.CartStoreItem.ToList())
                    {
                        _context.CartStoreItem.Delete(item);
                    }
                    _context.CartStore.Delete(cartStore);
                    _context.Save();
                }
            }
            currentCart.ItemCount = currentCart.CartStore.Sum(s => s.ItemCount);
            currentCart.EarnestRemainingPrice = currentCart.CartStore.Sum(s => s.EarnestRemainingPrice);

            currentCart.BasePrice = currentCart.CartStore.Sum(s => s.BasePrice);
            currentCart.Discount = currentCart.CartStore.Sum(s => s.Discount);
            currentCart.TotalPrice = currentCart.CartStore.Sum(s => s.TotalPrice);
            if (BaseWebsite.WebsiteSetting.HasCentralizedDelivery == false)
            {
                currentCart.TotalDeliveryPrice = currentCart.CartStore.Sum(s => s.DeliveryPrice);
            }
            currentCart.PayementPrice = (currentCart.TotalPrice + currentCart.TotalDeliveryPrice) - currentCart.RebatePrice;

            currentCart.LastUpdateDate = DateTime.Now;
            currentCart.MerchantId = 2;
            _context.Save();
            var result = new ResultVm<CartVm>();
            result.Status = Enumarables.ResultStatus.Success;
            result.Model = new CartVm() { AccountId = currentCart.AccountId, SyncCode = currentCart.SyncCode };
            result.Model.HaveAddress = currentCart.CartStore.Any(s => s.CartStoreItem.Any(x => x.Product.IsService != true));
            if (result.Model.HaveAddress == false)
            {
                result.Model.AddressId = 0;
                currentCart.AddressId = 0;
                var delivey = _context.Delivery.FirstOrDefault(s => s.BasePrice == 0 && s.MaxDays == 0);
                if (delivey != null)
                {
                    currentCart.DeliveryId = delivey.ID;
                    result.Model.DeliveryId = delivey.ID;
                }

                _context.Save();
            }
            //if (currentCart.CartStore.Any(s => s.CartStoreItem.Any(x => x.Product.ProductTypeId == 1) && s.CartStoreItem.Count() == 1))
            //{
            //    var delivey = _context.Delivery.FirstOrDefault(s => s.BasePrice == 0 && s.MaxDays == 0);
            //    if (delivey != null)
            //    {
            //        currentCart.DeliveryId = delivey.ID;
            //        result.Model.DeliveryId = delivey.ID;
            //    }

            //    _context.Save();
            //}
            return result;
        }
        private static void SetToDefault(Cart cart, int storeId)
        {
            cart.MerchantId = null;
            cart.RebateId = null;
            cart.RebatePrice = 0;
            cart.TotalDeliveryPrice = 0;
            cart.DeliveryId = null;
            cart.CashOnDelivery = false;
            var cartStore = cart.CartStore.FirstOrDefault(s => s.StoreId == storeId);
            cartStore.RebateId = null;
            cartStore.RebatePrice = 0;
            cartStore.DeliveryId = null;
            cartStore.DeliveryPrice = 0;
        }
        public static CartUpdateVm GetCurrentCartAccount()
        {
            var result = new CartUpdateVm();

            ViewAccount currentAccount = BaseWebsite.CurrentAccount;
            if (currentAccount != null)
            {
                result.AccountId = currentAccount.Id;
            }
            result.SyncCode = BaseCookie.GetCookie(key: CookieName.CardCookie.ToString());
            return result;
        }
        public static ResultVm<AccountOrderVm> NewOrder(UnitOfWork _context, CartUpdateVm cartUpdate)
        {
            var result = new ResultVm<AccountOrderVm>();
            var cart = _context.Cart.GetCartEntity(cartUpdate.AccountId, cartUpdate.SyncCode);
            if (cart.ItemCount < 1)
            {
                result.Status = ResultStatus.Error;
                result.Message = Resource.Notify.CartIsEmpty;
            }
            else if (cart.AddressId == null)
            {
                result.Status = ResultStatus.Error;
                result.Message = Resource.Notify.PleaseSelectAddress;
            }
            else if ((!cart.CashOnDelivery) && cart.MerchantId == null)
            {
                result.Status = ResultStatus.Error;
                result.Message = Resource.Notify.PleaseSpecifyThePaymentGateway;
            }
            if (BaseWebsite.WebsiteSetting.HasCentralizedDelivery)
            {
                if (cart.DeliveryId == null)
                {
                    result.Status = ResultStatus.Error;
                    result.Message = Resource.Notify.PleaseSelectTheShippingType;
                }
            }
            else if (cart.CartStore.Any(s => s.DeliveryId == null))
            {
                result.Status = ResultStatus.Error;
                result.Message = Resource.Notify.PleaseSelectTheShippingType;
            }
            if (result.Status == ResultStatus.Error)
            {
                return result;
            }
            var parentOrder = new AccountOrder();
            parentOrder.AccountId = cart.AccountId.Value;
            parentOrder.AddressId = cart.AddressId.Value;
            parentOrder.CustomerMessage = cart.CustomerMessage;
            parentOrder.Datetime = DateTime.Now;
            parentOrder.DeliveryId = cart.DeliveryId;
            parentOrder.DeliveryPrice = cart.TotalDeliveryPrice;
            parentOrder.DiscountPrice = cart.Discount;
            parentOrder.GiftDescription = cart.GiftMessage;
            if (!string.IsNullOrEmpty(parentOrder.GiftDescription))
            {
                parentOrder.IsGift = true;
            }

            var isFree = cart.PayementPrice == 1 || cart.PayementPrice < 20 ? true : false;
            cart.CashOnDelivery = isFree;
            if (cart.CashOnDelivery)
            {
                parentOrder.PaymentTypeId = _context.PaymentType.GetByLabel(Enum_PaymentType.PLACE).ID;
            }
            else
            {
                parentOrder.PaymentTypeId = _context.PaymentType.GetByLabel(Enum_PaymentType.ONLINE).ID;
            }
            parentOrder.Price = cart.PayementPrice;
            parentOrder.EarnestRemainingPrice = cart.EarnestRemainingPrice;
            parentOrder.RebateId = cart.RebateId;
            parentOrder.RebatePrice = cart.RebatePrice;
            parentOrder.StatusId = _context.Code.GetIdByLabel(Enum_Code.ORDER_STATUS_INSERTED);
            parentOrder.UniqueId = _context.AccountOrder.GetUniqueId();
            _context.AccountOrder.Insert(parentOrder);
            _context.Save();
            string description = "";
            foreach (var storeitem in cart.CartStore)
            {
                var childOrder = new AccountOrder();
                childOrder.ParentId = parentOrder.ID;
                childOrder.AccountId = parentOrder.AccountId;
                childOrder.AddressId = parentOrder.AddressId;
                childOrder.Datetime = parentOrder.Datetime;
                childOrder.StatusId = parentOrder.StatusId;
                childOrder.PaymentTypeId = parentOrder.PaymentTypeId;
                childOrder.UniqueId = parentOrder.UniqueId + "-" + storeitem.StoreId;
                childOrder.DeliveryId = storeitem.DeliveryId;
                childOrder.DeliveryPrice = storeitem.DeliveryPrice;
                childOrder.StoreId = storeitem.StoreId;
                childOrder.RebateId = storeitem.RebateId;
                childOrder.RebatePrice = storeitem.RebatePrice;
                childOrder.DiscountPrice = storeitem.Discount;
                childOrder.Price = storeitem.PayementPrice;
                childOrder.EarnestRemainingPrice = storeitem.EarnestRemainingPrice;
                childOrder.BenefitPercent = storeitem.Store.BenefitPercent;
                _context.AccountOrder.Insert(childOrder);
                _context.Save();

                foreach (var item in storeitem.CartStoreItem)
                {
                    var orderproduct = new AccountOrderProduct();
                    description += item.Description;
                    orderproduct.OrderId = childOrder.ID;
                    orderproduct.StoreProductQuantityId = item.StoreProductQuantityId;
                    orderproduct.ProductId = item.StoreProductQuantity.StoreProduct.ProductId;
                    orderproduct.Count = item.Count;
                    orderproduct.Datetime = childOrder.Datetime;
                    orderproduct.ProductPrice = item.SingleBasePrice;
                    orderproduct.ProductDiscount = item.SingleDiscount;
                    orderproduct.Price = item.TotalPrice;
                    orderproduct.SingleEarnestRemainingPrice = item.SingleEarnestRemainingPrice;
                    orderproduct.TotalEarnestRemainingPrice = item.TotalEarnestRemainingPrice;
                    _context.AccountOrderProduct.Insert(orderproduct);
                    _context.Save();
                }
            }


            PaymentSubject subjectObj = _context.PaymentSubject.GetByLabel(Enumarables.Enum_PaymentSubject.ORDER);
            string paymentDescription = string.Format(Resource.Pattern.OrderDescription, parentOrder.UniqueId);
            Code statusEntity = _context.Code.GetByLabel(Enumarables.Enum_Code.PAYMENT_STATUS_INSERTED);
            Payment paymentObj = _context.Payment.CreatePayment(accountId: parentOrder.AccountId, userId: null, subjectId: subjectObj.ID, price: parentOrder.Price, merchantId: cart.MerchantId, description: paymentDescription, payment_status: statusEntity.Label);
            _context.PaymentProductOrder.Insert(new PaymentProductOrder()
            {
                PaymentId = paymentObj.ID,
                OrderId = parentOrder.ID,
            });
            parentOrder.Description = description;
            _context.AccountOrder.Update(parentOrder);
            _context.Save();
            result.Model = new AccountOrderVm();
            result.Model.Payment = paymentObj.Map<Payment, PaymentVm>();
            result.Model.PaymentTypeId = parentOrder.PaymentTypeId;
            result.Status = ResultStatus.Success;
            return result;
        }
        public static void CleanCart(UnitOfWork _context)
        {
            var currentAccount = GetCurrentCartAccount();
            var cart = _context.Cart.GetCartEntity(accountId: currentAccount.AccountId);
            if (cart != null)
            {
                foreach (var store in cart.CartStore.ToList())
                {
                    foreach (var item in store.CartStoreItem.ToList())
                    {
                        _context.CartStoreItem.Delete(item);
                    }
                    _context.CartStore.Delete(store);
                }
                _context.Save();
                Update(_context, cart);
            }
        }
        public static void DoPaymentCallBack(UnitOfWork _context, Payment payment)
        {
            HttpContext.Current.Response.Cookies[".intercode"].Expires = DateTime.Now.AddDays(-1);

            if (payment.PaymentSubject.Label == Enum_PaymentSubject.ORDER.ToString())
            {
                PaymentProductOrder productOrder = payment.PaymentProductOrder.FirstOrDefault();
                AccountOrder parentOrder = null;
                string smstoken_1 = payment.RefNumber;
                string smstoken_2 = "";
                string smstoken_3 = "";
                string smsbody = "";
                if (productOrder != null)
                {
                    parentOrder = productOrder.AccountOrder;
                    parentOrder.StatusId = _context.Code.GetIdByLabel(Enum_Code.ORDER_STATUS_SUCCESS);
                    parentOrder.Datetime = DateTime.Now;
                    //SendSmsAdmin
                    smsbody = string.Format(Resource.Pattern.SmsAdminOrderSuccess, BaseContent.WebsiteDetail.Title, payment.RefNumber);
                    BaseSms.SaveNewSms(_context, BaseContent.WebsiteDetail.Mobile, Enum_SmsType.PAYMENT_SUCCESS, smsbody, smstoken_1, smstoken_2, smstoken_3);
                    //SendSmsCustomer
                    smsbody = string.Format(Resource.Pattern.SmsCustomerOrderSuccess, BaseContent.WebsiteDetail.Title, payment.RefNumber);
                    if (parentOrder.Account == null)
                    {
                        var ACCOUNT = _context.Account.GetById(parentOrder.AccountId);
                        BaseSms.SaveNewSms(_context, ACCOUNT.Mobile, Enum_SmsType.PAYMENT_SUCCESS, smsbody, smstoken_1, smstoken_2, smstoken_3);

                    }
                    else
                    {
                        BaseSms.SaveNewSms(_context, parentOrder.Account.Mobile, Enum_SmsType.PAYMENT_SUCCESS, smsbody, smstoken_1, smstoken_2, smstoken_3);

                    }
                    var emailBody = string.Format(Resource.Pattern.SmsCustomerOrderSuccess, BaseContent.WebsiteDetail.Title, payment.RefNumber);
                    //BaseSms.SaveNewSms(_context, parentOrder.Account.Mobile, Enum_SmsType.PAYMENT_SUCCESS, smsbody, smstoken_1, smstoken_2, smstoken_3);
                    //_context.Email.SaveNewEmail(parentOrder.AccountAddress.Account.Email, Enum_EmailType.PAYMENT_SUCCESS, emailBody, "پرداخت موفقیت آمیز");
                    //_context.Save();
                    foreach (var storeOrder in parentOrder.AccountOrder1)
                    {
                        storeOrder.StatusId = parentOrder.StatusId;
                        //SendSmsStore
                        smsbody = string.Format(Resource.Pattern.SmsStoreOrderSuccess, BaseContent.WebsiteDetail.Title, payment.RefNumber);
                        BaseSms.SaveNewSms(_context, storeOrder.Store.Phone1, Enum_SmsType.PAYMENT_SUCCESS_Store, smsbody, smstoken_1, smstoken_2, smstoken_3);

                        foreach (AccountOrderProduct orderProduct in storeOrder.AccountOrderProduct)
                        {
                            var quantity = _context.StoreProductQuantity.GetById(orderProduct.StoreProductQuantityId);
                            if (quantity != null)
                            {
                                quantity.Count = quantity.Count - orderProduct.Count;
                                if (quantity.Count < 0)
                                    quantity.Count = 0;
                                _context.StoreProductQuantity.Update(quantity);
                                _context.Save();
                                Base.BaseStore.UpdateProductQuantity(quantity.ProductQuantity.ProductId);
                            }
                        }
                    }
                    CleanCart(_context);
                }
            }
        }
        public static void DoPaymentCallBack2(UnitOfWork _context, PaymentCertificate payment)
        {

            //SendSmsAdmin
            string smstoken_1 = payment.RefNumber;
            string smstoken_2 = "";
            string smstoken_3 = "";
            string smsbody = "";
            smsbody = string.Format(Resource.Pattern.SmsAdminOrderSuccess, BaseContent.WebsiteDetail.Title, payment.RefNumber);
            BaseSms.SaveNewSms(_context, BaseContent.WebsiteDetail.Mobile, Enum_SmsType.PAYMENT_SUCCESS, smsbody, smstoken_1, smstoken_2, smstoken_3);
            //SendSmsCustomer
            smsbody = string.Format(Resource.Pattern.SmsCustomerOrderSuccess, BaseContent.WebsiteDetail.Title, payment.RefNumber);
            if (payment.Account == null)
            {
                var ACCOUNT = _context.Account.GetById(payment.AccountId);
                BaseSms.SaveNewSms(_context, ACCOUNT.Mobile, Enum_SmsType.PAYMENT_SUCCESS, smsbody, smstoken_1, smstoken_2, smstoken_3);

            }
            else
            {
                BaseSms.SaveNewSms(_context, payment.Account.Mobile, Enum_SmsType.PAYMENT_SUCCESS, smsbody, smstoken_1, smstoken_2, smstoken_3);

            }

        }
        public static void DoPaymentCallBack3(UnitOfWork _context, PaymentExame payment)
        {

            //SendSmsAdmin
            string smstoken_1 = payment.RefNumber;
            string smstoken_2 = "";
            string smstoken_3 = "";
            string smsbody = "";
            var paymentCourse = _context.PaymentExameCourseId.Where(s => s.PaymentExameId == payment.ID);
            if (paymentCourse.Any())
            {
                var exameAccountAnswer = _context.ExameAccountAnswer.GetById(payment.AccountAnswerId);

                foreach (var item in paymentCourse)
                {
                    var courseId = "-" + item.CourseId.ToString() + "-";
                    exameAccountAnswer.QuestionAnswer = exameAccountAnswer.QuestionAnswer.Replace(courseId, "-");

                }
                exameAccountAnswer.Finish = null;
                exameAccountAnswer.Open = true;
                _context.ExameAccountAnswer.Update(exameAccountAnswer);
                _context.Save();
            }
            else
            {
                var exameAccountAnswer = _context.ExameAccountAnswer.GetById(payment.AccountAnswerId);
                var courseId = "-" + payment.CourseId.ToString() + "-";
                exameAccountAnswer.QuestionAnswer = exameAccountAnswer.QuestionAnswer.Replace(courseId, "-");
                exameAccountAnswer.Finish = null;
                exameAccountAnswer.Open = true;
                _context.ExameAccountAnswer.Update(exameAccountAnswer);
                _context.Save();
            }
          
            smsbody = string.Format(Resource.Pattern.SmsAdminOrderSuccess, BaseContent.WebsiteDetail.Title, payment.RefNumber);
            BaseSms.SaveNewSms(_context, BaseContent.WebsiteDetail.Mobile, Enum_SmsType.PAYMENT_SUCCESS, smsbody, smstoken_1, smstoken_2, smstoken_3);
            //SendSmsCustomer
            smsbody = string.Format(Resource.Pattern.SmsCustomerOrderSuccess, BaseContent.WebsiteDetail.Title, payment.RefNumber);
            if (payment.Account == null)
            {
                var ACCOUNT = _context.Account.GetById(payment.AccountId);
                BaseSms.SaveNewSms(_context, ACCOUNT.Mobile, Enum_SmsType.PAYMENT_SUCCESS, smsbody, smstoken_1, smstoken_2, smstoken_3);

            }
            else
            {
                BaseSms.SaveNewSms(_context, payment.Account.Mobile, Enum_SmsType.PAYMENT_SUCCESS, smsbody, smstoken_1, smstoken_2, smstoken_3);

            }

        }
        public static ResultVm<CartVm> MergCart(UnitOfWork _context, CartUpdateVm cartUpdate)
        {
            var result = new ResultVm<CartVm>();
            var cart = new CartVm();
            if (cartUpdate.UpdateType == Enumarables.CartUpdateType.SetAccountId)
            {
                var getByAccountId = _context.Cart.FirstOrDefault(s => s.AccountId == cartUpdate.AccountId);
                var getBySyncCode = _context.Cart.FirstOrDefault(s => s.SyncCode == cartUpdate.SyncCode);
                if (getByAccountId != null && getBySyncCode != null)
                {
                    if (getByAccountId.ID != getBySyncCode.ID)
                    {
                        var items = getBySyncCode.CartStore.SelectMany(s => s.CartStoreItem);
                        foreach (var item in items)
                        {
                            CartUpdateVm AddcartUpdate = new CartUpdateVm() { UpdateType = CartUpdateType.Add, AccountId = cartUpdate.AccountId, Count = item.Count, StoreProductQuantityId = item.StoreProductQuantityId };
                            Update(_context: _context, AddcartUpdate);
                        }
                    }
                    cart.SyncCode = getByAccountId.SyncCode;
                }
                else if (getBySyncCode != null)
                {
                    getBySyncCode.AccountId = cartUpdate.AccountId;
                    _context.Cart.Update(getBySyncCode);
                    _context.Save();
                    cart.SyncCode = getBySyncCode.SyncCode;

                }

            }
            result.Model = cart;
            return result;
        }

    }
}
