using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Panel.Areas.Store.Controllers
{
    public class SmartOfferAnswerController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            List<SmartOfferAnswer> listSmartOfferAnswer = new List<SmartOfferAnswer>();
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.SmartOfferQuestion);
            if (id != null)
            {
                 listSmartOfferAnswer = _context.SmartOfferAnswer.Where(s => s.QuestionId == id.Value).ToList();

            }
            else
            {
                listSmartOfferAnswer = _context.SmartOfferAnswer.GetAll();

            }
            ViewBag.SmartOfferQuestion = _context.SmartOfferQuestion.GetById(id);
            return View(listSmartOfferAnswer);
        }

        

        public ActionResult Create(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.SmartOfferQuestion);
            ViewBag.SmartOfferQuestion = _context.SmartOfferQuestion.GetById(id);
            return View();
        }

        [HttpPost]
        public ActionResult Create(SmartOfferAnswer SmartOfferAnswer)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.SmartOfferQuestion);

           

            ViewMessage result = IsModelValid(SmartOfferAnswer);
            if (result.Type == Enum_MessageType.SUCCESS)
            {
                _context.SmartOfferAnswer.Insert(SmartOfferAnswer);
                _context.Save();

               
            }
            return new JsonResult() { Data = result };
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.SmartOfferQuestion);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            SmartOfferAnswer SmartOfferAnswer = _context.SmartOfferAnswer.GetById(id);
            if (SmartOfferAnswer == null)
                return HttpNotFound();

            ViewBag.Message = BaseMessage.GetMessage();
            return View(SmartOfferAnswer);
        }

        [HttpPost]
        public ActionResult Edit(SmartOfferAnswer SmartOfferAnswer)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.SmartOfferQuestion);
          
            ViewMessage result = IsModelValid(SmartOfferAnswer);
            if (IsModelValid(SmartOfferAnswer).Type == Enum_MessageType.SUCCESS)
            {
                _context.SmartOfferAnswer.Update(SmartOfferAnswer);
                _context.Save();

                
            }
            return new JsonResult() { Data = result };
        }

        private ViewMessage IsModelValid(SmartOfferAnswer SmartOfferAnswer)
        {
            ViewMessage result = new ViewMessage();
            if (SmartOfferAnswer.Answer == null)
                result = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_MENU_NAME);
            return result;
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.SmartOfferQuestion);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            SmartOfferAnswer SmartOfferAnswer = _context.SmartOfferAnswer.GetById(id);
            if (SmartOfferAnswer == null)
                return HttpNotFound();

            return View(SmartOfferAnswer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.SmartOfferQuestion);
            SmartOfferAnswer SmartOfferAnswer = _context.SmartOfferAnswer.GetById(id);
            try
            {
                int? QuestionId = SmartOfferAnswer.QuestionId;
                _context.SmartOfferAnswer.Delete(SmartOfferAnswer);
                _context.Save();
                return RedirectToAction("index", new { id = QuestionId });
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(SmartOfferAnswer);
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