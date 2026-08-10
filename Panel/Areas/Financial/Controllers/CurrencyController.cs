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

namespace Panel.Areas.Financial.Controllers
{
    public class CurrencyController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            SiteUser CurrentUser = _context.SiteUser.GetCurrentUser();
            _context.Permission.CheckPagePermission(CurrentUser, Enum_Permission.Currency);
            return View(_context.Currency.GetAll());
        }

        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Currency_Create);

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Currency currency)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Currency_Create);

            if (IsModelValid(currency).Type == Enum_MessageType.SUCCESS)
            {
                _context.Currency.Insert(currency);
                _context.Save();
                return RedirectToAction("index");
            }

            return View(currency);
        }



        private ViewMessage IsModelValid(Currency currency)
        {
            ViewMessage result = new ViewMessage();
            ViewBag.Message = result;
            return result;
        }


        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Currency_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Currency currency = _context.Currency.GetById(id);
            if (currency == null)
                return HttpNotFound();

            ViewBag.Message = BaseMessage.GetMessage();
            return View(currency);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Currency currency)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Currency_Edit);
            if (IsModelValid(currency).Type == Enum_MessageType.SUCCESS)
            {
                _context.Currency.Update(currency);
                _context.Save();
                return RedirectToAction("Index");
            }
            return View(currency);
        }



        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Crrency_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Currency currency = _context.Currency.GetById(id);
            if (currency == null)
                return HttpNotFound();

            return View(currency);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Currency_Detail);
            Currency currency = _context.Currency.GetById(id);
            if (currency == null)
            {
                return HttpNotFound();
            }
            return View(currency);
        }


        [ValidateAntiForgeryToken]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Crrency_Delete);
            Currency currency = _context.Currency.GetById(id);
            _context.Currency.Delete(currency);
            _context.Save();
            return RedirectToAction("Index");
        }

    }
}
