using Binbin.Linq;
using DataLayer.Api;
using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using ExpressMapper.Extensions;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Linq.Expressions;

namespace DataLayer.Repositories
{
    public class Entity_Product : BaseRepository<Product>
    {
        private DatabaseEntities _context;
        public Entity_Product(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public void InserData(Product product, UnitOfWork unitOfWork)
        {
            //if (product.InstallmentCount == null)
            //{
            //    ProductQuantity quantity = new ProductQuantity()
            //    {
            //        ColorId = null,
            //        ProductId = product.ID,
            //        SizeId = null,
            //        OptionItemId = null,
            //        Price = GetEuroPrice(product.EuroPrice.Value),
            //        EuroPrice = product.EuroPrice
            //    };
            //    unitOfWork.ProductQuantity.Insert(quantity);
            //    unitOfWork.Save();
            //}
            if (product.InstallmentCount > 0)
            {
                var listOptionItem = unitOfWork.ProductProductOptionItem.GetAll().Take(product.InstallmentCount.Value);
                var count = listOptionItem.Count();
                var pricEuro = product.EuroPrice.Value / count;
                double temPrice = product.EuroPrice.Value;
                foreach (ProductProductOptionItem optionItem in listOptionItem.OrderByDescending(s => s.ID))
                {

                    double computeprice = GetEuroPrice(temPrice);
                    ProductQuantity quantity = new ProductQuantity()
                    {
                        ColorId = null,
                        ProductId = product.ID,
                        SizeId = null,
                        OptionItemId = optionItem?.ProductOptionItemId,
                        Price = computeprice,
                        EuroPrice = temPrice,
                        Count=1000
                    };
                    unitOfWork.ProductQuantity.Insert(quantity);
                   
                    temPrice -= pricEuro;
                }
                
                //BaseStore.UpdateProductQuantity(item.ID, _context);
            }
            else
            {
                ProductQuantity quantity = new ProductQuantity()
                {
                    ColorId = null,
                    ProductId = product.ID,
                    SizeId = null,
                    OptionItemId = null,
                    Price = GetEuroPrice(product.EuroPrice.Value),
                    EuroPrice = product.EuroPrice
                };
                unitOfWork.ProductQuantity.Insert(quantity);



            }





        }

        public void InsertVm(ProductCreateVm productVm, UnitOfWork unitOfWork)
        {
            var productEntity = new Product();
            productEntity = productVm.Map<ProductCreateVm, Product>();
            productEntity.UpdateDatetime = productEntity.CreateDateTime = DateTime.Now;
            Insert(productEntity);
            if (productVm.ProductPictures != null && productVm.ProductPictures.Any())
            {
                productEntity.ProductPicture = new List<ProductPicture>();

                foreach (var picture in productVm.ProductPictures)
                {
                    productEntity.ProductPicture.Add(new ProductPicture() { PictureId = picture.ID });
                }
            }
            if (productVm.ColorIds != null && productVm.ColorIds.Any())
            {
                productEntity.ProductColor = new List<ProductColor>();

                foreach (var colorId in productVm.ColorIds)
                {
                    productEntity.ProductColor.Add(new ProductColor() { ColorId = colorId });
                }
            }
            if (productVm.SizeIds != null && productVm.SizeIds.Any())
            {
                productEntity.ProductSize = new List<ProductSize>();

                foreach (var sizeId in productVm.SizeIds)
                {
                    productEntity.ProductSize.Add(new ProductSize() { SizeId = sizeId });
                }
            }
            if (productVm.ProductOptionId != null && productVm.OptionItemIds != null && productVm.OptionItemIds.Any())
            {
                productEntity.ProductProductOptionItem = new List<ProductProductOptionItem>();

                foreach (var optionId in productVm.OptionItemIds)
                {
                    productEntity.ProductProductOptionItem.Add(new ProductProductOptionItem() { ProductOptionItemId = optionId });
                }
            }
            if (productVm.ProductLanguages != null && productVm.ProductLanguages.Any())
            {
                productEntity.ProductLanguage = new List<ProductLanguage>();
                productEntity.ProductLanguage = productVm.ProductLanguages.Map<List<ProductLanguageVm>, List<ProductLanguage>>();
            }
            if (productVm.Reviews != null && productVm.Reviews.Any())
            {
                productVm.Reviews = productVm.Reviews.Where(s => string.IsNullOrEmpty(s.Contect) == false).ToList();
                productEntity.Review = new List<Review>();
                productEntity.Review = productVm.Reviews.Map<List<ReviewVm>, List<Review>>();
            }
            //if (productVm.ProductTypeId == 1001)
            //{

            //    productEntity.IsService = true;
            //}
            Save();

            unitOfWork.ProductQuantity.UpsertProductQuantiy(productEntity.ID, productVm.EuroPrice, true, unitOfWork);
            if (BaseWebsite.WebsiteSetting.HasUnitPrice)
            {
                double computeprice = ComputeFee();
                UpdateProductPriceBaseUnitPrice(productEntity.ID, computeprice, unitOfWork);
            }
        }
        public void UpdateVm(ProductCreateVm productVm, UnitOfWork unitOfWork)
        {
            var productEntity = GetById(productVm.ID);

            productEntity = ExpressMapper.Mapper.Map(productVm, productEntity);
            productEntity.UpdateDatetime = DateTime.Now;
            foreach (var pic in productEntity.ProductPicture.ToList())
            {
                unitOfWork.ProductPicture.Delete(pic.ID);
            }
            if (productVm.ProductPictures != null && productVm.ProductPictures.Any())
            {
                foreach (var picture in productVm.ProductPictures)
                {
                    productEntity.ProductPicture.Add(new ProductPicture() { PictureId = picture.ID });
                }
            }
            if (productVm.ColorIds != null && productVm.ColorIds.Any())
            {

                foreach (var colorId in productVm.ColorIds)
                {
                    if (!productEntity.ProductColor.Any(s => s.ColorId == colorId))
                    {
                        productEntity.ProductColor.Add(new ProductColor() { ColorId = colorId, ProductId = productEntity.ID });
                    }
                }
            }

            if (productVm.SizeIds != null && productVm.SizeIds.Any())
            {

                foreach (var sizeId in productVm.SizeIds)
                {
                    if (!productEntity.ProductSize.Any(s => s.SizeId == sizeId))
                    {
                        productEntity.ProductSize.Add(new ProductSize() { SizeId = sizeId, ProductId = productEntity.ID });
                    }
                }
            }

            if (productVm.ProductOptionId != null && productVm.OptionItemIds != null && productVm.OptionItemIds.Any())
            {
                foreach (var optionId in productVm.OptionItemIds)
                {
                    if (!productEntity.ProductProductOptionItem.Any(s => s.ProductOptionItemId == optionId))
                    {
                        productEntity.ProductProductOptionItem.Add(new ProductProductOptionItem() { ProductOptionItemId = optionId, ProductId = productEntity.ID });
                    }

                }
            }
            foreach (var lang in productEntity.ProductLanguage.ToList())
            {
                unitOfWork.ProductLanguage.Delete(lang.ID);
            }
            if (productVm.ProductLanguages != null && productVm.ProductLanguages.Any())
            {
                productVm.ProductLanguages.ForEach(s => s.ProductId = productEntity.ID);
                productEntity.ProductLanguage = productVm.ProductLanguages.Map<List<ProductLanguageVm>, List<ProductLanguage>>();
            }
            foreach (var re in productEntity.Review.ToList())
            {
                unitOfWork.Review.Delete(re.ID);
            }
            if (productVm.Reviews != null && productVm.Reviews.Any())
            {
                productVm.Reviews = productVm.Reviews.Where(s => string.IsNullOrEmpty(s.Contect) == false).ToList();
                productVm.Reviews.ForEach(s => s.ProductId = productEntity.ID);
                productEntity.Review = productVm.Reviews.Map<List<ReviewVm>, List<Review>>();
            }
            //if (productVm.ProductTypeId == 1001 || productVm.ProductTypeId == 2)
            //{

            //    productEntity.IsService = true;
            //}
            Save();
            unitOfWork.ProductQuantity.UpsertProductQuantiy(productEntity.ID, productVm.EuroPrice, false, unitOfWork);
            if (BaseWebsite.WebsiteSetting.HasUnitPrice)
            {
                double computeprice = ComputeFee();
                UpdateProductPriceBaseUnitPrice(productEntity.ID, computeprice, unitOfWork);
            }
        }
        public void UpdateVm2(ProductCreateVm productVm, UnitOfWork unitOfWork)
        {
            var productEntity = GetById(productVm.ID);

            productEntity = ExpressMapper.Mapper.Map(productVm, productEntity);
            productEntity.UpdateDatetime = DateTime.Now;
            foreach (var pic in productEntity.ProductPicture.ToList())
            {
                unitOfWork.ProductPicture.Delete(pic.ID);
            }
            if (productVm.ProductPictures != null && productVm.ProductPictures.Any())
            {
                foreach (var picture in productVm.ProductPictures)
                {
                    productEntity.ProductPicture.Add(new ProductPicture() { PictureId = picture.ID });
                }
            }
            if (productVm.ColorIds != null && productVm.ColorIds.Any())
            {

                foreach (var colorId in productVm.ColorIds)
                {
                    if (!productEntity.ProductColor.Any(s => s.ColorId == colorId))
                    {
                        productEntity.ProductColor.Add(new ProductColor() { ColorId = colorId, ProductId = productEntity.ID });
                    }
                }
            }

            if (productVm.SizeIds != null && productVm.SizeIds.Any())
            {

                foreach (var sizeId in productVm.SizeIds)
                {
                    if (!productEntity.ProductSize.Any(s => s.SizeId == sizeId))
                    {
                        productEntity.ProductSize.Add(new ProductSize() { SizeId = sizeId, ProductId = productEntity.ID });
                    }
                }
            }
            List<int> OptionItemIds = new List<int>();
            if(productEntity.InstallmentCount > 0)
            {
                for(int i=1;i<= productEntity.InstallmentCount;i++)
                {
                    OptionItemIds.Add(i);
                }
            }
            productVm.OptionItemIds = OptionItemIds;
            if (productVm.ProductOptionId != null && productVm.OptionItemIds != null && productVm.OptionItemIds.Any())
            {
                foreach (var optionId in productVm.OptionItemIds)
                {
                    if (!productEntity.ProductProductOptionItem.Any(s => s.ProductOptionItemId == optionId))
                    {
                        productEntity.ProductProductOptionItem.Add(new ProductProductOptionItem() { ProductOptionItemId = optionId, ProductId = productEntity.ID });
                    }

                }
            }
            foreach (var lang in productEntity.ProductLanguage.ToList())
            {
                unitOfWork.ProductLanguage.Delete(lang.ID);
            }
            if (productVm.ProductLanguages != null && productVm.ProductLanguages.Any())
            {
                productVm.ProductLanguages.ForEach(s => s.ProductId = productEntity.ID);
                productEntity.ProductLanguage = productVm.ProductLanguages.Map<List<ProductLanguageVm>, List<ProductLanguage>>();
            }
            foreach (var re in productEntity.Review.ToList())
            {
                unitOfWork.Review.Delete(re.ID);
            }
            if (productVm.Reviews != null && productVm.Reviews.Any())
            {
                productVm.Reviews = productVm.Reviews.Where(s => string.IsNullOrEmpty(s.Contect) == false).ToList();
                productVm.Reviews.ForEach(s => s.ProductId = productEntity.ID);
                productEntity.Review = productVm.Reviews.Map<List<ReviewVm>, List<Review>>();
            }
            if (productVm.ProductTypeId == 1001 || productVm.ProductTypeId == 2)
            {

                productEntity.IsService = true;
            }
            Save();
            unitOfWork.ProductQuantity.UpsertProductQuantiy(productEntity.ID, productVm.EuroPrice, false, unitOfWork);
            if (BaseWebsite.WebsiteSetting.HasUnitPrice)
            {
                double computeprice = ComputeFee();
                UpdateProductPriceBaseUnitPrice(productEntity.ID, computeprice, unitOfWork);
            }
        }
        public ProductSearchVm SearchDetail(

            string Id = null,
            int? notId = null,
            string typeId = null,
            string categoryId = null,
            string subCategoryId = null,
            string colorId = null,
            string sizeId = null,
             string customFieldId = null,
            string option = null,
            double? pricefrom = null,
            double? priceto = null,
            string brandId = null,
            string storeId = null,
            string name = null,
            string summery = null,
            bool? hasDiscount = null,
            string custom = null,
            bool? active = true,
            bool? showHome = null,
            bool? isService = null,
            string latitude = null,
            string longitude = null,
            int distance = 0,
            string SyncNameProductCustomField = null,
            string valueProductCustom = null,
            bool? prstock = null,
            string prcustomLabel = "",
            DateTime? fromDate = null,
            DateTime? toDate = null,
            Enum_ProductOrder order = Enum_ProductOrder.NONE,
            Enum_Code? status = null)
        {
            var result = new ProductSearchVm();

            var query = GetSearchQuery2(
                       Id: Id,
                       notId: notId,
                       categoryId: categoryId,
                       hasDiscount: hasDiscount,
                       name: name,
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
                       custom: custom,
                       active: active,
                       option: option,
                       showHome: showHome,
                       isService: isService,
                       latitude: latitude,
                       longitude: longitude,
                       distance: distance,
                       prstock: prstock,
                       prcustomLabel: prcustomLabel,
                       SyncNameProductCustomField: SyncNameProductCustomField,
                       valueProductCustom: valueProductCustom,
                       status: status
                       );

            string order_status_post = Enum_Code.ORDER_STATUS_POST.ToString();

            string order_status_success = Enum_Code.ORDER_STATUS_SUCCESS.ToString();
            IEnumerable<Product> q = _context.Product.Where(query);
            if (distance > 0 && (!string.IsNullOrEmpty(latitude)) && (!string.IsNullOrEmpty(longitude)))
            {
                q = from s in q
                    where TrueDistance(s.Latitude, s.Longitude, latitude, longitude, distance) == true
                    select s;
            }
            result.TotalCount = q.Count();
            result.MaxPrice = q.Max(s => s.InexpensiveItemPrice);
            result.MinPrice = q.Min(s => s.InexpensiveItemPrice);

            return result;
        }
        public void UpdateAllProductPrice(UnitOfWork unitOfWork)
        {
            if (BaseWebsite.WebsiteSetting.HasUnitPrice)
            {
                var goldPrice = Base.BaseStore.GetGoldPrice();
                if (goldPrice > 0)
                {
                    unitOfWork.ShopWebsiteSetting.FirstOrDefault().UnitPrice = goldPrice;
                    unitOfWork.Save();
                    List<Product> productList = Where(x => x.Active && (!x.Deleted)).ToList();

                    double computeprice = ComputeFee();
                    foreach (var item in productList)
                    {
                        UpdateProductPriceBaseUnitPrice(item, computeprice, unitOfWork);
                    }
                }
            }
        }

        public double ComputeFee()
        {

            double unitPrice = Base.BaseWebsite.WebsiteSetting.UnitPrice;
            double computeprice = (unitPrice + ((unitPrice * 7) / 100));
            computeprice = ((computeprice * 9) / 100) + computeprice;
            return computeprice;
        }
        public double GetEuroPrice(double price)
        {
            double? unitPrice = _context.WebsiteDetail.FirstOrDefault().EuroPrice;
            return unitPrice != null ? unitPrice.Value * price : 1 * price;
        }
        public void UpdateProductPriceBaseUnitPrice(Product item, double computeprice, UnitOfWork unitOfWork)
        {
            double fee = 0;
            if (item.ProductType.ProductCustomField.Where(x => x.SyncName == "Fee").Any())
            {
                int ID = item.ProductType.ProductCustomField.FirstOrDefault(x => x.SyncName == "Fee").ID;
                var checkCustomValue = item.ProductCustomValue.FirstOrDefault(x => x.FieldId == ID);
                if (checkCustomValue != null)
                {
                    if (checkCustomValue.Value != null)
                    {
                        checkCustomValue.Value = checkCustomValue.Value.ToEnglishDigit();
                        fee = double.Parse(checkCustomValue.Value);
                    }
                }
            }
            double computeprice1 = ((computeprice * fee) / 100) + computeprice;
            double price = Math.Round((computeprice1 * item.Weight), 0);

            item.ProductQuantity.SelectMany(s => s.StoreProductQuantity).ToList().ForEach(s => s.Price = price);
            Save();
            Base.BaseStore.UpdateProductQuantity(item.ID, unitOfWork);
        }

        public void UpdateProductPriceBaseUnitPrice(int id, double computeprice, UnitOfWork unitOfWork)
        {
            var item = FirstOrDefault(x => x.ID == id, includes: "ProductType,ProductCustomValue");
            UpdateProductPriceBaseUnitPrice(item, computeprice, unitOfWork);
        }
        public List<Product> Search(string Id = null,bool IsHidden=false, int? notId = null, string typeId = null, string categoryId = null, int? userId = null, string subCategoryId = null, string brandId = null, string companyId = null, string storeId = null, string colorId = null, string sizeId = null, string shopId = null, int index = 1, int pageSize = 10, string name = null, string title = null, string discount = null, Enum_ProductOrder order = Enum_ProductOrder.NONE, int? pricefrom = null, int? priceto = null, string custom = null, bool? isService = null, string active = "true", bool? showHome = null, string latitude = null, string longitude = null, int distance = 0, Expression<Func<Product, bool>> query = null, string prstock = "ALL", bool activeLocation = false, string option = null)
        {
            List<Product> results = new List<Product>();
            int skipValue = pageSize * (index - 1);
            int pageValue = pageSize;
            if (query == null)
            {
                query = GetSearchQuery(
                                        Id: Id,
                                        IsHidden:IsHidden,
                                        notId: notId,
                                        categoryId: categoryId,
                                        discount: discount,
                                        name: name,
                                        pricefrom: pricefrom,
                                        priceto: priceto,
                                        brandId: brandId,
                                        storeId: storeId,
                                        colorId: colorId,
                                        sizeId: sizeId,
                                        shopId: shopId,
                                        subCategoryId: subCategoryId,
                                        typeId: typeId,
                                        custom: custom,
                                        active: active,
                                        option: option,
                                        showHome: showHome,
                                        isService: isService,
                                        latitude: latitude,
                                        longitude: longitude,
                                        distance: distance,
                                        prstock: prstock,
                                        activeLocation: activeLocation,
                                        companyId: companyId

                                        );
            }


            string order_status_post = Enum_Code.ORDER_STATUS_POST.ToString();
            //string order_status_process = Enum_Code.ORDER_STATUS_PROCESS.ToString();
            //string order_status_store = Enum_Code.ORDER_STATUS_STORE.ToString();
            string order_status_success = Enum_Code.ORDER_STATUS_SUCCESS.ToString();
            IEnumerable<Product> q = _context.Product.Where(query);
            if (distance > 0 && (!string.IsNullOrEmpty(latitude)) && (!string.IsNullOrEmpty(longitude)))
            {
                q = from s in q
                    where TrueDistance(s.Latitude, s.Longitude, latitude, longitude, distance) == true
                    select s;
            }
            if (order == Enum_ProductOrder.NONE)
                results = q
                    .OrderBy(p => p.Code.Value)
                    .ThenBy(p => p.ID)
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList();
            else if (order == Enum_ProductOrder.NEW)
                results = q
                    .OrderBy(p => p.Code.Value)
                    .ThenByDescending(p => p.ID)
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList();
            else if (order == Enum_ProductOrder.UPDATE)
                results = q
                    .OrderBy(p => p.Code.Value)
                    .ThenBy(p => p.UpdateDatetime)
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList();
            else if (order == Enum_ProductOrder.UPDATE_DESC)
                results = q
                    .OrderBy(p => p.Code.Value)
                    .ThenByDescending(p => p.UpdateDatetime)
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList();
            else if (order == Enum_ProductOrder.NEW_SYNC)
                results = q
                    .OrderBy(p => p.Code.Value)
                    .ThenByDescending(p => p.SyncDatetime)
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList();
            else if (order == Enum_ProductOrder.OLD)
                results = q
                    .OrderBy(p => p.Code.Value)
                    .ThenBy(p => p.ID)
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList();
            else if (order == Enum_ProductOrder.RANDOM)
                results = q
                    .OrderBy(p => p.Code.Value)
                    .ThenBy(p => Guid.NewGuid())
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList();
            else if (order == Enum_ProductOrder.PRICE_MAX_TO_MIN)
                results = q
                    .OrderBy(p => p.Code.Value)
                    .ThenByDescending(p => p.MinPrice > 0 ? p.MinPrice : p.Price)
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList();
            else if (order == Enum_ProductOrder.PRICE_MIN_TO_MAX)
                results = q
                    .OrderBy(p => p.Code.Value)
                    .ThenBy(p => p.MinPrice > 0 ? p.MinPrice : p.Price)
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList();
            else if (order == Enum_ProductOrder.MORE_SELL)
                results = q
                    .OrderBy(p => p.Code.Value)
                    .ThenByDescending(p => p.AccountOrderProduct.Count(qu =>
                        qu.AccountOrder.Code.Label == order_status_post ||
                        //qu.AccountOrder.Code.Label == order_status_process ||
                        //qu.AccountOrder.Code.Label == order_status_store ||
                        qu.AccountOrder.Code.Label == order_status_success))
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList();
            else if (order == Enum_ProductOrder.LESS_SELL)
                results = q
                    .OrderBy(p => p.Code.Value)
                    .ThenBy(p => p.AccountOrderProduct.Count(qu =>
                        qu.AccountOrder.Code.Label == order_status_post ||
                        //qu.AccountOrder.Code.Label == order_status_process ||
                        //qu.AccountOrder.Code.Label == order_status_store ||
                        qu.AccountOrder.Code.Label == order_status_success))
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList();
            else if (order == Enum_ProductOrder.SHOWNUMBER)
                results = q
                    .OrderBy(p => p.Code.Value)
                    .ThenBy(p => p.ShowNumber)
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList();
            else if (order == Enum_ProductOrder.SHOWNUMBER_DESC)
                results = q
                    .OrderBy(p => p.Code.Value)
                    .ThenByDescending(p => p.ShowNumber)
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList();
            else if (order == Enum_ProductOrder.MORE_VISIT)
                results = q
                    .OrderBy(p => p.Code.Value)
                    .ThenByDescending(p => p.VisitCount)
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList();
            else if (order == Enum_ProductOrder.MORE_LIKE)
                results = q
                    .OrderBy(p => p.Code.Value)
                    .ThenByDescending(p => p.ProductLike.Count)
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList();
            else if (order == Enum_ProductOrder.MORE_Rate)
                results = q
                    .OrderBy(p => p.Code.Value)
                    .ThenByDescending(p => p.GetRate() == null ? 0 : p.GetRate().Value)
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList();

            return results;
        }

        public Expression<Func<Product, bool>> GetSearchQuery(
            string Id = null,
            bool IsHidden= false,
            int? notId = null,
            string typeId = null,
            string colorId = null,
            string sizeId = null,
            int? userId = null,
            string categoryId = null,
            string subCategoryId = null,
            string shopId = null,
            int? pricefrom = null,
            int? priceto = null,
            string brandId = null,
            string competitiveId = null,
            string storeId = null,
            string name = null,
            string discount = null,
            string custom = null,
            string option = null,
            string active = "true",
            bool? showHome = null,
            string Summary = null,
            bool? isService = null,
            string latitude = null,
            string longitude = null,
            int distance = 0,
            string SyncNameProductCustomField = null,
            string valueProductCustom = null,
            string prstock = "ALL",
            string prcustomLabel = "",
            bool activeLocation = false,
            string cityId = null,
            string stateId = null,
            string companyId = null,
            DateTime? fromDate = null,
            DateTime? toDate = null
            )
        {
            active = active.ToUpper();
            bool? isActive = null;
            if (active == "TRUE")
                isActive = true;
            else if (active == "FALSE")
                isActive = false;
            var MyQuery = PredicateBuilder.True<Product>();
            MyQuery = MyQuery.And(p => p.Deleted == false);
            if (IsHidden != true)
            {
                MyQuery = MyQuery.And(p => p.IsHidden != true);
            }
            if (prstock == "AVAILABLE")
                MyQuery = MyQuery.And(p => p.Code.Label == Enum_Code.PRODUCT_STATUS_AVAILABLE.ToString());
            else if (prstock == "NOT_AVAILABLE")
                MyQuery = MyQuery.And(p => p.Code.Label == Enum_Code.PRODUCT_STATUS_NOT_AVAILABLE.ToString());
            if (Id != null)
            {
                var QueryStr = PredicateBuilder.False<Product>();
                string[] Split = Id.Split('-');
                foreach (var item in Split)
                {
                    int itemId = item.GetInteger();
                    if (itemId != 0)
                    {
                        QueryStr = QueryStr.Or(p =>
                            p.ID == itemId
                        );
                    }
                }
                MyQuery = MyQuery.And(QueryStr);
            }

            if (isService == true || isService == false)
                MyQuery = MyQuery.And(p => p.IsService == isService);
            if (!IsNullOrEmptyId(companyId))
            {
                int cpId = int.Parse(companyId);
                MyQuery = MyQuery.And(p => p.CompanyId == cpId);
            }
            if (fromDate != null && fromDate != default(DateTime))
                MyQuery = MyQuery.And(p => p.CreateDateTime >= fromDate);

            if (toDate != null && toDate != default(DateTime))
            {
                DateTime toDatetimeTemp = toDate.Value.AddDays(1);
                MyQuery = MyQuery.And(p => p.CreateDateTime <= toDatetimeTemp);
            }
            if (notId != null && notId != 0)
                MyQuery = MyQuery.And(p => p.ID != notId);
            if (pricefrom != null)
                MyQuery = MyQuery.And(p => p.Price >= pricefrom);
            if (priceto != null)
                MyQuery = MyQuery.And(p => p.Price <= priceto);
            if (isActive != null)
                MyQuery = MyQuery.And(p => p.Active == isActive);
            if (showHome != null)
                MyQuery = MyQuery.And(p => p.ShowHomePage == showHome);
            if (Summary != null)
                MyQuery = MyQuery.And(p => p.Summary == Summary);
            if (userId != null)
                MyQuery = MyQuery.And(p =>
                    p.Shop.ShopUser.Any(s => s.UserId == userId) ||
                    p.UserId == userId);

            if (IsNullOrEmptyId(discount) == false)
            {
                DateTime now = DateTime.Now;
                string type_shop = Enum_Code.DISCOUNT_TYPE_SHOP.ToString();
                string type_producttype = Enum_Code.DISCOUNT_TYPE_PRODUCTTYPE.ToString();
                string type_productbrand = Enum_Code.DISCOUNT_TYPE_BRAND.ToString();
                string type_productcategory = Enum_Code.DISCOUNT_TYPE_CATEGORY.ToString();
                string type_productsubcategory = Enum_Code.DISCOUNT_TYPE_SUBCATEGORY.ToString();
                string type_product = Enum_Code.DISCOUNT_TYPE_PRODUCT.ToString();

                MyQuery = MyQuery.And(s =>
                    s.Discount.Any(p =>
                        (p.Code.Label == type_product) &&
                        (p.ProductId == s.ID) &&
                        (p.Active == true) &&
                        (p.StartDatetime == null || (p.StartDatetime != null && p.StartDatetime <= now)) &&
                        (p.EndDatetime == null || (p.EndDatetime != null && p.EndDatetime >= now)) &&
                        (p.DiscountGroup.Label == discount && p.DiscountGroup.Active == true)
                    ) ||
                    (
                        s.Shop.Discount.Any(p =>
                        (p.Code.Label == type_shop) &&
                        (p.ShopId == s.ShopId) &&
                        (p.Active == true) &&
                        (p.StartDatetime == null || (p.StartDatetime != null && p.StartDatetime <= now)) &&
                        (p.EndDatetime == null || (p.EndDatetime != null && p.EndDatetime >= now)) &&
                        (p.DiscountGroup.Label == discount && p.DiscountGroup.Active == true))
                    ) ||
                    (
                        s.ProductType.Discount.Any(p =>
                        (p.Code.Label == type_producttype) &&
                        (p.ProductTypeId == s.ProductTypeId) &&
                        (p.Active == true) &&
                        (p.StartDatetime == null || (p.StartDatetime != null && p.StartDatetime <= now)) &&
                        (p.EndDatetime == null || (p.EndDatetime != null && p.EndDatetime >= now)) &&
                        (p.DiscountGroup.Label == discount && p.DiscountGroup.Active == true))
                    ) ||
                    (
                        s.ProductCategory.Discount.Any(p =>
                        (p.Code.Label == type_productcategory) &&
                        (p.ProductCategoryId == s.ProductCategoryId) &&
                        (p.Active == true) &&
                        (p.StartDatetime == null || (p.StartDatetime != null && p.StartDatetime <= now)) &&
                        (p.EndDatetime == null || (p.EndDatetime != null && p.EndDatetime >= now)) &&
                        (p.DiscountGroup.Label == discount && p.DiscountGroup.Active == true))
                    ) ||
                    (
                        s.ProductSubCategory.Discount.Any(p =>
                        (p.Code.Label == type_productsubcategory) &&
                        (p.ProductSubCategoryId == s.ProductSubCategoryId) &&
                        (p.Active == true) &&
                        (p.StartDatetime == null || (p.StartDatetime != null && p.StartDatetime <= now)) &&
                        (p.EndDatetime == null || (p.EndDatetime != null && p.EndDatetime >= now)) &&
                        (p.DiscountGroup.Label == discount && p.DiscountGroup.Active == true))
                    ) ||
                    (
                        s.ProductBrand.Discount.Any(p =>
                        (p.Code.Label == type_productbrand) &&
                        (p.ProductBrandId == s.BrandId) &&
                        (p.Active == true) &&
                        (p.StartDatetime == null || (p.StartDatetime != null && p.StartDatetime <= now)) &&
                        (p.EndDatetime == null || (p.EndDatetime != null && p.EndDatetime >= now)) &&
                        (p.DiscountGroup.Label == discount && p.DiscountGroup.Active == true))
                    )
                );
            }

            if (IsNullOrEmptyId(typeId) == false)
            {
                var QueryStr = PredicateBuilder.False<Product>();
                string[] Split = typeId.Split('-');
                foreach (var item in Split)
                {
                    int itemId = item.GetInteger();
                    if (itemId != 0)
                    {
                        QueryStr = QueryStr.Or(p =>
                            p.ProductTypeId == itemId
                        );
                    }
                }
                MyQuery = MyQuery.And(QueryStr);
            }

            if (IsNullOrEmptyId(colorId) == false)
            {
                var QueryStr = PredicateBuilder.False<Product>();
                string[] Split = colorId.Split('-');
                foreach (var item in Split)
                {
                    int itemId = item.GetInteger();
                    if (itemId != 0)
                    {
                        QueryStr = QueryStr.Or(p =>
                            p.ProductColor.Any(a => a.ColorId == itemId)
                        );
                    }
                }
                MyQuery = MyQuery.And(QueryStr);
            }

            if (IsNullOrEmptyId(sizeId) == false)
            {
                var QueryStr = PredicateBuilder.False<Product>();
                string[] Split = sizeId.Split('-');
                foreach (var item in Split)
                {
                    int itemId = item.GetInteger();
                    if (itemId != 0)
                    {
                        QueryStr = QueryStr.Or(p =>
                            p.ProductSize.Any(a => a.SizeId == itemId)
                        );
                    }
                }
                MyQuery = MyQuery.And(QueryStr);
            }

            if (IsNullOrEmptyId(categoryId) == false)
            {
                var QueryStr = PredicateBuilder.False<Product>();
                string[] Split = categoryId.Split('-');
                foreach (var item in Split)
                {
                    int itemId = item.GetInteger();
                    if (itemId != 0)
                    {
                        QueryStr = QueryStr.Or(p =>
                            p.ProductCategoryId == itemId
                        );
                    }
                }
                MyQuery = MyQuery.And(QueryStr);
            }

            if (IsNullOrEmptyId(subCategoryId) == false)
            {
                var QueryStr = PredicateBuilder.False<Product>();
                string[] Split = subCategoryId.Split('-');
                foreach (var item in Split)
                {
                    int itemId = item.GetInteger();
                    if (itemId != 0)
                    {
                        QueryStr = QueryStr.Or(p =>
                            p.ProductSubCategoryId == itemId
                        );
                    }
                }
                MyQuery = MyQuery.And(QueryStr);
            }

            if (IsNullOrEmptyId(brandId) == false)
            {
                var QueryStr = PredicateBuilder.False<Product>();
                string[] Split = brandId.Split('-');
                foreach (var item in Split)
                {
                    int itemId = item.GetInteger();
                    if (itemId != 0)
                    {
                        QueryStr = QueryStr.Or(p =>
                            p.BrandId == itemId
                        );
                    }
                }
                MyQuery = MyQuery.And(QueryStr);
            }


            if (IsNullOrEmptyId(storeId) == false)
            {
                var QueryStr = PredicateBuilder.False<Product>();
                string[] Split = storeId.Split('-');
                foreach (var item in Split)
                {
                    int itemId = item.GetInteger();
                    if (itemId != 0)
                    {
                        QueryStr = QueryStr.Or(p =>
                            p.StoreProduct.Any(s => s.StoreId == itemId)
                        );
                    }
                }
                MyQuery = MyQuery.And(QueryStr);
            }
            if (IsNullOrEmptyId(cityId) == false && IsNullOrEmptyId(stateId) == false)
            {
                int cityitemId = cityId.GetInteger();
                int stateitemId = stateId.GetInteger();
                MyQuery = MyQuery.And(p => p.StoreProduct.Any(s => s.Store.Account.CityId == cityitemId && s.Store.Account.StateId == stateitemId));
            }
            if (IsNullOrEmptyId(competitiveId) == false)
            {
                var QueryStr = PredicateBuilder.False<Product>();
                string[] Split = competitiveId.Split('-');
                foreach (var item in Split)
                {
                    int itemId = item.GetInteger();
                    if (itemId != 0)
                    {
                        QueryStr = QueryStr.Or(p =>
                            p.StoreProduct.Any(s => s.Store.StoreCompetitiveFeature.Any(x => x.CompetitiveFeatureId == itemId) && s.StoreProductQuantity.Count > 0)
                        );
                    }
                }
                MyQuery = MyQuery.And(QueryStr);
            }

            if (IsNullOrEmptyId(shopId) == false)
            {
                var QueryStr = PredicateBuilder.False<Product>();
                string[] Split = shopId.Split('-');
                foreach (var item in Split)
                {
                    int itemId = item.GetInteger();
                    if (itemId != 0)
                    {
                        QueryStr = QueryStr.Or(p =>
                            p.ShopId == itemId
                        );
                    }
                }
                MyQuery = MyQuery.And(QueryStr);
            }
            if (IsNullOrEmptyId(option) == false)
            {
                var QueryStr = PredicateBuilder.False<Product>();
                string[] Split = option.Split('*');
                foreach (var item in Split)
                {
                    int itemId = item.GetInteger();
                    if (itemId != 0)
                    {
                        QueryStr = QueryStr.Or(p =>
                            p.ProductQuantity.Any(s => s.ProductOptionItem.ID == itemId)
                        );
                    }
                }
                MyQuery = MyQuery.And(QueryStr);
            }


            if (IsNullOrEmptyId(prcustomLabel) == false)
                MyQuery = MyQuery.And(p => p.ProductSubCategory.CustomLabel == prcustomLabel || p.ProductType.CustomLabel == prcustomLabel || p.ProductCategory.CustomLabel == prcustomLabel);



            int listLanguage = BaseLanguage.GetWebsiteLanguages().Count();


            if (IsNullOrEmptyId(name) == false)
            {
                var nameQuery = PredicateBuilder.True<Product>();
                name = name.ToPersianChar();
                string[] nameSplit = name.Split(' ');
                foreach (var item in nameSplit)
                {
                    nameQuery = nameQuery.And(p =>
                    p.Name.Contains(item) ||
                    //p.ID.ToString() == item ||
                    //(p.ProductSubCategoryId != null && p.ProductSubCategory.Name.Contains(item)) ||
                    //(p.CodeValue.Contains(item)) ||
                    //(p.ProductCategoryId != null && p.ProductCategory.Name.Contains(item)) ||
                    //(p.ProductTypeId != null && p.ProductType.Name.Contains(item)) ||
                    //(p.ProductBrand.Name.Contains(item)) ||
                  ((listLanguage > 1) && (
                    (p.ProductLanguage.Any() && p.ProductLanguage.Any(x => x.Name.Contains(item)))
                     //||
                     // (p.ProductSubCategoryId != null && p.ProductSubCategory.ProductSubCategoryLanguage.Any() && p.ProductSubCategory.ProductSubCategoryLanguage.Any(x => x.Name.Contains(item))) ||
                     // (p.ProductCategoryId != null && p.ProductCategory.ProductCategoryLanguage.Any() && p.ProductCategory.ProductCategoryLanguage.Any(x => x.Name.Contains(item))) ||
                     // (p.ProductType != null && p.ProductType.ProductTypeLanguage.Any() && p.ProductType.ProductTypeLanguage.Any(x => x.Name.Contains(item)))
                     )));
                }
                MyQuery = MyQuery.And(nameQuery);
            }

            if (IsNullOrEmptyId(custom) == false)
            {
                var QueryStr = PredicateBuilder.False<Product>();
                string[] searchParts = custom.Split('*');
                foreach (string oneItem in searchParts)
                {
                    string[] splitedOne = oneItem.Split('@');
                    int fieldId = splitedOne[0].GetInteger();
                    int itemId = splitedOne[1].GetInteger();

                    if (itemId != 0)
                    {
                        QueryStr = QueryStr.Or(p =>
                            p.ProductCustomValue.Any(s => s.FieldId == fieldId && s.ItemId == itemId)
                        );
                    }

                }
                MyQuery = MyQuery.And(QueryStr);

            }

            if (SyncNameProductCustomField != null && SyncNameProductCustomField != string.Empty)
            {
                var itemQuery = PredicateBuilder.True<Product>();

                MyQuery = MyQuery.And(s => s.ProductCustomValue.Any(
                      p => p.ProductCustomField.SyncName == SyncNameProductCustomField
                      && (
                            (p.ItemId != null && p.ProductCustomItem.Value == valueProductCustom) ||
                      p.Value == valueProductCustom)
                      )
                    );
                MyQuery = MyQuery.And(itemQuery);
            }
            //if (distance > 1 && (!string.IsNullOrEmpty(latitude)) && (!string.IsNullOrEmpty(longtude)))
            //{
            //    var latLngQuery = PredicateBuilder.True<Product>();
            //    latLngQuery.And(s => (TrueDistance(s.Latitude, s.Longitude, latitude, longtude, distance) == true));
            //    MyQuery = MyQuery.And(latLngQuery);
            //}

            if (activeLocation == true)
            {
                if (BaseWebsite.WebsiteSetting.HasStore)
                {
                    var location = BaseWebsite.CurrentLocation;

                    MyQuery = MyQuery.And(p => p.StoreProduct.Any(s => s.Store.ShippingCity.Any(x =>
                       (x.CountryId == location.CountryId || x.CountryId == 0) &&
                       (x.CityId == location.CityId || x.CityId == 0) &&
                       (x.StateId == location.StateId || x.StateId == 0))));
                }
            }




            return MyQuery;
        }

        public ProductSearchVm SearchVm(
            int pageSize = 8,
             bool? hasExpire = null,
             bool IsHidden=false,
            bool? isLat = null,
            int index = 1,
            bool? HasEuro = null,
            string Id = null,
            int? notId = null,
            string typeId = null,
            string ProductsId=null,
            string categoryId = null,
            string subCategoryId = null,
            string colorId = null,
            string sizeId = null,
             string customFieldId = null,
            string option = null,
            double? pricefrom = null,
            double? priceto = null,
            string brandId = null,
            string storeId = null,
            string name = null,
            string summery = null,
            bool? hasDiscount = null,
            string custom = null,
            bool? active = true,
            bool? showHome = null,
            bool? isService = null,
            string latitude = null,
            string longitude = null,
              int? stateId = null,
            int? cityId = null,
            int distance = 0,
            string SyncNameProductCustomField = null,
            string valueProductCustom = null,
            bool? prstock = null,
            string prcustomLabel = "",
            int? discountGroupId = null,
            DateTime? fromDate = null,
            DateTime? toDate = null,
            Enum_ProductOrder order = Enum_ProductOrder.NONE,
            Enum_Code? status = null,
            bool? showAll = null
            )
        {
            IQueryable<Product> queryResults = null;
            var result = new ProductSearchVm();
            result.PageIndex = index;
            result.PageSize = pageSize;
            int skipValue = pageSize * (index - 1);
            int pageValue = pageSize;

            var query = GetSearchQuery2(
                IsHidden: IsHidden,
                                     Id: Id,
                                     ProductsId: ProductsId,
                                     hasExpire: hasExpire,
                                     isLat: isLat,
                                     notId: notId,
                                     categoryId: categoryId,
                                     hasDiscount: hasDiscount,
                                     name: name,
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
                                     HasEuro: HasEuro,
                                     custom: custom,
                                     active: active,
                                     option: option,
                                     showHome: showHome,
                                     isService: isService,
                                     latitude: latitude,
                                     longitude: longitude,
                                      cityId: cityId,
                                     stateId: stateId,
                                     distance: distance,
                                     prstock: prstock,
                                     prcustomLabel: prcustomLabel,
                                     SyncNameProductCustomField: SyncNameProductCustomField,
                                     valueProductCustom: valueProductCustom,
                                     status: status,
                                     discountGroupId: discountGroupId
                                     );

            string order_status_post = Enum_Code.ORDER_STATUS_POST.ToString();

            string order_status_success = Enum_Code.ORDER_STATUS_SUCCESS.ToString();
            IQueryable<Product> q = _context.Product.AsNoTracking().Where(query);
            if (distance > 0 && (!string.IsNullOrEmpty(latitude)) && (!string.IsNullOrEmpty(longitude)))
            {
                q = from s in q
                    where TrueDistance(s.Latitude, s.Longitude, latitude, longitude, distance) == true
                    select s;
            }
            if (order == Enum_ProductOrder.NONE)
                queryResults = q
                    .OrderBy(p => p.Code.Value)
                    .ThenBy(p => p.ID)
                    .Skip(skipValue)
                    .Take(pageValue)
                    ;
            else if (order == Enum_ProductOrder.NEW)
                queryResults = q
                    .OrderByDescending(p => p.ID)
                    //.ThenByDescending(p => p.ID)
                    .Skip(skipValue)
                    .Take(pageValue)
                    ;
            else if (order == Enum_ProductOrder.UPDATE)
                queryResults = q
                    .OrderBy(p => p.Code.Value)
                    .ThenBy(p => p.UpdateDatetime)
                    .Skip(skipValue)
                    .Take(pageValue)
                    ;
            else if (order == Enum_ProductOrder.UPDATE_DESC)
                queryResults = q
                    .OrderBy(p => p.Code.Value)
                    .ThenByDescending(p => p.UpdateDatetime)
                    .Skip(skipValue)
                    .Take(pageValue)
                    ;
            else if (order == Enum_ProductOrder.NEW_SYNC)
                queryResults = q
                    .OrderBy(p => p.Code.Value)
                    .ThenByDescending(p => p.SyncDatetime)
                    .Skip(skipValue)
                    .Take(pageValue)
                    ;
            else if (order == Enum_ProductOrder.OLD)
                queryResults = q
                    .OrderBy(p => p.Code.Value)
                    .ThenBy(p => p.ID)
                    .Skip(skipValue)
                    .Take(pageValue)
                    ;
            else if (order == Enum_ProductOrder.RANDOM)
                queryResults = q
                    .OrderBy(p => p.Code.Value)
                    .ThenBy(p => Guid.NewGuid())
                    .Skip(skipValue)
                    .Take(pageValue)
                    ;
            else if (order == Enum_ProductOrder.PRICE_MAX_TO_MIN)
                queryResults = q
                    .OrderBy(p => p.Code.Value)
                    .ThenByDescending(p => p.MinPrice > 0 ? p.MinPrice : p.Price)
                    .Skip(skipValue)
                    .Take(pageValue)
                    ;
            else if (order == Enum_ProductOrder.PRICE_MIN_TO_MAX)
                queryResults = q
                    .OrderBy(p => p.Code.Value)
                    .ThenBy(p => p.MinPrice > 0 ? p.MinPrice : p.Price)
                    .Skip(skipValue)
                    .Take(pageValue)
                    ;
            else if (order == Enum_ProductOrder.MORE_SELL)
                queryResults = q
                    .OrderBy(p => p.Code.Value)
                    .ThenByDescending(p => p.AccountOrderProduct.Count(qu =>
                        qu.AccountOrder.Code.Label == order_status_post ||
                        qu.AccountOrder.Code.Label == order_status_success))
                    .Skip(skipValue)
                    .Take(pageValue)
                    ;
            else if (order == Enum_ProductOrder.LESS_SELL)
                queryResults = q
                    .OrderBy(p => p.Code.Value)
                    .ThenBy(p => p.AccountOrderProduct.Count(qu =>
                        qu.AccountOrder.Code.Label == order_status_post ||
                        qu.AccountOrder.Code.Label == order_status_success))
                    .Skip(skipValue)
                    .Take(pageValue)
                    ;
            else if (order == Enum_ProductOrder.SHOWNUMBER)
                queryResults = q
                    .OrderBy(p => p.ShowNumber)
                    .ThenBy(p => p.Code.Value)
                    .ThenByDescending(p => p.ID)
                    .Skip(skipValue)
                    .Take(pageValue)
                    ;
            else if (order == Enum_ProductOrder.SHOWNUMBER_DESC)
                queryResults = q
                    .OrderBy(p => p.Code.Value)
                    .ThenByDescending(p => p.ShowNumber)
                    .Skip(skipValue)
                    .Take(pageValue)
                    ;
            else if (order == Enum_ProductOrder.MORE_VISIT)
                queryResults = q
                    .OrderBy(p => p.Code.Value)
                    .ThenByDescending(p => p.VisitCount)
                    .Skip(skipValue)
                    .Take(pageValue)
                    ;
            else if (order == Enum_ProductOrder.MORE_LIKE)
                queryResults = q
                    .OrderBy(p => p.Code.Value)
                    .ThenByDescending(p => p.ProductLike.Count)
                    .Skip(skipValue)
                    .Take(pageValue)
                    ;
            else if (order == Enum_ProductOrder.MORE_Rate)
                queryResults = q
                    .OrderBy(p => p.Code.Value)
                    .ThenByDescending(p => p.ProductComment.Where(s => s.Rate != null).Average(s => s.Rate))
                    .Skip(skipValue)
                    .Take(pageValue)
                    ;
            result.TotalCount = q.Count();
            if (result.TotalCount > 0)
                result.MaxPrice = Max(s => s.MinPrice);
            if (showAll == true || (ProductsId!=null && ProductsId!=""))
            {
                result.Products2 = queryResults.Map<IQueryable<Product>, List<ProductShowVm>>();

            }
            else
            {
                result.Products = queryResults.Map<IQueryable<Product>, List<ProductMinVm>>();

            }
            return result;
        }
        public ProductSearchVm SearchVm2(

           string answer = null,
           int pageSize = 1000,
            int index = 1,
           bool? prstock = null,
           string prcustomLabel = "",
           double? pricefrom = null,
           double? priceto = null,
           bool? showAll = null
         )
        {
            IQueryable<Product> queryResults = null;
            var result = new ProductSearchVm();
            result.PageSize = pageSize;
            int skipValue = pageSize * (index - 1);

            int pageValue = pageSize;

            var query = GetSearchQuery3(pricefrom: pricefrom,
                                         priceto: priceto,
                                         answer: answer,
                                         prstock: prstock,
                                         prcustomLabel: prcustomLabel

                                         );

            string order_status_post = Enum_Code.ORDER_STATUS_POST.ToString();

            string order_status_success = Enum_Code.ORDER_STATUS_SUCCESS.ToString();
            IQueryable<Product> q = _context.Product.AsNoTracking().Where(query);

            queryResults = q
                .OrderBy(p => p.Code.Value)
                .ThenByDescending(p => p.ID)
                .Skip(skipValue)
                .Take(pageValue)
                ;

            result.TotalCount = q.Count();
            if (result.TotalCount > 0)
                result.MaxPrice = Max(s => s.MinPrice);
            if (showAll == true)
            {
                result.Products2 = queryResults.Map<IQueryable<Product>, List<ProductShowVm>>();

            }
            else
            {
                result.Products = queryResults.Map<IQueryable<Product>, List<ProductMinVm>>();

            }
            return result;





        }

        public Expression<Func<Product, bool>> GetSearchQuery2(
            string Id = null,
             bool? hasExpire = null,
            bool? isLat = null,
            int? notId = null,
            bool? HasEuro = null,
            string typeId = null,
            string categoryId = null,
            string subCategoryId = null,
            string colorId = null,
            string customFieldId = null,
            string sizeId = null,
            string option = null,
            string ProductsId=null,
            double? pricefrom = null,
            double? priceto = null,
            string brandId = null,
            string storeId = null,
            string name = null,
            string summery = null,
            bool? hasDiscount = null,
            string custom = null,
            bool? active = true,
            bool? showHome = null,
            bool? isService = null,
            string latitude = null,
            string longitude = null,
             int? cityId = null,
            int? stateId = null,
            int distance = 0,
            string SyncNameProductCustomField = null,
            string valueProductCustom = null,
            bool? prstock = null,
            string prcustomLabel = "",
                        int? discountGroupId = null,

            DateTime? fromDate = null,
            DateTime? toDate = null,
            Enum_Code? status = null,
            bool? IsHidden=false

            )
        {

            var MyQuery = PredicateBuilder.True<Product>();
            MyQuery = MyQuery.And(p => p.Deleted == false);

            if (IsHidden != true)
            {
                MyQuery = MyQuery.And(p => p.IsHidden != true);
            }
            
            if (HasEuro != null)
            {
                MyQuery = MyQuery.And(p => p.HasEuro == HasEuro);
            }
            if (active != null)
                MyQuery = MyQuery.And(p => p.Active == active);
            if (cityId != null)
                    MyQuery = MyQuery.And(p => p.CityId == cityId);
            if (stateId != null)
                MyQuery = MyQuery.And(p => p.StateId == stateId);
            if (status.HasValue)
                MyQuery = MyQuery.And(p => p.Code.Label == status.ToString());
            if (prstock == true)
                MyQuery = MyQuery.And(p => p.Code.Label == Enum_Code.PRODUCT_STATUS_AVAILABLE.ToString());
            else if (prstock == false)
                MyQuery = MyQuery.And(p => p.Code.Label == Enum_Code.PRODUCT_STATUS_NOT_AVAILABLE.ToString());
            if (discountGroupId.HasValue)
            {
                MyQuery = MyQuery.And(p => p.DiscountGroupId == discountGroupId);

            }
            if (isLat == true)
            {
                MyQuery = MyQuery.And(p => p.Latitude != null && p.Longitude != null);

            }
            if (hasExpire == false)
            {
                DateTime now = DateTime.Now;
                MyQuery = MyQuery.And(p => (p.StartDatetime == null || (p.StartDatetime != null && p.StartDatetime <= now)) &&
                        (p.EndDatetime == null || (p.EndDatetime != null && p.EndDatetime >= now)));

            }
            if (IsNullOrEmptyId(prcustomLabel) == false && prcustomLabel.ToLower() != "all")
                MyQuery = MyQuery.And(p =>
                p.ProductSubCategory.CustomLabel == prcustomLabel
                || p.ProductType.CustomLabel == prcustomLabel
                || p.ProductCategory.CustomLabel == prcustomLabel
                || p.ProductExteraType.Any(s => s.ProductType.CustomLabel == prcustomLabel)
                || p.ProductExteraCategory.Any(s => s.ProductCategory.CustomLabel == prcustomLabel)
                || p.ProductExteraSubCategory.Any(s => s.ProductSubCategory.CustomLabel == prcustomLabel)
                );
            if (!string.IsNullOrEmpty(SyncNameProductCustomField))
            {
                var itemQuery = PredicateBuilder.True<Product>();
                MyQuery = MyQuery.And(s => s.ProductCustomValue.Any(
                      p => p.ProductCustomField.SyncName.ToLower() == SyncNameProductCustomField.ToLower()
                      && (
                            (p.ItemId != null && p.ProductCustomItem.Value.ToLower() == valueProductCustom.ToLower()) ||
                      p.Value.ToLower() == valueProductCustom.ToLower())
                      )
                    );
                MyQuery = MyQuery.And(itemQuery);
            }

            if (pricefrom != null)
                MyQuery = MyQuery.And(p => p.InexpensiveItemPrice >= pricefrom);
            if (priceto != null)
                MyQuery = MyQuery.And(p => p.InexpensiveItemPrice <= priceto);
            if (!string.IsNullOrEmpty(Id))
            {
                var QueryStr = PredicateBuilder.False<Product>();
                string[] Split = Id.Split('-');
                foreach (var item in Split)
                {
                    int itemId = item.GetInteger();
                    if (itemId != 0)
                    {
                        QueryStr = QueryStr.Or(p =>
                            p.ID == itemId
                        );
                    }
                }
                MyQuery = MyQuery.And(QueryStr);
            }
            if (notId != null && notId != 0)
                MyQuery = MyQuery.And(p => p.ID != notId);
            if (IsNullOrEmptyId(typeId) == false)
            {
                var QueryStr = PredicateBuilder.False<Product>();
                string[] Split = typeId.Split('-');
                foreach (var item in Split)
                {
                    int itemId = item.GetInteger();
                    if (itemId != 0)
                    {
                        QueryStr = QueryStr.Or(p =>
                            p.ProductTypeId == itemId
                        );
                    }
                }
                MyQuery = MyQuery.And(QueryStr);
            }
            if (IsNullOrEmptyId(ProductsId) == false)
            {
                var QueryStr = PredicateBuilder.False<Product>();
                string[] Split = ProductsId.Split(',');
                foreach (var item in Split)
                {
                    if(item!="" && item != null)
                    {
                        int itemId = item.GetInteger();
                        if (itemId != 0)
                        {
                            QueryStr = QueryStr.Or(p =>
                                p.ID == itemId
                            );
                        }
                    }
                    
                }
                MyQuery = MyQuery.And(QueryStr);
            }
            
            if (IsNullOrEmptyId(categoryId) == false)
            {
                var QueryStr = PredicateBuilder.False<Product>();
                string[] Split = categoryId.Split('-');
                foreach (var item in Split)
                {
                    int itemId = item.GetInteger();
                    if (itemId != 0)
                    {
                        QueryStr = QueryStr.Or(p =>
                            p.ProductCategoryId == itemId
                        );
                    }
                }
                MyQuery = MyQuery.And(QueryStr);
            }

            if (IsNullOrEmptyId(subCategoryId) == false)
            {
                var QueryStr = PredicateBuilder.False<Product>();
                string[] Split = subCategoryId.Split('-');
                foreach (var item in Split)
                {
                    int itemId = item.GetInteger();
                    if (itemId != 0)
                    {
                        QueryStr = QueryStr.Or(p =>
                            p.ProductSubCategoryId == itemId
                        );
                    }
                }
                MyQuery = MyQuery.And(QueryStr);
            }
            if (IsNullOrEmptyId(colorId) == false)
            {
                var QueryStr = PredicateBuilder.False<Product>();
                string[] Split = colorId.Split('-');
                foreach (var item in Split)
                {
                    int itemId = item.GetInteger();
                    if (itemId != 0)
                    {
                        QueryStr = QueryStr.Or(p =>
                            p.ProductColor.Any(a => a.ColorId == itemId)
                        );
                    }
                }
                MyQuery = MyQuery.And(QueryStr);
            }
            if (IsNullOrEmptyId(customFieldId) == false)
            {
                var QueryStr = PredicateBuilder.True<Product>();
                string[] Split = customFieldId.Split('-');
                foreach (var item in Split)
                {
                    int itemId = item.GetInteger();
                    if (itemId != 0)
                    {
                        QueryStr = QueryStr.And(p =>
                            p.ProductCustomValue.Any(a => a.ItemId == itemId)
                        );
                    }
                }
                MyQuery = MyQuery.And(QueryStr);
            }

            if (IsNullOrEmptyId(sizeId) == false)
            {
                var QueryStr = PredicateBuilder.False<Product>();
                string[] Split = sizeId.Split('-');
                foreach (var item in Split)
                {
                    int itemId = item.GetInteger();
                    if (itemId != 0)
                    {
                        QueryStr = QueryStr.Or(p =>
                            p.ProductSize.Any(a => a.SizeId == itemId)
                        );
                    }
                }
                MyQuery = MyQuery.And(QueryStr);
            }
            if (IsNullOrEmptyId(option) == false)
            {
                var QueryStr = PredicateBuilder.False<Product>();
                string[] Split = option.Split('*');
                foreach (var item in Split)
                {
                    int itemId = item.GetInteger();
                    if (itemId != 0)
                    {
                        QueryStr = QueryStr.Or(p =>
                            p.ProductQuantity.Any(s => s.ProductOptionItem.ID == itemId)
                        );
                    }
                }
                MyQuery = MyQuery.And(QueryStr);
            }

            if (IsNullOrEmptyId(brandId) == false)
            {
                var QueryStr = PredicateBuilder.False<Product>();
                string[] Split = brandId.Split('-');
                foreach (var item in Split)
                {
                    int itemId = item.GetInteger();
                    if (itemId != 0)
                    {
                        QueryStr = QueryStr.Or(p =>
                            p.BrandId == itemId
                        );
                    }
                }
                MyQuery = MyQuery.And(QueryStr);
            }
            if (IsNullOrEmptyId(storeId) == false)
            {
                var QueryStr = PredicateBuilder.False<Product>();
                string[] Split = storeId.Split('-');
                foreach (var item in Split)
                {
                    int itemId = item.GetInteger();
                    if (itemId != 0)
                    {
                        QueryStr = QueryStr.Or(p =>
                            p.StoreProduct.Any(s => s.StoreId == itemId)
                        );
                    }
                }
                MyQuery = MyQuery.And(QueryStr);
            }
            if (IsNullOrEmptyId(name) == false)
            {
                var nameQuery = PredicateBuilder.True<Product>();
                var nameEn = name;
                name = name.ToPersianChar();

                string[] nameSplit = name.Split(' ');
                foreach (var item in nameSplit)
                {

                    nameQuery = nameQuery.And(p =>
                    p.Name.Contains(item) || p.Name.Contains(nameEn)
                    || p.State.Name.Contains(item) || p.State.Name.Contains(nameEn)
                    || p.City.Name.Contains(item) || p.City.Name.Contains(nameEn)

                    || (!string.IsNullOrEmpty(p.CodeValue) && p.CodeValue.ToLower() == name.ToLower())
                    || (p.ProductLanguage.Any()
                  && p.ProductLanguage.Any(x => x.Name.Contains(item) || x.Name.Contains(nameEn))));
                }


                MyQuery = MyQuery.And(nameQuery);
            }
            if (IsNullOrEmptyId(summery) == false)
            {
                var summeryQuery = PredicateBuilder.True<Product>();
                summery = summery.ToPersianChar();
                string[] summerySplit = summery.Split(' ');
                foreach (var item in summerySplit)
                {
                    summeryQuery = summeryQuery.And(p =>
                    p.Summary.Contains(item)
                    || (p.ProductLanguage.Any()
                  && p.ProductLanguage.Any(x => x.Summary.Contains(item))));
                }
                MyQuery = MyQuery.And(summeryQuery);
            }

            if (hasDiscount != null)
            {
                if (hasDiscount.Value == true)
                {
                    MyQuery = MyQuery.And(p => p.DiscountValue > 0);
                }
                else
                {
                    MyQuery = MyQuery.And(p => p.DiscountValue == 0);
                }
            }
            if (IsNullOrEmptyId(custom) == false)
            {
                var QueryStr = PredicateBuilder.False<Product>();
                string[] searchParts = custom.Split('*');
                foreach (string oneItem in searchParts)
                {
                    string[] splitedOne = oneItem.Split('@');
                    int fieldId = splitedOne[0].GetInteger();
                    int itemId = splitedOne[1].GetInteger();

                    if (itemId != 0)
                    {
                        QueryStr = QueryStr.Or(p =>
                            p.ProductCustomValue.Any(s => s.FieldId == fieldId && s.ItemId == itemId)
                        );
                    }

                }
                MyQuery = MyQuery.And(QueryStr);

            }

            if (showHome != null)
                MyQuery = MyQuery.And(p => p.ShowHomePage == showHome);
            if (isService != null)
                MyQuery = MyQuery.And(p => p.IsService == isService);

            if (fromDate != null && fromDate != default(DateTime))
                MyQuery = MyQuery.And(p => p.CreateDateTime >= fromDate);
            if (toDate != null && toDate != default(DateTime))
            {
                DateTime toDatetimeTemp = toDate.Value.AddDays(1);
                MyQuery = MyQuery.And(p => p.CreateDateTime <= toDatetimeTemp);
            }
            return MyQuery;
        }
        public Expression<Func<Product, bool>> GetSearchQuery3(
           string answer = null,
           bool? prstock = null,
           string prcustomLabel = "",
           double? pricefrom = null,
           double? priceto = null)
        {

            var MyQuery = PredicateBuilder.True<Product>();
            MyQuery = MyQuery.And(p => p.Deleted != true);
            MyQuery = MyQuery.And(p => p.Active == true);
            if (prstock == true)
                MyQuery = MyQuery.And(p => p.Code.Label == Enum_Code.PRODUCT_STATUS_AVAILABLE.ToString());
            else if (prstock == false)
                MyQuery = MyQuery.And(p => p.Code.Label == Enum_Code.PRODUCT_STATUS_NOT_AVAILABLE.ToString());

            if (IsNullOrEmptyId(answer) == false)
            {
                var QueryStr = PredicateBuilder.True<Product>();
                string[] Split = answer.Split('-');
                foreach (var item in Split)
                {
                    if (item != "")
                    {
                        QueryStr = QueryStr.And(p =>
                                                  p.ProductCustomValue.Any(s => s.Value.Contains(item)));

                    }


                }
                MyQuery = MyQuery.And(QueryStr);
            }
            if (pricefrom != null)
                MyQuery = MyQuery.And(p => p.MinPrice >= pricefrom);
            if (priceto != null)
                MyQuery = MyQuery.And(p => p.MinPrice <= priceto);
            return MyQuery;
        }
        public bool TrueDistance(string lat1, string lng1, string lat2, string lng2, int distanceKm)
        {
            if ((!string.IsNullOrEmpty(lat1)) && (!string.IsNullOrEmpty(lat2)) && (!string.IsNullOrEmpty(lng1)) && (!string.IsNullOrEmpty(lng2)) && distanceKm > 0)
            {
                double dLat1 = double.Parse(lat1);
                double dLng1 = double.Parse(lng1);
                double dLat2 = double.Parse(lat2);
                double dLng2 = double.Parse(lng2);
                var coord1 = new GeoCoordinate(dLat1, dLng1);
                var coord2 = new GeoCoordinate(dLat2, dLng2);
                double meterDistance = distanceKm * 1000;
                var currentDistance = coord1.GetDistanceTo(coord2);
                if (currentDistance <= meterDistance)
                {
                    return true;
                }
            }
            return false;
        }

        public void IncreaseSellProduct(int productId)
        {
            Product productModel = _context.Product.Find(productId);
            productModel.SellCount = productModel.SellCount + 1;
            _context.SaveChanges();
        }
        public void UpdateProductQuantity(int productId)
        {

            var unitOfWork = new UnitOfWork();
            var product = _context.Product.Find(productId);
            var storeProductQuantity = unitOfWork.StoreProductQuantity.GetActives(productId).ToApi();
            if (storeProductQuantity.Any())
            {

                product.MinPrice = storeProductQuantity.Min(s => s.Price);
                if (product.MinPrice > 0)
                {
                    product.StatusId = _context.Code.FirstOrDefault(s => s.Label == Enumarables.Enum_Code.PRODUCT_STATUS_AVAILABLE.ToString()).ID;
                }
                else
                {
                    product.StatusId = _context.Code.FirstOrDefault(s => s.Label == Enumarables.Enum_Code.PRODUCT_STATUS_NOT_AVAILABLE.ToString()).ID;

                }
            }
            else
            {
                product.MinPrice = 0;
                product.StatusId = _context.Code.FirstOrDefault(s => s.Label == Enumarables.Enum_Code.PRODUCT_STATUS_NOT_AVAILABLE.ToString()).ID;
            }
            Save();
            unitOfWork.Dispose();
        }
        public void Hide(int productId)
        {
            var product = GetById(productId);
            product.Deleted = true;
        }



        public bool ProductIsSalable(int productId, bool isEarnest)
        {
            var model = GetById(productId);
            if (model != null && model.Active && (!model.Deleted) && (model.Code.Label == Enum_Code.PRODUCT_STATUS_AVAILABLE.ToString() || isEarnest))
            {
                return true;
            }
            return false;
        }
        public ProductShowVm GetProduct2(int productId, bool visit = false, int? accountId = null)
        {
            var product = GetById(productId);
            if (product != null  && product.Deleted != true)
            {
                if (visit)
                {
                    IncreaseVisit(product);
                }
                if (accountId.HasValue)
                {
                    var productVisit = product.ProductVisit.FirstOrDefault(a => a.AccountId == accountId);
                    if (productVisit != null)
                    {
                        productVisit.DateVisit = DateTime.Now;
                    }
                    else
                    {
                        var productVistes = _context.ProductVisit.Where(a => a.AccountId == accountId).OrderBy(o => o.DateVisit);
                        if (productVistes.Count() > 4)
                        {
                            productVistes.First().DateVisit = DateTime.Now;
                            productVistes.First().ProductId = productId;

                        }
                        else
                        {
                            _context.ProductVisit.Add(new ProductVisit() { AccountId = accountId, ProductId = productId, DateVisit = DateTime.Now });
                        }

                    }
                    Save();
                }
                return product.Map<Product, ProductShowVm>();

            }
            return null;
        }

        public ProductShowVm GetProduct(int productId, bool visit = false, int? accountId = null)
        {
            var product = GetById(productId);
            if (product != null && product.Active && product.Deleted != true)
            {
                if (visit)
                {
                    IncreaseVisit(product);
                }
                if (accountId.HasValue)
                {
                    var productVisit = product.ProductVisit.FirstOrDefault(a => a.AccountId == accountId);
                    if (productVisit != null)
                    {
                        productVisit.DateVisit = DateTime.Now;
                    }
                    else
                    {
                        var productVistes = _context.ProductVisit.Where(a => a.AccountId == accountId).OrderBy(o => o.DateVisit);
                        if (productVistes.Count() > 4)
                        {
                            productVistes.First().DateVisit = DateTime.Now;
                            productVistes.First().ProductId = productId;

                        }
                        else
                        {
                            _context.ProductVisit.Add(new ProductVisit() { AccountId = accountId, ProductId = productId, DateVisit = DateTime.Now });
                        }

                    }
                    Save();
                }
                return product.Map<Product, ProductShowVm>();

            }
            return null;
        }
        public bool GetProductLike(int productId, int accountId)
        {
            var product = GetById(productId);
            if (product != null)
            {
                var productLike = product.ProductLike.FirstOrDefault(a => a.AccountId == accountId);
                if (productLike != null)
                {
                    return true;
                }
                else
                {
                    return false;

                }
            }
            return false;
        }
        public void IncreaseVisit(int productId)
        {
            var item = GetById(productId);
            IncreaseVisit(item);
        }
        public void IncreaseVisit(Product product)
        {
            try
            {
                if (product.VisitCount == null)
                    product.VisitCount = 1;
                else
                {
                    product.VisitCount++;
                }
                Update(product);
                Save();
            }
            catch
            {

            }

        }

        public ProductCompareVm GetProductsForCompare(UnitOfWork unitOfWork, int[] products)
        {
            var result = new ProductCompareVm();
            bool first = true;
            if (products != null && products.Any())
            {
                int[] take4Products = products.Reverse().Take(4).ToArray();
                foreach (var productId in take4Products)
                {
                    var product = GetById(productId);
                    if (product != null && product.Deleted != true)
                    {
                        result.Products.Add(product.Map<Product, ProductShowVm>());
                        if (first)
                        {
                            var customFields = unitOfWork.ProductCustomField.GetAllForProduct(product);
                            result.CustomFields = customFields;
                            result.SubCategoryId = product.ProductSubCategoryId;
                        }
                        first = false;
                    }
                }
                result.Count = result.Products.Count;
            }
            return result;
        }
        public ProductCreateVm GetForEdit(int productId)
        {
            var product = GetById(productId);
            return product.Map<Product, ProductCreateVm>();
        }
       
        public void EditProductCustomValues(ProductCreateVm productVm)
        {
            var product = GetById(productVm.ID);
            foreach (var item in productVm.ProductCustomField)
            {
                var existCustom = product.ProductCustomValue.FirstOrDefault(x => x.FieldId == item.ID);
                if (existCustom != null)
                {
                    if (string.IsNullOrEmpty(item.Value) && item.ValueItemId == null)
                    {
                        _context.ProductCustomValue.Remove(existCustom);
                    }
                    else
                    {
                        existCustom.Value = item.Value;
                        existCustom.ItemId = item.ValueItemId;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(item.Value) == false || item.ValueItemId != null)
                    {
                        _context.ProductCustomValue.Add(new ProductCustomValue() { ProductId = productVm.ID, FieldId = item.ID, Value = item.Value, ItemId = item.ValueItemId });
                    }
                }
                Save();

            }
        }

        public List<Product> GetForDiscount(Discount discount)
        {

            var query = GetSearchQuery2(storeId: discount.StoreId.ToString(),
                typeId: discount.ProductTypeId.ToString(),
                categoryId: discount.ProductCategoryId.ToString(),
                subCategoryId: discount.ProductSubCategoryId.ToString(),
                brandId: discount.ProductBrandId.ToString(),
                Id: discount.ProductId.ToString(), prstock: true);
            return Where(query).ToList();
        }
        public List<ProductTypeVm> GetProductCategories(int productId)
        {

            var result = new List<ProductTypeVm>();
            var allCategories = _context.ProductType.Where(s => s.Deleted != true && s.Active == true).ToList();
            foreach (var itemType in allCategories)
            {
                var type = new ProductTypeVm();
                type = itemType.Map<ProductType, ProductTypeVm>();
                type.ProductCategory = new List<ProductCategoryVm>();

                if (itemType.ProductExteraType.Any(s => s.ProductId == productId))
                {
                    type.Select = true;
                }
                foreach (var itemCat in itemType.ProductCategory.Where(s => s.Deleted != true && s.Active == true).ToList())
                {
                    var cat = new ProductCategoryVm();

                    cat = itemCat.Map<ProductCategory, ProductCategoryVm>();
                    cat.ProductSubCategory = new List<ProductSubCategoryVm>();

                    if (itemCat.ProductExteraCategory.Any(s => s.ProductId == productId))
                    {
                        cat.Select = true;
                    }
                    foreach (var itemSubCat in itemCat.ProductSubCategory.Where(s => s.Deleted != true && s.Active == true).ToList())
                    {
                        var sub = new ProductSubCategoryVm();
                        sub = itemSubCat.Map<ProductSubCategory, ProductSubCategoryVm>();
                        if (itemSubCat.ProductExteraSubCategory.Any(s => s.ProductId == productId))
                        {
                            sub.Select = true;
                        }
                        cat.ProductSubCategory.Add(sub);
                    }
                    type.ProductCategory.Add(cat);
                }
                result.Add(type);
            }
            return result;
        }
        public List<Product> SearchBox(string name = null, int size = 5)
        {
            return Where(GetSearchQuery2(name: name, active: true)).OrderByDescending(s => s.ID).Take(size).ToList();
        }
    }
}
