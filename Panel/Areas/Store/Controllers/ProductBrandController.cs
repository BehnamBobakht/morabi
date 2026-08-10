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
using DataLayer.ViewModels;

namespace Panel.Areas.Store.Controllers
{
    public class ProductBrandController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductBrand);
            return View(_context.ProductBrand.GetAll().ToView());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductBrand_Details);
            ProductBrand brand = _context.ProductBrand.GetById(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }

        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductBrand_New);
            FillDropDowns(null);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(new ProductBrand() { IsPublic = true });
        }

        [HttpPost]
        [ValidateInput(false)]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(ProductBrand brand)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductBrand_New);
            List<ProductBrandLanguage> listLanguage = brand.ProductBrandLanguage.ToList();
            brand.ProductBrandLanguage.Clear();
            ViewMessage result = IsModelValid(brand);
            if (result.Type == Enum_MessageType.SUCCESS)
            {
                brand.SyncDatetime = DateTime.Now;
                _context.ProductBrand.Insert(brand);
                _context.Save();

                foreach (ProductBrandLanguage item in listLanguage)
                {
                    item.ProductBrandId = brand.ID;
                    _context.ProductBrandLanguage.Insert(item);
                    _context.Save();
                }
                BaseWebsite.UpdatingLastContent();

            }
            else
            {
                return new JsonResult()
                {
                    Data = Resource.Lang.Error,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult() { Data = result };

        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductBrand_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ProductBrand brand = _context.ProductBrand.GetById(id);
            if (brand == null)
                return HttpNotFound();

            FillDropDowns(brand);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(brand);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(ProductBrand brand)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductBrand_Edit);
            List<ProductBrandLanguage> listLanguage = brand.ProductBrandLanguage.ToList();
            brand.ProductBrandLanguage.Clear();

            ViewMessage result = IsModelValid(brand);
            if (result.Type == Enum_MessageType.SUCCESS)
            {
                brand.SyncDatetime = DateTime.Now;
                _context.ProductBrand.Update(brand);
                _context.Save();
                _context.ProductBrandLanguage.DeleteByBrandId(brand.ID);
                foreach (ProductBrandLanguage item in listLanguage)
                {
                    item.ProductBrandId = brand.ID;
                    _context.ProductBrandLanguage.Insert(item);
                }
                _context.Save();  
                BaseWebsite.UpdatingLastContent();
            }
            else
            {
                return new JsonResult()
                {
                    Data = Resource.Lang.Error,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

            return new JsonResult() { Data = result };
        }

        private ViewMessage IsModelValid(ProductBrand brand)
        {
            ViewMessage result = new ViewMessage();
            if (brand.Name == null)
                result = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PRODUCT_BRAND_NAME);
            else if (brand.ProductTypeId == 0)
                result = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PRODUCT_TYPE_NAME);
            return result;
        }

        public new ActionResult User(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductBrand_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ProductBrand brand = _context.ProductBrand.GetById(id);
            if (brand == null)
                return HttpNotFound();

            ViewBag.User = _context.SiteUser.GetAll();
            return View(brand);
        }

        [HttpPost]
        public new ActionResult User(int? BrandId, int[] UserId)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductBrand_Edit);
            if (BrandId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ProductBrand brand = _context.ProductBrand.GetById(BrandId);
            if (brand == null)
                return HttpNotFound();

            _context.ProductBrandUser.DeleteByBrandId(BrandId.Value);
            if (UserId != null)
            {
                foreach (int item in UserId)
                {
                    ProductBrandUser entity = new ProductBrandUser()
                    {
                        ProductBrandId = BrandId.Value,
                        UserId = item
                    };
                    _context.ProductBrandUser.Insert(entity);
                }
            }
            _context.Save();
            return RedirectToAction("Index");
        }

        private void FillDropDowns(ProductBrand brand)
        {
            ViewBag.ProductTypeId = new SelectList(_context.ProductType.GetAll(), "ID", "Name", brand != null ? brand.ProductTypeId : 0);
        }

        public ActionResult Types(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductBrand_Edit);

            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            List<ProductBrand> list = _context.ProductBrand.GetAll();
            List<ProductTypeBrand> listShop = _context.ProductTypeBrand.GetAllByBrandId(id.Value);
            List<ViewProductTypeBrand> listOutput = new List<ViewProductTypeBrand>();
            foreach (ProductBrand item in list)
            {
                listOutput.Add(new ViewProductTypeBrand()
                {
                    ProductTypeId = item.ID,
                    ProductTypeName = item.Name,
                    Selected = listShop.Any(p => p.ProductBrandId == item.ID)
                });
            }

            ViewBag.Message = BaseMessage.GetMessage();
            ViewBag.BrandId = id;
            ViewBag.ProductBrand = _context.ProductBrand.GetById(id);
            return View(listOutput);
        }

        [HttpPost]
        public ActionResult Types(int BrandId, int[] ProductBrands)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductBrand_Edit);

            _context.ProductTypeBrand.DeleteByBrandId(BrandId);
            if (ProductBrands != null)
            {
                foreach (int item in ProductBrands)
                {
                    ProductTypeBrand entity = new ProductTypeBrand()
                    {
                        ProductBrandId = item,
                        ProductTypeId = BrandId
                    };
                    _context.ProductTypeBrand.Insert(entity);
                }
                _context.Save();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductBrand_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ProductBrand brand = _context.ProductBrand.GetById(id);
            if (brand == null)
                return HttpNotFound();

            return View(brand);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductBrand_Delete);
            ProductBrand brand = _context.ProductBrand.GetById(id);
            try
            {
                _context.ProductBrand.Delete(brand);
                _context.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(brand);
            }

        }
        public ActionResult ProductBrandDropDown(int? typeId = null, int? value = null)
        {
            ViewBag.BrandId = new SelectList(_context.ProductBrand.Search(withAll:true, typeId: typeId, pageSize: 1000), "ID", "Name", value);
            return PartialView();
        }
        public ActionResult ProductBrandDropDown2(int? typeId = null, int? value = null)
        {
            ViewBag.ProductBrandId = new SelectList(_context.ProductBrand.Search(withAll:true, typeId: typeId, pageSize: 1000), "ID", "Name", value);
            return PartialView();
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