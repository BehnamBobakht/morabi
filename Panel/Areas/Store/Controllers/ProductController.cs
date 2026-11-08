using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using System.Data;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.ExcelAc;

namespace Panel.Areas.Store.Controllers
{
    [ValidateInput(false)]
    public class ProductController : Controller
    {

        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(
      int prpageSize = 8,
     int prindex = 1,
     int? productTypeId = null,
     int? productCategoryId = null,
     int? productSubCategoryId = null,
     string brandId = null,
     string storeId = null,
     string name = null,
     bool? hasDiscount = null,
     bool? active = null,
     bool? isService = null,
     bool? prstock = null,
     string prcustomLabel = "all",
     Enum_ProductOrder prorder = Enum_ProductOrder.NEW,
            Enum_Code? status = null

     )
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product);

            var result = new ProductSearchVm();
            result = _context.Product.SearchVm(
                IsHidden: true,
                                    index: prindex,
                                    pageSize: prpageSize,
                                    hasDiscount: hasDiscount,
                                    name: name,
                                    brandId: brandId,
                                    storeId: storeId,
                                    subCategoryId: productSubCategoryId.ToString(),
                                    categoryId: productCategoryId.ToString(),
                                    typeId: productTypeId.ToString(),
                                    active: active,
                                    isService: isService,
                                    prstock: prstock,
                                    order: prorder,
                                    prcustomLabel: prcustomLabel,
                                    status: status
                                    ); ;
            result.PageSize = prpageSize;
            result.PageIndex = prindex;
            result.Name = name;
            result.ProductTypeId = productTypeId;
            result.ProductCategoryId = productCategoryId;
            result.ProductSubCategoryId = productSubCategoryId;
            result.Status = status;
            result.BrandId = brandId;
            result.Prstock = prstock;
            result.prorder = prorder;
            result.PrcustomLabel = prcustomLabel;
            result.HasDiscount = hasDiscount;
            result.Active = active;
            FillDropDown(result);
            ViewBag.StoreId = new SelectList(_context.Store.GetAll(), "ID", "Name", storeId);

