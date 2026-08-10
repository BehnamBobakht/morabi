using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Panel.Areas.Content.Controllers
{
    [ValidateInput(false)]

    public class StateController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.State);
            return View(_context.State.GetAll());
        }


        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.State);
          
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( State State)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.State);
          
            if (IsModelValid(State))
            {
                _context.State.Insert(State);
                _context.Save();
                return RedirectToAction("Index");
            }

           
            return View(State);
        }
       
       
        public ActionResult Edit(int? id)
        {
            //_context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            State State = _context.State.GetById(id);
            if (State == null)
                return HttpNotFound();

            ViewBag.Message = BaseMessage.GetMessage();
            return View(State);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(State State)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.State);
            if (IsModelValid(State))
            {
                _context.State.Update(State);
                _context.Save();
                return RedirectToAction("Index");
            }
            return View(State);
        }

        private bool IsModelValid(State State)
        {
            bool result = false;
            if (State.Name == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_BANNER_NAME);
            else
                result = true;

            return result;
        }

       

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.State);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            State State = _context.State.GetById(id);
            if (State == null)
                return HttpNotFound();

            return View(State);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.State);
            State State = _context.State.GetById(id);
            _context.State.Delete(State);
            _context.Save();
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