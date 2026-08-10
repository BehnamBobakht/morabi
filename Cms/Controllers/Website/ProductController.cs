using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using static QRCoder.PayloadGenerator;
using Cms.Controllers.Website;
using System.Web;

namespace Cms.Controllers
{
    public class ProductController : Controller
    {
        UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(int prid, string pridname = "",int ? intercode=null)
        {
            int? accountId = null;
            if (intercode > 0)
            {
                HttpCookie locationCookie = new HttpCookie(".intercode");
                locationCookie.Values.Add("code", intercode.ToString());
                locationCookie.Expires = DateTime.Now.AddMonths(1);
                HttpContext.Response.Cookies.Add(locationCookie);
               
            }
           
            var currentAccount = BaseWebsite.CurrentAccount;
            if (currentAccount != null)
            {
                accountId = currentAccount.Id;
            }
            var product = new ProductShowVm();
            var cacheSetting = BaseCache.GetByLabel(Enum_OutputCache.ProductIndex);
            if (cacheSetting.Active && cacheSetting.Duration > 1)
            {
                var cache = HttpContext.Cache.Get("pr" + BaseLanguage.GetCurrentLanguageString() + prid);
                if (cache == null)
                {
                    product = _context.Product.GetProduct(prid, visit: true, accountId: accountId);
                    if (product == null)
                    {
                        return Redirect(BaseController.RedirectToError(errorCode: 410));
                    }
                    HttpContext.Cache.Add(key: "pr" + BaseLanguage.GetCurrentLanguageString() + prid, value: product, dependencies: null, absoluteExpiration: System.Web.Caching.Cache.NoAbsoluteExpiration, slidingExpiration: new TimeSpan(0, 0, cacheSetting.Duration), priority: System.Web.Caching.CacheItemPriority.Normal, null);
                }
                else
                {
                    product = (ProductShowVm)cache;
                    _context.Product.IncreaseVisit(product.ID);
                }
            }
            else
            {
                product = _context.Product.GetProduct(prid, visit: true, accountId: accountId);

            }
            if (product != null)
            {
                if (product.IsHidden == true)
                {
                    var package = _context.Product.FirstOrDefault(s => s.ProductsId.Contains(product.ID.ToString()));
                    if (package != null)
                    {
                        BaseUrl.Redirect301(package.GetLinkWithUrl());
                    }
                }
                else
                {
                    BaseUrl.Redirect301(product.ProductUrl);
                }

                return View(BaseController.GetView(this), model: product);
            }
            else
            {
                return Redirect(BaseController.RedirectToError(errorCode: 410));
            }
        }
        public ActionResult SearchAccountOrder(
              Enum_Code aoStatus = Enum_Code.ORDER_STATUS_NONE,
              string aoViewName = null, int? typeId = null, int? accountId = null, string statusId2 = "")
        {

            aoViewName = aoViewName == null ? "Search" : aoViewName;
            List<AccountOrderProduct> results = _context.AccountOrderProduct.Search(
                  accountId: accountId,
                  status: aoStatus, typeId: typeId, statusId2: statusId2);

            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, aoViewName, results);
        }
        public ActionResult SearchCourse(
              Enum_Code aoStatus = Enum_Code.ORDER_STATUS_NONE,
              string aoViewName = null, int? typeId = null, int? accountId = null)
        {

            aoViewName = aoViewName == null ? "Search" : aoViewName;
            List<Students> results = _context.Students.Where(s => s.UserId == accountId).ToList();

            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, aoViewName, results);
        }
        public ActionResult Error()
        {

            return View(BaseController.GetView(this));

        }
        public ActionResult IndexInner(ProductShowVm model, int productId, int? colorId = null, int? sizeId = null, int? optionItemId = null)
        {
            if (model == null)
            {
                model = _context.Product.GetProduct(productId);
            }
            model.Variety = BaseStore.GetProductVariety(_context: _context, productId: productId, colorId: colorId, sizeId: sizeId, optionItemId: optionItemId);
            ViewAccount CurrentUser = _context.Account.GetCurrentAccount();
            if (CurrentUser != null)
            {
                try
                {
                    var cart = _context.Cart.GetCartEntity(accountId: CurrentUser.Id);

                    if (cart != null)
                    {
                        
                            ViewBag.TestId = cart.CartStore.Any(s => s.CartStoreItem.Any(x => x.ProductId == productId || model.ProductsId.Contains(x.ProductId.ToString())));

                        

                    }
                }
                catch
                {

                }
            }
            return PartialView(BaseController.GetView(this), model: model);
        }
        [CustomCache(Duration = 1, cacheType = Enum_OutputCache.ProductIndex)]
        public ActionResult IndexDetails(ProductShowVm model, int productId,
                        string lang = "")
        {

            return PartialView(BaseController.GetView(this), model: model);
        }
        public ActionResult GetPrice( int productId)
        {
            ViewAccount CurrentUser = _context.Account.GetCurrentAccount();
            if (CurrentUser != null)
            {
                var prodcut = _context.Product.GetById(productId);
                var order = _context.AccountOrderProduct.Where(p => (p.ProductId == productId) && (p.AccountOrder.StatusId == 2068 || p.AccountOrder.StatusId == 2073) && p.AccountOrder.AccountId == CurrentUser.Id);
                double price = 0;
                foreach (var sub in order)
                {
                    price += sub.Price;
                }
                if (price > 0)
                {
                    double count = price / prodcut.MinPrice;
                    return PartialView(BaseController.GetView(this), model:(int)Math.Ceiling(count));
                }
                else
                {
                    return PartialView(BaseController.GetView(this), model: 0);
                }
                
            }
            else
            {
               

                return PartialView(BaseController.GetView(this), model: 0);
            }
        }
        public ActionResult IndexPartial(ProductShowVm model, int productId,
                        string lang = "")
        {
            if (model == null || model.ID == 0)
            {
                model = _context.Product.GetProduct(productId);
            }
            return PartialView(BaseController.GetView(this), model: model);
        }
        public ActionResult IndexProductLike(int productId, string prviewName = "IndexProductLike")
        {
            ViewInnerProductLike model = new ViewInnerProductLike();
            model.ProductId = productId;
            var account = _context.Account.GetCurrentAccount();
            if (productId != 0 && account != null)
            {
                var IsLike = _context.Product.GetProductLike(productId, account.Id);

                model.Islike = IsLike;
                return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, prviewName, model);

                //return PartialView(BaseController.GetView(this), model: model);
            }


            model.Islike = false;
            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, prviewName, model);

            //return PartialView(BaseController.GetView(this), model:model );
        }
        [CustomCache(Duration = 1, cacheType = Enum_OutputCache.ProductIndex)]
        public ActionResult IndexPictures(ProductShowVm model, int productId)
        {
            return PartialView(BaseController.GetView(this), model: model);
        }
        public ActionResult ShowModal()
        {
            var cart = new ResultVm<CartVm>();
            cart.Model = new CartVm();
            var currentCartAccount = BaseCart.GetCurrentCartAccount();
            if (currentCartAccount.AccountId.HasValue || (!string.IsNullOrEmpty(currentCartAccount.SyncCode)))
            {
                cart = BaseCart.GetCurrentCard(_context: _context, accountId: currentCartAccount.AccountId, cartSyncCode: currentCartAccount.SyncCode);
                if (cart.Status == ResultStatus.Success)
                    BaseCookie.SetCookie(key: CookieName.CardCookie.ToString(), cart.Model.SyncCode, 5);

            }
            return PartialView(BaseController.GetView(this), model: cart);
        }
        public ActionResult Search(
             int prpageSize = 20,
            int prindex = 1,
            string prId = null,
            int? notId = null,
            string typeId = null,
            string categoryId = null,
            string subCategoryId = null,
            string colorId = null,
            List<int> selectedColors = null,
            List<int> selectedCustomFields = null,
             string customFieldId = null,
            string sizeId = null,
            string option = null,
            double? pricefrom = null,
            double? priceto = null,
            string brandId = null,
            List<int> selectedBrands = null,
            string storeId = null,
            string prname = null,
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
            bool? hasExpire = false,

            int distance = 0,
            string SyncNameProductCustomField = null,
            string valueProductCustom = null,
            bool? prstock = null,
            string prcustomlabel = "all",
            DateTime? fromDate = null,
            DateTime? toDate = null,
            string prviewName = "Search",
            bool? HasEuro = null,
            bool? isLat = null,
            Enum_ProductOrder prorder = Enum_ProductOrder.NEW,
            Enum_ProductOutput proutput = Enum_ProductOutput.ENTITY,
            Enum_ResultType prresultType = Enum_ResultType.RESULT_TYPE_VIEWNAME,
            bool? showAll = null
            )
        {
            if(prcustomlabel== "exam")
            {
                prpageSize = 1000;
            }
            priceto = priceto == 0 ? null : priceto;
            if (selectedColors != null)
            {
                colorId = string.Join("-", selectedColors);
            }
            else
            {
                selectedColors = new List<int>();
                try
                {
                    if (!string.IsNullOrEmpty(colorId))
                    {
                        var stringList = colorId.Split('-').ToList();
                        foreach (var item in stringList)
                        {
                            selectedColors.Add(Convert.ToInt32(item));
                        }
                    }
                }
                catch
                {

                }

            }
            if (selectedCustomFields != null)
            {
                customFieldId = string.Join("-", selectedCustomFields);
            }
            else
            {
                selectedCustomFields = new List<int>();
                try
                {
                    if (!string.IsNullOrEmpty(customFieldId))
                    {
                        var stringList = customFieldId.Split('-').ToList();
                        foreach (var item in stringList)
                        {
                            selectedCustomFields.Add(Convert.ToInt32(item));
                        }
                    }
                }
                catch
                {

                }
            }
            if (selectedBrands != null)
            {
                brandId = string.Join("-", selectedBrands);
            }
            else
            {
                selectedBrands = new List<int>();
                try
                {
                    if (!string.IsNullOrEmpty(brandId))
                    {
                        var stringList = brandId.Split('-').ToList();
                        foreach (var item in stringList)
                        {
                            selectedBrands.Add(Convert.ToInt32(item));
                        }
                    }
                }
                catch
                {

                }
            }

            var result = new ProductSearchVm();

            if (prcustomlabel == "exam")
            {

                showAll = true;
            };
            result = _context.Product.SearchVm(
                                    Id: prId,
                                    hasExpire: hasExpire,
                                    isLat: isLat,
                                    notId: notId,
                                    index: prindex,
                                    pageSize: prpageSize,
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
                                    custom: custom,
                                    HasEuro: HasEuro,
                                    active: active,
                                    option: option,
                                    showHome: showHome,
                                    isService: isService,
                                    latitude: latitude,
                                    longitude: longitude,
                                     stateId: stateId,
                                    cityId: cityId,
                                    distance: distance,
                                    prstock: prstock,
                                    order: prorder,
                                    showAll: showAll,
                                    prcustomLabel: prcustomlabel,
                                    SyncNameProductCustomField: SyncNameProductCustomField,
                                    valueProductCustom: valueProductCustom);
            if (showAll == true)
            {
                List<ProductShowVm> newlist = new List<ProductShowVm>();
                foreach (var item in result.Products2)
                {
                    ProductShowVm temp = new ProductShowVm();
                    temp = _context.Product.GetProduct(item.ID);
                    temp.Variety = BaseStore.GetProductVariety(_context: _context, productId: item.ID);
                    newlist.Add(temp);
                }
                result.Products2 = newlist;
            }
            result.PageSize = prpageSize;
            result.PageIndex = prindex;
            result.Name = prname;
            result.BrandId = brandId;
            result.ColorId = colorId;
            result.SelectedColors = selectedColors;
            result.SelectedCustomFields = selectedCustomFields;
            result.SelectedBrands = selectedBrands;
            result.PriceFrom = pricefrom;
            result.PriceTo = priceto;
            if (cityId != null)
            {
                var city = _context.City.FirstOrDefault(s => s.ID == cityId);
                if (city != null)
                {
                    result.Latitude = city.Latitude;
                    result.Longitude = city.Longitude;
                    result.CityName = city.Name.ToPersianChar();


                }
            }
            if (stateId != null)
            {
                var city = _context.State.FirstOrDefault(s => s.ID == stateId);
                if (city != null)
                {
                    result.Latitude = city.Latitude;
                    result.Longitude = city.Longitude;
                    result.CityName = city.Name.ToPersianChar();


                }
            }
            if (prname != null && prname != "")
            {
                var city = _context.City.FirstOrDefault(s => s.Name.Contains(prname));
                if (city != null)
                {
                    result.Latitude = city.Latitude;
                    result.Longitude = city.Longitude;

                }
            }
            if (result.PriceTo == null || result.PriceTo == 0)
            {
                result.PriceTo = result.MaxPrice;
            }
            result.HasExpire = hasExpire;
            result.Prstock = prstock;
            result.prorder = prorder;
            result.PrcustomLabel = prcustomlabel;
            result.HasDiscount = hasDiscount;
            result = BaseStore.GetCategoriesByCustomLabel(_context: _context, productSearch: result, customLabel: prcustomlabel);
            if (result.IncorrectCustomLabel)
            {
                return Redirect(BaseController.RedirectToError());

            }
            return BaseController.GetView(this, prresultType, prviewName, result);
        }
        public ActionResult Landing(
             int prpageSize = 20,
            string prcustomlabel = "all",
            string prviewName = "Landing",
            Enum_ProductOrder prorder = Enum_ProductOrder.SHOWNUMBER,
            Enum_ResultType prresultType = Enum_ResultType.RESULT_TYPE_VIEWNAME
            )
        {

            var result = new ProductSearchVm();
            result = _context.Product.SearchVm(

                                    pageSize: prpageSize,

                                    order: prorder,
                                    prcustomLabel: prcustomlabel
                                   );
            result.PageSize = prpageSize;

            result.prorder = prorder;
            result.PrcustomLabel = prcustomlabel;
            result = BaseStore.GetCategoriesByCustomLabel(_context: _context, productSearch: result, customLabel: prcustomlabel);
            if (result.IncorrectCustomLabel)
            {
                return Redirect(BaseController.RedirectToError());

            }
            return BaseController.GetView(this, prresultType, prviewName, result);
        }
        [CustomCache(Duration = 1, cacheType = Enum_OutputCache.ProductSearch,
            VaryByParam = "*")]
        public ActionResult SearchPartial(
             int prpageSize = 8,
            int prindex = 1,
            bool IsHidden = false,
             bool? HasEuro = null,
             bool? isLat = null,
            string prId = null,
            int? notId = null,
            string typeId = null,
            string ProductsId = null,
            string categoryId = null,
            string subCategoryId = null,
            string colorId = null,
            List<int> selectedColors = null,
            List<int> selectedCustomFields = null,
             string customFieldId = null,

            string sizeId = null,
            string option = null,
            double? pricefrom = null,
            double? priceto = null,
            string brandId = null,
            List<int> selectedBrands = null,
            string storeId = null,
            string prname = null,
            string summery = null,
            bool? hasDiscount = null,
            string custom = null,
            bool? active = true,
            bool? Isactive = null,
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
            string prcustomlabel = "all",
            int? discountGroupId = null,
              bool? hasExpire = null,
            string prviewName = "",
            Enum_ProductOrder prorder = Enum_ProductOrder.NEW,
            Enum_ResultType prresultType = Enum_ResultType.RESULT_TYPE_VIEWNAME,
            string lang = ""
            )
        {
            if (Isactive == true)
            {
                active = null;
            }
            var result = new ProductSearchVm();
            result = _context.Product.SearchVm(
                                    Id: prId,
                                    IsHidden: IsHidden,
                                    ProductsId: ProductsId,
                                    isLat: isLat,
                                    notId: notId,
                                    hasExpire: hasExpire,
                                    HasEuro: HasEuro,
                                    index: prindex,
                                    pageSize: prpageSize,
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
                                    custom: custom,
                                    active: active,
                                    option: option,
                                    showHome: showHome,
                                    isService: isService,
                                    latitude: latitude,
                                    cityId: cityId,
                                    stateId: stateId,
                                    longitude: longitude,
                                    distance: distance,
                                    prstock: prstock,
                                    order: prorder,
                                    prcustomLabel: prcustomlabel,
                                    SyncNameProductCustomField: SyncNameProductCustomField,
                                    valueProductCustom: valueProductCustom,
                                    discountGroupId: discountGroupId
                                    );
            if (prcustomlabel != "all")
                result = BaseStore.GetCategoriesByCustomLabel(_context: _context, productSearch: result, customLabel: prcustomlabel);
            if (ProductsId != "" && ProductsId != null)
            {
                List<ProductShowVm> newlist = new List<ProductShowVm>();
                foreach (var item in result.Products2)
                {
                    ProductShowVm temp = new ProductShowVm();
                    temp = _context.Product.GetProduct2(item.ID);
                    temp.Variety = BaseStore.GetProductVariety(_context: _context, productId: item.ID);
                    newlist.Add(temp);
                }
                result.Products2 = newlist;
            }
            return BaseController.GetView(this, prresultType, prviewName, result);
        }


        [Route("product/getqr")]
        public ActionResult GetQR(int productId)
        {
            Product product = _context.Product.GetById(productId);
            var generator = new Url(product.GetLinkWithUrl());
            string payload = generator.ToString();
            var qrGenerator = new QRCoder.QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(payload, QRCoder.QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCoder.QRCode(qrCodeData);
            var qrCodeAsBitmap = qrCode.GetGraphic(6);
            var memoryStream = new System.IO.MemoryStream();
            qrCodeAsBitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
            memoryStream.Position = 0;
            return File(memoryStream, "image/Png");
        }
        public ActionResult SearchProductIds(string id)
        {
            if (id != "")
            {
                int temId = int.Parse(id);
                var product = _context.Product.GetById(temId);
                if (product != null)
                {
                    return PartialView(BaseController.GetView(this), model: product);
                }
                else
                {
                    return PartialView(BaseController.GetView(this), model: null);
                }
               
            }
            return PartialView(BaseController.GetView(this), model: null);
        }
        private bool IsNullOrEmpty(string value)
        {
            bool result = false;
            if (value == null)
                result = true;
            else if (value == "0")
                result = true;
            else if (value == "")
                result = true;
            else if (value == "null")
                result = true;
            return result;
        }

        public ActionResult Like(int ProductId, string Function)
        {
            Product product = _context.Product.GetById(ProductId);
            ViewAccount CurrentUser = _context.Account.GetCurrentAccount();
            if (CurrentUser != null)
            {
                if (Function == "ADD")
                {
                    ProductLike like = new ProductLike()
                    {
                        AccountId = CurrentUser.Id,
                        ProductId = ProductId,
                        Datetime = DateTime.Now
                    };
                    _context.ProductLike.Insert(like);
                }
                else
                {
                    ProductLike like = _context.ProductLike.GetByProductIdAndAccountId(ProductId, CurrentUser.Id);
                    _context.ProductLike.Delete(like);
                }
                _context.Save();

                return RedirectToAction("index", new { prid = ProductId, prname = BaseUrl.StandardUrl(product.Name) });
            }
            else
            {
                return RedirectToAction("login", "Account", new { back = product.GetLink() });
            }
        }

        public ActionResult Compare(int[] ProductId)
        {
            if (ProductId != null && ProductId.Any())
            {
                ProductCompareVm list = new ProductCompareVm();
                list = _context.Product.GetProductsForCompare(unitOfWork: _context, products: ProductId);
                return View(BaseController.GetView(this), list);
            }
            else
            {
                return Redirect("/");
            }

        }
        public ActionResult UpdateAll()
        {
            var products = _context.Product.GetAll();
            BaseStore.UpdateProductQuantity(products, _context);
            return null;
        }
        public ActionResult Update19()
        {
            var products = _context.Product.GetById(19);
            BaseStore.UpdateProductQuantity(products, _context);
            return null;
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