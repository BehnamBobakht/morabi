using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Text;
using DataLayer.Helpers.Common;
using System.Data;
using DataLayer.ViewModels;

namespace Panel.Areas.Crm.Controllers
{
    public class AccountOrderController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(
            string status = null,
            int? fromorderid = null,
            int? toorderid = null,
            int? merchantId = null,
            int[] statusId = null,
            string from = null,
            string to = null,
            string refid = null,
            string customer = null,
            string product = null,
            string rebate = null,
            string pageIndex = null,
            string pageSize = null,
            int? storeId = null,
            int? typeId = null,
            int? optionId = null,
            int? accountId = null,
            int[] stores = null,
            string giftMessage = null

            )
        {

            if (statusId == null && status != null)
            {
                statusId = status.Split('-').Select(n => Convert.ToInt32(n)).ToArray();
            }

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.AccountOrder);

            DateTime fromDate = string.IsNullOrEmpty(from) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(from))).GetValueOrDefault();
            DateTime toDate = string.IsNullOrEmpty(to) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(to))).GetValueOrDefault();
            int size = pageSize == null ? 50 : pageSize.GetInteger();
            int index = pageIndex == null ? 1 : pageIndex.GetInteger();
            ViewBag.OptionId = optionId;
            ViewBag.TypeId = typeId;
            ViewBag.FromOrderId = fromorderid;
            ViewBag.ToOrderId = toorderid;
            ViewBag.StatusId_2 = statusId;
            ViewBag.GiftMessage = giftMessage;
            ViewBag.Status = status;
            ViewBag.Product = product;
            ViewBag.FromDatetime = from;
            ViewBag.ToDatetime = to;
            ViewBag.RefId = refid;
            ViewBag.Customer = customer;
            ViewBag.Rebate = rebate;
            ViewBag.SelectedId = 0;
            ViewBag.PageIndex = index.ToString();
            ViewBag.PageSize = size.ToString();
            ViewBag.MerchantId = merchantId;
            ViewBag.StoreId = storeId != null ? storeId.ToString() : null;

            FillDropDowns();

            ViewBag.TotalCount = _context.AccountOrder.SearchCount(
                    merchantId: merchantId,
                    product: product,
                    storeId: storeId,
                    accountId: accountId,
                    stores: stores,
                    typeId: typeId,
                    statusId: statusId,
                    refId: refid,
                    customer: customer,
                    fromDatetime: fromDate,
                    toDatetime: toDate,
                    fromorderid: fromorderid,
                    rebate: rebate,
                    toorderid: toorderid, giftMessage: giftMessage);

            List<AccountOrder> listOrder = _context.AccountOrder.Search(
                    merchantId: merchantId,
                    statusId: statusId,
                    pageSize: size,
                    product: product,
                    storeId: storeId,
                    index: index,
                    accountId: accountId,
                    refId: refid,
                    customer: customer,
                    fromDatetime: fromDate,
                    toDatetime: toDate,
                    fromorderid: fromorderid,
                    rebate: rebate,
                    toorderid: toorderid,
                    typeId: typeId,
                    stores: stores, giftMessage: giftMessage)
                .ToList();
            ViewBag.SumPrice = listOrder.Sum(s => s.Price).GetCurrencyFormat();
            return View(listOrder.ToView());
        }

        public ActionResult ProductList(
            int? fromorderid = null,
            int? toorderid = null,
            int? merchantId = null,
            string status = null,
            string from = null,
            string to = null,
            string refid = null,
            string customer = null,
            string pageIndex = null,
            string pageSize = null,
            int? typeId = null)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.AccountOrder);

            DateTime fromDate = string.IsNullOrEmpty(from) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(from))).GetValueOrDefault();
            DateTime toDate = string.IsNullOrEmpty(to) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(to))).GetValueOrDefault();
            int size = pageSize == null ? 50 : pageSize.GetInteger();
            int index = pageIndex == null ? 1 : pageIndex.GetInteger();
            int[] statusId = status.Split('-').Select(n => Convert.ToInt32(n)).ToArray();

            List<AccountOrder> list = _context.AccountOrder.Search(
                merchantId: merchantId,
                statusId: statusId,
                pageSize: size,
                index: index,
                typeId: typeId,
                refId: refid,
                customer: customer,
                fromDatetime: fromDate,
                toDatetime: toDate,
                fromorderid: fromorderid,
                toorderid: toorderid);

            List<KeyValuePair<Product, int>> productList = new List<KeyValuePair<Product, int>>();
            List<AccountOrderProduct> listOrder = new List<AccountOrderProduct>();
            foreach (AccountOrder order in list)
            {
                listOrder.AddRange(order.AccountOrderProduct.ToList());
            }

            foreach (AccountOrderProduct product in listOrder)
            {
                var item = productList.Where(p => p.Key.ID == product.ProductId);
                if (item.Any())
                {
                    int oldCount = item.First().Value;
                    productList.Remove(item.First());
                    productList.Add(new KeyValuePair<Product, int>(product.Product, product.Count + oldCount));
                }
                else
                {
                    productList.Add(new KeyValuePair<Product, int>(product.Product, product.Count));
                }
            }

            productList = productList.OrderByDescending(p => p.Value).ToList();

            return View(productList);
        }
        public ActionResult ProductListByTypeId(

           string status = null,
           string from = null,
           string to = null,

           string customer = null,
           int? typeId = null,
           int? optionId = null
           )
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.AccountOrder);
            ViewBag.FromDatetime = from;
            ViewBag.toDatetime = from;

            ViewBag.TypeId = typeId;
            ViewBag.OptionId = optionId;

            DateTime fromDate = string.IsNullOrEmpty(from) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(from))).GetValueOrDefault();
            DateTime toDate = string.IsNullOrEmpty(to) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(to))).GetValueOrDefault();

            int[] statusId = status.Split('-').Select(n => Convert.ToInt32(n)).ToArray();
            ViewBag.statusString = status;
            List<AccountOrderProduct> list = _context.AccountOrderProduct.Search(
                statusId: statusId,
                fromDatetime: fromDate,
                toDatetime: toDate,
                optionId: optionId,
                typeId: typeId
                );


            return View(list);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.AccountOrder_Details);
            AccountOrder order = _context.AccountOrder.GetById(id);

            if (order == null)
            {
                return HttpNotFound();
            }

            SmsSetting setting = _context.SmsSetting.GetBySmsType(Enum_SmsType.ORDER_POST);
            if (setting != null)
            {
                StringBuilder body = new StringBuilder();
                body.AppendLine("سفارش شما در سایت " /*+ order.Shop.Name + " برای شما ارسال شد"*/);
                ViewBag.SmsBody = body;
            }
            ViewBag.Message = BaseMessage.GetMessage();
            FillDropDowns(order.StatusId);
            return View(order);
        }
        [HttpPost]
        public ActionResult ChangeStatus(int OrderId, int StatusId, string PostalCode, string Description, string SmsBody)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.AccountOrder_Details);
            AccountOrder order = _context.AccountOrder.GetById(OrderId);
            int last_status = order.StatusId;
            Code status = _context.Code.GetById(StatusId);

            order.StatusId = StatusId;
            order.Description = Description;
            _context.AccountOrder.Update(order);
            _context.Save();

            if (last_status != status.ID && status.Label == Enum_Code.ORDER_STATUS_CANCEL.ToString())
            {
                string token1 = order.Account.FullName;
                string token2 = order.UniqueId;
                string token3 = DateTime.Now.ToPersianComplete();
                _context.Sms.SaveNewSms(order.Account.Mobile, Enum_SmsType.ORDER_CANCEL, SmsBody, token1, token2, token3);
                _context.Save();
            }
            if (last_status != status.ID && status.Label == Enum_Code.ORDER_STATUS_PROCESS.ToString())
            {
                string token1 = order.Account.FullName;
                string token2 = order.UniqueId;
                string token3 = "";

                _context.Sms.SaveNewSms(order.Account.Mobile, Enum_SmsType.ORDER_PROCESS, SmsBody, token1, token2, token3);
                _context.Save();
            }
            if (last_status != status.ID && status.Label == Enum_Code.ORDER_STATUS_POST.ToString())
            {
                string token1 = order.Account.FullName;
                string token2 = order.UniqueId;
                string token3 = (order.Delivery != null ? order.Delivery.DeliveryType.Name : "مامور ارسال");
                _context.Sms.SaveNewSms(order.Account.Mobile, Enum_SmsType.ORDER_POST, "", token1, token2, token3);
                _context.Save();
            }


            FillDropDowns(order.StatusId);

            return RedirectToAction("Index", new { status = last_status });
        }
        [HttpGet]
        public ActionResult ChangeStatusAll(string product = null)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.AccountOrder_Details);
            var list = _context.AccountOrder.Where(p => p.StatusId == 2068 && p.AccountOrderProduct.Any(q =>
                      q.Product.Name.Contains(product) ||
                      q.Product.Summary.Contains(product) ||
                      q.Product.ProductType.Name.Contains(product) ||
                      q.Product.ProductCategory.Name.Contains(product) ||
                      q.Product.ProductSubCategory.Name.Contains(product))).ToList();
            foreach (var item in list)
            {


                item.StatusId = 2073;

                _context.AccountOrder.Update(item);
                _context.Save();
            }


            return RedirectToAction("Index", new { status = 2068 });
        }

        public ActionResult Print(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.AccountOrder_Details);
            AccountOrder order = _context.AccountOrder.GetById(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            return View(order);
        }

        public ActionResult PrintAddress(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.AccountOrder_Details);
            AccountOrder order = _context.AccountOrder.GetById(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            return View(order);
        }

        private void FillDropDowns(int selected = 0)
        {
            bool hasDefault = selected == 0 ? false : true;
            ViewBag.StatusId = new SelectList(_context.Code.GetAllByCodeGroup(Enum_CodeGroup.ORDER_STATUS, hasDefault), "ID", "Name", selected);
            ViewBag.MerchantList = _context.Merchant.GetAll();
            ViewBag.ProductTypeList = _context.ProductType.GetAll();
            ViewBag.OptionList = _context.ProductOptionItem.Where(s => s.OptionId == 1).ToList();

            ViewBag.Stores = new SelectList(_context.Store.GetAll(), "ID", "Name");

        }

        public ActionResult GoToStore(
            string status = null,
            int? fromorderid = null,
            int? toorderid = null,
            int? merchantId = null,
            string from = null,
            string to = null,
            string refid = null,
            string customer = null,
            string pageIndex = null,
            string pageSize = null,
            string orderId = null)
        {
            int[] statusId = status.Split('-').Select(n => Convert.ToInt32(n)).ToArray();
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.AccountOrder_Details);
            if (orderId != null)
            {
                List<string> listOrder = orderId.Split(',').ToList();
                foreach (string IdStr in listOrder)
                {
                    int Id = IdStr.GetInteger();
                    if (Id != 0)
                    {
                        AccountOrder order = _context.AccountOrder.GetById(Id);
                        if (order != null &&
                            (
                                order.Code.Label == Enum_Code.ORDER_STATUS_SUCCESS.ToString() ||
                                order.Code.Label == Enum_Code.ORDER_STATUS_PROCESS.ToString())
                            )
                        {
                            //order.StatusId = _context.Code.GetIdByLabel(Enum_Code.ORDER_STATUS_STORE);
                            _context.AccountOrder.Update(order);
                            _context.Save();
                        }
                    }
                }
            }

            return RedirectToAction("index", new
            {
                @fromorderid = fromorderid,
                @toorderid = toorderid,
                @statusId = statusId,
                @merchantid = merchantId,
                @from = from,
                @to = to,
                @refid = refid,
                @customer = customer,
                @pagesize = pageSize,
                @pageIndex = pageIndex
            });
        }

        public ActionResult GoToProcess(
            string status = null,
            int? fromorderid = null,
            int? toorderid = null,
            int? merchantId = null,
            string from = null,
            string to = null,
            string refid = null,
            string customer = null,
            string pageIndex = null,
            string pageSize = null,
            string orderId = null)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.AccountOrder_Details);
            if (orderId != null)
            {
                List<string> listOrder = orderId.Split(',').ToList();
                foreach (string IdStr in listOrder)
                {
                    int Id = IdStr.GetInteger();
                    if (Id != 0)
                    {
                        AccountOrder order = _context.AccountOrder.GetById(Id);
                        if (order != null &&
                            (
                                order.Code.Label == Enum_Code.ORDER_STATUS_SUCCESS.ToString() ||
                                order.Code.Label == Enum_Code.ORDER_STATUS_PROCESS.ToString())
                            )
                        {
                            order.StatusId = _context.Code.GetIdByLabel(Enum_Code.ORDER_STATUS_PROCESS);
                            _context.AccountOrder.Update(order);
                            _context.Save();
                        }
                    }
                }
            }

            int[] statusId = status.Split('-').Select(n => Convert.ToInt32(n)).ToArray();

            return RedirectToAction("index", new
            {
                @fromorderid = fromorderid,
                @toorderid = toorderid,
                @merchantid = merchantId,
                @statusId = statusId,
                @from = from,
                @to = to,
                @refid = refid,
                @customer = customer,
                @pagesize = pageSize,
                @pageIndex = pageIndex
            });
        }

        public ActionResult AddressList(
            string status = null,
            int? fromorderid = null,
            int? toorderid = null,
            int? merchantId = null,
            int[] statusId = null,
            string from = null,
            string to = null,
            string refid = null,
            string customer = null,
            string product = null,
            string rebate = null,
            string pageIndex = null,
            string pageSize = null,
            int? storeId = null,
            int[] stores = null
            )
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.AccountOrder_Details);
            if (statusId == null && status != null)
            {
                statusId = status.Split('-').Select(n => Convert.ToInt32(n)).ToArray();
            }
            DateTime fromDate = string.IsNullOrEmpty(from) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(from))).GetValueOrDefault();
            DateTime toDate = string.IsNullOrEmpty(to) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(to))).GetValueOrDefault();
            int size = pageSize == null ? 5000000 : pageSize.GetInteger();
            int index = pageIndex == null ? 1 : pageIndex.GetInteger();
            ViewBag.Product = product;
            var listOrders = _context.AccountOrder.Search(
                    merchantId: merchantId,
                    statusId: statusId,
                    pageSize: size,
                    product: product,
                    storeId: storeId,
                    index: index,
                    refId: refid,
                    customer: customer,
                    fromDatetime: fromDate,
                    toDatetime: toDate,
                    fromorderid: fromorderid,
                    rebate: rebate,
                    toorderid: toorderid,
                    stores: stores)
                .OrderByDescending(p => p.ID)
                .ToList().ToView();
            return View(listOrders);
        }

        public ActionResult ExportExcel(
            string status = null,
            int? fromorderid = null,
            int? toorderid = null,
            int? merchantId = null,
            int[] statusId = null,
            string from = null,
            string to = null,
            string refid = null,
            string customer = null,
            string product = null,
            string rebate = null,
            string pageIndex = null,
            string pageSize = null,
            int? storeId = null,
            int[] stores = null
            )
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.AccountOrder_Details);
            if (statusId == null && status != null)
            {
                statusId = status.Split('-').Select(n => Convert.ToInt32(n)).ToArray();
            }
            DateTime fromDate = string.IsNullOrEmpty(from) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(from))).GetValueOrDefault();
            DateTime toDate = string.IsNullOrEmpty(to) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(to))).GetValueOrDefault();
            int size = pageSize == null ? 50 : pageSize.GetInteger();
            int index = pageIndex == null ? 1 : pageIndex.GetInteger();

            var listOrders = _context.AccountOrder.Search(
                    merchantId: merchantId,
                    //statusId: statusId,
                    pageSize: size,
                    product: product,
                    storeId: storeId,
                    index: index,
                    refId: refid,
                    customer: customer,
                    fromDatetime: fromDate,
                    toDatetime: toDate,
                    fromorderid: fromorderid,
                    rebate: rebate,
                    toorderid: toorderid,
                    stores: stores)
                .OrderByDescending(p => p.ID)
                .ToList().ToView();

            DataTable table = new DataTable();
            table.Columns.Add("#");
            table.Columns.Add(Resource.Lang.FactureNumber);
            table.Columns.Add(Resource.Lang.Name_Family);
            table.Columns.Add(Resource.Lang.Mobile);
            table.Columns.Add($"{Resource.Lang.PaiedAmount}({Resource.Lang.Toman})");
            table.Columns.Add(Resource.Lang.Status);
            table.Columns.Add(Resource.Lang.PaymentType);
            table.Columns.Add(Resource.Lang.Date);
            table.Columns.Add(Resource.Lang.Shipping);
            table.Columns.Add(Resource.Lang.ShippingPrice);
            table.Columns.Add(Resource.Lang.Products);
            table.Columns.Add(Resource.Lang.StateName);
            table.Columns.Add(Resource.Lang.CityName);
            table.Columns.Add(Resource.Lang.Address);
            table.Columns.Add(Resource.Lang.PostalCode);

            for (int i = 0; i < listOrders.Count; i++)
            {
                var item = listOrders[i];
                string products = "";
                foreach (var p in item.AccountOrderProducts)
                {
                    products += $"{p.Product.Name} - {Resource.Lang.Count}({p.Count}) || ";
                }
                table.Rows.Add(
                    (i + 1),
                    item.UniqueId,
                    item.Account.FullName,
                    item.Account.Mobile,
                    item.Price.GetCurrencyFormat(),
                    item.Status.Name,
                    item.PaymentType.Name,
                    item.Datetime.ToPersianWithTime(),
                    item.DeliveryType,
                    item.DeliveryPrice.GetCurrencyFormat(),
                    products,
                    item.AccountAddress?.State.Name,
                    item.AccountAddress?.City.Name,
                    item.AccountAddress?.AddressValue,
                    item.AccountAddress?.PostalCode

                    );
            }

            BaseExcel.ExportExcel(table);
            return View(listOrders);
        }
        public ActionResult ExportExcel2(
            string status = null,
            int? fromorderid = null,
            int? toorderid = null,
            int? merchantId = null,
            int[] statusId = null,
            string from = null,
            string to = null,
            string refid = null,
            string customer = null,
            string product = null,
            string rebate = null,
            string pageIndex = null,
            string pageSize = null,
            int? storeId = null,
            int[] stores = null
            )
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.AccountOrder_Details);
            if (statusId == null && status != null)
            {
                statusId = status.Split('-').Select(n => Convert.ToInt32(n)).ToArray();
            }
            DateTime fromDate = string.IsNullOrEmpty(from) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(from))).GetValueOrDefault();
            DateTime toDate = string.IsNullOrEmpty(to) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(to))).GetValueOrDefault();
            int size = pageSize == null ? 900000000 : pageSize.GetInteger();
            int index = pageIndex == null ? 1 : pageIndex.GetInteger();

            var listOrders = _context.AccountOrder.Search(
                    merchantId: merchantId,
                    statusId: statusId,
                    pageSize: size,
                    product: product,
                    storeId: storeId,
                    index: index,
                    refId: refid,
                    customer: customer,
                    fromDatetime: fromDate,
                    toDatetime: toDate,
                    fromorderid: fromorderid,
                    rebate: rebate,
                    toorderid: toorderid,
                    stores: stores)
                .OrderByDescending(p => p.ID)
                .ToList().ToView();

            DataTable table = new DataTable();
            table.Columns.Add("#");
            table.Columns.Add(Resource.Lang.FactureNumber);
            table.Columns.Add(Resource.Lang.Name_Family);
            table.Columns.Add(Resource.Lang.Mobile);
            table.Columns.Add($"{Resource.Lang.PaiedAmount}({Resource.Lang.Toman})");
            table.Columns.Add(Resource.Lang.Status);
            table.Columns.Add(Resource.Lang.PaymentType);
            table.Columns.Add(Resource.Lang.Date);
            table.Columns.Add(Resource.Lang.Shipping);
            table.Columns.Add(Resource.Lang.ShippingPrice);
            table.Columns.Add(Resource.Lang.Products);
            table.Columns.Add(Resource.Lang.StateName);
            table.Columns.Add(Resource.Lang.CityName);
            table.Columns.Add(Resource.Lang.Address);
            table.Columns.Add(Resource.Lang.PostalCode);

            for (int i = 0; i < listOrders.Count; i++)
            {
                var item = listOrders[i];
                string products = "";
                foreach (var p in item.AccountOrderProducts)
                {
                    products += $"{p.Product.Name} - {Resource.Lang.Count}({p.Count}) || ";
                }
                table.Rows.Add(
                    (i + 1),
                    item.UniqueId,
                    item.Account.FullName,
                    item.Account.Mobile,
                    item.Price.GetCurrencyFormat(),
                    item.Status.Name,
                    item.PaymentType.Name,
                    item.Datetime.ToPersianWithTime(),
                    item.DeliveryType,
                    item.DeliveryPrice.GetCurrencyFormat(),
                    products,
                    item.AccountAddress?.State.Name,
                    item.AccountAddress?.City.Name,
                    item.AccountAddress?.AddressValue,
                    item.AccountAddress?.PostalCode

                    );
            }
            
            //table.Rows.Add(
            //      "شماره فاکتور",
            //      "نام و نام خانوادگی",
            //      "شماره موبایل",
            //      "مبلغ کل",
            //      "گواهی برای دوره",
            //      "تاریخ"


            //      );

          
            //var paymentCertificate = _context.PaymentCertificate.Where(s => s.RefNumber != null && s.Datetime >= fromDate && s.Datetime <= toDatetimeTemp);
            //foreach (var item in paymentCertificate)
            //{
               
            //    table.Rows.Add(

            //        item.ID,
            //        item.Account.FullName,
            //        item.Account.Mobile,
            //        item.Amount.GetCurrencyFormat(),
            //        item.Product.Name,
            //        item.Datetime.ToPersianWithTime()

            //        );
            //}
            BaseExcel.ExportExcel(table);
            return View(listOrders);
        }
        public ActionResult ExportExcel4(
           string status = null,
           int? fromorderid = null,
           int? toorderid = null,
           int? merchantId = null,
           int[] statusId = null,
           string from = null,
           string to = null,
           string refid = null,
           string customer = null,
           string product = null,
           string rebate = null,
           string pageIndex = null,
           string pageSize = null,
           int? storeId = null,
           int[] stores = null
           )
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.AccountOrder_Details);
            if (statusId == null && status != null)
            {
                statusId = status.Split('-').Select(n => Convert.ToInt32(n)).ToArray();
            }
            DateTime fromDate = string.IsNullOrEmpty(from) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(from))).GetValueOrDefault();
            DateTime toDate = string.IsNullOrEmpty(to) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(to))).GetValueOrDefault();
            int size = pageSize == null ? 900000000 : pageSize.GetInteger();
            int index = pageIndex == null ? 1 : pageIndex.GetInteger();
            DataTable table = new DataTable();

            table.Columns.Add("شماره فاکتور");
            table.Columns.Add("نام و نام خانوادگی");
            table.Columns.Add("شماره موبایل");
            table.Columns.Add("مبلغ کل");
            table.Columns.Add("آزمون ها");
            table.Columns.Add("تاریخ");

            DateTime toDatetimeTemp = toDate.AddDays(1);
            var paymentExame = _context.PaymentExame.Where(s => s.RefNumber != null && s.Datetime >= fromDate && s.Datetime <= toDatetimeTemp).ToList().ToView(); ;
            foreach (var item in paymentExame)
            {
                var exameName = "";
                foreach (var sub in item.ExameOrder)
                {
                    exameName += sub.Name + "-";
                }
                table.Rows.Add(

                    item.Id,
                    item.Account.FullName,
                    item.Account.Mobile,
                    item.Price.GetCurrencyFormat(),
                    exameName,
                    item.Datetime.ToPersianWithTime()

                    );
            }
            
            BaseExcel.ExportExcel(table);
            return RedirectToAction("index", new
            {
                @fromorderid = fromorderid,
                @toorderid = toorderid,
                @merchantid = merchantId,
                @statusId = statusId,
                @from = from,
                @to = to,
                @refid = refid,
                @customer = customer,
                @pagesize = pageSize,
                @pageIndex = pageIndex
            });
        }
        public ActionResult ExportExcel5(
   string status = null,
   int? fromorderid = null,
   int? toorderid = null,
   int? merchantId = null,
   int[] statusId = null,
   string from = null,
   string to = null,
   string refid = null,
   string customer = null,
   string product = null,
   string rebate = null,
   string pageIndex = null,
   string pageSize = null,
   int? storeId = null,
   int[] stores = null
   )
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.AccountOrder_Details);
            if (statusId == null && status != null)
            {
                statusId = status.Split('-').Select(n => Convert.ToInt32(n)).ToArray();
            }
            DateTime fromDate = string.IsNullOrEmpty(from) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(from))).GetValueOrDefault();
            DateTime toDate = string.IsNullOrEmpty(to) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(to))).GetValueOrDefault();
            int size = pageSize == null ? 900000000 : pageSize.GetInteger();
            int index = pageIndex == null ? 1 : pageIndex.GetInteger();
            DataTable table = new DataTable();

            table.Columns.Add("شماره فاکتور");
            table.Columns.Add("نام و نام خانوادگی");
            table.Columns.Add("شماره موبایل");
            table.Columns.Add("مبلغ کل");
            table.Columns.Add("گواهی برای");
            table.Columns.Add("تاریخ");

            DateTime toDatetimeTemp = toDate.AddDays(1);
            var paymentExame = _context.PaymentCertificate.Where(s => s.RefNumber != null && s.Datetime >= fromDate && s.Datetime <= toDatetimeTemp).ToList().ToView(); ;
            foreach (var item in paymentExame)
            {               
                table.Rows.Add(

                    item.Id,
                    item.Account.FullName,
                    item.Account.Mobile,
                    item.Price.GetCurrencyFormat(),
                    item.Product.Name,
                    item.Datetime.ToPersianWithTime()

                    );
            }
            BaseExcel.ExportExcel(table);
            return RedirectToAction("index", new
            {
                @fromorderid = fromorderid,
                @toorderid = toorderid,
                @merchantid = merchantId,
                @statusId = statusId,
                @from = from,
                @to = to,
                @refid = refid,
                @customer = customer,
                @pagesize = pageSize,
                @pageIndex = pageIndex
            });
        }
        public ActionResult ExportExcel3(
           string status = null,
           int? fromorderid = null,
           int? toorderid = null,
           int? merchantId = null,
           int[] statusId = null,
           string from = null,
           string to = null,
           string refid = null,
           string customer = null,
           string product = null,
           string rebate = null,
           string pageIndex = null,
           string pageSize = null,
           int? storeId = null,
           int[] stores = null
           )
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.AccountOrder_Details);

            DateTime fromDate = string.IsNullOrEmpty(from) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(from))).GetValueOrDefault();
            DateTime toDate = string.IsNullOrEmpty(to) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(to))).GetValueOrDefault();
            int size = pageSize == null ? 50 : pageSize.GetInteger();
            int index = pageIndex == null ? 1 : pageIndex.GetInteger();
            if (fromDate != null && toDate != null && fromDate != default(DateTime) && toDate != default(DateTime))
            {
                DateTime toDatetimeTemp = toDate.AddDays(1);
                var list = _context.AccountOrderProduct
                    .Where(s => s.Datetime >= fromDate && s.Datetime <= toDatetimeTemp && s.AccountOrder.StatusId != 2088 && s.AccountOrder.StatusId != 67).GroupBy(s => s.Product)
    .Select(g => new ViewPayOrder { Name = g.Key.Name, Price = g.Sum(c => c.ProductPrice.Value), Count = g.Sum(c => c.Count) }).ToList();
                DataTable table = new DataTable();
                table.Columns.Add("#");
                table.Columns.Add(Resource.Lang.Name);
                table.Columns.Add(Resource.Lang.Price);
                table.Columns.Add(Resource.Lang.Count);
                int i = 1;
                foreach (var item in list.OrderByDescending(s => s.Price))
                {
                    table.Rows.Add(
                        (i + 1),
                        item.Name,
                        item.Price.GetCurrencyFormat(),
                        item.Count


                        );
                }


                BaseExcel.ExportExcel(table);
                return RedirectToAction("index", new
                {
                    @fromorderid = fromorderid,
                    @toorderid = toorderid,
                    @merchantid = merchantId,
                    @statusId = statusId,
                    @from = from,
                    @to = to,
                    @refid = refid,
                    @customer = customer,
                    @pagesize = pageSize,
                    @pageIndex = pageIndex
                });

            }
            else
            {
                var list = _context.AccountOrderProduct
                    .Where(s => s.AccountOrder.StatusId != 2088 && s.AccountOrder.StatusId != 67).GroupBy(s => s.Product)
    .Select(g => new ViewPayOrder { Name = g.Key.Name, Price = g.Sum(c => c.ProductPrice.Value), Count = g.Sum(c => c.Count) });

                DataTable table = new DataTable();
                table.Columns.Add("#");
                table.Columns.Add(Resource.Lang.Name);
                table.Columns.Add(Resource.Lang.Price);
                table.Columns.Add(Resource.Lang.Count);
                int i = 1;
                foreach (var item in list.OrderByDescending(s => s.Price))
                {
                    table.Rows.Add(
                        (i + 1),
                        item.Name,
                        item.Price.GetCurrencyFormat(),
                        item.Count


                        );
                }


                BaseExcel.ExportExcel(table);
                return RedirectToAction("index", new
                {
                    @fromorderid = fromorderid,
                    @toorderid = toorderid,
                    @merchantid = merchantId,
                    @statusId = statusId,
                    @from = from,
                    @to = to,
                    @refid = refid,
                    @customer = customer,
                    @pagesize = pageSize,
                    @pageIndex = pageIndex
                });
            }



        }
        [HttpGet]
        public ActionResult Report(int? accountId = null,
    int? storeId = null,
    int[] stores = null,
    int? merchantId = null,
    string refId = null,
    int[] statusId = null,
    List<string> statusStr = null,
    string customer = null,
    string product = null,
    int? fromorderid = null,
    int? toorderid = null,
    string from = null,
            string to = null,
    string rebate = null,
    bool? clear = null,
    Enum_Report reportType = Enum_Report.ORDER_STORE_SELL)
        {
            DateTime fromDate = string.IsNullOrEmpty(from) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(from))).GetValueOrDefault();
            DateTime toDate = string.IsNullOrEmpty(to) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(to))).GetValueOrDefault();

            var model = BaseReport.AccountOrder(
                unitOfWork: _context,
                accountId: accountId,
                storeId: storeId,
                stores: stores,
                merchantId: merchantId,
                customer: customer,
                fromDatetime: fromDate,
                fromorderid: fromorderid,
                product: product,
                rebate: rebate,
                refId: refId,
                statusId: statusId,
                statusStr: statusStr,
                toDatetime: toDate,
                toorderid: toorderid,
                clear: clear,
                reportType: reportType);
            var a = Json(model, JsonRequestBehavior.AllowGet);
            return a;
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