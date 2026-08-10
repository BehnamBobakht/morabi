using DataLayer.Entities;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer.Enumarables;
using DataLayer.ViewModels;

namespace Panel.Areas.Content.Controllers
{
    [ValidateInput(false)]
    public class PostController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(string name = null, string pageIndex = null,
            string pageSize = null, int? categoryId = null)
        {
            //FillDropDowns(null);
            //ViewBag.ProductTypeId = new SelectList(new List<ProductType>(), "ID", "Name");
            ViewBag.CategoryId = new SelectList(_context.Category.GetAllByType(Enum_CodeGroup.CATEGORY_TYPE), "ID", "Name", categoryId != null ? categoryId : 0);
            int size = pageSize == null ? 50 : pageSize.GetInteger();
            int index = pageIndex == null ? 1 : pageIndex.GetInteger();
            ViewBag.PageIndex = index.ToString();
            ViewBag.PageSize = size.ToString();
            ViewBag.Name = name;
            ViewBag.CategoryId = categoryId;
            ViewBag.TotalCount = _context.Post.SearchCount(
                   name: name,
                   categoryId: categoryId);

            List<Post> listPost = _context.Post.Search(
                    name: name,
                    pageSize: size,
                    index: index,
                     categoryId: categoryId
                  )
                .ToList();

            return View(listPost.ToView());
            //_context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Post);
            //var menus = _context.Post.GetAll().OrderByDescending(x => x.ID);
            //return View(menus.ToList().ToView());
        }
        [HttpGet]
        public ActionResult FillCategory(int? ShopId)
        {
            return new JsonResult()
            {
                Data = _context.Category.GetAllByType(Enum_CodeGroup.CATEGORY_TYPE).ToView(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Post_Details);
            Post post = _context.Post.GetById(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Post_New);
            FillDropDowns(null);
            Post post = new Post()
            {
                Active = true,
                CreateDateTime = DateTime.Now,
                UpdateDateTime = DateTime.Now,
                IsSlider = false,
                VisitCount = 0
            };
            ViewBag.Message = BaseMessage.GetMessage();
            return View(post);
        }

        private bool IsModelValid(Post post, out ViewMessage msg)
        {
            bool result = false;

            //product.DocId = product.DocId != 0 ? product.DocId : null;
            post.PictureId = post.PictureId == 0 ? null : post.PictureId;

            //product.ProductTypeId = product.ProductTypeId != 0 ? product.ProductTypeId : null;
            //product.ProductCategoryId = product.ProductCategoryId != 0 ? product.ProductCategoryId : null;
            //product.ProductSubCategoryId = product.ProductSubCategoryId != 0 ? product.ProductSubCategoryId : null;
            //if (product.ShopId == 0)
            //    msg = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PRODUCT_SHOP);
            if (post.Name == null)
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

        [HttpPost]
        public ActionResult Create(Post post)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Post_New);
            ViewMessage error = new ViewMessage() { Type = Enum_MessageType.SUCCESS };
            List<PostLanguage> listLanguage = post.PostLanguage.ToList();
            List<TagPost> listTagPost = post.TagPost.ToList();
            post.CreateDateTime = DateTime.Now;
            post.UpdateDateTime = DateTime.Now;
            post.PostLanguage.Clear();
            if (IsModelValid(post, out error))
            {
                _context.Post.Insert(post);
                bool result = _context.Save();
                if (result == true)
                {
                    foreach (PostLanguage item in listLanguage)
                    {
                        item.PostId = post.ID;
                        _context.PostLanguage.Insert(item);
                    }
                    _context.Save();
                    foreach (TagPost item in listTagPost)
                    {
                        TagPost entity = new TagPost()
                        {
                            PostId = post.ID,
                            TagId = item.TagId
                        };
                        _context.TagPost.Insert(entity);
                    }
                    _context.Save();
                    BaseWebsite.UpdatingLastContent();

                }
            }
            return new JsonResult() { Data = error };
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Post_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Post post = _context.Post.GetById(id);
            if (post == null)
                return HttpNotFound();

            FillDropDowns(post);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(post);
        }

