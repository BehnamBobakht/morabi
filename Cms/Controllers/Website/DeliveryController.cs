using DataLayer.Base;
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
    public class DeliveryController : Controller
    {
        UnitOfWork _context = new UnitOfWork();



        public ActionResult CartAccountDelivery(int storeId,double price)
        {
            ViewAccount currentAccount = BaseWebsite.CurrentAccount;
            var model = _context.AccountAddress.GetAllByAccountVm(accountId: currentAccount.Id);
            //ViewBag.addressId = addressId;
            return PartialView(BaseController.GetView(this), model);
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