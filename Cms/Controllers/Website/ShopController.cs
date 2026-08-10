using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer.ViewModels;

namespace Cms.Controllers
{
    public class ShopController : Controller
    {
        UnitOfWork _context = new UnitOfWork();
        
        public ActionResult Index(int? shId = null, string shidname = null)
        {
            Shop shop = _context.Shop.GetById(shId);
            if (shop == null)
                return RedirectPermanent("/error/404");
            ViewBag.Shop = shop;
            return PartialView(BaseController.GetView(this), shop);
        }

        public ActionResult Products(string shopid, int? notid = null, string typeid = null, string brandId = null, string categoryId = null, string subCategoryId = null, string name = null, string title = null,int? pagesize = null, int? prindex = null)
        {
            Shop shop = _context.Shop.GetById(shopid);
            List<Product> results = _context.Product.Search(typeId: typeid, title: title, shopId: shopid);
            int TotalCount = _context.Product.SearchDetail(
                        notId: notid,
                        typeId: typeid,
                        categoryId: categoryId,
                        subCategoryId: subCategoryId,
                        brandId: brandId,
                        name: name).TotalCount;

            ViewSearchProduct search = new ViewSearchProduct()
            {
                PageIndex = prindex.GetValueOrDefault(),
                PageSize = pagesize.GetValueOrDefault(),
                Results = results,
                TotalCount = TotalCount,
            };
            ViewBag.Shop = shop;
            return PartialView(BaseController.GetView(this), search);
        }

        public new ActionResult Profile(int? shId = null, string shidname = null)
        {
            Shop shop = _context.Shop.GetById(shId);
            if (shop == null)
                return RedirectPermanent("/error/404");
            ViewBag.Shop = shop;
            return PartialView(BaseController.GetView(this), shop);
        }

        public ActionResult Contact(int? shId = null, string shidname = null)
        {
            Shop shop = _context.Shop.GetById(shId);
            if (shop == null)
                return RedirectPermanent("/error/404");
            ViewBag.Shop = shop;
            return PartialView(BaseController.GetView(this), shop);
        }

        public ActionResult SearchWithProduct(int? id, int? index, int? pageSize = null, string name = null, string ViewName = null)
        {
            pageSize = pageSize == null ? 10 : pageSize;
            index = index == null ? 1 : index;
            name = name == null ? "" : name;
            ViewBag.CurrentAccount = _context.Account.GetCurrentAccount();

            List<Shop> list = _context.Shop.Where(p =>
                p.Name.Contains(name) ||
                p.Product.Any(s => s.Name.Contains(name))
            ).ToList();

            return PartialView(BaseController.GetView(this, ViewName), list);
        }

        public ActionResult Edit()
        {
            SiteUser user = _context.SiteUser.GetCurrentUser();
            ShopUser shopUser = user.ShopUser.FirstOrDefault();
            if (shopUser == null)
                return RedirectToAction("register", "siteuser");

            Shop shop = shopUser.Shop;
            ViewBag.Message = BaseMessage.GetMessage();
            ViewBag.WebsiteId = new SelectList(_context.Website.GetAllByType(Enum_Code.SYSTEM_TYPE_SHOP), "ID", "Title", shop != null ? shop.WebsiteId : 0);
            ViewBag.CityId = new SelectList(_context.City.GetAll(), "ID", "Name", shop != null ? shop.CityId : 0);
            ViewBag.TypeId = new SelectList(_context.Code.GetAllByCodeGroup(Enum_CodeGroup.SHOP_TYPE), "ID", "Name", shop != null ? shop.TypeId : 0);
            ViewBag.ProductType = _context.ProductType.GetAll();
            ViewBag.PaymentType = _context.PaymentType.GetAll();

            return PartialView(BaseController.GetView(this), shop);
        }

        [HttpPost]
        public ActionResult Edit(Shop shop, string nextUrl)
        {
            if (IsModelValid(shop))
            {
                _context.Shop.Update(shop);
                _context.Save();

                if (nextUrl != null)
                    return Redirect(nextUrl);
            }
            return PartialView(BaseController.GetView(this), shop);
        }

        public ActionResult Package()
        {
            SiteUser user = _context.SiteUser.GetCurrentUser();
            ViewBag.SiteUser = user;
            ViewBag.Package = _context.Package.GetAll();
            ShopUser shopUser = user.ShopUser.FirstOrDefault();
            if (shopUser == null)
                return RedirectToAction("register", "siteuser");

            return PartialView(BaseController.GetView(this), shopUser.Shop);
        }

