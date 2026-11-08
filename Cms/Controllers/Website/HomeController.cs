using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using Cms.Controllers.Website;
using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using MySql.Data.MySqlClient;

namespace Cms.Controllers
{
    public class HomeController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();


        public ActionResult Index()
        {
            return PartialView(BaseController.GetView(this));
        }

        [ChildActionOnly]
        public ActionResult HomeCourses(int take = 12)
        {
            // Card-only projection: title, category, image, link, price and discount.
            var cards = _context.Product.GetHomeCards(productTypeId: 2, take: take, prioritizeDiscount: true);
            return PartialView("~/Views/" + BaseWebsite.WebsiteLabel + "/Home/_HomeCourses.cshtml", cards);
        }

        [ChildActionOnly]
        public ActionResult HomeBooks(int take = 12)
        {
            // "books" is the existing custom-label convention used by the old home action.
            var cards = _context.Product.GetHomeCards(customLabel: "books", take: take, prioritizeDiscount: true);
            return PartialView("~/Views/" + BaseWebsite.WebsiteLabel + "/Home/_HomeBooks.cshtml", cards);
        }

        [ChildActionOnly]
        [OutputCache(Duration = 300, VaryByParam = "categoryLabel;take")]
        public ActionResult HomePosts(string categoryLabel = "blog", int take = 5)
        {
            // Card-only projection: title, summary, category, image and destination URL.
            var cards = _context.Post.GetHomeCards(categoryLabel: categoryLabel, take: take);
            return PartialView("~/Views/" + BaseWebsite.WebsiteLabel + "/Home/_HomePosts.cshtml", cards);
        }

        [ChildActionOnly]
        [OutputCache(Duration = 300, VaryByParam = "take")]
        public ActionResult HomeVideos(int take = 10)
        {
            var cards = _context.Post.GetHomeCards(categoryLabel: "video", take: 10, orderByShowNumber: true);
            return PartialView("~/Views/" + BaseWebsite.WebsiteLabel + "/Home/_HomeVideos.cshtml", cards);
        }
    

        public ActionResult Footer()
        {
            return PartialView("/Views/" + BaseWebsite.WebsiteLabel + "/shared/_footer.cshtml");
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
