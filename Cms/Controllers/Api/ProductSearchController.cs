using DataLayer.Api;
using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cms.Controllers.Api
{

    public class ProductSearchController : ApiController
    {
        UnitOfWork _context = new UnitOfWork();

        [HttpGet]

        public ResultApi Search(
            int pageSize = 40,
            int pageIndex = 1,
            string prId = null,
            int? notId = null,
            string typeId = null,
            string categoryId = null,
            string subCategoryId = null,
            string colorId = null,         
             string customFieldId = null,
            string sizeId = null,
            string option = null,
            double? pricefrom = null,
            double? priceto = null,
            string brandId = null,
            string storeId = null,
            string prname = null,
            string summery = null,
            bool? hasDiscount = null,
            bool? active = true,
            bool? isService = null,
            string latitude = null,
            string longitude = null,
            int distance = 0,
            string SyncNameProductCustomField = null,
            string valueProductCustom = null,
            bool? prstock = null,
            string prcustomlabel = "all",
            Enum_ProductOrder prorder = Enum_ProductOrder.SHOWNUMBER
            )
       {
  
            var result = new ProductSearchVm();
            result = _context.Product.SearchVm(
                                    Id: prId,
                                    notId: notId,
                                    index: pageIndex,
                                    pageSize: pageSize,
                                    categoryId: categoryId,
                                    hasDiscount: hasDiscount,
                                    name: prname,
                                    summery: summery,
                                    pricefrom: pricefrom,
                                    priceto: priceto,
                                    brandId: brandId,
                                    storeId: storeId,
                                    colorId: colorId,
                                    customFieldId: customFieldId,
                                    sizeId: sizeId,
                                    subCategoryId: subCategoryId,
                                    typeId: typeId,
                                    active: active,
                                    option: option,
                                    isService: isService,
                                    latitude: latitude,
                                    longitude: longitude,
                                    distance: distance,
                                    prstock: prstock,
                                    order: prorder,
                                    prcustomLabel: prcustomlabel,
                                    SyncNameProductCustomField: SyncNameProductCustomField,
                                    valueProductCustom: valueProductCustom);
            result.PageSize = pageSize;
            result.PageIndex = pageIndex;
            result.Name = prname;
            result.BrandId = brandId;
            result.ColorId = colorId;

            result.PriceFrom = pricefrom;
            result.PriceTo = priceto;
            if (result.PriceTo == null || result.PriceTo == 0)
            {
                result.PriceTo = result.MaxPrice;
            }
            result.Prstock = prstock;
            result.prorder = prorder;
            result.PrcustomLabel = prcustomlabel;
            result.HasDiscount = hasDiscount;
            result = BaseStore.GetCategoriesByCustomLabel(_context: _context, productSearch: result, customLabel: prcustomlabel);
            var apiResult = new ResultApi();
            if (result.IncorrectCustomLabel)
            {
                apiResult.Status = ResultStatus.Error;
                apiResult.Message = Resource.Notify.FailedOperation;
            }
            else
            {
                apiResult.Status = ResultStatus.Success;
                apiResult.Message = Resource.Notify.OperationSuccessful;
                apiResult.Value = result;
            }
            return apiResult;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
