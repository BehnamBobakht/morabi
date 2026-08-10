using DataLayer.Entities;
using DataLayer.ViewModels;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer.Enumarables;

namespace Panel.Areas.Content.Controllers
{
    public class MenuController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Menu);
            return View(_context.Menu.GetAll());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Menu_Details);
            Menu menu = _context.Menu.GetById(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Menu_New);
            Menu menu = new Menu() { Active = true };
            FillDropDowns(menu);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(menu);
        }

        [HttpPost]
        public ActionResult Create(Menu menu)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Menu_New);

            List<MenuLanguage> listLanguage = menu.MenuLanguage.ToList();
            menu.MenuLanguage.Clear();

            ViewMessage result = IsModelValid(menu);
            if (result.Type == Enum_MessageType.SUCCESS)
            {
                _context.Menu.Insert(menu);
                _context.Save();

                foreach (MenuLanguage item in listLanguage)
                {
                    item.MenuId = menu.ID;
                    _context.MenuLanguage.Insert(item);
                    _context.Save();
                }
            }
            return new JsonResult() { Data = result };
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Menu_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Menu menu = _context.Menu.GetById(id);
            if (menu == null)
                return HttpNotFound();

            FillDropDowns(menu);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(menu);
        }

        [HttpPost]
        public ActionResult Edit(Menu menu)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Menu_Edit);
            List<MenuLanguage> listLanguage = menu.MenuLanguage.ToList();
            menu.MenuLanguage.Clear();
            ViewMessage result = IsModelValid(menu);
            if (IsModelValid(menu).Type == Enum_MessageType.SUCCESS)
            {
                _context.Menu.Update(menu);
                _context.Save();

                _context.MenuLanguage.DeleteByMenuId(menu.ID);
                _context.Save();
                foreach (MenuLanguage item in listLanguage)
                {
                    item.MenuId = menu.ID;
                    _context.MenuLanguage.Insert(item);
                    _context.Save();
                }
            }
            return new JsonResult() { Data = result };
        }

        private ViewMessage IsModelValid(Menu menu)
        {
            menu.UpdateDatetime = DateTime.Now;
            menu.ParentId = menu.ParentId != 0 ? menu.ParentId : null;
            menu.PostId = menu.PostId != 0 ? menu.PostId : null;
            menu.CategoryId = menu.CategoryId != 0 ? menu.CategoryId : null;
            menu.GalleryId = menu.GalleryId != 0 ? menu.GalleryId : null;

            ViewMessage result = new ViewMessage();
            if (menu.Name != null)
            {
                if (menu.TypeId != 0)
                {
                    Code SelectedType = _context.Code.GetById(menu.TypeId);
                    if (SelectedType.Label == Enum_Code.MENU_TYPE_POST.ToString() && menu.PostId == null)
                    {
                        menu.Link = null;
                        menu.CategoryId = null;
                        menu.GalleryId = null;
                        result = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_MENU_POST_ITEM);
                    }
                    else if (SelectedType.Label == Enum_Code.MENU_TYPE_CATEGORY.ToString() && menu.CategoryId == null)
                    {
                        menu.Link = null;
                        menu.PostId = null;
                        menu.GalleryId = null;
                        result = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_MENU_CATEGORY_ITEM);
                    }
                    else if (SelectedType.Label == Enum_Code.MENU_TYPE_GALLERY.ToString() && menu.GalleryId == 0)
                    {
                        menu.CategoryId = null;
                        menu.PostId = null;
                        menu.GalleryId = null;
                        result = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_MENU_GALLERY_ITEM);
                    }
                    else if (SelectedType.Label == Enum_Code.MENU_TYPE_LINK.ToString())
                    {
                        menu.PostId = null;
                        menu.GalleryId = null;
                        menu.CategoryId = null;
                    }
                }
                else
                    result = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_MENU_TYPE_ITEM);
            }
            else
                result = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_MENU_NAME);
            return result;
        }

        private void FillDropDowns(Menu menu)
        {
            string firstMenuName = menu.Name;
            List<Menu> list = _context.Menu.Search(pageSize: 10000);
            List<Menu> listForDropDown = new List<Menu>() {
                new Menu() {
                    ID = 0,
                    Name = "-"
                }
            };
            for (int i = 0; i < list.Count; i++)
            {
                Menu entity = list[i];
                string menuName = entity.Name;
                if (entity.ParentId != null)
                {
                    if (entity.Menu2.ParentId != null)
                        menuName = entity.Menu2.Menu2.Name + "-" + entity.Menu2.Name + "-" + entity.Name;
                    else
                        menuName = entity.Menu2.Name + "-" + entity.Name;
                }
                entity.Name = menuName;
                listForDropDown.Add(entity);
            }
            menu.Name = firstMenuName;
            ViewBag.ParentId = new SelectList(listForDropDown, "ID", "Name", menu != null && menu.ParentId != null ? menu.ParentId : 0);
            ViewBag.TypeId = new SelectList(_context.Code.GetAllByCodeGroup(Enum_CodeGroup.MENU_TYPE, true), "ID", "Name", menu != null ? menu.TypeId : 0);
            ViewBag.CategoryId = new SelectList(_context.Category.GetAllByType(Enum_CodeGroup.CATEGORY_TYPE, true), "ID", "Name", menu != null && menu.CategoryId != null ? menu.CategoryId : 0);
            ViewBag.GalleryId = new SelectList(_context.Gallery.GetAll(true), "ID", "Name", menu != null && menu.GalleryId != null ? menu.CategoryId : 0);
            ViewBag.PostId = new SelectList(_context.Post.Search(), "ID", "Name", menu != null && menu.PostId != null ? menu.PostId : 0);
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Menu_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Menu menu = _context.Menu.GetById(id);
            if (menu == null)
                return HttpNotFound();

            return View(menu);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Menu_Delete);
            Menu menu = _context.Menu.GetById(id);
            try
            {
                _context.Menu.Delete(menu);
                _context.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(menu);
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