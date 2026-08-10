using DataLayer.ViewModels;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Panel.Areas.Store.Controllers
{
    public class SmartOfferQuestionController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.SmartOfferQuestion);
            return View(_context.SmartOfferQuestion.GetAll().ToList());
        }

        private void FillDropDowns(SmartOfferQuestion question)
        {
            ViewBag.TypeId = new SelectList(_context.Code.GetAllByCodeGroup(Enum_CodeGroup.SMARTOFFER_TYPE), "ID", "Name", question != null ? question.TypeId : 0);
        }
        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.SmartOfferQuestion);
            FillDropDowns(null);
            return View(new SmartOfferQuestion());
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(SmartOfferQuestion SmartOfferQuestion)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.SmartOfferQuestion);


            ViewMessage result = IsModelValid(SmartOfferQuestion);
            if (result.Type == Enum_MessageType.SUCCESS)
            {


                _context.SmartOfferQuestion.Insert(SmartOfferQuestion);
                _context.Save();
                return new JsonResult() { Data = result };

            }
            else
            {
                return new JsonResult()
                {
                    Data = Resource.Lang.Error,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }




        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.SmartOfferQuestion);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            SmartOfferQuestion SmartOfferQuestion = _context.SmartOfferQuestion.GetById(id);
            if (SmartOfferQuestion == null)
                return HttpNotFound();
            FillDropDowns(SmartOfferQuestion);

            ViewBag.Message = BaseMessage.GetMessage();
            return View(SmartOfferQuestion);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(SmartOfferQuestion SmartOfferQuestion)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.SmartOfferQuestion);

            ViewMessage result = IsModelValid(SmartOfferQuestion);
            if (result.Type == Enum_MessageType.SUCCESS)
            {
                _context.SmartOfferQuestion.Update(SmartOfferQuestion);
                _context.Save();
                return new JsonResult() { Data = result };
            }
            else
            {
                return new JsonResult()
                {
                    Data = Resource.Lang.Error,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }



        private ViewMessage IsModelValid(SmartOfferQuestion SmartOfferQuestion)
        {
            ViewMessage result = new ViewMessage();
            if (SmartOfferQuestion.Question == null)
                result = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PRODUCT_TYPE_NAME);
            return result;
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.SmartOfferQuestion);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            SmartOfferQuestion SmartOfferQuestion = _context.SmartOfferQuestion.GetById(id);
            if (SmartOfferQuestion == null)
                return HttpNotFound();

            return View(SmartOfferQuestion);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.SmartOfferQuestion);
            SmartOfferQuestion item = _context.SmartOfferQuestion.GetById(id);
            try
            {
                var answers = _context.SmartOfferAnswer.Where(s => s.QuestionId == id).ToList();
                foreach (var answeritem in answers)
                {
                    _context.SmartOfferAnswer.Delete(answeritem);
                    _context.Save();
                }
                _context.SmartOfferQuestion.Delete(item);
                _context.Save();
            }
            catch
            {

            }
            return RedirectToAction("Index");

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