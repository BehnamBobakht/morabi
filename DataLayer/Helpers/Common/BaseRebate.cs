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
    public static class BaseRebate
    {

        public static ResultVm<RebateVm> GetRebate(UnitOfWork _context, Cart cart, string code)
        {
            var rebateVm = _context.Rebate.GetByCode(code: code);
            if (rebateVm.Status != ResultStatus.Success)
            {
                return rebateVm;
            }
            else
            {
                //var cart = BaseCart.GetCurrentCard(_context: _context, accountId: accountId);
                if (rebateVm.Model.StoreId == null)
                {
                    if (cart.BasePrice < 1)
                    {
                        rebateVm.Status = ResultStatus.Error;
                        rebateVm.Message = Resource.Notify.UnspecifiedError;
                        rebateVm.Model = null;
                    }
                    else if (cart.BasePrice < rebateVm.Model.MinInvoiceValue)
                    {
                        rebateVm.Status = ResultStatus.Error;
                        rebateVm.Message = Resource.Notify.YourPurchaseAmountHasNotReachedTheQuorum;
                    }

                    else
                    {
                        if (rebateVm.Model.IsPriceRebate)
                        {
                            if (rebateVm.Model.PriceValue >= cart.BasePrice)
                            {
                                rebateVm.Status = ResultStatus.Error;
                                rebateVm.Message = Resource.Notify.YourPurchaseAmountHasNotReachedTheQuorum;
                            }
                            else
                            {
                                rebateVm.Model.RebateFinalValue = rebateVm.Model.PriceValue;
                                rebateVm.Message = Resource.Notify.OperationSuccessful;

                                rebateVm.Status = ResultStatus.Success;
                            }

                        }
                        else
                        {
                            rebateVm.Model.RebateFinalValue = Math.Round(cart.BasePrice * (((double)rebateVm.Model.PercentValue) / 100), 0);
                            rebateVm.Message = Resource.Notify.OperationSuccessful;

                            rebateVm.Status = ResultStatus.Success;
                        }

                    }
                }
                else
                {
                    var cartStore = cart.CartStore.FirstOrDefault(s => s.StoreId == rebateVm.Model.StoreId);
                    if (cartStore == null)
                    {
                        rebateVm.Status = ResultStatus.Error;
                        rebateVm.Message = Resource.Notify.YouHaveNotPurchasedThisDiscountCodeFromTheStore;
                    }
                    else if (cartStore.BasePrice < rebateVm.Model.MinInvoiceValue)
                    {
                        rebateVm.Status = ResultStatus.Error;
                        rebateVm.Message = Resource.Notify.YourPurchaseAmountHasNotReachedTheQuorum;
                    }
                    else
                    {
                        if (rebateVm.Model.IsPriceRebate)
                        {

                            if (rebateVm.Model.PriceValue >= cartStore.BasePrice)
                            {
                                rebateVm.Status = ResultStatus.Error;
                                rebateVm.Message = Resource.Notify.YourPurchaseAmountHasNotReachedTheQuorum;
                            }
                            else
                            {
                                rebateVm.Model.RebateFinalValue = rebateVm.Model.PriceValue;
                                rebateVm.Message = Resource.Notify.OperationSuccessful;
                                rebateVm.Status = ResultStatus.Success;
                            }
                        }
                        else
                        {
                            rebateVm.Model.RebateFinalValue = Math.Round(cartStore.BasePrice * ((double)rebateVm.Model.PercentValue / 100), 0);
                            if (rebateVm.Model.RebateFinalValue > rebateVm.Model.MaxRebatePrice)
                            {
                                rebateVm.Model.RebateFinalValue = rebateVm.Model.MaxRebatePrice;
                                rebateVm.Message = Resource.Notify.OperationSuccessful;
                                rebateVm.Status = ResultStatus.Success;
                            }
                        }

                    }
                }

            }
            return rebateVm;
        }

    }
}
