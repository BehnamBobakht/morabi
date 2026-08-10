using DataLayer.Api;
using DataLayer.Api.Model;
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
    public class PackageController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Package);
            return View(_context.Package.GetAll());
        }

        public ActionResult Select()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Package);
            Shop shop = GetCurrentShop();
            if (shop == null)
                return HttpNotFound();
            ViewBag.Shop = shop;
            return View(_context.Package.GetAll());
        }
        
        public ActionResult StartPayment(int packageId)
        {
            SiteUser user = _context.SiteUser.GetCurrentUser();
            _context.Permission.CheckPagePermission(user, Enum_Permission.Package);

            Package package = _context.Package.GetById(packageId);
            Shop shop = GetCurrentShop();
            if (shop == null)
                return HttpNotFound();
            
            ViewPayment payment = new ViewPayment()
            {
                Description = "خرید بسته " + package.Name,
                Price = package.Price,
                PaymentSubject = new ViewPaymentSubject()
                {
                    Label = Enum_PaymentSubject.PACKAGE.ToString()
                }
            };
            ApiResult result = ApiPayment.Post(_context, null, user.ID, payment);
            ViewPayment tempPayment = (ViewPayment)result.Value;
            string redirectUrl = BaseWebsite.ShopUrl + "/payment/start/" + tempPayment.Id;
            return Redirect(redirectUrl);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Package_Details);
            Package package = _context.Package.GetById(id);
            if (package == null)
            {
                return HttpNotFound();
            }
            return View(package);
        }

        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Package_New);
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Package package)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Package_New);

            if (IsModelValid(package))
            {
                _context.Package.Insert(package);
                _context.Save();
                return RedirectToAction("Index");
            }
            
            return View(package);
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Package_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Package package = _context.Package.GetById(id);
            if (package == null)
                return HttpNotFound();
            
            ViewBag.Message = BaseMessage.GetMessage();
            return View(package);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Package package)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Package_Edit);
            if (IsModelValid(package))
            {
                _context.Package.Update(package);
                _context.Save();
                return RedirectToAction("Index");
            }
            return View(package);
        }

        private bool IsModelValid(Package package)
        {
            bool result = false;
            if (package.Name == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PACKAGE_NAME);
            else
                result = true;
            return result;
        }
        
        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Package_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Package package = _context.Package.GetById(id);
            if (package == null)
                return HttpNotFound();

            return View(package);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Package_Delete);
            Package package = _context.Package.GetById(id);
            try
            {
                _context.Package.Delete(package);
                _context.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(package);
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

        private Shop GetCurrentShop(int? id = null)
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
    }

}