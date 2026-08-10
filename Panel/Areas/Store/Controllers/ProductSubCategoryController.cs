using DataLayer.ViewModels;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Panel.Areas.Store.Controllers
{
    public class ProductSubCategoryController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(
            string typeId = null,
            string categoryId = null,
            int? index = null,
            int? pagesize = null,
            string name = null)
        {
            index = index == null ? 1 : index;
            pagesize = pagesize == null ? 100000 : pagesize;
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductSubCategory);

            List<ProductSubCategoryVm> list = _context.ProductSubCategory.Search(
                typeId: typeId,
                categoryId: categoryId,
                index: index,
                name: name,
                pageSize: pagesize.GetValueOrDefault());

            return View(list);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductSubCategory_Details);
            ProductSubCategory subcategory = _context.ProductSubCategory.GetById(id);
            if (subcategory == null)
            {
                return HttpNotFound();
            }
            return View(subcategory);
        }

        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductSubCategory_New);
            FillDropDowns(null);
            return View(new ProductSubCategory());
        }

        [HttpPost]
        public ActionResult Create(ProductSubCategory subcategory)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductSubCategory_New);
            List<TagSubcategory> tagSubcategory = subcategory.TagSubcategory.ToList();
            List<ProductSubCategoryLanguage> listLanguage = subcategory.ProductSubCategoryLanguage.ToList();
            subcategory.ProductSubCategoryLanguage.Clear();

            ViewMessage result = IsModelValid(subcategory);
            if (result.Type == Enum_MessageType.SUCCESS)
            {
                var tempProductType = _context.ProductType.Where(s => s.CustomLabel == subcategory.CustomLabel).FirstOrDefault();
                var tempProductCategory = _context.ProductCategory.Where(s => s.CustomLabel == subcategory.CustomLabel).FirstOrDefault();
                var tempProductSubCategory = _context.ProductSubCategory.Where(s => s.CustomLabel == subcategory.CustomLabel).FirstOrDefault();
                if (tempProductSubCategory == null && tempProductCategory == null && tempProductType == null)
                {
                    if (subcategory.Priority != null)
                    {
                        List<ProductSubCategory> lst = _context.ProductSubCategory.Where(x => x.Priority == subcategory.Priority && x.CategoryId == subcategory.CategoryId).ToList();
                        if (lst != null && lst.Count() > 0)
                        {
                            result.Body = "اولویت نمایش تکراری است";
                            result.Type = Enum_MessageType.ERROR;
                            return new JsonResult() { Data = result };
                        }
                    }
                    subcategory.UpdateDatetime = DateTime.Now;
                    _context.ProductSubCategory.Insert(subcategory);
                    _context.Save();
                    foreach (TagSubcategory item in tagSubcategory)
                    {
                        TagSubcategory entity = new TagSubcategory()
                        {
                            SubCategoryId = subcategory.ID,
                            TagId = item.TagId
                        };
                        _context.TagSubCategory.Insert(entity);
                    }
                    _context.Save();
                    foreach (ProductSubCategoryLanguage item in listLanguage)
                    {
                        item.ProductSubCategoryId = subcategory.ID;
                        _context.ProductSubCategoryLanguage.Insert(item);
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
            }
            return new JsonResult() { Data = result };
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductSubCategory_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ProductSubCategory subcategory = _context.ProductSubCategory.GetById(id);
            if (subcategory == null)
                return HttpNotFound();

            FillDropDowns(subcategory);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(subcategory);
        }

        [HttpPost]
        public ActionResult Edit(ProductSubCategory subcategory)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductSubCategory_Edit);
            List<ProductSubCategoryLanguage> listLanguage = subcategory.ProductSubCategoryLanguage.ToList();
            List<TagSubcategory> tagSubcategory = subcategory.TagSubcategory.ToList();
            subcategory.ProductSubCategoryLanguage.Clear();
            subcategory.TagSubcategory.Clear();
            ViewMessage result = IsModelValid(subcategory);
            if (IsModelValid(subcategory).Type == Enum_MessageType.SUCCESS)
            {
                var tempProductType = _context.ProductType.Where(s => s.CustomLabel == subcategory.CustomLabel).FirstOrDefault();
                var tempProductCategory = _context.ProductCategory.Where(s => s.CustomLabel == subcategory.CustomLabel).FirstOrDefault();
                var tempProductSubCategory = _context.ProductSubCategory.Where(s => s.CustomLabel == subcategory.CustomLabel && s.ID != subcategory.ID).FirstOrDefault();
                if (tempProductSubCategory == null && tempProductCategory == null && tempProductType == null)
                {
                    if (subcategory.Priority != null)
                    {
                        List<ProductSubCategory> lst = _context.ProductSubCategory.Where(x => x.Priority == subcategory.Priority && x.CategoryId == subcategory.CategoryId && x.ID != subcategory.ID).ToList();
                        if (lst != null && lst.Count() > 0)
                        {
                            result.Body = "اولویت نمایش تکراری است";
                            result.Type = Enum_MessageType.ERROR;
                            return new JsonResult() { Data = result };
                        }
                    }
                    subcategory.UpdateDatetime = DateTime.Now;

                    _context.ProductSubCategory.Update(subcategory);
                    bool a = _context.Save();

                    _context.ProductSubCategoryLanguage.DeleteByProductSubCategoryId(subcategory.ID);
                    bool b = _context.Save();
                    foreach (ProductSubCategoryLanguage item in listLanguage)
                    {
                        item.ProductSubCategoryId = subcategory.ID;
                        _context.ProductSubCategoryLanguage.Insert(item);
                        bool c = _context.Save();
                    }
                    _context.TagSubCategory.DeleteByProductSubCategoryId(subcategory.ID);
                    _context.Save();
                    foreach (TagSubcategory item in tagSubcategory)
                    {
                        TagSubcategory entity = new TagSubcategory()
                        {
                            SubCategoryId = subcategory.ID,
                            TagId = item.TagId
                        };
                        _context.TagSubCategory.Insert(entity);
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
            }
            return new JsonResult() { Data = result };
        }
        [HttpPost]
        public ActionResult AddTage(string tageName)
        {
            ViewMessage result = new ViewMessage();
            var tag = _context.Tag.Where(s => s.FaName == tageName).FirstOrDefault();
            if (tag == null)
            {
                Tag newTage = new Tag()
                {
                    FaName = tageName
                };
                _context.Tag.Insert(newTage);
                _context.Save();
                result.Body = "ثبت با موفقیت انجام شد";
                result.Type = Enum_MessageType.SUCCESS;
                return new JsonResult()
                {
                    Data = result,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

            else
            {
                result.Body = "امکان درج تگ تکراری وجود ندارد";
                result.Type = Enum_MessageType.ERROR;
                return new JsonResult()
                {
                    Data = result,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

        }


        public ActionResult GetTags(int? id)
        {
            ViewTag model = new ViewTag();
            List<Tag> lst = new List<Tag>();
            lst = _context.Tag.GetAll().OrderBy(s => s.Id).ToList();
            model.AllTags = lst;
            if (id != null)
            {
                lst = _context.Tag.Where(s => s.TagSubcategory.Any(x => x.SubCategoryId == id)).OrderBy(s => s.Id).ToList();
                model.SourceTags = lst;
            }
            return PartialView("Tag", model);
        }

        private ViewMessage IsModelValid(ProductSubCategory subcategory)
        {
            ViewMessage result = new ViewMessage();
            if (subcategory.Name == null)
                result = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PRODUCT_SUB_CATEGORY_NAME);
            return result;
        }

        private void FillDropDowns(ProductSubCategory subcategory)
        {
            ViewBag.CategoryId = new SelectList(_context.ProductCategory.GetAll().ToView(), "ID", "FullName", subcategory != null ? subcategory.CategoryId : 0);
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductSubCategory_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ProductSubCategory subcategory = _context.ProductSubCategory.GetById(id);
            if (subcategory == null)
                return HttpNotFound();

            return View(subcategory);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductSubCategory_Delete);
            ProductSubCategory subcategory = _context.ProductSubCategory.GetById(id);
            try
            {
                _context.ProductSubCategory.Delete(subcategory);
                _context.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                subcategory.Deleted = true;
                _context.ProductSubCategory.Update(subcategory);
                _context.Save();
                return RedirectToAction("Index");
            }

        }
        public ActionResult PriorityList(int categoryId)
        {
            List<ProductSubCategory> lst = _context.ProductSubCategory.Where(x => x.CategoryId == categoryId).OrderBy(x => x.Priority).ToList();
            return PartialView(lst);
        }

        public ActionResult ProductSubCategoryDropDown(int? categoryId = null, int? value = null)
        {
            ViewBag.ProductSubCategoryId = new SelectList(_context.ProductSubCategory.Search(categoryId: categoryId.ToString(), pageSize: 1000), "ID", "Name", value);
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