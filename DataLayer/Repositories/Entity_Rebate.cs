using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.ViewModels;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Binbin.Linq;
using ExpressMapper.Extensions;

namespace DataLayer.Repositories
{
    public class Entity_Rebate : BaseRepository<Rebate>
    {
        private DatabaseEntities _context;
        public Entity_Rebate(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public ResultVm<RebateVm> GetByCode(string code)
        {
            var result = new ResultVm<RebateVm>();
            var rebate = FirstOrDefault(s => s.CodeValue == code && s.Active && s.Deleted == false);
            var today = DateTime.Now.Date;
            if (rebate == null)
            {
                result.Status = ResultStatus.Error;
                result.Message = Resource.Notify.TheRebateCodeIsWrong;
            }
            else if (rebate.UseCount > 0 && rebate.UsedCount >= rebate.UseCount)
            {
                result.Status = ResultStatus.Error;
                result.Message = Resource.Notify.TheCountOfDiscountCodeHasBeenCompleted;
            }
            else if (rebate.StartDatetime.HasValue && rebate.StartDatetime.Value.Date > today)
            {
                result.Status = ResultStatus.Error;
                result.Message = Resource.Notify.ItIsNotTimeToUseTheRebateCode;
            }
            else if (rebate.EndDatetime.HasValue && rebate.EndDatetime.Value.Date < today)
            {
                result.Status = ResultStatus.Error;
                result.Message = Resource.Notify.TheRebateCodeHasExpired;
            }
            if (result.Status == ResultStatus.Error)
            {
                return result;
            }
            result.Status = ResultStatus.Success;
            result.Model = rebate.Map<Rebate, RebateVm>();
            return result;
        }
        public ResultVm<RebateVm> Insert(RebateVm rebateVm)
        {
            var result = new ResultVm<RebateVm>();
            if (string.IsNullOrEmpty(rebateVm.Name))
            {
                result.Status = ResultStatus.Error;
                result.Message = Resource.Notify.PleaseInsertTheName;
            }         
            else if (string.IsNullOrEmpty(rebateVm.CodeValue))
            {
                result.Status = ResultStatus.Error;
                result.Message = Resource.Notify.PleaseInsertTheCode;
            } 
            else if (!CodeIsUnique(rebateVm.CodeValue))
            {
                result.Status = ResultStatus.Error;
                result.Message = Resource.Notify.RebateCodeIsDuplicate;
            }
            else if (rebateVm.IsPriceRebate && rebateVm.PriceValue < 1)
            {
                result.Status = ResultStatus.Error;
                result.Message = Resource.Notify.PleaseInsertThePrice;
            }
            else if (rebateVm.IsPriceRebate == false && rebateVm.PercentValue < 1)
            {
                result.Status = ResultStatus.Error;
                result.Message = Resource.Notify.PleaseInsertThePercent;
            }
            else if (rebateVm.StartDatetime.HasValue && rebateVm.EndDatetime.HasValue && rebateVm.StartDatetime.Value > rebateVm.EndDatetime.Value)
            {
                result.Status = ResultStatus.Error;
                result.Message = Resource.Notify.PleaseInsertThePercent;
            }
            if (result.Status == ResultStatus.Error)
            {
                return result;
            }
            var model = rebateVm.Map<RebateVm, Rebate>();
            Insert(model);
            Save();
            result.Status = ResultStatus.Success;
            return result;
        }
        public bool CodeIsUnique(string code)
        {
            var item = FirstOrDefault(s => s.CodeValue == code && s.Deleted == false);
            if (item != null)
                return false;
            return true;
        }
        public void Delete(int id)
        {
            var model = FirstOrDefault(s => s.ID == id);
            if (model != null)
            {
                model.Deleted = true;
                Update(model);
                Save();
            }
        }
        public void Active(int id)
        {
            var model = FirstOrDefault(s => s.ID == id);
            if (model != null)
            {
                if (model.Active == true)
                    model.Active = false;
                else
                    model.Active = true;

                Update(model);
                Save();
            }
        }

        public dynamic GetRebateReport()
        {
            List<ViewReportRebate> query = _context.AccountOrder
                    .Where(p =>
                        p.Code.Label == Enum_Code.ORDER_STATUS_SUCCESS.ToString() ||
                        p.Code.Label == Enum_Code.ORDER_STATUS_PROCESS.ToString() ||
                        //p.Code.Label == Enum_Code.ORDER_STATUS_STORE.ToString() ||
                        p.Code.Label == Enum_Code.ORDER_STATUS_POST.ToString()
                    )
                    .GroupBy(p => p.Rebate.Name + "-" + p.Rebate.CodeValue)
                    .Select(g =>
                        new ViewReportRebate()
                        {
                            Name = g.Key,
                            OrderCount = g.Count(),
                            SumPrice = g.Sum(q => q.Price),
                            RebatePrice = g.Sum(q => q.RebatePrice)
                        })
                    .OrderByDescending(p => p.OrderCount)
                    .ToList();
            return query;
        }

        public Rebate GetFromRebateValue(string CodeValue)
        {
            return _context.Rebate.FirstOrDefault(p => p.CodeValue == CodeValue);
        }
        //public List<Rebate> GetFromRebateValue(double orderPrice, string rebateValue, out double rebatePrice, out Enum_Message rebateMessage)
        //{
        //    DateTime now = DateTime.Now;
        //    rebateMessage = Enum_Message.NONE;
        //    string status_order_success = Enum_Code.ORDER_STATUS_SUCCESS.ToString();
        //    string status_order_process = Enum_Code.ORDER_STATUS_PROCESS.ToString();
        //    string status_order_store = Enum_Code.ORDER_STATUS_STORE.ToString();
        //    string status_order_post = Enum_Code.ORDER_STATUS_POST.ToString();

        //    //if (order.RebateId != null)
        //    //{
        //    //    rebateMessage = Enum_Message.INVALID_ORDER_REBATE_BEFORE;
        //    //    rebatePrice = 0;
        //    //    return new List<Rebate>();
        //    //}

        //    List<Rebate> rebateFirstList = _context.Rebate.Where(p =>
        //        p.Active == true &&
        //        p.CodeValue == rebateValue
        //    ).ToList();

        //    List<Rebate> rebateList = new List<Rebate>();
        //    foreach (Rebate item in rebateFirstList)
        //    {
        //        if ((item.StartDatetime != null && item.StartDatetime >= now) ||
        //            (item.EndDatetime != null && item.EndDatetime <= now))
        //        {
        //            rebateMessage = Enum_Message.INVALID_REBATE_DATETIME;
        //            rebateList.Clear();
        //            break;
        //        }
        //        else if (item.UseCount != null &&
        //                 item.AccountOrder.Count(s =>
        //                    s.Code.Label == status_order_success ||
        //                    s.Code.Label == status_order_process ||
        //                    s.Code.Label == status_order_store ||
        //                    s.Code.Label == status_order_post) >= item.UseCount)
        //        {
        //            rebateMessage = Enum_Message.INVALID_REBATE_USECOUNT;
        //            rebateList.Clear();
        //            break;
        //        }
        //        else if (item.MinInvoiceValue != null && item.MinInvoiceValue > orderPrice)
        //        {
        //            rebateMessage = Enum_Message.INVALID_REBATE_MIN_INVOICE_VALUE;
        //            rebateList.Clear();
        //            break;
        //        }
        //        else if (item.MaxInvoiceValue != null && item.MaxInvoiceValue < orderPrice)
        //        {
        //            rebateMessage = Enum_Message.INVALID_REBATE_MAX_INVOICE_VALUE;
        //            rebateList.Clear();
        //            break;
        //        }
        //        //else if (string.IsNullOrEmpty(item.Mobile) == false && item.Mobile != order.Account.Mobile)
        //        //{
        //        //    rebateMessage = Enum_Message.INVALID_REBATE_MOBILE;
        //        //    rebateList.Clear();
        //        //    break;
        //        //}
        //        else
        //        {
        //            rebateList.Add(item);
        //        }
        //    }

        //    rebatePrice = 0;
        //    string shopRebate = Enum_Code.REBATE_TYPE_SHOP.ToString();

        //    if (rebateList.Count(p => p.Code.Label == shopRebate) == rebateList.Count && rebateList.Count > 1)
        //    {
        //        rebateList = new List<Rebate>() { rebateList.OrderByDescending(p => p.PercentValue).FirstOrDefault() };
        //    }

        //    foreach (Rebate rebate in rebateList)
        //    {
        //        double order_base_price = orderPrice;
        //        //if (rebate.Code.Label == Enum_Code.REBATE_TYPE_SHOP.ToString())
        //        //{
        //        //    if (rebate.IsFinalPercentPrice == true)
        //        //        order_base_price = order.AccountOrderProduct.Where(p => p.Product.ShopId == rebate.ShopId).Sum(p => (p.ProductPrice * p.Count)).GetValueOrDefault();
        //        //    else
        //        //        order_base_price = order.AccountOrderProduct.Where(p => p.Product.ShopId == rebate.ShopId).Sum(p => p.Price);
        //        //}
        //        //else if (rebate.Code.Label == Enum_Code.REBATE_TYPE_PRODUCTTYPE.ToString())
        //        //    order_base_price = order.AccountOrderProduct.Where(p => p.Product.ProductTypeId == rebate.ProductTypeId).Sum(p => p.Price);
        //        //else if (rebate.Code.Label == Enum_Code.REBATE_TYPE_PRODUCTCATEGORY.ToString())
        //        //    order_base_price = order.AccountOrderProduct.Where(p => p.Product.ProductCategoryId == rebate.ProductCategoryId).Sum(p => p.Price);
        //        //else if (rebate.Code.Label == Enum_Code.REBATE_TYPE_PRODUCTSUBCATEGORY.ToString())
        //        //    order_base_price = order.AccountOrderProduct.Where(p => p.Product.ProductSubCategoryId == rebate.ProductSubCategoryId).Sum(p => p.Price);
        //        //else if (rebate.Code.Label == Enum_Code.REBATE_TYPE_PRODUCTBRAND.ToString())
        //        //    order_base_price = order.AccountOrderProduct.Where(p => p.Product.BrandId == rebate.ProductBrandId).Sum(p => p.Price);

        //        if (rebate.IsPriceRebate)
        //        {
        //            if (order_base_price < rebate.PriceValue.GetValueOrDefault())
        //                rebatePrice = rebatePrice + order_base_price;
        //            else
        //                rebatePrice = rebatePrice + rebate.PriceValue.GetValueOrDefault();
        //        }
        //        else
        //        {
        //            double temp_1 = order_base_price / 100;
        //            rebatePrice = rebatePrice + (temp_1 * rebate.PercentValue.GetValueOrDefault());
        //        }

        //        //if (rebate.IsFinalPercentPrice == true)
        //        //{
        //        //    double sumDiscount = order.AccountOrderProduct.Where(p => p.Product.ShopId == rebate.ShopId).Sum(p => p.ProductDiscount).GetValueOrDefault();
        //        //    sumDiscount = sumDiscount < 0 ? 0 : sumDiscount;
        //        //    rebatePrice = rebatePrice - sumDiscount;
        //        //}

        //        /*
        //        if (rebate.MaxRebatePrice != null && rebate.MaxRebatePrice != 0 && rebatePrice > rebate.MaxRebatePrice)
        //            rebatePrice = rebate.MaxRebatePrice.GetValueOrDefault();
        //        */
        //    }

        //    rebatePrice = rebatePrice < 0 ? 0 : rebatePrice;
        //    if(rebatePrice > 0)
        //    {
        //        rebateMessage = Enum_Message.SUCCESSFULL_REBATE_HAS;

        //    }
        //    return rebateList;
        //}

        //public List<Rebate> GetFromRebateValue(AccountOrder order, string rebateValue, out double rebatePrice, out Enum_Message rebateMessage)
        //{
        //    DateTime now = DateTime.Now;
        //    rebateMessage = Enum_Message.NONE;
        //    string status_order_success = Enum_Code.ORDER_STATUS_SUCCESS.ToString();
        //    string status_order_process = Enum_Code.ORDER_STATUS_PROCESS.ToString();
        //    string status_order_store = Enum_Code.ORDER_STATUS_STORE.ToString();
        //    string status_order_post = Enum_Code.ORDER_STATUS_POST.ToString();

        //    if (order.RebateId != null)
        //    {
        //        rebateMessage = Enum_Message.INVALID_ORDER_REBATE_BEFORE;
        //        rebatePrice = 0;
        //        return new List<Rebate>();
        //    }

        //    List<Rebate> rebateFirstList = _context.Rebate.Where(p =>
        //        p.Active == true &&
        //        p.CodeValue == rebateValue
        //    ).ToList();

        //    List<Rebate> rebateList = new List<Rebate>();
        //    foreach (Rebate item in rebateFirstList)
        //    {
        //        if ((item.StartDatetime != null && item.StartDatetime <= now) ||
        //            (item.EndDatetime != null && item.EndDatetime >= now))
        //        {
        //            rebateMessage = Enum_Message.INVALID_REBATE_DATETIME;
        //            rebateList.Clear();
        //            break;
        //        }
        //        else if (item.UseCount != null &&
        //                 item.AccountOrder.Count(s =>
        //                    s.Code.Label == status_order_success ||
        //                    s.Code.Label == status_order_process ||
        //                    s.Code.Label == status_order_store ||
        //                    s.Code.Label == status_order_post) >= item.UseCount)
        //        {
        //            rebateMessage = Enum_Message.INVALID_REBATE_USECOUNT;
        //            rebateList.Clear();
        //            break;
        //        }
        //        else if (item.MinInvoiceValue != null && item.MinInvoiceValue < order.Price)
        //        {
        //            rebateMessage = Enum_Message.INVALID_REBATE_MIN_INVOICE_VALUE;
        //            rebateList.Clear();
        //            break;
        //        }
        //        else if (item.MaxInvoiceValue != null && item.MaxInvoiceValue <= order.Price)
        //        {
        //            rebateMessage = Enum_Message.INVALID_REBATE_MAX_INVOICE_VALUE;
        //            rebateList.Clear();
        //            break;
        //        }
        //        else if (string.IsNullOrEmpty(item.Mobile) == false && item.Mobile != order.Account.Mobile)
        //        {
        //            rebateMessage = Enum_Message.INVALID_REBATE_MOBILE;
        //            rebateList.Clear();
        //            break;
        //        }
        //        else
        //        {
        //            rebateList.Add(item);
        //        }
        //    }

        //    rebatePrice = 0;
        //    string shopRebate = Enum_Code.REBATE_TYPE_SHOP.ToString();

        //    if (rebateList.Count(p => p.Code.Label == shopRebate) == rebateList.Count && rebateList.Count > 1)
        //    {
        //        rebateList = new List<Rebate>() { rebateList.OrderByDescending(p => p.PercentValue).FirstOrDefault() };
        //    }

        //    foreach (Rebate rebate in rebateList)
        //    {
        //        double order_base_price = 0;
        //        if (rebate.Code.Label == Enum_Code.REBATE_TYPE_SHOP.ToString())
        //        {
        //            if (rebate.IsFinalPercentPrice == true)
        //                order_base_price = order.AccountOrderProduct.Where(p => p.Product.ShopId == rebate.ShopId).Sum(p => (p.ProductPrice * p.Count)).GetValueOrDefault();
        //            else
        //                order_base_price = order.AccountOrderProduct.Where(p => p.Product.ShopId == rebate.ShopId).Sum(p => p.Price);
        //        }
        //        else if (rebate.Code.Label == Enum_Code.REBATE_TYPE_PRODUCTTYPE.ToString())
        //            order_base_price = order.AccountOrderProduct.Where(p => p.Product.ProductTypeId == rebate.ProductTypeId).Sum(p => p.Price);
        //        else if (rebate.Code.Label == Enum_Code.REBATE_TYPE_PRODUCTCATEGORY.ToString())
        //            order_base_price = order.AccountOrderProduct.Where(p => p.Product.ProductCategoryId == rebate.ProductCategoryId).Sum(p => p.Price);
        //        else if (rebate.Code.Label == Enum_Code.REBATE_TYPE_PRODUCTSUBCATEGORY.ToString())
        //            order_base_price = order.AccountOrderProduct.Where(p => p.Product.ProductSubCategoryId == rebate.ProductSubCategoryId).Sum(p => p.Price);
        //        else if (rebate.Code.Label == Enum_Code.REBATE_TYPE_PRODUCTBRAND.ToString())
        //            order_base_price = order.AccountOrderProduct.Where(p => p.Product.BrandId == rebate.ProductBrandId).Sum(p => p.Price);

        //        if (rebate.IsPriceRebate)
        //        {
        //            if (order_base_price < rebate.PriceValue.GetValueOrDefault())
        //                rebatePrice = rebatePrice + order_base_price;
        //            else
        //                rebatePrice = rebatePrice + rebate.PriceValue.GetValueOrDefault();
        //        }
        //        else
        //        {
        //            double temp_1 = order_base_price / 100;
        //            rebatePrice = rebatePrice + (temp_1 * rebate.PercentValue.GetValueOrDefault());
        //        }

        //        if (rebate.IsFinalPercentPrice == true)
        //        {
        //            double sumDiscount = order.AccountOrderProduct.Where(p => p.Product.ShopId == rebate.ShopId).Sum(p => p.ProductDiscount).GetValueOrDefault();
        //            sumDiscount = sumDiscount < 0 ? 0 : sumDiscount;
        //            rebatePrice = rebatePrice - sumDiscount;
        //        }

        //        /*
        //        if (rebate.MaxRebatePrice != null && rebate.MaxRebatePrice != 0 && rebatePrice > rebate.MaxRebatePrice)
        //            rebatePrice = rebate.MaxRebatePrice.GetValueOrDefault();
        //        */
        //    }

        //    rebatePrice = rebatePrice < 0 ? 0 : rebatePrice;

        //    return rebateList;
        //}

        public List<RebateVm> Search(int index = 1, int pageSize = 10, string name = null, string codeValue = null)
        {
            List<RebateVm> results = new List<RebateVm>();
            int skipValue = pageSize * (index - 1);
            int pageValue = pageSize;

            var MyQuery = GetSearchQuery(name: name, codeValue: codeValue);
            return _context.Rebate.Where(MyQuery)
                    .OrderByDescending(p => p.ID)
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList().Map<List<Rebate>, List<RebateVm>>();
        }

        public int SearchCount(string name = null, string codeValue = null)
        {
            var MyQuery = GetSearchQuery(
                name: name,
                codeValue: codeValue);
            return _context.Rebate.Count(MyQuery);
        }

        private Expression<Func<Rebate, bool>> GetSearchQuery(
            string name = null,
            string codeValue = null)
        {
            var MyQuery = PredicateBuilder.True<Rebate>();
            MyQuery = MyQuery.And(p => p.Deleted==false);

            if (IsNullOrEmptyId(name) == false)
            {
                MyQuery = MyQuery.And(p => p.Name.Contains(name));
            }
            if (IsNullOrEmptyId(codeValue) == false)
            {
                MyQuery = MyQuery.And(p => p.CodeValue.Contains(codeValue));
            }
            return MyQuery;
        }
    }
}