            return View(result);
        }
        public ActionResult InterUser(string fullName = null, string NationalCode = null, string fromDate = null,
           string toDate = null, string pageIndex = null, string Mobile = null,
          string pageSize = null,
          int? productId = null, int? typeId = null, int? productidInter = null)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account);
            int size = pageSize == null ? 50 : pageSize.GetInteger();
            int index = pageIndex == null ? 1 : pageIndex.GetInteger();
            DateTime from = string.IsNullOrEmpty(fromDate) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(fromDate))).GetValueOrDefault();
            DateTime to = string.IsNullOrEmpty(toDate) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(toDate))).GetValueOrDefault();
            ViewBag.FromDatetime = fromDate;
            ViewBag.ToDatetime = toDate;
            ViewBag.typeId = typeId;
            ViewBag.ProductIdInter = productidInter;
            ViewBag.Url = _context.Product.FirstOrDefault(s => s.ID == productidInter).GetLinkWithUrl();
            ViewBag.PageIndex = index.ToString();
            ViewBag.PageSize = size.ToString();
            ViewBag.fullName = fullName;
            ViewBag.NationalCode = NationalCode;
            ViewBag.Mobile = Mobile;

            ViewBag.productId = productId;
            ViewBag.TotalCount = _context.Account.SearchCount(
                   fullName: fullName,
                   NationalCode: NationalCode,
                    fromDatetime: from,
                    Mobile: Mobile,
                    toDatetime: to,
                    productId: productId
                    );

            List<Account> listOrder = _context.Account.Search(
                    fullName: fullName,
                     fromDatetime: from,
                     NationalCode: NationalCode,
                    toDatetime: to,
                     Mobile: Mobile,
                    productId: productId,
                    pageSize: size,
                    index: index
                  )
                .ToList();
            var list = listOrder.ToView();

            return View(list);
            //return View(_context.Account.Where(s=>s.Deleted==false).ToList().ToView());
        }
        public void FillDropDown(ProductSearchVm search)
        {
            ViewBag.StateId = new SelectList(_context.State.Search(pageSize: 100), "ID", "Name");
            ViewBag.ProductId = new SelectList(_context.Product.Search(typeId: "2", IsHidden: true, order: Enum_ProductOrder.NEW, pageSize: 10000), "ID", "Name");
            ViewBag.CoursId = new SelectList(_context.ExameCourse.GetAll().Where(s => s.Deleted != true).ToList(), "ID", "Name");

            ViewBag.ProductTypeId = new SelectList(_context.ProductType.Search(pageSize: 100), "ID", "Name");
            string status = null;
            if (search.Status.HasValue)
            {
                status = search.Status.Value.ToString();
            }
            ViewBag.Status = new SelectList(_context.Code.GetAllByCodeGroup(Enum_CodeGroup.PRODUCT_STATUS), "Label", "Name", status);
        }

        public ActionResult PrintReport(int id, int? userId = null)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product_New);
            var product = _context.Product.GetById(id);
            ViewBag.NumberString = product.NumberString;
            ViewBag.DateFa = product.DateFa;
            ViewBag.Name = product.NameFa;
            ViewBag.NameEn = product.NameEn;
            ViewBag.DateEn = product.DateEn;
            ViewBag.DatePrint = product.DatePrint;


            List<Account> listOrder = _context.Account.Search(
                     productId: id,
                     pageSize: 10000,
                     id: userId
                   )
                 .ToList();
            return View(listOrder);
        }
        public ActionResult QuestionRegister(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product_New);

            ViewBag.ID = id;
            ViewBag.QuestionRegisterProduct = _context.QuestionRegisterProduct.Where(s => s.ProductId == id).ToList();

            List<QuestionRegister> list = _context.QuestionRegister.GetAll().ToList();
            return View(list);
        }
        [HttpPost]
        public ActionResult QuestionRegister(List<QuestionRegisterProduct> list)
        {
            ViewMessage result = new ViewMessage();
            if (list.Any())
            {
                var id = list.FirstOrDefault().ProductId;
                var listold = _context.QuestionRegisterProduct.Where(s => s.ProductId == id).ToList();
                foreach (var item in listold)
                {

                    _context.QuestionRegisterProduct.Delete(item);

                }
                _context.Save();
            }
            foreach (var item in list)
            {

                _context.QuestionRegisterProduct.Insert(item);

            }
            _context.Save();

            return new JsonResult() { Data = result };

        }
        public ActionResult PrintReportCertificateAllNumber(int id)
        {

            ViewBag.Id = id;
            ViewBag.TotalCount = _context.Account.Search(
                     productId: id,
                     pageSize: 10000
                   )
                 .Count();
            return View();
        }
        public ActionResult PrintReportCertificateAll(int id, int index)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product_New);
            var product = _context.Product.GetById(id);
            ViewBag.NumberString = product.NumberString;
            ViewBag.DateFa = product.DateFa;
            ViewBag.Name = product.NameFa;
            ViewBag.NameEn = product.NameEn;
            ViewBag.DateEn = product.DateEn;
            ViewBag.Hours = product.Hours;
            ViewBag.TeacherNameEn = product.TeacherNameEn;
            ViewBag.TeacherNameFa = product.TeacherNameFa;

            ViewBag.DatePrint = product.DatePrint;


            List<Account> listOrder = _context.Account.Search(
                     productId: id,
                     index: index,
                     pageSize: 50
                   )
                 .ToList();
            return View(listOrder);
        }
        public ActionResult PrintReportCertificate(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product_New);
            var product = _context.Product.GetById(id);
            ViewBag.NumberString = product.NumberString;
            ViewBag.DateFa = product.DateFa;
            ViewBag.Name = product.NameFa;
            ViewBag.NameEn = product.NameEn;
            ViewBag.DateEn = product.DateEn;
            ViewBag.Hours = product.Hours;
            ViewBag.TeacherNameEn = product.TeacherNameEn;
            ViewBag.TeacherNameFa = product.TeacherNameFa;

            ViewBag.DatePrint = product.DatePrint;

            var PaymentCertificate = _context.PaymentCertificate.Where(s => s.ProductId == id && s.StatusId == 1072 && s.Type == 2).OrderByDescending(s => s.ID).ToList();
            return View(PaymentCertificate);
        }
        public ActionResult ExcelCertificate(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product_New);
            var product = _context.Product.GetById(id);
            var NumberString = product.NumberString;
            var DateFa = product.DateFa;
            var Name = product.NameFa;
            var NameEn = product.NameEn;
            var DateEn = product.DateEn;
            var Hours = product.Hours;
            var TeacherNameEn = product.TeacherNameEn;
            var TeacherNameFa = product.TeacherNameFa;

            var DatePrint = product.DatePrint;

            var list = _context.PaymentCertificate.Where(s => s.ProductId == id && s.StatusId == 1072 && s.Type == 2).OrderByDescending(s => s.ID).ToList();
            DataTable dt = new DataTable("sheet1");

            dt.Columns.AddRange(new DataColumn[14] { new DataColumn("#"),
                                                     new DataColumn("نام و نام خانوادگی"),
                                                     new DataColumn("نام لاتین "),
                                                     new DataColumn("موبایل"),
                                                     new DataColumn("ایمیل"),
                                                     new DataColumn("کد ملی"),
                                                     new DataColumn("کد معرف"),
                                                     new DataColumn("آدرس"),
                     new DataColumn("تاریخ ثبت نام"),
                     new DataColumn("نام دوره"),
                     new DataColumn("نام مدرس"),
                     new DataColumn("ساعت دوره"),
                     new DataColumn("تاریخ دوره"),
                     new DataColumn("شماره چاپ")


                                                  });
            for (int i = 0; i < list.Count(); i++)
            {
                var item = list[i];

                dt.Rows.Add(i + 1, item.Account.FullName + " " + item.Account.LatName, item.Account.FullNameEn + " " + item.Account.LatNameEn, item.Account.Mobile, item.Account.Email, item.Account.NationalCode, "10" + item.ID, item.Account.Address, item.Account.CreateDatetime.ToPersian(), Name + "-" + NameEn, TeacherNameFa + "-" + TeacherNameEn, Hours , DateFa + "-" + DateEn , NumberString);
            }

            string fileName = string.Format("{0} - {1}.xlsx", DateTime.Now.Date.ToShortDateString(), 10);
            using (XLWorkbook wb = new XLWorkbook()) //Install ClosedXml from Nuget for XLWorkbook  
            {
                wb.RightToLeft = true;
                wb.Worksheets.Add(dt);

                using (MemoryStream stream = new MemoryStream()) //using System.IO;  
                {
                    wb.SaveAs(stream);

                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
        public ActionResult ExcelCertificateOnline(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product_New);
            var product = _context.Product.GetById(id);
            var NumberString = product.NumberString;
            var DateFa = product.DateFa;
            var Name = product.NameFa;
            var NameEn = product.NameEn;
            var DateEn = product.DateEn;
            var Hours = product.Hours;
            var TeacherNameEn = product.TeacherNameEn;
            var TeacherNameFa = product.TeacherNameFa;

            var DatePrint = product.DatePrint;

            var list = _context.PaymentCertificate.Where(s => s.ProductId == id && s.StatusId == 1072 && s.Type == 1).OrderByDescending(s => s.ID).ToList();
            DataTable dt = new DataTable("sheet1");

            dt.Columns.AddRange(new DataColumn[14] { new DataColumn("#"),
                                                     new DataColumn("نام و نام خانوادگی"),
                                                     new DataColumn("نام لاتین "),
                                                     new DataColumn("موبایل"),
                                                     new DataColumn("ایمیل"),
                                                     new DataColumn("کد ملی"),
                                                     new DataColumn("کد معرف"),
                                                     new DataColumn("آدرس"),
                     new DataColumn("تاریخ ثبت نام"),
                     new DataColumn("نام دوره"),
                     new DataColumn("نام مدرس"),
                     new DataColumn("ساعت دوره"),
                     new DataColumn("تاریخ دوره"),
                     new DataColumn("شماره چاپ")


                                                  });
            for (int i = 0; i < list.Count(); i++)
            {
                var item = list[i];


                dt.Rows.Add(i + 1, item.Account.FullName + " " + item.Account.LatName, item.Account.FullNameEn + " " + item.Account.LatNameEn, item.Account.Mobile, item.Account.Email, item.Account.NationalCode, "10" + item.ID, item.Account.Address, item.Account.CreateDatetime.ToPersian(), Name + "-" + NameEn, TeacherNameFa + "-" + TeacherNameEn, Hours, DateFa + "-" + DateEn, NumberString);
            }

            string fileName = string.Format("{0} - {1}.xlsx", DateTime.Now.Date.ToShortDateString(), 10);
            using (XLWorkbook wb = new XLWorkbook()) //Install ClosedXml from Nuget for XLWorkbook  
            {
                wb.RightToLeft = true;
                wb.Worksheets.Add(dt);

                using (MemoryStream stream = new MemoryStream()) //using System.IO;  
                {
                    wb.SaveAs(stream);

                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
        public ActionResult PaymentCertificate(int id, int pageSize = 10, int pageIndex = 1, string name = "", string nationalCode = "", string mobile = "", string type = "")
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product_New);
            ViewBag.Id = id;
            int size = pageSize;
            int index = pageIndex;
            ViewBag.PageIndex = index.ToString();
            ViewBag.PageSize = size.ToString();
            ViewBag.Name = name;
            ViewBag.NationalCode = nationalCode;
            ViewBag.Mobile = mobile;
            ViewBag.Type = type;
            ViewBag.TotalCount = _context.PaymentCertificate.SearchCount(id, name, nationalCode, mobile, type);
            var PaymentCertificate = _context.PaymentCertificate.Search(id, pageSize, index, name, nationalCode, mobile, type);
            return View(PaymentCertificate);
        }
        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product_New);
            var model = new ProductCreateVm();
            var langs = BaseLanguage.GetWebsiteLanguagesVm();
            if (langs.Any())
            {
                foreach (var lang in langs)
                {
                    var langItem = new ProductLanguageVm();
                    langItem.LanguageId = lang.LanguageId;
                    langItem.LanguageName = lang.LanguageName;
                    model.ProductLanguages.Add(langItem);
                }

            }
            ViewBag.ProductTypeId = new SelectList(_context.ProductType.Search(pageSize: 100), "ID", "Name");
            ViewBag.StatusId = new SelectList(_context.Code.GetAllByCodeGroupVm(Enum_CodeGroup.PRODUCT_STATUS), "ID", "Name");
            ViewBag.StateId = new SelectList(_context.State.Search(pageSize: 100), "ID", "Name");
            ViewBag.ProductId2 = new SelectList(_context.Product.Search(typeId: "2", order: Enum_ProductOrder.NEW, IsHidden: true, pageSize: 10000), "ID", "Name");
            ViewBag.CoursId = new SelectList(_context.ExameCourse.GetAll().Where(s => s.Deleted != true).ToList(), "ID", "Name");

            return View(model);
        }
        [HttpPost]
        public ActionResult Create(ProductCreateVm productVm)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product_New);

          
            
            bool status = true;
            if (string.IsNullOrEmpty(productVm.Name))
            {
                ModelState.AddModelError("", Resource.Notify.PleaseInsertTheName);
                status = false;
            }
            if (status == false)
            {
                ViewBag.ProductTypeId = new SelectList(_context.ProductType.Search(pageSize: 100), "ID", "Name", productVm.ProductTypeId);
                ViewBag.StatusId = new SelectList(_context.Code.GetAllByCodeGroupVm(Enum_CodeGroup.PRODUCT_STATUS), "ID", "Name", productVm.StatusId);
                ViewBag.StateId = new SelectList(_context.State.Search(pageSize: 100), "ID", "Name", productVm.StateId);
                ViewBag.ProductId2 = new SelectList(_context.Product.Search(typeId: "2", order: Enum_ProductOrder.NEW, IsHidden: true, pageSize: 10000), "ID", "Name");
                ViewBag.CoursId = new SelectList(_context.ExameCourse.GetAll().Where(s => s.Deleted != true).ToList(), "ID", "Name", productVm.CoursId);

                return View(productVm);
            }
            _context.Product.InsertVm(productVm: productVm, unitOfWork: _context);
            BaseWebsite.UpdatingLastContent();

            return RedirectToAction("index");

        }
        [HttpGet]
        public ActionResult updatePrice()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product_New);
            //var products = _context.Product.Where(s => s.HasEuro == true).ToList();

            //var model = new List<ProductToStoreVm>();
            //List<ProductQuantity> listquantity = new List<ProductQuantity>();
            //foreach (var item in products)
            //{
            //    if (item.InstallmentCount > 0)
            //    {
            //        var listOptionItem = _context.ProductOptionItem.GetAll().Take(item.InstallmentCount.Value).ToList();
            //        var count = listOptionItem.Count();
            //        var pricEuro = item.EuroPrice.Value / count;
            //        double temPrice = item.EuroPrice.Value;
            //        foreach (ProductOptionItem optionItem in listOptionItem.OrderByDescending(s => s.ID))
            //        {

            //            double computeprice = GetEuroPrice(temPrice);
            //            ProductQuantity quantity = new ProductQuantity()
            //            {
            //                ColorId = null,
            //                ProductId = item.ID,
            //                SizeId = null,
            //                OptionItemId = optionItem?.ID,
            //                Price = computeprice,
            //                EuroPrice = temPrice,
            //                Count = 1000
            //            };
            //            ProductProductOptionItem newo = new ProductProductOptionItem();
            //            newo.ProductId = item.ID;
            //            newo.ProductOptionItemId = optionItem.ID;
            //            _context.ProductProductOptionItem.Insert(newo);
            //            _context.Save();
            //            listquantity.Add(quantity);
            //            temPrice -= pricEuro;
            //        }

            //        //BaseStore.UpdateProductQuantity(item.ID, _context);
            //    }
            //    else
            //    {
            //        ProductQuantity quantity = new ProductQuantity()
            //        {
            //            ColorId = null,
            //            ProductId = item.ID,
            //            SizeId = null,
            //            OptionItemId = null,
            //            Price = GetEuroPrice(item.EuroPrice.Value),
            //            EuroPrice = item.EuroPrice
            //        };
            //        listquantity.Add(quantity);



            //    }
            //}
            //foreach (var q in listquantity)
            //{
            //    _context.ProductQuantity.Insert(q);
            //    _context.Save();
            //}
            //var products = _context.Product.Where(s => s.HasEuro == false).ToList();

            //var model = new List<ProductToStoreVm>();
            //List<ProductQuantity> listquantity = new List<ProductQuantity>();
            //foreach (var item in products)
            //{

            //        ProductQuantity quantity = new ProductQuantity()
            //        {
            //            ColorId = null,
            //            ProductId = item.ID,
            //            SizeId = null,
            //            OptionItemId = null,
            //            Price = item.Price,
            //            EuroPrice = 0
            //        };
            //        listquantity.Add(quantity);




            //}
            //foreach (var q in listquantity)
            //{
            //    _context.ProductQuantity.Insert(q);
            //    _context.Save();
            //}
            //return RedirectToAction("index");
            //var products = _context.Product.Where(s => s.ProductTypeId == 3).ToList();

            //var model = new List<ProductToStoreVm>();
            //List<ProductQuantity> listquantity = new List<ProductQuantity>();
            //foreach (var item in products)
            //{

            //    ProductQuantity quantity = new ProductQuantity()
            //    {
            //        ColorId = null,
            //        ProductId = item.ID,
            //        SizeId = null,
            //        OptionItemId = null,
            //        Price = item.Price,
            //        Count=100,
            //        EuroPrice = 0
            //    };
            //    listquantity.Add(quantity);




            //}
            //foreach (var q in listquantity)
            //{
            //    _context.ProductQuantity.Insert(q);
            //    _context.Save();
            //}
            var products = _context.Product.Where(s => s.ProductTypeId == 1001).ToList();

            var model = new List<ProductToStoreVm>();
            List<ProductCustomValue> listcustome = new List<ProductCustomValue>();
            foreach (var item in products)
            {
                int id = 0;
                if (item.CodeValue == "درجه سه")
                {
                    id = 1006;
                }
                else if (item.CodeValue == "درجه دو")
                {
                    id = 1005;
                }
                else
                {
                    id = 1004;

                }
                ProductCustomValue custome = new ProductCustomValue()
                {
                    FieldId = 1009,
                    ProductId = item.ID,
                    ItemId = id,

                };
                listcustome.Add(custome);




            }
            foreach (var q in listcustome)
            {
                _context.ProductCustomValue.Insert(q);
                _context.Save();
            }
            return RedirectToAction("index");

        }
        public double GetEuroPrice(double price)
        {
            double? unitPrice = _context.WebsiteDetail.FirstOrDefault().EuroPrice;
            return unitPrice != null ? unitPrice.Value * price : 1 * price;
        }
        public ActionResult Edit(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product_Edit);
            var cache = HttpContext.Cache.Get("pr" + id);

            var model = _context.Product.GetForEdit(productId: id);
            ViewBag.ProductTypeId = new SelectList(_context.ProductType.Search(pageSize: 100), "ID", "Name", model.ProductTypeId);
            ViewBag.StatusId = new SelectList(_context.Code.GetAllByCodeGroupVm(Enum_CodeGroup.PRODUCT_STATUS), "ID", "Name", model.StatusId);
            ViewBag.StateId = new SelectList(_context.State.Search(pageSize: 100), "ID", "Name", model.StateId);
            ViewBag.ProductId2 = new SelectList(_context.Product.Search(typeId: "2", order: Enum_ProductOrder.NEW, IsHidden: true, pageSize: 10000), "ID", "Name");
            ViewBag.CoursId = new SelectList(_context.ExameCourse.GetAll().Where(s => s.Deleted != true).ToList(), "ID", "Name", model.CoursId);

            var langs = BaseLanguage.GetWebsiteLanguagesVm();
            if (langs.Any())
            {
                foreach (var lang in langs)
                {
                    if (!model.ProductLanguages.Any(a => a.LanguageId == lang.LanguageId))
                    {
                        var langItem = new ProductLanguageVm();
                        langItem.LanguageId = lang.LanguageId;
                        langItem.LanguageName = lang.LanguageName;
                        model.ProductLanguages.Add(langItem);
                    }

                }

            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(ProductCreateVm productVm)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product_Edit);

            // Keep the selected course end date valid through the end of that day.
            
         
            bool status = true;
            if (string.IsNullOrEmpty(productVm.Name))
            {
                ModelState.AddModelError("", Resource.Notify.PleaseInsertTheName);
                status = false;
            }
            if (status == false)
            {
                ViewBag.ProductTypeId = new SelectList(_context.ProductType.Search(pageSize: 100), "ID", "Name", productVm.ProductTypeId);
                ViewBag.StatusId = new SelectList(_context.Code.GetAllByCodeGroupVm(Enum_CodeGroup.PRODUCT_STATUS), "ID", "Name", productVm.StatusId);
                ViewBag.StateId = new SelectList(_context.State.Search(pageSize: 100), "ID", "Name", productVm.StateId);
                ViewBag.ProductId2 = new SelectList(_context.Product.Search(typeId: "2", order: Enum_ProductOrder.NEW, IsHidden: true, pageSize: 10000), "ID", "Name");
                ViewBag.CoursId = new SelectList(_context.ExameCourse.GetAll().Where(s => s.Deleted != true).ToList(), "ID", "Name", productVm.CoursId);

                return View(productVm);
            }
            _context.Product.UpdateVm(productVm: productVm, unitOfWork: _context);
            BaseWebsite.UpdatingLastContent();

            return RedirectToAction("index");

        }
        public ActionResult Colors(int? productTypeId = null, int? productId = null, List<int> colorIds = null)
        {
            var model = new List<ColorVm>();
            if (productTypeId.HasValue)
                model = _context.Color.GetAllByProductTypeForProduct(productTypeId: productTypeId.Value, productId: productId, selectedColors: colorIds);
            return PartialView(model: model);
        }
        public ActionResult Sizes(int? productTypeId = null, int? productId = null, List<int> sizeIds = null)
        {
            var model = new List<SizeVm>();
            if (productTypeId.HasValue)
                model = _context.Size.GetAllByProductTypeForProduct(productTypeId: productTypeId.Value, productId: productId, selectedSizes: sizeIds);
            return PartialView(model: model);
        }
        public ActionResult NewReview()
        {
            var model = new ReviewVm();
            return PartialView("_Review", model: model);
        }

        public ActionResult EditCustomFields(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product_Edit);

            var product = _context.Product.GetForEdit(id);
            product.ProductCustomField = _context.ProductCustomField.GetAllForProductWithValue(id);
            return View(product);
        }
        [HttpPost]
        public ActionResult EditCustomFields(ProductCreateVm productVm)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product_Edit);

            //if(productVm.ProductCustomField.Any(s=>s.IsRequired && string.IsNullOrEmpty(s.Value) && s.ValueItemId==null))
            //{
            //    ModelState.AddModelError("", Resource.Notify.PleaseFillInOrSelectTheRequiredFields);
            //    return View(productVm);
            //}

            _context.Product.EditProductCustomValues(productVm);
            return RedirectToAction("Index");
        }
        public void DownloadExcel(int storeId)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product_Edit);

            var products = _context.Product.Search(pageSize: 9000000);
            var model = new List<ProductToStoreVm>();
            foreach (var item in products)
            {
                var ProductToStoreVm = new ProductToStoreVm();
                var ProductQuantities = _context.ProductQuantity.GetForStore(productId: item.ID, storeId: storeId);
                if (ProductQuantities.Any())
                {
                    ProductToStoreVm.StoreId = storeId;
                    ProductToStoreVm.ProductId = item.ID;
                    ProductToStoreVm.ProductQuantities = ProductQuantities;
                    model.Add(ProductToStoreVm);
                }

            }
            var dataResult = model;
            if (!dataResult.Any()) return;

            var excelPackage = new ExcelPackage();
            var sheet = excelPackage.Workbook.Worksheets.Add(Resource.Lang.StoreProducts);

            var row = 1;
            sheet.Cells[$"A{row}"].Value = "آیدی";

            sheet.Cells[$"B{row}"].Value = "آیدی فروشنده";
            sheet.Cells[$"C{row}"].Value = "آیدی محصول";

            sheet.Cells[$"D{row}"].Value = Resource.Lang.ProductName;

            sheet.Cells[$"E{row}"].Value = $"{Resource.Lang.Price} ({Resource.Lang.Toman})";
            sheet.Cells[$"F{row}"].Value = Resource.Lang.Count;

            foreach (var item in dataResult)
            {

                foreach (var spq in item.ProductQuantities)
                {
                    row++;
                    sheet.Cells[$"A{row}"].Value = spq.ID;

                    sheet.Cells[$"B{row}"].Value = item.StoreId;
                    sheet.Cells[$"C{row}"].Value = item.ProductId;

                    sheet.Cells[$"D{row}"].Value = spq.ProductName;

                    sheet.Cells[$"E{row}"].Value = spq.Price;
                    sheet.Cells[$"F{row}"].Value = spq.Count;
                }

            }

            sheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AppendHeader("Content-Disposition", "attachment; filename=StoreProducts.xlsx");
            Response.BinaryWrite(excelPackage.GetAsByteArray());
            Response.End();
        }
        public ActionResult UpdateEuroPrice(int price)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product_Edit);

            var products = _context.Product.Where(s => s.HasEuro == true).ToList();
            var model = new List<ProductToStoreVm>();
            foreach (var item in products)
            {
                var listQuantity = _context.ProductQuantity.Where(s => s.ProductId == item.ID);
                foreach (var q in listQuantity)
                {
                    var temp = _context.ProductQuantity.FirstOrDefault(s => s.ID == q.ID);
                    temp.Price = temp.EuroPrice * price;
                    _context.ProductQuantity.Update(temp);

                }
                _context.Save();
                var ProductToStoreVm = new ProductToStoreVm();
                var ProductQuantities = _context.ProductQuantity.GetForStore(productId: item.ID, storeId: 1);
                if (ProductQuantities.Any())
                {
                    ProductToStoreVm.StoreId = 1;
                    ProductToStoreVm.ProductId = item.ID;
                    ProductToStoreVm.ProductQuantities = ProductQuantities;
                    model.Add(ProductToStoreVm);
                }
                //foreach (var quantity in ProductToStoreVm.ProductQuantities)
                //{
                //    if (quantity.EuroPrice > 0)
                //    {
                //        //quantity.Price = quantity.EuroPrice * price;
                //        var temp = _context.ProductQuantity.FirstOrDefault(s => s.ID == quantity.ID && s.EuroPrice > 0);
                //        if (temp != null)
                //        {
                //            temp.Price = quantity.EuroPrice * price;
                //            _context.ProductQuantity.Update(temp);
                //            _context.Save();
                //        }

                //    }
                //    else
                //    {
                //        //quantity.Price = quantity.Price;
                //        var temp = _context.ProductQuantity.FirstOrDefault(s => s.ID == quantity.ID);
                //        if (temp != null)
                //        {
                //            temp.Price = quantity.Price;
                //            temp.Count = 100;
                //            _context.ProductQuantity.Update(temp);
                //            _context.Save();
                //        }
                //    }


                //}
            }
            foreach (var itemStock in model)
            {
                _context.ProductQuantity.AddProductToStore(unitOfWork: _context, productToStore: itemStock);

            }
            var website = _context.WebsiteDetail.FirstOrDefault();
            website.EuroPrice = price;
            _context.WebsiteDetail.Update(website);
            _context.Save();
            return RedirectToAction("Index");

        }
        [HttpPost]
        public ActionResult UploadExcel(HttpPostedFileBase file)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product_Edit);

            if (file == null || file.ContentLength <= 0) return RedirectToAction("Index");
            //var fileContentType = file.ContentType;
            //var fileBytes = new byte[file.ContentLength];
            //var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));

            using (var package = new ExcelPackage(file.InputStream))
            {
                var currentSheet = package.Workbook.Worksheets;
                var workSheet = currentSheet.First();
                var noOfCol = workSheet.Dimension.End.Column;
                var noOfRow = workSheet.Dimension.End.Row;
                for (var rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    var id = (workSheet.Cells[rowIterator, 1].Value ?? "").ToString();

                    var storeId = (workSheet.Cells[rowIterator, 2].Value ?? "").ToString();
                    var productId = (workSheet.Cells[rowIterator, 3].Value ?? "").ToString();
                    var price = (workSheet.Cells[rowIterator, 5].Value ?? 0).ToString();
                    var count = (workSheet.Cells[rowIterator, 6].Value ?? 0).ToString();
                    var model = new ProductToStoreVm();

                    var ProductQuantity = new ProductQuantityVm();
                    var list = new List<ProductQuantityVm>();

                    if (storeId != null && productId != null && id != null)
                    {
                        model.ProductId = int.Parse(productId);
                        ProductQuantity.ProductId = int.Parse(productId);

                        model.StoreId = int.Parse(storeId);
                        ProductQuantity.ID = int.Parse(id);
                        if (price != null && count != null)
                        {
                            ProductQuantity.Count = int.Parse(count);
                            ProductQuantity.Price = int.Parse(price);
                            list.Add(ProductQuantity);
                            model.ProductQuantities = list;
                        }


                        _context.ProductQuantity.AddProductToStore(unitOfWork: _context, productToStore: model);

                    }
                }


            }
            return RedirectToAction("Index");
        }
        public ActionResult AddToStore(int productId, int? storeId)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product_Edit);
            var model = new ProductToStoreVm();
            model.ProductId = productId;
            model.StoreId = storeId;
            model.ProductQuantities = _context.ProductQuantity.GetForStore(productId: productId, storeId: storeId);
            ViewBag.ProductId = new SelectList(_context.Product.Search(pageSize: 9000000, IsHidden: true), "ID", "Name", productId);
            ViewBag.StoreId = new SelectList(_context.Store.GetAll(), "ID", "Name", storeId);

            return View(model);
        }
        [HttpPost]
        public ActionResult AddToStore(ProductToStoreVm productToStoreVm)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product_Edit);
            if (productToStoreVm.StoreId.HasValue)
            {
                _context.ProductQuantity.AddProductToStore(unitOfWork: _context, productToStore: productToStoreVm);
            }
            else
            {
                RedirectToAction("AddToStore", new { productId = productToStoreVm.ProductId, storeId = productToStoreVm.StoreId });
            }
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Product product = _context.Product.GetById(id);
            if (product == null)
                return HttpNotFound();

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product_Delete);
            //Product product = _context.Product.GetById(id);

            //_context.ProductPicture.DeleteByProductId(product.ID);
            //_context.ProductLanguage.DeleteByProductId(product.ID);
            //_context.ProductLike.DeleteByProductId(product.ID);
            //_context.Discount.DeleteByProductId(product.ID);
            //_context.ProductCustomValue.DeleteByProductId(product.ID);
            //_context.ProductColor.DeleteByProductId(product.ID);
            //_context.ProductSize.DeleteByProductId(product.ID);
            //_context.ProductQuantity.DeleteByProductId(product.ID);
            //_context.Save();

            _context.Product.Hide(id);
            _context.Save();
            return RedirectToAction("Index");
        }

        public ActionResult ProductDropDown(string storeId = null, string productTypeId = null, string productcategoryId = null, string productSubCategoryId = null, int? value = null)
        {
            if (!string.IsNullOrEmpty(storeId))
            {
                var products = _context.Product.SearchVm(storeId: storeId, pageSize: 10000, typeId: productTypeId, categoryId: productcategoryId, subCategoryId: productSubCategoryId)
                    .Products.OrderByDescending(p => p.ID).ToList();
                ViewBag.ProductId = new SelectList(products, "ID", "NameId", value);
            }
            else
            {
                ViewBag.ProductId = new SelectList(new List<ProductShowVm>(), "ID", "NameId");
            }

            return PartialView();
        }
        public ActionResult StoreProductQuantityDropDown(int? storeId = null, int? productid = null, int? value = null)
        {
            if (storeId != null && productid != null)
            {
                ViewBag.StoreProductQuantityId = new SelectList(_context.StoreProductQuantity.GetForProduct(productId: productid.Value, storeId: storeId.Value), "ID", "FullName", value);
            }
            else
            {
                ViewBag.StoreProductQuantityId = new SelectList(new List<StoreProductQuantityVm>(), "ID", "FullName");
            }

            return PartialView();
        }

        public ActionResult ProductExteraCategories(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product_Edit);

            var product = _context.Product.GetProduct(id);
            if (product != null)
            {
                var model = new ProductExteraVm();
                model.ProductId = product.ID;
                model.ProductName = product.Name;
                model.Categories = _context.Product.GetProductCategories(id);
                return View(model);
            }
            return null;
        }
        [HttpPost]
        public ActionResult ProductExteraCategories(ProductExteraVm productExtera)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product_Edit);

            var product = _context.Product.GetById(productExtera.ProductId);

            _context.productExteraType.DeleteByProductId(product.ID);
            _context.ProductExteraCategory.DeleteByProductId(product.ID);
            _context.ProductExteraSubCategory.DeleteByProductId(product.ID);

            _context.Save();
            if (productExtera.Categories != null && productExtera.Categories.Any())
            {
                foreach (var itemType in productExtera.Categories)
                {
                    if (itemType.Select)
                    {
                        product.ProductExteraType.Add(new ProductExteraType() { ProductId = product.ID, ProductTypeId = itemType.ID });
                    }
                    if (itemType.ProductCategory != null && itemType.ProductCategory.Any())
                    {
                        foreach (var itemCat in itemType.ProductCategory)
                        {
                            if (itemCat.Select)
                            {
                                product.ProductExteraCategory.Add(new ProductExteraCategory() { ProductId = product.ID, ProductCategoryId = itemCat.ID });
                            }
                            if (itemCat.ProductSubCategory != null && itemCat.ProductSubCategory.Any())
                            {
                                foreach (var itemSub in itemCat.ProductSubCategory)
                                {
                                    if (itemSub.Select)
                                    {
                                        product.ProductExteraSubCategory.Add(new ProductExteraSubCategory() { ProductId = product.ID, ProductSubCategoryId = itemSub.ID });
                                    }
                                }
                            }
                        }
                    }

                }

            }
            _context.Save();
            return RedirectToAction("index");
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
