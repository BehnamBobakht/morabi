using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer.Entities;
using DataLayer.Base;
using DataLayer.Api;
using Cms.Areas.Store.Security;
using System.IO;
using DataLayer.Enumarables;

namespace Cms.Areas.Store.Controllers
{
    public class AccountsController : Controller
    {
        UnitOfWork _context = new UnitOfWork();

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(DataLayer.Entities.Store store)
        {
            var result = DataLayer.Api.Model.ApiStore.Post_DoRegister(_context, store);
            ViewBag.Message = result;
            if (result.Code == ApiResult.ResponseCode.Success)
                return View(new DataLayer.Entities.Store());
            else
                return View(store);
        }
        [AuthorizeFilter(Enum_StorePanel = DataLayer.Enumarables.Enum_StorePanel.StoreState)]
        public ActionResult Edit()
        {
            var currentAccount = BaseWebsite.CurrentAccount;
            var model = _context.Store.GetById(currentAccount.StoreId);
            return View(model);
        }
        [HttpPost]
        [AuthorizeFilter(Enum_StorePanel = DataLayer.Enumarables.Enum_StorePanel.StoreState)]
        public ActionResult Edit(DataLayer.Entities.Store store, HttpPostedFileBase imgUp)
        {
            var currentAccount = BaseWebsite.CurrentAccount;
            store.ID = currentAccount.StoreId;
            if (imgUp != null)
            {
                Picture picture = _context.Picture.CreateAndUpload(Enum_Code.SYSTEM_TYPE_CMS, imgUp);
                if (picture != null)
                    store.PictureId = picture.ID;
                else
                    store.PictureId = currentAccount.Store.PictureId;
            }
            else
            {
                store.PictureId = currentAccount.Store.PictureId;
            }
            _context.Store.EditByStore(store);
            return RedirectToAction("edit");
        }
        [HttpPost]
        public ActionResult AddImage(HttpPostedFileBase imgUp,int id)
        {
            DataLayer.Entities.Store store = _context.Store.FirstOrDefault(x => x.ID == id);
            if (imgUp != null)
            {
                Picture picture = _context.Picture.CreateAndUpload(Enum_Code.SYSTEM_TYPE_CMS, imgUp);
                if (picture != null)
                    store.PictureId = picture.ID;
            }
            _context.Save();
            return RedirectToAction("edit");
        }

        private string GetFilePath(string fileName)
        {
            Random rand = new Random();
            int folder1 = rand.Next(1, 11);
            int folder2 = rand.Next(1, 11);
            int folder3 = rand.Next(1, 11);
            string PicName = BasePath.UploadPicture + folder1 + "/" + folder2 + "/" + folder3 + "/" + Guid.NewGuid() + Path.GetExtension(fileName);
            return PicName;
        }

        //private bool IsModelValid(DataLayer.Entities.Store store)
        //{
        //    bool result = false;
        //    if (BaseWebsite.CurrentAccount == null)
        //        ViewBag.Message = BaseMessage.GetMessage(Enum_Message.REQUIRED_STORE_LOGIN);
        //    else if (store.Name == null)
        //        ViewBag.Message = BaseMessage.GetMessage(Enum_Message.REQUIRED_STORE_NAME);
        //    else if (store.AddressValue == null)
        //        ViewBag.Message = BaseMessage.GetMessage(Enum_Message.REQUIRED_STORE_ADDRESS_VALUE);
        //    else if (store.Description == null)
        //        ViewBag.Message = BaseMessage.GetMessage(Enum_Message.REQUIRED_STORE_DESCRIPTION);
        //    else
        //        result = true;

        //    return result;
        //}

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