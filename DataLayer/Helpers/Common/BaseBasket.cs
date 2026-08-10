using DataLayer.Entities;
using DataLayer.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.ViewModels;
using DataLayer.Api.Model;

namespace DataLayer.Base
{
    public class BaseBasket
    {
        public static ViewApiExtera GetAccountBasket(UnitOfWork _context, int? accountId = null, int? cityId = null, int? stateId = null, int? countyId = null)
        {
            var model = new ViewModels.ViewApiExtera();
            var producsList = new List<AccountBasket>();

            if (accountId.HasValue)
            {
                producsList = _context.AccountBasket.GetAllByAccount(accountId.Value);
            }
            else
            {
                producsList = _context.AccountBasket.GetAllFromCookie(isComplete: true);
            }
            var viewList = producsList.ToApi();
            model.List = viewList;
            model.Count = viewList.Sum(s => s.Count);
            model.TotalPrice = viewList.Sum(s => s.Price);
            model.TotalDiscount = viewList.Sum(s => s.Discount);
            return model;
        }
        public static ViewApiExtera GetAccountBasketStore(UnitOfWork _context, out double price, out double discount, int? accountId = null, AccountAddress address = null)
        {
            var model = new ViewApiExtera();
            var producsList = new List<AccountBasket>();
            var viewList = new List<ViewAccountBasketStore>();
            if (accountId.HasValue)
            {
                producsList = _context.AccountBasket.GetAllByAccount(accountId.Value);
            }
            else
            {
                producsList = _context.AccountBasket.GetAllFromCookie(isComplete: true);
            }
            viewList = producsList.ToApiStore(_context,address);

            if (BaseWebsite.WebsiteSetting.HasStore)
            {
                foreach (var item in viewList)
                {
                    if (item.ShippingCity == null || item.ShippingCity.SendPrice == null)
                    {
                        List<ViewProduct> list = new List<ViewProduct>();
                        foreach (var bsaket in item.Products)
                        {
                            if (accountId != null)
                            {
                                ApiBasket.Delete(_context, accountId.Value, bsaket.Id);
                               
                                list.Add(bsaket.Product);                               
                            }
                            else
                            {
                                ApiBasket.Delete(_context, int.Parse(bsaket.Product.Id), bsaket.ColorId, bsaket.SizeId,
                                    bsaket.OptionItemId, bsaket.StoreId);
                                list.Add(bsaket.Product);
                            }
                        }
                        _context.AccountBasket.SetOutOfSendStore(list);
                    }
                }
            }

            if (viewList != null)
            {
                if (accountId.HasValue)
                {
                    producsList = _context.AccountBasket.GetAllByAccount(accountId.Value);
                }
                else
                {
                    producsList = _context.AccountBasket.GetAllFromCookie(isComplete: true);
                }
                 viewList = producsList.ToApiStore(_context,address);
            }
            price = viewList.Sum(s => s.Price);
            discount = viewList.Sum(s => s.Discount);
            if (Base.BaseWebsite.WebsiteSetting.HasSendByPost)
            {
                var sumSendPrice = 0;
                foreach(var item in viewList)
                {
                    var sendStore = _context.Account.GetSendType(item.Store.ID);
                    if (sendStore == 2)
                    {

                        sumSendPrice += item.ShippingCity.SendPricePost.Value;
                    }
                    else if (sendStore == 3)
                    {
                        sumSendPrice += item.ShippingCity.SendPricePostP.Value;
                    }
                    else
                    {
                        sumSendPrice +=(int)(item.ShippingCity.SendPrice);
                    }
                }
                model.ShippingPrice = (double)(sumSendPrice);
            }
            else
            {
                model.ShippingPrice = double.Parse(viewList.Where(s => s.ShippingCity != null).Sum(s => s.ShippingCity.SendPrice).ToString());
            }

            model.List = viewList;
            model.TotalPrice = price;
            model.TotalDiscount = discount;
            model.Count = viewList.Sum(s => s.Count);
            return model;
        }


    }
}
