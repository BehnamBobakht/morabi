using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Panel.Areas.Store.Controllers
{
    [ValidateInput(false)]
    public class ShopController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            SiteUser CurrentUser = _context.SiteUser.GetCurrentUser();
            _context.Permission.CheckPagePermission(CurrentUser, Enum_Permission.Shop);
            List<SiteUserUserGroup> list = _context.SiteUserUserGroup.GetAllBySiteUserId(CurrentUser.ID);
            if (_context.UserRole.HasUserRole(list, CurrentUser, Enum_UserRole.ADMIN))
                return View(_context.Shop.GetAll().ToView());
            else
                return View(_context.Shop.GetAllShopForUserId(_context.SiteUser.GetCurrentUser().ID).ToView());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Shop_Details);
            Shop shop = _context.Shop.GetById(id);
            if (shop == null)
            {
                return HttpNotFound();
            }
            return View(shop);
        }

        public ActionResult ProductTypes(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Shop_Edit);

            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            List<ProductType> list = _context.ProductType.GetAll();
            List<ShopProductType> listShop = _context.ShopProductType.GetAllByShopId(id.Value);
            List<ViewShopProductType> listOutput = new List<ViewShopProductType>();
            foreach (ProductType item in list)
            {
                listOutput.Add(new ViewShopProductType() {
                    ProductTypeId = item.ID,
                    ProductTypeName = item.Name,
                    Selected = listShop.Any(p => p.ProductTypeId == item.ID)
                });
            }

            ViewBag.Message = BaseMessage.GetMessage();
            ViewBag.ShopId = id;
            return View(listOutput);
        }

        [HttpPost]
        public ActionResult ProductTypes(int ShopId, int[] ProductTypes)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Shop_Edit);

            _context.ShopProductType.DeleteByShopId(ShopId);
            if (ProductTypes != null)
            {
                foreach (int item in ProductTypes)
                {
                    ShopProductType entity = new ShopProductType() {
                        ProductTypeId = item,
                        ShopId = ShopId
                    };
                    _context.ShopProductType.Insert(entity);
                }
                _context.Save();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Shop_New);
            Shop shop = new Shop() {
                Active = true,
                Approved = false
            };
            ViewBag.Message = BaseMessage.GetMessage();
            FillDropDowns(null);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CityId,WebsiteId,Name,Label,Description")] Shop shop, HttpPostedFileBase file)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Shop_New);

            if (IsModelValid(shop, file).Type == Enum_MessageType.SUCCESS)
            {
                shop.UserCreatorId = _context.SiteUser.GetCurrentUser().ID;
                _context.Shop.Insert(shop);
                _context.Save();

                return RedirectToAction("index");
            }

            FillDropDowns(shop);
            return View(shop);
        }

        public new ActionResult Profile(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Shop_Profile);
            Shop shop = GetCurrentShop(id);
            if (shop == null)
                return HttpNotFound();

            FillDropDowns(shop);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(shop);
        }

        [HttpPost]
        public new ActionResult Profile(Shop shop)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Shop_Profile);
            SiteUser CurrentUser = _context.SiteUser.GetCurrentUser();
            if (_context.ShopUser.HasShopUser(shop.ID, CurrentUser.ID) || shop.UserCreatorId == CurrentUser.ID)
            {
                ViewMessage result = IsModelValid(shop, null);

                List<ShopContact> listContact = shop.ShopContact.ToList();
                List<ShopAddress> listAddress = shop.ShopAddress.ToList();
                List<ShopProductType> listShopProductType = shop.ShopProductType.ToList();
                List<ShopPaymentType> listShopPaymentType = shop.ShopPaymentType.ToList();

                shop.ShopContact.Clear();
                shop.ShopAddress.Clear();
                shop.ShopProductType.Clear();
                shop.ShopPaymentType.Clear();

                if (result.Type == Enum_MessageType.SUCCESS)
                {
                    _context.Shop.Update(shop);
                    _context.Save();

                    _context.ShopContact.DeleteByShopId(shop.ID);
                    _context.ShopAddress.DeleteByShopId(shop.ID);
                    _context.ShopProductType.DeleteByShopId(shop.ID);
                    _context.ShopPaymentType.DeleteByShopId(shop.ID);
                    _context.Save();

                    foreach (ShopContact item in listContact)
                    {
                        _context.ShopContact.Insert(new ShopContact()
                        {
                            ShopId = shop.ID,
                            TypeId = item.TypeId,
                            Value = item.Value
                        });
                    }

                    foreach (ShopAddress item in listAddress)
                    {
                        _context.ShopAddress.Insert(new ShopAddress()
                        {
                            ShopId = shop.ID,
                            Address = item.Address,
                            Latitude = item.Latitude,
                            Longitude = item.Longitude,
                            Name = item.Name
                        });
                    }

                    foreach (ShopProductType item in listShopProductType)
                    {
                        _context.ShopProductType.Insert(new ShopProductType()
                        {
                            ShopId = shop.ID,
                            ProductTypeId = item.ProductTypeId
                        });
                    }

                    foreach (ShopPaymentType item in listShopPaymentType)
                    {
                        _context.ShopPaymentType.Insert(new ShopPaymentType()
                        {
                            ShopId = shop.ID,
                            PaymentTypeId = item.PaymentTypeId
                        });
                    }
                    _context.Save();
                }
                return new JsonResult() { Data = result };
            }
            else
                return new JsonResult() { Data = BaseMessage.GetMessage(Enum_Message.INVALID_SHOP_USER) };
        }

        public ActionResult Follow(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Shop_Follow);
            Shop shop = GetCurrentShop(id);
            if (shop == null)
                return HttpNotFound();
            
            return View(shop.ShopFollow.ToList().ToView());
        }

        public ActionResult Chat(int? id, int? accountId)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Shop_Chat);
            Shop shop = GetCurrentShop(id);
            if (shop == null)
                return HttpNotFound();

            ViewBag.Shop = shop;
            Account account = _context.Account.GetById(accountId.Value);

            _context.ShopChat.DoReadAllChatByAccountForShop(account, shop.ID);
            _context.Save();

            return View(account);
        }

        [HttpPost]
        public ActionResult Chat(int AccountId, int ShopId, string Body)
        {
            SiteUser CurrentUser = _context.SiteUser.GetCurrentUser();
            _context.Permission.CheckPagePermission(CurrentUser, Enum_Permission.Shop_Chat);
            Shop shop = _context.Shop.GetById(ShopId);
            if (shop == null)
                return HttpNotFound();

            if (shop.ShopUser.Any(p => p.UserId == CurrentUser.ID) == false)
                return HttpNotFound();

            if (string.IsNullOrEmpty(Body) == false)
            {
                ShopChat chat = new ShopChat()
                {
                    AccountId = AccountId,
                    Body = Body,
                    Datetime = DateTime.Now,
                    IsAccountSend = false,
                    IsRead = false,
                    ShopId = ShopId
                };
                _context.ShopChat.Insert(chat);
                _context.Save();
            }
            return RedirectToAction("chat", new { accountId = AccountId });
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Shop_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Shop shop = _context.Shop.GetById(id);
            if (shop == null)
                return HttpNotFound();

            FillDropDowns(shop);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(shop);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,UserCreatorId,CityId,WebsiteId,PictureId,Name,Label,Description")]Shop shop, HttpPostedFileBase file)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Shop_Edit);
            if (IsModelValid(shop, file).Type == Enum_MessageType.SUCCESS)
            {
                _context.Shop.Update(shop);
                _context.Save();
                return RedirectToAction("Index");
            }
            FillDropDowns(shop);
            return View(shop);
        }

        private ViewMessage IsModelValid(Shop shop, HttpPostedFileBase file)
        {
            ViewMessage result = new ViewMessage();
            if (shop.Name == null)
                result = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_SHOP_NAME);
            ViewBag.Message = result;
            if (file != null && result.Type == Enum_MessageType.SUCCESS)
            {
                Picture picture = _context.Picture.CreateAndUpload(Enum_Code.SYSTEM_TYPE_PANEL, file);
                shop.PictureId = picture.ID;
            }
            return result;
        }

        private void FillDropDowns(Shop shop)
        {
            ViewBag.WebsiteId = new SelectList(_context.Website.GetAllByType(Enum_Code.SYSTEM_TYPE_SHOP), "ID", "Title", shop != null ? shop.WebsiteId : 0);
            ViewBag.CityId = new SelectList(_context.City.GetAll(), "ID", "Name", shop != null ? shop.CityId : 0);
            ViewBag.TypeId = new SelectList(_context.Code.GetAllByCodeGroup(Enum_CodeGroup.SHOP_TYPE), "ID", "Name", shop != null ? shop.TypeId : 0);
            ViewBag.ProductType = _context.ProductType.GetAll();
            ViewBag.PaymentType = _context.PaymentType.GetAll();
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Shop_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Shop shop = _context.Shop.GetById(id);
            if (shop == null)
                return HttpNotFound();

            return View(shop);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Shop_Delete);
            Shop shop = _context.Shop.GetById(id);
            try
            {
                _context.ShopAddress.DeleteByShopId(shop.ID);
                _context.ShopChat.DeleteByShopId(shop.ID);
                _context.ShopContact.DeleteByShopId(shop.ID);
                _context.ShopFollow.DeleteByShopId(shop.ID);
                _context.ShopPaymentType.DeleteByShopId(shop.ID);
                _context.ShopProductType.DeleteByShopId(shop.ID);
                _context.ShopUser.DeleteByShopId(shop.ID);

                _context.Shop.Delete(shop);
                _context.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(shop);
            }

        }

        private Shop GetCurrentShop(int? id)
        {
            SiteUser CurrrentUser = _context.SiteUser.GetCurrentUser();
            Shop shop = null;
            if (id == null)
                shop = _context.Shop.GetFirstOrDefaultForUserId(_context.SiteUser.GetCurrentUser().ID);
            else
            {
                shop = _context.Shop.GetById(id.Value);
                shop = shop.ShopUser.Any(p => p.UserId == CurrrentUser.ID) || shop.UserCreatorId == CurrrentUser.ID ? shop : null;
            }
            return shop;
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