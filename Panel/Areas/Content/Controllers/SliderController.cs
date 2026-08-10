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

namespace Panel.Areas.Content.Controllers
{
    public class SliderController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Slider);
            return View(_context.Slider.GetAll().ToView());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Slider_Details);
            Slider slider = _context.Slider.GetById(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Slider_New);
            FillDropDowns(null);
            Slider slider = new Slider();

            return View(slider);
        }

        [HttpPost]

        public ActionResult Create(Slider slider)
        {

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Slider_New);
            ViewMessage error = new ViewMessage() { Type = Enum_MessageType.SUCCESS };
            List<SliderLanguage> listLanguage = slider.SliderLanguage.ToList();
            slider.SliderLanguage.Clear();
            if (IsModelValid(slider, out error))
            {
                _context.Slider.Insert(slider);
                bool result = _context.Save();
                if (result == true)
                {
                    foreach (SliderLanguage item in listLanguage)
                    {
                        item.SliderId = slider.ID;
                        _context.SliderLanguage.Insert(item);
                    }
                    _context.Save();
                }
            }
            return new JsonResult() { Data = error };
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Slider_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Slider slider = _context.Slider.GetById(id);
            if (slider == null)
                return HttpNotFound();

            FillDropDowns(slider);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(slider);
        }

        [HttpPost]
        public ActionResult Edit(Slider slider)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Slider_Edit);
            ViewMessage error = new ViewMessage() { Type = Enum_MessageType.SUCCESS };
            List<SliderLanguage> listLanguage = slider.SliderLanguage.ToList();
            slider.SliderLanguage.Clear();
            _context.SliderLanguage.DeleteBySliderId(slider.ID);

            if (IsModelValid(slider, out error))
            {
                _context.Slider.Update(slider);
                _context.Save();
                foreach (SliderLanguage item in listLanguage)
                {
                    item.SliderId = slider.ID;
                    _context.SliderLanguage.Insert(item);
                }
                _context.Save();
            }
            return new JsonResult() { Data = error };
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ID,WebsiteId,UserId,PictureId,Name,SecondName,Link,ShowNumber")] Slider slider, HttpPostedFileBase file)
        //{
        //    _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Slider_Edit);
        //    if (IsModelValid(slider, file))
        //    {
        //        _context.Slider.Update(slider);
        //        _context.Save();
        //        return RedirectToAction("Index");
        //    }
        //    FillDropDowns(slider);
        //    return View(slider);
        //}

        //private bool IsModelValid(Slider slider, HttpPostedFileBase file)
        //{
        //    bool result = false;
        //    if (slider.Name == null)
        //        ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_SLIDER_NAME);
        //    else if (slider.PictureId == null && file == null)
        //        ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_SLIDER_PICTURE);
        //    else
        //        result = true;

        //    if (file != null)
        //    {
        //        slider.PictureId = _context.Picture.CreateAndUpload(Enum_Code.SYSTEM_TYPE_PANEL, file).ID;
        //    }

        //    return result;
        //}
        private bool IsModelValid(Slider slider, out ViewMessage msg)
        {
            bool result = false;
            //product.DocId = product.DocId != 0 ? product.DocId : null;
            //product.PictureId = product.PictureId != 0 ? product.PictureId : null;
            //product.ProductTypeId = product.ProductTypeId != 0 ? product.ProductTypeId : null;
            //product.ProductCategoryId = product.ProductCategoryId != 0 ? product.ProductCategoryId : null;
            //product.ProductSubCategoryId = product.ProductSubCategoryId != 0 ? product.ProductSubCategoryId : null;
            //if (product.ShopId == 0)
            //    msg = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PRODUCT_SHOP);
            if (slider.Name == null)
            {
                msg = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PRODUCT_NAME);
            }
            //else if (product.StatusId == 0)
            //    msg = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PRODUCT_STATUS);
            //else if (product.Name.Length > 40)
            //    msg = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PRODUCT_STATUS);
            else
            {
                msg = BaseMessage.GetMessage(Enum_MessageType.SUCCESS);
                result = true;
            }
            return result;
        }

        private void FillDropDowns(Slider slider)
        {
            List<Website> listWebsite = _context.Website.GetAllByType(Enum_Code.SYSTEM_TYPE_WEBSITE, Enum_Code.SYSTEM_TYPE_CMS, Enum_Code.SYSTEM_TYPE_SHOP);
            ViewBag.WebsiteId = new SelectList(listWebsite, "ID", "Title", slider != null ? slider.WebsiteId : 0);
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Slider_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Slider slider = _context.Slider.GetById(id);
            if (slider == null)
                return HttpNotFound();

            return View(slider);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Slider_Delete);
            Slider slider = _context.Slider.GetById(id);
            try
            {
                List<SliderLanguage> sliderLanguage = _context.SliderLanguage.Where(x => x.SliderId == slider.ID).ToList();
                if (sliderLanguage.Count > 0)
                {
                    foreach (var item in sliderLanguage)
                    {
                        _context.SliderLanguage.Delete(item);
                    }

                }
                _context.Slider.Delete(slider);
                _context.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(slider);
            }

        }

        public ActionResult GetSliderPicture(int sliderId)
        {
            try
            {
                if (_context.Permission.HasPermission(Enum_Permission.Slider))
                {
                    return new JsonResult()
                    {
                        Data = _context.Slider.FirstOrDefault(s => s.ID == sliderId).Picture.GetUrl(),
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        MaxJsonLength = int.MaxValue
                    };
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return new JsonResult()
                {
                    Data = ex.Message,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
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