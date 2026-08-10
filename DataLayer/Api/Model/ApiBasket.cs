using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.ViewModels;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Api.Model
{
    public class ApiBasket : ApiResponse
    {


        public static ApiResult Get(UnitOfWork _context,  int? accountId, AccountAddress address = null)
        {
            _context.AccountBasket.DeleteNotAvailableItems(_context, accountId);

            object model;
            if (Base.BaseWebsite.WebsiteSetting.HasStore)
            {
                double price,discount;
                model = Base.BaseBasket.GetAccountBasketStore(_context: _context,price: out price,discount: out discount, accountId: accountId,address:address);
            }
            else
            {
                model = BaseBasket.GetAccountBasket(_context: _context, accountId: accountId);
            }
               
                return CreateSuccessResult(model);
           
        }

        public static ApiResult Post(UnitOfWork _context, int? accountId, int productId, int? colorId = null, int? sizeId = null, int? optionItemId = null, int? resellerId = null, int? storeId = null, bool exteraResult = false,int count=1, AccountAddress address = null)
        {
            //accountId = 3161;
            //int ? test = storeId;
            List<AccountBasket> list = new List<AccountBasket>();
            Product product = _context.Product.GetById(productId);
            if (product.Active == false)
                return CreateErrorResult(Enum_Message.INVALID_PRODUCT_ACTIVE);
            bool isAvailable = _context.AccountBasket.AddToBasket(listOutput: out list, product: product, accountId: accountId, colorId: colorId, sizeId: sizeId, optionItemId: optionItemId, resellerId: resellerId, storeId: storeId,count: count);
            if (isAvailable == false)
            {
                if (colorId != null && sizeId != null)
                    return CreateErrorResult(Enum_Message.INVALID_PRODUCT_QUANTITY_COLOR_SIZE);
                else if (colorId != null)
                    return CreateErrorResult(Enum_Message.INVALID_PRODUCT_QUANTITY_COLOR);
                else if (sizeId != null)
                    return CreateErrorResult(Enum_Message.INVALID_PRODUCT_QUANTITY_SIZE);
                else
                    return CreateErrorResult(Enum_Message.LastAddToBasket);
            }
            
            var res = Get(_context: _context,accountId: accountId, address);
            if (storeId != null)
            {
                var model = _context.ShippingCity.GetShippingPrice(storeId: storeId,address);
                var listtemp = _context.AccountBasket.GetAllSetOutOfSendStoreFromCookie();
                if (model.Id == 0 || listtemp.Count() != 0)
                {
                    if (address == null)
                    {
                        return CreateErrorResult(Enum_Message.OutOfSend_Store);
                    }
                   
                   
                }

            }
            return CreateSuccessResult(Enum_Message.SUCCESSFULL_ADD_BASKET, res.Value);



        }
        public static ApiResult Delete(UnitOfWork _context, int? accountId = null)
        {
            _context.AccountBasket.ClearBasket(accountId);
            _context.Save();

            List<AccountBasket> list = new List<AccountBasket>();
            return CreateSuccessResult(Enum_Message.SUCCESSFULL_DELETE_BASKET, list.ToApi());
        }

        public static ApiResult Delete(UnitOfWork _context, int accountId, int basketId, AccountAddress address = null)
        {
            AccountBasket basket = _context.AccountBasket.GetById(basketId);
            if (basket == null)
                return CreateErrorResult(Enum_Message.INVALID_DATA);
            if (basket.AccountId != accountId)
                return CreateInvalidKeyResult();
            _context.AccountBasket.Delete(basket);
            _context.Save();
            var res = Get(_context: _context,accountId: accountId,address: address);
            return CreateSuccessResult(Enum_Message.SUCCESSFULL_DELETE_BASKET, res.Value);
        }

        public static ApiResult Delete(UnitOfWork _context, int productId, int? colorId, int? sizeId, int? optionItemId = null, int? storeId = null, bool exteraResult = false)
        {
            List<AccountBasket> listBasket = _context.AccountBasket.GetAllFromCookie(false);
            AccountBasket basket = listBasket.FirstOrDefault(p =>
                p.ProductId == productId &&
                p.ProductColorId == colorId &&
                p.ProductSizeId == sizeId &&
                p.ProductOptionItemId == optionItemId &&
                p.StoreId == storeId
            );
            if (basket == null)
                return CreateErrorResult(Enum_Message.INVALID_DATA);

            int basketIndex = listBasket.FindIndex(p =>
                            p.ProductId == basket.ProductId &&
                            p.ProductColorId == basket.ProductColorId &&
                            p.ProductSizeId == basket.ProductSizeId &&
                            p.StoreId == basket.StoreId);

            listBasket.RemoveAt(basketIndex);
            _context.AccountBasket.SetCookieBasket(listBasket);
            listBasket = _context.AccountBasket.CastCookieBasket(listBasket);
            return CreateSuccessResult(Enum_Message.SUCCESSFULL_DELETE_BASKET, listBasket.ToApi());
        }



        public static ApiResult Put(UnitOfWork _context, string actionType, int? accountId = null, int? basketId = null, int? productId = null, int? colorId = null, int? sizeId = null, int? optionItemId = null, int? storeId = null, bool exteraResult = false, AccountAddress address=null)
        {
            AccountBasket basket = new AccountBasket();
            List<AccountBasket> listBasket = new List<AccountBasket>();
            int basketIndex = 0;
            if (basketId != null)
            {
                basket = _context.AccountBasket.GetById(basketId);
                if (basket == null)
                    return CreateErrorResult(Enum_Message.INVALID_DATA);
                if (basket.AccountId != accountId)
                    return CreateInvalidKeyResult();
            }
            else
            {
                listBasket = _context.AccountBasket.GetAllFromCookie();
                basket = listBasket.FirstOrDefault(p =>
                    p.ProductId == productId &&
                    p.ProductColorId == colorId &&
                    p.ProductSizeId == sizeId &&
                    p.ProductOptionItemId == optionItemId &&
                    p.StoreId == storeId
                );
                if (basket == null)
                    return CreateErrorResult(Enum_Message.INVALID_DATA);
                basketIndex = listBasket.FindIndex(p =>
               p.ProductId == basket.ProductId &&
               p.ProductColorId == basket.ProductColorId &&
               p.ProductSizeId == basket.ProductSizeId &&
               p.ProductOptionItemId == basket.ProductOptionItemId &&
               p.StoreId == basket.StoreId
               );
            }
            if (basket.Count == 1 && actionType == "remove")
                return CreateSuccessResult(Enum_Message.SUCCESSFULL_API);

            if (basket.StoreId == null)
            {
                //if (basket.ProductColorId != null || basket.ProductSizeId != null || basket.ProductOptionItemId != null)
                //{
                ProductQuantity quantity = _context.ProductQuantity.FirstOrDefault(p =>
                    p.ColorId == basket.ProductColorId &&
                    p.SizeId == basket.ProductSizeId &&
                    p.OptionItemId == basket.ProductOptionItemId &&
                    p.ProductId == basket.ProductId
                );

                if (quantity != null)
                {
                    if (Base.BaseWebsite.WebsiteSetting.ProductMaxOrderCount && quantity.Product.MaxOrderCount.HasValue)
                    {
                        if (actionType == "add" && (basket.Count + 1) > quantity.Product.MaxOrderCount.Value)
                        {
                            return CreateErrorResult(Enum_Message.ERROR_BASKET_QUANTITY);

                        }
                    }
                    if (actionType == "add" && quantity.Count < basket.Count + 1)
                    {

                        return CreateErrorResult(Enum_Message.ERROR_BASKET_QUANTITY);
                    }
                    else
                    {
                        basket.Count = (actionType == "add") ? basket.Count + 1 : basket.Count - 1;
                    }
                    basket.Price = (basket.Count * quantity.GetDiscountPrice());
                }
                else
                    basket.Price = (basket.Count * basket.Product.GetDiscountPrice());
                //}
                //else
                //    basket.Price = (basket.Count * basket.Product.GetDiscountPrice());
            }
            else
            {
                var storeProductQuantity = _context.StoreProductQuantity.FirstOrDefault(p =>
                    p.ProductQuantity.ProductId == basket.Product.ID &&
                    p.ProductQuantity.ColorId == basket.ProductColorId &&
                    p.ProductQuantity.SizeId == basket.ProductSizeId &&
                    p.ProductQuantity.OptionItemId == basket.ProductOptionItemId &&
                    p.StoreProduct.StoreId == basket.StoreId
                );
                if (storeProductQuantity != null)
                {
                    if (Base.BaseWebsite.WebsiteSetting.ProductMaxOrderCount && storeProductQuantity.StoreProduct.Product.MaxOrderCount.HasValue)
                    {
                        if (actionType == "add" && (basket.Count + 1) > storeProductQuantity.StoreProduct.Product.MaxOrderCount.Value)
                        {
                            return CreateErrorResult(Enum_Message.ERROR_BASKET_QUANTITY);

                        }
                    }
                    if (actionType == "add" && storeProductQuantity.Count < basket.Count + 1)
                    {
                        return CreateErrorResult(Enum_Message.ERROR_BASKET_QUANTITY);
                    }
                    else
                    {
                        basket.Count = (actionType == "add") ? basket.Count + 1 : basket.Count - 1;
                    }

                  var product_price = storeProductQuantity.GetFromPriceRange(basket.Count);
                   var discount_price = storeProductQuantity.GetDiscountPrice(basket.Count);
                  var price = product_price - discount_price;
                   

                    basket.ProductPrice = product_price;
                    basket.ProductDiscount = discount_price;
                    basket.Price = price * basket.Count;
                }
                else
                {
                    return CreateErrorResult(Enum_Message.INVALID_DATA);
                }
            }
            if (basketId != null)
            {
                _context.AccountBasket.Update(basket);
                _context.Save();
                var res = Get(_context: _context,accountId: accountId.GetValueOrDefault(), address);
                return CreateSuccessResult(Enum_Message.SUCCESSFULL_API, res.Value);
            }
            else
            {
                listBasket[basketIndex] = basket;
                _context.AccountBasket.SetCookieBasket(listBasket);
                listBasket = _context.AccountBasket.CastCookieBasket(listBasket);
                return CreateSuccessResult(Enum_Message.SUCCESSFULL_API, listBasket.ToApi());
            }


        }
    }
}
