using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Panel.Areas.Store.Controllers
{
    public class PriceRangeController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product_Edit);

            var storeProductQuantity = _context.StoreProductQuantity.GetStoreProductQuantityVm(id);
            return View(storeProductQuantity);
        }
        [HttpGet]
        public ActionResult Create(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product_Edit);

            var model = new PriceRangeVm();
            model.StoreProductQuantityId = id;
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(PriceRangeVm priceRangeVm)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product_Edit);

            var result = _context.PriceRange.AddRange(priceRange: priceRangeVm);
            if (result.Status == ResultStatus.Success)
                return RedirectToAction("index", new { id = priceRangeVm.StoreProductQuantityId });

            ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR,result.Message);
            return View(priceRangeVm);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product_Edit);

            var model = _context.PriceRange.GetPriceRangeVm(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(PriceRangeVm priceRangeVm)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product_Edit);

            var result = _context.PriceRange.Update(priceRangeVm: priceRangeVm);
            if (result.Status == ResultStatus.Success)
                return RedirectToAction("index", new { id = priceRangeVm.StoreProductQuantityId });

            ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR,result.Message);
            return View(priceRangeVm);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product_Edit);
            var model = _context.PriceRange.GetById(id);
            int spqId = 0;
            if(model!=null)
            {
                spqId = model.StoreProductQuantityId;
                _context.PriceRange.Delete(model);
                _context.Save();
            }
            return RedirectToAction("index",new { id= spqId });
        }
    }
}