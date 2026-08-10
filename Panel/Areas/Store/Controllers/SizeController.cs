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
    public class SizeController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Size);
            List<Size> listSize = _context.Size.GetAllProductTypeId(id.GetValueOrDefault());
            ViewBag.ProductType = _context.ProductType.GetById(id);
            return View(listSize.ToView());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Size_Details);
            Size size = _context.Size.GetById(id);
            if (size == null)
            {
                return HttpNotFound();
            }
            return View(size);
        }

        public ActionResult Create(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Size_New);
            ViewBag.ProductType = _context.ProductType.GetById(id);
            return View();
        }

        [HttpPost]
        public ActionResult Create(Size size)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Size_New);

            List<SizeLanguage> listLanguage = size.SizeLanguage.ToList();
            size.SizeLanguage.Clear();

            ViewMessage result = IsModelValid(size);
            if (result.Type == Enum_MessageType.SUCCESS)
            {
                _context.Size.Insert(size);
                _context.Save();

                foreach (SizeLanguage item in listLanguage)
                {
                    item.SizeId = size.ID;
                    _context.SizeLanguage.Insert(item);
                    _context.Save();
                }
            }
            return new JsonResult() { Data = result };
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Size_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Size size = _context.Size.GetById(id);
            if (size == null)
                return HttpNotFound();

            ViewBag.Message = BaseMessage.GetMessage();
            return View(size);
        }

        [HttpPost]
        public ActionResult Edit(Size size)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Size_Edit);
            List<SizeLanguage> listLanguage = size.SizeLanguage.ToList();
            size.SizeLanguage.Clear();
            ViewMessage result = IsModelValid(size);
            if (IsModelValid(size).Type == Enum_MessageType.SUCCESS)
            {
                _context.Size.Update(size);
                _context.Save();

                _context.SizeLanguage.DeleteBySizeId(size.ID);
                _context.Save();
                foreach (SizeLanguage item in listLanguage)
                {
                    item.SizeId = size.ID;
                    _context.SizeLanguage.Insert(item);
                    _context.Save();
                }
            }
            return new JsonResult() { Data = result };
        }

        private ViewMessage IsModelValid(Size size)
        {
            ViewMessage result = new ViewMessage();
            if (size.Name == null)
                result = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_SIZE_NAME);
            return result;
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Size_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Size size = _context.Size.GetById(id);
            if (size == null)
                return HttpNotFound();

            return View(size);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Size_Delete);
            Size size = _context.Size.GetById(id);
            try
            {
                int? typeId = size.ProductTypeId;
                _context.Size.Delete(size);
                _context.Save();
                return RedirectToAction("index", new { id = typeId });
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(size);
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