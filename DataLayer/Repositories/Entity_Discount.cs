using Binbin.Linq;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using ExpressMapper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_Discount : BaseRepository<Discount>
    {
        private DatabaseEntities _context;
        public Entity_Discount(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        private static DateTime IranToday()
        {
            return DateTime.UtcNow.AddHours(3.5).Date;
        }

        public List<Discount> GetAllByDiscountGroupId(int GroupId)
        {
            return _context.Discount.Where(p =>
                p.GroupId == GroupId
            ).ToList();
        }

        public List<Discount> GetAllByProduct(Product product)
        {
            DateTime now = IranToday();
            string type_shop = Enum_Code.DISCOUNT_TYPE_SHOP.ToString();
            string type_producttype = Enum_Code.DISCOUNT_TYPE_PRODUCTTYPE.ToString();
            string type_productcategory = Enum_Code.DISCOUNT_TYPE_CATEGORY.ToString();
            string type_productsubcategory = Enum_Code.DISCOUNT_TYPE_SUBCATEGORY.ToString();
            string type_product = Enum_Code.DISCOUNT_TYPE_PRODUCT.ToString();
            string type_store = Enum_Code.DISCOUNT_TYPE_VARIENTY.ToString();

            return _context.Discount.Where(p =>
                ((p.Code.Label == type_product && p.ProductId == product.ID) ||
                    (p.Code.Label == type_store && p.Product.StoreProduct.Any()) ||
                    (p.Code.Label == type_shop && p.ShopId == product.ShopId) ||
                    (p.Code.Label == type_producttype && p.ProductTypeId == product.ProductTypeId) ||
                    (p.Code.Label == type_productcategory && p.ProductCategoryId == product.ProductCategoryId) ||
                    (p.Code.Label == type_productsubcategory && p.ProductSubCategoryId == product.ProductSubCategoryId)
                ) &&
                (p.Active == true) && (p.Store.Active == true) && (p.Store.Approve == true) &&
                (p.StartDatetime == null || (p.StartDatetime != null && p.StartDatetime <= now)) &&
                (p.EndDatetime == null || (p.EndDatetime != null && p.EndDatetime >= now))
            ).ToList();
        }
        public List<Discount> GetAllByStoreId(StoreProductQuantity storeProductQuantity)
        {
            DateTime now = IranToday();
            string type_shop = Enum_Code.DISCOUNT_TYPE_SHOP.ToString();
            string type_brand = Enum_Code.DISCOUNT_TYPE_BRAND.ToString();
            string type_producttype = Enum_Code.DISCOUNT_TYPE_PRODUCTTYPE.ToString();
            string type_productcategory = Enum_Code.DISCOUNT_TYPE_CATEGORY.ToString();
            string type_productsubcategory = Enum_Code.DISCOUNT_TYPE_SUBCATEGORY.ToString();
            string type_product = Enum_Code.DISCOUNT_TYPE_PRODUCT.ToString();
            string type_varienty = Enum_Code.DISCOUNT_TYPE_VARIENTY.ToString();
            return _context.Discount.Where(p =>
            (p.Deleted == false) &&  
            (p.Active == true) &&
            (p.StoreId == storeProductQuantity.StoreProduct.StoreId)
            &&
                (
                 (p.Code.Label == type_varienty && p.StoreProductQuantityId == storeProductQuantity.ID) ||
                 (p.Code.Label == type_product && p.ProductId == storeProductQuantity.StoreProduct.ProductId) ||
                    (p.Code.Label == type_producttype && p.ProductTypeId == storeProductQuantity.StoreProduct.Product.ProductTypeId) ||
                    (p.Code.Label == type_productcategory && p.ProductCategoryId == storeProductQuantity.StoreProduct.Product.ProductCategoryId) ||
                    (p.Code.Label == type_productsubcategory && p.ProductSubCategoryId == storeProductQuantity.StoreProduct.Product.ProductSubCategoryId) ||
                    (p.Code.Label == type_brand && p.ProductBrandId == storeProductQuantity.StoreProduct.Product.BrandId) ||
                     (p.Code.Label == type_shop)

                    ) &&

                (p.StartDatetime == null || (p.StartDatetime != null && p.StartDatetime <= now)) &&
                (p.EndDatetime == null || (p.EndDatetime != null && p.EndDatetime >= now))
            ).ToList();
        }
        public List<Discount> GetAllByProductId(int productId)
        {
            string type_product = Enum_Code.DISCOUNT_TYPE_PRODUCT.ToString();
            return _context.Discount.Where(p =>
                p.ProductId == productId &&
                p.Code.Label == type_product
            ).ToList();
        }

        public DiscountSearchVm Search(
            Enum_Code type = Enum_Code.DISCOUNT_TYPE_NONE,
            int? typeId = null,
            string label = null,
            string name = null,
            int? notId = null,
            int? index = null,
            int? pageSize = null,
            Boolean? active = null,
            int? storeId = null)
        {
            index = index == null ? 1 : index;
            pageSize = pageSize == null ? 10 : pageSize;
            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;
            var MyQuery = GetSearchQuery(type: type,typeId:typeId, label: label, notId: notId, active: active, storeId: storeId, name: name);
            var result = new DiscountSearchVm();
            var discountes = Where(MyQuery);
            result.TotalCount = discountes.Count();
            result.PageIndex = index.Value;
            result.PageSize = pageSize.Value;
            result.Active = active;
            result.StoreId = storeId;
            result.TypeId = typeId;

               result.Discounts=discountes.OrderBy(p => p.ID)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList().Map<List<Discount>, List<DiscountVm>>();
            return result;

        }

        public int SearchCount(
            Enum_Code type = Enum_Code.DISCOUNT_TYPE_NONE,
            string label = null,
                        string name = null,

            int? notId = null,
            Boolean? active = null,
            int? storeId = null)
        {
            var MyQuery = GetSearchQuery(type: type, label: label, notId: notId, active: active, storeId: storeId, name: name);
            return _context.Discount.Count(MyQuery);
        }

        private Expression<Func<Discount, bool>> GetSearchQuery(
     Enum_Code type = Enum_Code.DISCOUNT_TYPE_NONE,
                 int? typeId = null,

            string label = null,
                        string name = null,
            int? notId = null,
            Boolean? active = null,
            int? storeId = null)
        {
            var MyQuery = PredicateBuilder.True<Discount>();
            string typeString = type.ToString();
            MyQuery = MyQuery.And(p => p.Deleted == false);
            if (active != null)
            {
                MyQuery = MyQuery.And(p => p.Active == active);
            }
            if (storeId != null)
                MyQuery = MyQuery.And(p => p.StoreId == storeId);
            if (notId != null && notId != 0)
                MyQuery = MyQuery.And(p => p.ID != notId);
            if (label != null)
                MyQuery = MyQuery.And(p => p.DiscountGroup.Label == label);

           
            if (typeId != null)
                MyQuery = MyQuery.And(p => p.Code.ID == typeId);

            else if (type != Enum_Code.DISCOUNT_TYPE_NONE)
                MyQuery = MyQuery.And(p => p.Code.Label == typeString);

          
            if (!string.IsNullOrEmpty(name))
            {
                MyQuery = MyQuery.And(p =>
                (p.DiscountGroup != null && p.DiscountGroup.Name.Contains(name))
                || (p.ProductType != null && p.ProductType.Name.Contains(name))
                || (p.ProductCategory != null && p.ProductCategory.Name.Contains(name))
                || (p.ProductSubCategory != null && p.ProductSubCategory.Name.Contains(name))
                || (p.ProductBrand != null && p.ProductBrand.Name.Contains(name))
                || (p.Product != null && p.Product.Name.Contains(name))
                );
            }
            return MyQuery;
        }
        public void DeleteByProductId(int productId)
        {
            List<Discount> list = GetAllByProductId(productId);
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }
        public CreateDiscountVm GetDiscountVm(int id)
        {
            var item = GetById(id);
            if (item != null)
            {
                return item.Map<Discount, CreateDiscountVm>();
            }
            return null;
        }
        public ResultVm<CreateDiscountVm> Insert(CreateDiscountVm discount)
        {
            var result = new ResultVm<CreateDiscountVm>();
            var today = IranToday();
            var tomorrow = today.AddDays(1);
            if (discount.StartDatetime.HasValue) { discount.StartDatetime = discount.StartDatetime.Value.Date; };
            if (discount.EndDatetime.HasValue) { discount.EndDatetime = discount.EndDatetime.Value.Date.AddDays(1).AddTicks(-1); };
            if (discount.TypeId == 0)
            {
                result.Status = ResultStatus.Error;
                result.Message = Resource.Notify.PleaseSelectTheTypeOfDiscount;
                return result;
            }
            var discounttype = _context.Code.FirstOrDefault(s => s.ID == discount.TypeId);
            if (discounttype == null)
            {
                result.Status = ResultStatus.Exception;
                result.Message = Resource.Notify.UnspecifiedError;
                return result;
            }
            if (discount.StoreId == null)
            {
                result.Status = ResultStatus.Error;
                result.Message = Resource.Notify.PleaseSelectTheSeller;
                return result;
            }
            if (discount.ValueTypeId == null)
            {
                result.Status = ResultStatus.Error;
                result.Message = Resource.Notify.PleaseSelectTheTypeOfAmount;
                return result;
            }
            if (discounttype.Label == Enumarables.Enum_Code.DISCOUNT_TYPE_SHOP.ToString())
            {
                discount.ProductTypeId = null;
                discount.ProductCategoryId = null;
                discount.ProductSubCategoryId = null;
                discount.ProductId = null;
                discount.StoreProductQuantityId = null;
                discount.ProductBrandId = null;
            }
            else if (discounttype.Label == Enumarables.Enum_Code.DISCOUNT_TYPE_BRAND.ToString())
            {
                if (discount.ProductBrandId == null)
                {
                    result.Status = ResultStatus.Error;
                    result.Message = Resource.Notify.PleaseSelectTheBrand;
                    return result;
                }
                else
                {
                    discount.ProductTypeId = null;
                    discount.ProductCategoryId = null;
                    discount.ProductSubCategoryId = null;
                    discount.ProductId = null;
                    discount.StoreProductQuantityId = null;
                }
            }
            else if (discounttype.Label == Enumarables.Enum_Code.DISCOUNT_TYPE_PRODUCTTYPE.ToString())
            {
                if (discount.ProductTypeId == null)
                {
                    result.Status = ResultStatus.Error;
                    result.Message = Resource.Notify.PleaseSelectTheProductType;
                    return result;
                }
                else
                {
                    discount.ProductCategoryId = null;
                    discount.ProductSubCategoryId = null;
                    discount.ProductId = null;
                    discount.StoreProductQuantityId = null;
                    discount.ProductBrandId = null;
                }

            }
            else if (discounttype.Label == Enumarables.Enum_Code.DISCOUNT_TYPE_CATEGORY.ToString())
            {
                if (discount.ProductCategoryId == null)
                {
                    result.Status = ResultStatus.Error;
                    result.Message = Resource.Notify.PleaseSelectTheProductCategory;
                    return result;
                }
                else
                {
                    discount.ProductSubCategoryId = null;
                    discount.ProductId = null;
                    discount.StoreProductQuantityId = null;
                    discount.ProductBrandId = null;
                }

            }
            else if (discounttype.Label == Enumarables.Enum_Code.DISCOUNT_TYPE_SUBCATEGORY.ToString())
            {
                if (discount.ProductSubCategoryId == null)
                {
                    result.Status = ResultStatus.Error;
                    result.Message = Resource.Notify.PleaseSelectTheProductSubCategory;
                    return result;
                }
                else
                {
                    discount.ProductId = null;
                    discount.StoreProductQuantityId = null;
                    discount.ProductBrandId = null;
                }

            }
            else if (discounttype.Label == Enumarables.Enum_Code.DISCOUNT_TYPE_PRODUCT.ToString())
            {
                if (discount.ProductId == null)
                {
                    result.Status = ResultStatus.Error;
                    result.Message = Resource.Notify.PleaseSelectTheProduct;
                    return result;
                }
                else
                {
                    discount.StoreProductQuantityId = null;
                    discount.ProductBrandId = null;
                }

            }
            else if (discounttype.Label == Enumarables.Enum_Code.DISCOUNT_TYPE_VARIENTY.ToString())
            {
                if (discount.StoreProductQuantityId == null)
                {
                    result.Status = ResultStatus.Error;
                    result.Message = Resource.Notify.PleaseSelectTheVarity;
                    return result;
                }
                else
                {
                    discount.ProductBrandId = null;
                }

            }
            var exist = FirstOrDefault(x =>
            x.GroupId == discount.GroupId
              && x.StoreId == discount.StoreId
              && x.TypeId == discount.TypeId
              && x.ProductTypeId == discount.ProductTypeId
              && x.ProductCategoryId == discount.ProductCategoryId
              && x.ProductSubCategoryId == discount.ProductSubCategoryId
              && x.ProductBrandId == discount.ProductBrandId
              && x.ProductId == discount.ProductId
              && x.StoreProductQuantityId == discount.StoreProductQuantityId
              && x.Active ==true
              && x.Deleted==false
              );
            if(exist!=null)
            {
                result.Status = ResultStatus.Error;
                result.Message = Resource.Notify.ASimilarItemHasAlreadyBeenRegistered;
                return result;
            }
            if (discount.ValueTypeId == 52)
            {
                if (discount.PriceValue == null || discount.PriceValue < 1)
                {
                    result.Status = ResultStatus.Error;
                    result.Message = Resource.Notify.PleaseEnterTheAmount;
                    return result;
                }
                else
                {
                    discount.PercentValue = null;
                }
            }
            else
            {
                if (discount.PercentValue == null || discount.PercentValue < 1)
                {
                    result.Status = ResultStatus.Error;
                    result.Message = Resource.Notify.PleaseEnterThePercent;
                    return result;
                }
                if (discount.PercentValue > 99 || discount.PercentValue < 1)
                {
                    result.Status = ResultStatus.Error;
                    result.Message = Resource.Notify.PleaseEnterThePercentageCorrectly;
                    return result;
                }
                else
                {
                    discount.PriceValue = null;
                }
            }

            if (discount.StartDatetime == null)
            {
                result.Status = ResultStatus.Error;
                result.Message = Resource.Notify.PleaseEnterTheStartDate;
                return result;
            }
            if (discount.StartDatetime < today)
            {
                result.Status = ResultStatus.Error;
                result.Message = Resource.Notify.PleaseEnterTheTimePeriodCorrectly;
                return result;
            }
            else if (discount.EndDatetime != null && discount.StartDatetime > discount.EndDatetime)
            {
                result.Status = ResultStatus.Error;
                result.Message = Resource.Notify.PleaseEnterTheTimePeriodCorrectly;
                return result;
            }
            if (discount.GroupId != null)
            {
                var group = _context.DiscountGroup.FirstOrDefault(x => x.ID == discount.GroupId);
                if (group.Deleted == true || group.Active == false || (group.EndDatetime!=null && group.EndDatetime< tomorrow))
                {
                    result.Status = ResultStatus.Error;
                    result.Message = Resource.Notify.UnspecifiedError;
                    return result;
                }
                if (group.StartDatetime != null && group.StartDatetime > discount.StartDatetime)
                {
                    result.Status = ResultStatus.Error;
                    result.Message = Resource.Notify.TheTimePeriodEnteredDoesNotMatchTheTimePeriodOfTheDiscountGroup;
                    return result;
                    
                }
                if (group.EndDatetime != null && (discount.EndDatetime ==null|| group.EndDatetime < discount.EndDatetime))
                {
                    result.Status = ResultStatus.Error;
                    result.Message = Resource.Notify.TheTimePeriodEnteredDoesNotMatchTheTimePeriodOfTheDiscountGroup;
                    return result;
                }


            }
            var entityModel = new Discount();
            entityModel = discount.Map<CreateDiscountVm, Discount>();
            Insert(entityModel);
            Save();
            result.Status = ResultStatus.Success;
            return result;
        }
        public ResultVm<CreateDiscountVm> Update(CreateDiscountVm discount)
        {
            var result = new ResultVm<CreateDiscountVm>();
            if (discount.StartDatetime.HasValue) discount.StartDatetime = discount.StartDatetime.Value.Date;
            if (discount.EndDatetime.HasValue) discount.EndDatetime = discount.EndDatetime.Value.Date.AddDays(1).AddTicks(-1);
            if (discount.ValueTypeId == null)
            {
                result.Status = ResultStatus.Error;
                result.Message = Resource.Notify.PleaseSelectTheTypeOfAmount;
                return result;
            }

            if (discount.ValueTypeId == 52)
            {
                if (discount.PriceValue == null || discount.PriceValue < 1)
                {
                    result.Status = ResultStatus.Error;
                    result.Message = Resource.Notify.PleaseEnterTheAmount;
                    return result;
                }
                else
                {
                    discount.PercentValue = null;
                }
            }
            else
            {
                if (discount.PercentValue == null || discount.PercentValue < 1)
                {
                    result.Status = ResultStatus.Error;
                    result.Message = Resource.Notify.PleaseEnterThePercent;
                    return result;
                }
                if (discount.PercentValue > 99 || discount.PercentValue < 1)
                {
                    result.Status = ResultStatus.Error;
                    result.Message = Resource.Notify.PleaseEnterThePercentageCorrectly;
                    return result;
                }
                else
                {
                    discount.PriceValue = null;
                }
            }
            if (discount.StartDatetime == null)
            {
                result.Status = ResultStatus.Error;
                result.Message = Resource.Notify.PleaseEnterTheStartDate;
                return result;
            }
            else if (discount.EndDatetime != null && discount.StartDatetime > discount.EndDatetime)
            {
                result.Status = ResultStatus.Error;
                result.Message = Resource.Notify.PleaseEnterTheTimePeriodCorrectly;
                return result;
            }
            var discountEntity = GetById(discount.ID);
            discountEntity.ValueTypeId = discount.ValueTypeId;
            discountEntity.PriceValue = discount.PriceValue;
            discountEntity.PercentValue = discount.PercentValue;
            discountEntity.StartDatetime = discount.StartDatetime;
            discountEntity.EndDatetime = discount.EndDatetime;
            discountEntity.Active = discount.Active;
            discountEntity.Synced = false;
            Update(discountEntity);
            Save();
            result.Status = ResultStatus.Success;
            return result;
        }
        public ResultVm<CreateDiscountVm> Delete(int id)
        {
            var result = new ResultVm<CreateDiscountVm>();
            result.Status = ResultStatus.Error;
            result.Message = Resource.Notify.UnspecifiedError;
            var discount = GetById(id);
            if (discount != null)
            {
                discount.Deleted = true;
                discount.Active = false;
                discount.Synced = false;
                Save();
                result.Status = ResultStatus.Success;
                result.Message = Resource.Notify.OperationSuccessful;
            }
            return result;
        }
        public void SyncDiscount(UnitOfWork unitOfwork)
        {
            var today = DateTime.Now.Date;
            var discounts = Where(x =>
            (x.Synced == false ) ||             
            (x.Active == true && (x.EndDatetime.HasValue && x.EndDatetime < today))         
            ).ToList();
            var products = new List<Product>();
            foreach (var item in discounts)
            {
                if(item.StartDatetime.HasValue && item.StartDatetime > DateTime.Now)
                {
                    continue;
                }
                var getProducts = unitOfwork.Product.GetForDiscount(item);
                products.AddRange(getProducts);
            }
            products = products.Distinct().ToList();
            DataLayer.Base.BaseStore.UpdateProductQuantity(products: products, unitOfwork);
            foreach (var item in discounts)
            {
                if (item.StartDatetime.HasValue && item.StartDatetime > today)
                {
                    continue;
                }
                if ((item.Active == true && (item.EndDatetime.HasValue && item.EndDatetime < today)))
                {
                    item.Active = false;
                }
                item.Synced = true;
                unitOfwork.Discount.Update(item);
                unitOfwork.Save();
            }
            unitOfwork.Dispose();
        }
    }
}
