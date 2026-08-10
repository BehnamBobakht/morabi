using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer.ViewModels;

namespace Panel.Areas.Store.Controllers
{
    public class QuestionRegisterController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {


            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCustomField);

            List<QuestionRegister> list = _context.QuestionRegister.GetAll();
            return View(list);
        }

       
        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCustomField_New);
            FillDropDowns(null);



            QuestionRegister field = new QuestionRegister()
            {
               
            };

            ViewBag.Message = BaseMessage.GetMessage();

            return View(field);
        }

        [HttpPost]
        public ActionResult Create(QuestionRegister field)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCustomField_New);

       
            ViewMessage result = IsModelValid(field);
            if (result.Type == Enum_MessageType.SUCCESS)
            {
                _context.QuestionRegister.Insert(field);
                _context.Save();

               
            }
            return new JsonResult() { Data = result };
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCustomField_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            QuestionRegister field = _context.QuestionRegister.GetById(id);
            if (field == null)
                return HttpNotFound();

            FillDropDowns(field);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(field);
        }

        [HttpPost]
        public ActionResult Edit(QuestionRegister field)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCustomField_Edit);
          

            ViewMessage result = IsModelValid(field);
            if (result.Type == Enum_MessageType.SUCCESS)
            {
                _context.QuestionRegister.Update(field);
                _context.Save();

               
            }
            return new JsonResult() { Data = result };
        }

        private ViewMessage IsModelValid(QuestionRegister field)
        {
            ViewMessage result = new ViewMessage();
            if (field.Name == null)
                result = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_CUSTOM_FIELD_NAME);
            return result;
        }

        private void FillDropDowns(QuestionRegister field)
        {
            ViewBag.TypeId = new SelectList(_context.Code.GetAllByCodeGroup(Enum_CodeGroup.FIELD_TYPE), "ID", "Name", field != null ? field.TypeId : 0);
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCustomField_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            QuestionRegister field = _context.QuestionRegister.GetById(id);
            if (field == null)
                return HttpNotFound();

            return View(field);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int ID)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductCustomField_Delete);
            QuestionRegister field = _context.QuestionRegister.GetById(ID);
            try
            {
               
                _context.QuestionRegister.Delete(field);
                _context.Save();

                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(field);
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