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

namespace Cms.Controllers
{
    public class SmartOfferQuestionController : Controller
    {
        UnitOfWork _context = new UnitOfWork();
        public ActionResult Index()
        {
            var question = _context.SmartOfferQuestion.Where(s => s.Active != false).ToList();
            return View(BaseController.GetView(this), model: question);
        }


        public ActionResult Search(
            string answerId = null,
            bool? prstock = true,
            string prcustomlabel = "all",
            double? pricefrom = null,
            double? priceto = null,
            string prviewName = "Search",
            Enum_ResultType prresultType = Enum_ResultType.RESULT_TYPE_VIEWNAME
            )
        {

            string[] Split = answerId.Split(',');
            string answer = "";
            foreach (var item in Split)
            {
                var id = int.Parse(item);
                var offerItem = _context.SmartOfferAnswer.GetById(id);

                if (offerItem != null)
                {
                    if (offerItem.SmartOfferQuestion.Code.Label == Enum_Code.OFFER_TYPE_PRICE.ToString())
                    {
                        string[] priceSplit = offerItem.Answer.Split('-');
                        priceSplit[0]=priceSplit[0].Trim().Replace(",","");
                        priceSplit[1] = priceSplit[1].Trim().Replace(",", "");

                        pricefrom = double.Parse(priceSplit[0].Trim());
                        priceto = double.Parse(priceSplit[1].Trim());
                        if (priceto == 2)
                        {
                            priceto = null;

                        }
                        if (pricefrom == 1)
                        {
                            pricefrom = 0;

                        }
                    }
                    else
                    {
                        answer += offerItem.Answer += "-";
                    }

                }
            }

            var result = new ProductSearchVm();
            result = _context.Product.SearchVm2(answer: answer,
                prstock: prstock,
                prcustomLabel: prcustomlabel,
                pricefrom: pricefrom,
                priceto: priceto
                );


            result.Prstock = prstock;
            result.PrcustomLabel = prcustomlabel;

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
            int distance = 0,
            string SyncNameProductCustomField = null,
            string valueProductCustom = null,
            bool? prstock = null,
            string prcustomlabel = "all",
            int? discountGroupId = null,

            string prviewName = "",
            Enum_ProductOrder prorder = Enum_ProductOrder.NEW,
            Enum_ResultType prresultType = Enum_ResultType.RESULT_TYPE_VIEWNAME,
            string lang = ""
            )
        {

            var result = new ProductSearchVm();
            result = _context.Product.SearchVm(
                                    Id: prId,
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