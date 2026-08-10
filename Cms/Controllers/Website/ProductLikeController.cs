using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using static QRCoder.PayloadGenerator;

namespace Cms.Controllers
{
    public class ProductLikeController : Controller
    {
        // GET: ProductLike
        UnitOfWork _context = new UnitOfWork();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Search(int plAccountId ,  int index = 1, int pageSize = 10, string plviewName = null)
        {
            plviewName = plviewName == null ? "Search" : plviewName;
            var currentLang = BaseLanguage.GetCurrentLanguage();
            List<ProductLike> results = _context.ProductLike.GetAllByAccountId(accountId: plAccountId, index: index, pageSize: pageSize); 


            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, plviewName, results);

        }
        [HttpDelete]
        public ActionResult DeleteByAccountAndProduct(int productId)
        {
            ViewAccount account = _context.Account.GetCurrentAccount();
            if (account == null)
                return RedirectToAction("index", new { controller = "home" });
            else
            {
                _context.ProductLike.DeleteByProductIdAndAccountId(account.Id, productId);
                return PartialView();
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