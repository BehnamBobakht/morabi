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
    public class ClearingController : Controller
    {
        UnitOfWork _context = new UnitOfWork();
        public ActionResult Index(
            string text = null,
            int? storeId = null,
            int[] codeId = null,
            string codeStr = null,
            string from = null,
            string to = null,
            string pageIndex = null,
            string pageSize = null)
        {
            if (codeId == null && codeStr != null)
            {
                codeId = codeStr.Split('-').Select(n => Convert.ToInt32(n)).ToArray();
            }
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Clearing);
            DateTime fromDate = string.IsNullOrEmpty(from) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(from))).GetValueOrDefault();
            DateTime toDate = string.IsNullOrEmpty(to) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(to))).GetValueOrDefault();
            int size = pageSize == null ? 50 : pageSize.GetInteger();
            int index = pageIndex == null ? 1 : pageIndex.GetInteger();
            ViewBag.CodeId_2 = codeId; 
            ViewBag.CodeStr = codeStr;
            ViewBag.FromDatetime = from;
            ViewBag.ToDatetime = to;
            ViewBag.PageIndex = index.ToString();
            ViewBag.PageSize = size.ToString();
            ViewBag.Text = text;
            ViewBag.StoreId = storeId != null ? storeId.ToString() : null;

            FillDropDowns();

            ViewBag.TotalCount = _context.Clearing.SearchCount(
                codeId: codeId,
                storeId: storeId,
                text: text,
                    fromDatetime: fromDate,
                    toDatetime: toDate
                   );

            List<Clearing> listClearing = _context.Clearing.Search(
                    codeId: codeId,
                    storeId: storeId,
                    text: text,
                    fromDatetime: fromDate,
                    toDatetime: toDate,
                    pageSize: size,
                    index: index)
                .ToList();

            return View(listClearing.ToView());
        }
        private void FillDropDowns(int selected = 0)
        {
            bool hasDefault = selected == 0 ? false : true;
            ViewBag.CodeId = new SelectList(_context.Code.GetAllByCodeGroup(Enum_CodeGroup.CLEARING_TYPE, hasDefault), "ID", "Name", selected);
            ViewBag.Stores = new SelectList(_context.Store.GetAll(), "ID", "Name");

        }
        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Clearing_New);
            FillDropDowns();
            ViewBag.StoreId = new SelectList(_context.Store.GetAll(), "ID", "Name");

            return View();
        }
        [HttpPost]
        public ActionResult Create(Clearing clearing, HttpPostedFileBase file, string date)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Clearing_New);
            ViewMessage msg = new ViewMessage();

            if (IsModelValid(clearing, file, date, out msg))
            {
                var orderStatus = _context.Code.GetByLabel(Enum_Code.ORDER_STATUS_POST);
                int[] status = { orderStatus.ID };
                var orders = _context.AccountOrder.Search(pageSize: 2000, storeId: clearing.StoreId, statusId: status, clear: false);
                clearing.CreateDate = DateTime.Now;
                _context.Clearing.Insert(clearing);
                _context.AccountOrder.UpdateClearing(orders, clearing.ID);
                _context.Save();
                return RedirectToAction("index");
            }
            ViewBag.msg = msg;
            FillDropDowns();
            ViewBag.StoreId = new SelectList(_context.Store.GetAll(), "ID", "Name");
            return View(clearing);
        }
        private bool IsModelValid(Clearing clearing, HttpPostedFileBase file, string date, out ViewMessage msg)
        {
            bool result = false;
            if (clearing.Price < 1)
                msg = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_CLEARING_PRICE);
            else if (clearing.Detail == null)
                msg = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_CLEARING_DETAIL);
            else if (clearing.FromAccount == null)
                msg = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_CLEARING_FROMACCOUNT);
            else if (clearing.ToAccount == null)
                msg = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_CLEARING_TOACCOUNT);
            else if (clearing.StoreId < 1)
                msg = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_CLEARING_STOREID);
            else if (clearing.CodeId < 1)
                msg = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_CLEARING_CODE);

            else
            {
                if (file != null)
                {
                    Picture picture = _context.Picture.CreateAndUpload(Enum_Code.SYSTEM_TYPE_PANEL, file);
                    clearing.PictureId = picture.ID;
                }
                if (!string.IsNullOrEmpty(date))
                {
                    DateTime dateTime = DateTime.Now;
                    if(string.IsNullOrEmpty(date) != true)
                    {
                        dateTime= BaseDate.GetGregorian(new CustomDate(date)).GetValueOrDefault();

                    }
                    clearing.CreateDate = dateTime;
                }
                else
                {
                    msg = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.INVALID_REBATE_DATETIME);
                    return result;
                }
                msg = BaseMessage.GetMessage(Enum_MessageType.SUCCESS);
                result = true;
            }
            return result;
        }
        public ActionResult GetStoreClearing(int storeId)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Clearing_New);

            var store = _context.Store.GetById(storeId);
            var result = new Clearing();
            result.Price = _context.AccountOrder.GetOrderPrice(store.ID);
            result.ToAccount = $"بانک :{store.BankName} -شماره حساب : {store.BankAccountNo} -شماره کارت : {store.BankAccountCardNo} -شماره شبا : {store.BankAccountShaba} ";
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}