        [HttpPost]
        public ActionResult Edit(Post post)
        {
            ViewMessage error = new ViewMessage() { Type = Enum_MessageType.SUCCESS };

            post.UpdateDateTime = DateTime.Now;
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Post_Edit);
            List<PostLanguage> listLanguage = post.PostLanguage.ToList();
            post.PostLanguage.Clear();
            List<TagPost> listTagPost = post.TagPost.ToList();
            post.TagPost.Clear();
            _context.PostLanguage.DeleteByPostId(post.ID);
            _context.TagPost.DeleteByPostId(post.ID);
            _context.Save();
            if (IsModelValid(post, out error))
            {
                _context.Post.Update(post);
                _context.Save();

                foreach (PostLanguage item in listLanguage)
                {
                    item.PostId = post.ID;
                    _context.PostLanguage.Insert(item);
                }
                _context.Save();
                foreach (TagPost item in listTagPost)
                {
                    TagPost entity = new TagPost()
                    {
                        PostId = post.ID,
                        TagId = item.TagId
                    };
                    _context.TagPost.Insert(entity);
                }
                _context.Save();
                BaseWebsite.UpdatingLastContent();

            }
            return new JsonResult() { Data = error };
        }

        private bool IsModelValid(Post post, HttpPostedFileBase file, HttpPostedFileBase attach)
        {
            bool result = false;
            post.ShowDateTime = post.ShowDateTime.GetGregorian();
            if (post.Name == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_POST_NAME);
            else
                result = true;

            if (file != null)
            {
                Picture picture = _context.Picture.CreateAndUpload(Enum_Code.SYSTEM_TYPE_PANEL, file);
                post.PictureId = picture.ID;
            }

            if (attach != null)
            {
                WebsiteDocument document = _context.WebsiteDocument.CreateAndUpload(Enum_Code.SYSTEM_TYPE_PANEL, attach);
                post.FileId = document.ID;
            }

            return result;
        }

        private void FillDropDowns(Post post)
        {
            ViewBag.WebsiteId = new SelectList(_context.Website.GetAllByType(Enum_Code.SYSTEM_TYPE_WEBSITE, Enum_Code.SYSTEM_TYPE_SHOP), "ID", "Title", post != null ? post.WebsiteId : 0);
            ViewBag.CategoryId = new SelectList(_context.Category.GetAllByType(Enum_CodeGroup.CATEGORY_TYPE), "ID", "Name", post != null ? post.CategoryId : 0);

            ViewBag.StateId = new SelectList(_context.State.GetAll(), "ID", "Name", post != null ? post.StateId : 0);
            if (post != null && post.ShowDateTime != null)
            {
                ViewBag.ShowDateTime = post.ShowDateTime.Value.ToPersian();
            }
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Post_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Post post = _context.Post.GetById(id);
            if (post == null)
                return HttpNotFound();

            return View(post);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Post_Delete);
            Post post = _context.Post.GetById(id);
            try
            {
                //---
                List<PostLanguage> postLang = _context.PostLanguage.Where(x => x.PostId == post.ID).ToList();

                if (postLang != null && postLang.Any())
                {
                    foreach (var item in postLang)
                    {
                        _context.PostLanguage.Delete(item);
                    }
                }
                //----

                _context.Post.Delete(post);
                _context.Save();

                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(post);
            }

        }
        //[HttpPost]
        //public ActionResult AddTage(string tageName)
        //{
        //    ViewMessage result = new ViewMessage();
        //    var tag = _context.Tag.Where(s => s.FaName == tageName).FirstOrDefault();
        //    if (tag == null)
        //    {
        //        Tag newTage = new Tag()
        //        {
        //            FaName = tageName
        //        };
        //        _context.Tag.Insert(newTage);
        //        _context.Save();
        //        result.Body = "ثبت با موفقیت انجام شد";
        //        result.Type = Enum_MessageType.SUCCESS;
        //        return new JsonResult()
        //        {
        //            Data = result,
        //            JsonRequestBehavior = JsonRequestBehavior.AllowGet
        //        };
        //    }

        //    else
        //    {
        //        result.Body = "امکان درج تگ تکراری وجود ندارد";
        //        result.Type = Enum_MessageType.ERROR;
        //        return new JsonResult()
        //        {
        //            Data = result,
        //            JsonRequestBehavior = JsonRequestBehavior.AllowGet
        //        };
        //    }

        //}


        public ActionResult GetTags(int? id)
        {
            ViewTag model = new ViewTag();
            List<Tag> lst = new List<Tag>();
            lst = _context.Tag.GetAll().OrderBy(s => s.Id).ToList();
            model.AllTags = lst;
            if (id != null)
            {
                lst = _context.Tag.Where(s => s.TagPost.Any(x => x.PostId == id)).OrderBy(s => s.Id).ToList();
                model.SourceTags = lst;
            }
            return PartialView("Tag", model);
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