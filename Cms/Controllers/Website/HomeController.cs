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
            ViewBag.CountVisit = 10000; /*_context.WebsiteView.Count();*/
            ViewBag.CountRegCourse = 10000; /*_context.CoursePayment.Count() + _context.AccountOrderProduct.SearchCount(typeId: 2, status: DataLayer.Enumarables.Enum_Code.ORDER_STATUS_SUCCESS);*/
            ViewBag.CountQuiz = 10000; /*_context.QuizPayment.Where(s => s.Status == 1).ToList().Count() + _context.AccountOrderProduct.SearchCount(typeId: 1001, status: DataLayer.Enumarables.Enum_Code.ORDER_STATUS_SUCCESS);*/
            return PartialView(BaseController.GetView(this));
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