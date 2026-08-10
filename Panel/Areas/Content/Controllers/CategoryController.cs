using DataLayer.Entities;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer.Enumarables;
using DataLayer.ViewModels;

namespace Panel.Areas.Content.Controllers
{
    public class CategoryController : Controller
    {
        private UnitOfWork Context = new UnitOfWork();

        public ActionResult Index()
        {
            Context.Permission.CheckPagePermission(Context.SiteUser.GetCurrentUser(), Enum_Permission.Category);
            return View(Context.Category.GetAll().ToView());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Context.Permission.CheckPagePermission(Context.SiteUser.GetCurrentUser(), Enum_Permission.Category_Details);
            Category category = Context.Category.GetById(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }


        public ActionResult Create()
        {
            Context.Permission.CheckPagePermission(Context.SiteUser.GetCurrentUser(), Enum_Permission.Category_New);
            FillDropDowns(null);
            Category category = new Category();
            return View(category);
        }

        [HttpPost]

        public ActionResult Create(Category category)
        {
            Context.Permission.CheckPagePermission(Context.SiteUser.GetCurrentUser(), Enum_Permission.Category_New);
            ViewMessage error = new ViewMessage() { Type = Enum_MessageType.SUCCESS };
            List<CategoryLanguage> listLanguage = category.CategoryLanguage.ToList();
            category.CategoryLanguage.Clear();
            //--------
            if (category.URL != null && category.URL != string.Empty)
            {
                var tempcategory = Context.Category.Where(s => s.URL == category.URL).FirstOrDefault();
                if (tempcategory != null)
                {
                    error.Body = "Url تکراری است";
                    error.Type = Enum_MessageType.ERROR;
                    return new JsonResult() { Data = error };
                }
            }
            //----
            var tempcategoyLabel = Context.Category.Where(s => s.Label== category.Label).FirstOrDefault();
            if (tempcategoyLabel != null)
            {
                error.Body = "برچسب تکراری است";
                error.Type = Enum_MessageType.ERROR;
                return new JsonResult() { Data = error };
            }
            //-----
            if (IsModelValid(category, out error))
            {
                Context.Category.Insert(category);
                bool result = Context.Save();
                if (result == true)
                {
                    foreach (CategoryLanguage item in listLanguage)
                    {
                        item.CategoryId = category.ID;
                        Context.CategoryLanguage.Insert(item);
                    }
                    Context.Save();
                }
            }
            return new JsonResult() { Data = error };
        }

        public ActionResult Edit(int? id)
        {
            Context.Permission.CheckPagePermission(Context.SiteUser.GetCurrentUser(), Enum_Permission.Category_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Category category = Context.Category.GetById(id);
            if (category == null)
                return HttpNotFound();

            FillDropDowns(category);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(category);
        }

        [HttpPost]
        public ActionResult Edit(Category category )
        {
            Context.Permission.CheckPagePermission(Context.SiteUser.GetCurrentUser(), Enum_Permission.Category_Edit);
            ViewMessage error = new ViewMessage() { Type = Enum_MessageType.SUCCESS };
            List<CategoryLanguage> listLanguage = category.CategoryLanguage.ToList();
            category.CategoryLanguage.Clear();
            Context.CategoryLanguage.DeleteByCategoryId(category.ID);
            //--------
            if (category.URL != null && category.URL != string.Empty)
            {
                var tempcategory = Context.Category.Where(s => s.URL == category.URL && s.ID != category.ID).FirstOrDefault();
                if (tempcategory != null )
                {
                    error.Body = "Url تکراری است";
                    error.Type = Enum_MessageType.ERROR;
                    return new JsonResult() { Data = error };
                }
            }
            //----
            var tempcategoyLabel = Context.Category.Where(s => s.Label == category.Label && s.ID != category.ID).FirstOrDefault();
            if (tempcategoyLabel != null)
            {
                error.Body = "برچسب تکراری است";
                error.Type = Enum_MessageType.ERROR;
                return new JsonResult() { Data = error };
            }
            //-----

            if (IsModelValid(category, out error))
            {
                Context.Category.Update(category);
                Context.Save();
                foreach (CategoryLanguage item in listLanguage)
                {
                    item.CategoryId = category.ID;
                    Context.CategoryLanguage.Insert(item);
                }
                Context.Save();
            }
            return new JsonResult() { Data = error };
        }

        //private bool IsModelValid(Category category, HttpPostedFileBase file)
        //{
        //    bool result = false;
        //    if (category.Name == null)
        //        ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_CATEGORY_NAME);
        //    else if (category.WebsiteId == 0)
        //        ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_CATEGORY_WEBSITE_NAME);
        //    else
        //        result = true;

        //    if (file != null)
        //    {
        //        Picture picture = Context.Picture.CreateAndUpload(Enum_Code.SYSTEM_TYPE_PANEL, file);
        //        category.PictureId = picture.ID;
        //    }
        //    return result;
        //}


        private bool IsModelValid(Category category, out ViewMessage msg)
        {
            bool result = false;
            //product.DocId = product.DocId != 0 ? product.DocId : null;
            //product.PictureId = product.PictureId != 0 ? product.PictureId : null;
            //product.ProductTypeId = product.ProductTypeId != 0 ? product.ProductTypeId : null;
            //product.ProductCategoryId = product.ProductCategoryId != 0 ? product.ProductCategoryId : null;
            //product.ProductSubCategoryId = product.ProductSubCategoryId != 0 ? product.ProductSubCategoryId : null;
            //if (product.ShopId == 0)
            //    msg = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PRODUCT_SHOP);
            if (category.Name == null)
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

        private void FillDropDowns(Category category)
        {
            ViewBag.TypeId = new SelectList(Context.Code.GetAllByCodeGroup(Enum_CodeGroup.CATEGORY_TYPE), "ID", "Name", category != null ? category.TypeId : 0);
            ViewBag.WebsiteId = new SelectList(Context.Website.GetAll(), "ID", "Title", category != null ? category.WebsiteId : 0);
        }

        public ActionResult Delete(int? id)
        {
            Context.Permission.CheckPagePermission(Context.SiteUser.GetCurrentUser(), Enum_Permission.Category_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Category category = Context.Category.GetById(id);
            if (category == null)
                return HttpNotFound();

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Context.Permission.CheckPagePermission(Context.SiteUser.GetCurrentUser(), Enum_Permission.Category_Delete);
            Category category = Context.Category.GetById(id);
            try
            {
                //---
                List<CategoryLanguage> langList = Context.CategoryLanguage.Where(x => x.CategoryId == category.ID).ToList();

                if (langList != null && langList.Any())
                {
                    foreach (var item in langList)
                    {
                       Context.CategoryLanguage.Delete(item);
                    }
                }
                //----

                Context.Category.Delete(category);
                Context.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(category);
            }

        }

        public ActionResult GetCategoryPicture(int categoryId)
        {
            try
            {
                if (Context.Permission.HasPermission(Enum_Permission.Product))
                {
                    return new JsonResult()
                    {
                        Data = Context.Category.FirstOrDefault(s => s.ID == categoryId).Picture.GetUrl(),
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
                Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}