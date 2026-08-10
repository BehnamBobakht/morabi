using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer.Entities;
using DataLayer.Base;
using System.Net;
using DataLayer.Enumarables;
using DataLayer.ViewModels;

namespace Panel.Areas.Store.Controllers
{
    public class QuestionRegisterItemController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCustomFieldItem);

            QuestionRegister field = _context.QuestionRegister.GetById(id);
            ViewBag.QuestionRegister = field;


            ViewBag.Id = id;
            return View(_context.QuestionRegisterItem.Search(fieldId:id.Value, pageSize: 1000));
        }

       

        public ActionResult Create(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCustomFieldItem_New);
            ViewBag.Id = id;
            QuestionRegister field = _context.QuestionRegister.GetById(id);
            QuestionRegisterItem item = new QuestionRegisterItem()
            {
                FieldId = field.ID,
                QuestionRegister = field
            };

            ViewBag.Message = BaseMessage.GetMessage();
            return View(item);
        }

        [HttpPost]
        public ActionResult Create(QuestionRegisterItem customItem)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCustomFieldItem_New);

         
            ViewMessage result = IsModelValid(customItem);
            if (result.Type == Enum_MessageType.SUCCESS)
            {
                _context.QuestionRegisterItem.Insert(customItem);
                _context.Save();

                
            }

            return new JsonResult() { Data = result };
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCustomFieldItem_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            QuestionRegisterItem customItem = _context.QuestionRegisterItem.GetById(id);
            if (customItem == null)
                return HttpNotFound();
            
            ViewBag.Message = BaseMessage.GetMessage();
            return View(customItem);
        }

        [HttpPost]
        public ActionResult Edit(QuestionRegisterItem customItem)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCustomFieldItem_Edit);
           
            ViewMessage result = IsModelValid(customItem);
            if (result.Type == Enum_MessageType.SUCCESS)
            {
                _context.QuestionRegisterItem.Update(customItem);
                _context.Save();

               

                
            }
            return new JsonResult() { Data = result };
        }

        private ViewMessage IsModelValid(QuestionRegisterItem customItem)
        {
            ViewMessage result = new ViewMessage();
            if (customItem.Value == null)
                result = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PRODUCT_CUSTOM_ITEM_VALUE);
            return result;
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCustomFieldItem_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            QuestionRegisterItem customItem = _context.QuestionRegisterItem.GetById(id);
            if (customItem == null)
                return HttpNotFound();

            return View(customItem);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCustomFieldItem_Delete);
            QuestionRegisterItem customItem = _context.QuestionRegisterItem.GetById(id);
            try
            {
                _context.QuestionRegisterItem.Delete(customItem);
                _context.Save();
                return RedirectToAction("index", new { id = customItem.FieldId });
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(customItem);
            }

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