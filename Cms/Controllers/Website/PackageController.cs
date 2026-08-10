using DataLayer.Api;
using DataLayer.Api.Model;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cms.Controllers.Website
{
    public class PackageController : Controller
    {
        UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(int? id)
        {
            List<Package> listPackage = _context.Package.GetAll();
            return PartialView(BaseController.GetView(this), listPackage);
        }

        public ActionResult StartPayment(int id)
        {
            SiteUser user = _context.SiteUser.GetCurrentUser();

            ShopUser shopUser = user.ShopUser.FirstOrDefault();
            if (shopUser != null)
            {
                Package package = _context.Package.GetById(id);
                Shop shop = shopUser.Shop;
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
                return RedirectToAction("start", "payment", new { id = tempPayment.Id });
            }
            else
                return null;
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