using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.ViewModels;
using DataLayer.Entities;
using DataLayer.Enumarables;
using ExpressMapper.Extensions;

namespace DataLayer.Base
{
    public static class BaseDiscount
    {

        public static StoreProductQuantityVm GetDiscount(int quantityId, UnitOfWork _context = null)
        {
            if (_context == null)
            {
                _context = new UnitOfWork();
            }
            var model = _context.StoreProductQuantity.GetById(quantityId);
            return model.GetDiscount(_context);
        }

        public static StoreProductQuantityVm GetDiscount(this StoreProductQuantity quantity, UnitOfWork _context = null)
        {
            if (_context == null)
            {
                _context = new UnitOfWork();
            }
            var discounts = _context.Discount.GetAllByStoreId(quantity);
            var currentDiscount = new Discount();
            if (discounts.Any())
            {
                if (discounts.Any(s => s.Code.Label == Enum_Code.DISCOUNT_TYPE_VARIENTY.ToString()))
                {
                    currentDiscount = discounts.FirstOrDefault(s => s.Code.Label == Enum_Code.DISCOUNT_TYPE_VARIENTY.ToString());
                }
                else if (discounts.Any(s => s.Code.Label == Enum_Code.DISCOUNT_TYPE_PRODUCT.ToString()))
                {
                    currentDiscount = discounts.FirstOrDefault(s => s.Code.Label == Enum_Code.DISCOUNT_TYPE_PRODUCT.ToString());
                }
                else if (discounts.Any(s => s.Code.Label == Enum_Code.DISCOUNT_TYPE_SUBCATEGORY.ToString()))
                {
                    currentDiscount = discounts.FirstOrDefault(s => s.Code.Label == Enum_Code.DISCOUNT_TYPE_SUBCATEGORY.ToString());
                }
                else if (discounts.Any(s => s.Code.Label == Enum_Code.DISCOUNT_TYPE_CATEGORY.ToString()))
                {
                    currentDiscount = discounts.FirstOrDefault(s => s.Code.Label == Enum_Code.DISCOUNT_TYPE_CATEGORY.ToString());
                }
                else if (discounts.Any(s => s.Code.Label == Enum_Code.DISCOUNT_TYPE_PRODUCTTYPE.ToString()))
                {
                    currentDiscount = discounts.FirstOrDefault(s => s.Code.Label == Enum_Code.DISCOUNT_TYPE_PRODUCTTYPE.ToString());
                }
                else if (discounts.Any(s => s.Code.Label == Enum_Code.DISCOUNT_TYPE_BRAND.ToString()))
                {
                    currentDiscount = discounts.FirstOrDefault(s => s.Code.Label == Enum_Code.DISCOUNT_TYPE_BRAND.ToString());
                }
                else if (discounts.Any(s => s.Code.Label == Enum_Code.DISCOUNT_TYPE_SHOP.ToString()))
                {
                    currentDiscount = discounts.FirstOrDefault(s => s.Code.Label == Enum_Code.DISCOUNT_TYPE_SHOP.ToString());
                }
                if (currentDiscount != null)
                {
                    var result = new StoreProductQuantityVm();
                        var discountView = GetDiscountPriceAndPercent(currentDiscount, quantity.Price);
                    result.DiscountValue = discountView.PriceValue.Value;
                    result.DiscountPercent = discountView.PercentValue.Value;
                    result.DiscountExpire = discountView.EndDatetime;
                    result.BasePrice = quantity.Price;
                    result.Price = result.BasePrice - result.DiscountValue;
                    if (discountView.GroupId.HasValue)
                        result.DiscountGroupId = discountView.GroupId;
                        result.DiscountGroup = discountView.Group.Map<ViewDiscountGroup,DiscountGroupVm>();
                    return result;
                }

            }
            return null;
        }
        public static ViewDiscount GetDiscountPriceAndPercent(Discount currentDiscount, double basePrice)
        {
            var result = new ViewDiscount() { PriceValue = 0, PercentValue = 0, EndDatetime = currentDiscount.EndDatetime };
            if (currentDiscount != null && currentDiscount.ValueTypeId != null)
            {
                if (currentDiscount.Code1.Label == Enumarables.Enum_Code.DISCOUNTGROUP_TYPE_PRICE.ToString() && currentDiscount.PriceValue.HasValue)
                {
                    result.PriceValue = currentDiscount.PriceValue.Value;
                    var percent = (result.PriceValue.Value * 100) / basePrice;
                    result.PercentValue = (int)Math.Ceiling(percent);
                }

                else if (currentDiscount.Code1.Label == Enumarables.Enum_Code.DISCOUNTGROUP_TYPE_PERCENT.ToString() && currentDiscount.PercentValue.HasValue)
                {

                    var discount = (basePrice * (double)((double)currentDiscount.PercentValue.Value / 100));
                    result.PriceValue = Math.Round(discount, 0);
                    result.PercentValue = currentDiscount.PercentValue.Value;
                }
                if(currentDiscount.GroupId.HasValue)
                {
                    result.GroupId = currentDiscount.GroupId;
                result.Group = new ViewDiscountGroup(currentDiscount.DiscountGroup);
                }
            }
            return result;
        }


    }
}