        private void FillViewBagValues(SiteUser user)
        {
            ViewBag.User = user;
            UserGroup group = _context.UserGroup.GetFirstByRole(Enum_UserRole.SHOP);
            ViewBag.UserGroupId = group.ID;
        }

        public ActionResult Follow(int ShopId, string Function, string Back)
        {
            Shop shop = _context.Shop.GetById(ShopId);
            ViewAccount CurrentUser = _context.Account.GetCurrentAccount();
            if (CurrentUser != null)
            {
                if (Function == "ADD")
                {
                    ShopFollow follow = new ShopFollow()
                    {
                        AccountId = CurrentUser.Id,
                        ShopId = ShopId,
                        Datetime = DateTime.Now
                    };
                    _context.ShopFollow.Insert(follow);
                }
                else
                {
                    ShopFollow follow = _context.ShopFollow.GetByShopIdAndAccountId(ShopId, CurrentUser.Id);
                    _context.ShopFollow.Delete(follow);
                }
                _context.Save();

                return Redirect(Back);
            }
            else
            {
                return RedirectToAction("login", "Account", new { back = Back });
            }
        }

        [HttpPost]
        public ActionResult ReportBlock(int ShopId, string Function, string Back)
        {
            Shop shop = _context.Shop.GetById(ShopId);
            ViewAccount CurrentUser = _context.Account.GetCurrentAccount();
            if (CurrentUser != null)
            {
                if (Function == "ADD")
                {
                    ShopReportBlock block = new ShopReportBlock()
                    {
                        AccountId = CurrentUser.Id,
                        ShopId = ShopId,
                        Datetime = DateTime.Now
                    };
                    _context.ShopReportBlock.Insert(block);
                }
                else
                {
                    ShopReportBlock block = _context.ShopReportBlock.GetByShopIdAndAccountId(ShopId, CurrentUser.Id);
                    _context.ShopReportBlock.Delete(block);
                }
                _context.Save();

                return Redirect(Back);
            }
            else
            {
                return RedirectToAction("login", "Account", new { back = Back });
            }
        }

        public ActionResult GetAll(int ShopId, string ViewName)
        {
            List<Shop> list = _context.Shop.Where(p =>
                p.ID != ShopId
            ).ToList();

            return PartialView(BaseController.GetView(this, ViewName), list);
        }
        
        private void FillDropDowns(Shop shop)
        {
            ViewBag.CityId = new SelectList(_context.City.GetAll(), "ID", "Name", shop != null ? shop.CityId : 0);
        }

        private bool IsModelValid(Shop shop)
        {
            bool result = false;
            if (shop.Name == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_SHOP_NAME);
            else if (shop.UserCreatorId != null)
            {
                SiteUser creator = _context.SiteUser.GetById(shop.UserCreatorId);
                if (creator == null)
                {
                    ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.INVALID_SHOP_CREATOR);
                    result = false;
                }
                else
                    result = true;
            }
            else
                result = true;
            return result;
        }

        /*
        public ActionResult Register(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            SiteUser user = _context.SiteUser.GetById(id);

            List<Shop> list = _context.Shop.GetAllShopForUserId(user.ID);
            if (list.Count > 0)
            {
                return Redirect(BaseWebsite.PanelUrl);
            }

            FillViewBagValues(user);

            Shop shop = new Shop()
            {
                Active = true,
                Approved = false,
                CityId = 0,
                WebsiteId = BaseWebsite.WebsiteId
            };

            FillDropDowns(shop);
            ViewBag.Message = BaseMessage.GetMessage();
            return PartialView(BaseController.GetView(this), shop);
        }
        
        [HttpPost]
        public ActionResult Register(Shop shop, int? UserGroupId, int? UserId)
        {
            ViewBag.Message = BaseMessage.GetMessage();
            UserGroup group = _context.UserGroup.GetById(UserGroupId);
            SiteUser user = _context.SiteUser.GetById(UserId);
            if (IsModelValid(shop))
            {
                _context.Shop.Insert(shop);
                _context.Save();
                SiteUserUserGroup usergroupuser = new SiteUserUserGroup() {
                    SiteUserId = user.ID,
                    UserGroupId = group.ID,
                };
                _context.SiteUserUserGroup.Insert(usergroupuser);
                ShopUser shopuser = new ShopUser() {
                    UserId = user.ID,
                    ShopId = shop.ID
                };
                _context.ShopUser.Insert(shopuser);
                _context.Save();
                
                return Redirect(BaseWebsite.PanelUrl + "profile/login");
            }
            FillViewBagValues(user);
            FillDropDowns(shop);
            return PartialView(BaseController.GetView(this), shop);
        }
        */

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