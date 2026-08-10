using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Api
{
    public class ApiProduct : ApiResponse
    {
        //public static ApiResult Search(
        //    UnitOfWork _context,
        //    int? notId = null,
        //    int? index = null, 
        //    int? pageSize = null, 
        //    string shopId = null, 
        //    string typeId = null, 
        //    string categoryId = null,
        //    string colorId = null,
        //    string sizeId = null,
        //    string subCategoryId = null,
        //    string brandId = null,
        //    string name = null,
        //    string custom = null,
        //    int? pricefrom = null,
        //    int? priceto = null,
        //    bool? extra = true,
        //    string discount = null,
        //    Enum_ProductOrder order = Enum_ProductOrder.NONE,
        //    bool? isService = null,
        //     string latitude = null,
        //    string longitude = null,
        //    int distance = 0,
        //    bool exteraResult=false,
        //    int ? notProductId=null,
        //     string option = null,
        //    bool activeLocation = true, string stock = "ALL",
        //      string storeId = null
        //    )
        //{
        //    pageSize = pageSize == null ? 10 : pageSize;
        //    index = index == null ? 1 : index;
        //    List<Product> list = _context.Product.Search(
        //        notId: notId,
        //        shopId: shopId,
        //        typeId: typeId,
        //        categoryId: categoryId,
        //        brandId: brandId,
        //        subCategoryId: subCategoryId,
        //        index: index.Value,
        //        pageSize: pageSize.Value,
        //        name: name,
        //        custom: custom,
        //        pricefrom: pricefrom,
        //        priceto: priceto,
        //        order: order,
        //        discount: discount,
        //        isService:isService,
        //        latitude:latitude,
        //        longitude:longitude,
        //        distance:distance,
        //        colorId: colorId,
        //        sizeId: sizeId,
        //        option: option,
        //        prstock: stock,
        //        storeId: storeId,
        //        activeLocation:activeLocation );
        //    if(exteraResult)
        //    {
        //        var exteraModel = new ViewModels.ViewApiExtera();
        //        var toApi = list.ToApi(extra);
        //        exteraModel.List = toApi;
        //        var detail= _context.Product.SearchDetail(
        //        notId: notId,
        //        typeId: typeId,
        //        categoryId: categoryId,
        //        brandId: brandId,
        //        subCategoryId: subCategoryId,
        //        name: name,
        //        custom: custom,
        //        pricefrom: pricefrom,
        //        priceto: priceto,
        //        isService: isService,
        //        storeId:storeId,
        //        option:option,
        //        latitude: latitude,
        //        longitude: longitude,
        //        distance: distance,
        //        colorId: colorId,
        //        sizeId: sizeId
        //        );
        //        exteraModel.Count = detail.Count;
        //        exteraModel.MinPrice = detail.MinPrice;
        //        exteraModel.MaxPrice = detail.MaxPrice;
        //        return ApiResponse.CreateSuccessResult(exteraModel);
        //    }
        //    return ApiResponse.CreateSuccessResult(list.ToApi(extra));
        //}



    }
}
