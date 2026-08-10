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
    public class ColorController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Color);
            List<Color> listColor = _context.Color.GetAllProductTypeId(id.GetValueOrDefault());
            ViewBag.ProductType = _context.ProductType.GetById(id);
            return View(listColor.ToView());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Color_Details);
            Color color = _context.Color.GetById(id);
            if (color == null)
            {
                return HttpNotFound();
            }
            return View(color);
        }

        public ActionResult Create(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Color_New);
            ViewBag.ProductType = _context.ProductType.GetById(id);
            return View();
        }

        [HttpPost]
        public ActionResult Create(Color color)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Color_New);

            List<ColorLanguage> listLanguage = color.ColorLanguage.ToList();
            color.ColorLanguage.Clear();

            ViewMessage result = IsModelValid(color);
            if (result.Type == Enum_MessageType.SUCCESS)
            {
                _context.Color.Insert(color);
                _context.Save();

                foreach (ColorLanguage item in listLanguage)
                {
                    item.ColorId = color.ID;
                    _context.ColorLanguage.Insert(item);
                    _context.Save();
                }
            }
            return new JsonResult() { Data = result };
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Color_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Color color = _context.Color.GetById(id);
            if (color == null)
                return HttpNotFound();

            ViewBag.Message = BaseMessage.GetMessage();
            return View(color);
        }

        [HttpPost]

        public ActionResult Edit(Color color)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Color_Edit);
            List<ColorLanguage> listLanguage = color.ColorLanguage.ToList();
            color.ColorLanguage.Clear();
            ViewMessage result = IsModelValid(color);
            if (IsModelValid(color).Type == Enum_MessageType.SUCCESS)
            {
                _context.Color.Update(color);
                _context.Save();

                _context.ColorLanguage.DeleteByColorId(color.ID);
                bool b = _context.Save();
                foreach (ColorLanguage item in listLanguage)
                {
                    item.ColorId = color.ID;
                    _context.ColorLanguage.Insert(item);
                    _context.Save();
                }
            }
            return new JsonResult() { Data = result };
        }

        private ViewMessage IsModelValid(Color color)
        {
            ViewMessage result = new ViewMessage();
            if (color.Name == null)
                result = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_COLOR_NAME);
            else if (color.HexValue == null)
                result = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_COLOR_HEX);
            color.HexValue = color.HexValue.Replace("#", "");
            return result;
        }
        
        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Color_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Color color = _context.Color.GetById(id);
            if (color == null)
                return HttpNotFound();

            return View(color);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Color_Delete);
            Color color = _context.Color.GetById(id);

            try
            {
                int? typeId = color.ProductTypeId;
                _context.Color.Delete(color);
                _context.Save();
                return RedirectToAction("index", new { id = typeId });
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(color);
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