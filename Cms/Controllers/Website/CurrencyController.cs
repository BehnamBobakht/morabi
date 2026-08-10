using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cms.Controllers.Website
{
    public class CurrencyController : Controller
    {
        UnitOfWork _context = new UnitOfWork();

        // GET: Currency
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetList()
        {
            List<Currency> currencyList = _context.Currency.GetAll();
            return View(currencyList);
        }
    }
}