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

    public class CityController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(string name=null)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.City);
            ViewBag.Name = name;
            if (name != null)
            {
                name = name.ToPersianChar();
                return View(_context.City.Where(s => s.Name.Contains(name)));
            }
            else
            {
                return View(_context.City.GetAll());
            }
           
        }


        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.City);
            FillDropDowns(null);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( City City)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.City);
            FillDropDowns(City);
            if (IsModelValid(City))
            {
                _context.City.Insert(City);
                _context.Save();
                return RedirectToAction("Index");
            }

           
            return View(City);
        }
        private void FillDropDowns(City city)
        {
            ViewBag.StateId = new SelectList(_context.State.GetAll(), "ID", "Name", city != null ? city.StateId : 0);
        }
        public ActionResult CityDropDown(int? stateId = null, int? value = null)
        {
            ViewBag.CityId = new SelectList(_context.City.Search(stateId: stateId, pageSize: 1000), "ID", "Name",value);
            return PartialView();
        }
        public ActionResult Edit(int? id)
        {
            //_context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.City);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            City City = _context.City.GetById(id);
            if (City == null)
                return HttpNotFound();
            FillDropDowns(City);

            ViewBag.Message = BaseMessage.GetMessage();
            return View(City);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(City City)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.City);
            if (IsModelValid(City))
            {
                _context.City.Update(City);
                _context.Save();
                return RedirectToAction("Index");
            }
            FillDropDowns(City);
            return View(City);
        }

        private bool IsModelValid(City City)
        {
            bool result = false;
            if (City.Name == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_BANNER_NAME);
            else
                result = true;

            return result;
        }

       

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.City);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            City City = _context.City.GetById(id);
            if (City == null)
                return HttpNotFound();

            return View(City);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.City);
            City City = _context.City.GetById(id);
            _context.City.Delete(City);
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