using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Panel.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Index(int id)
        {
            string Url = Request.Url.AbsoluteUri;
            if (Url.Contains(";"))
            {

            }

            ViewError error = new ViewError() {
                Code = id,
                BackUrl = "/",
                Message = "در صورت بروز هر گونه اخلال در عملکرد حتما به مدیر سامانه اطلاع دهید",
            };

            if (Request.UrlReferrer != null)
                error.BackUrl = Request.UrlReferrer.AbsoluteUri;

            if (id == (int)Enum_PageError.ACCESS_DENIED)
                error.Title = "شما مجوز دسترسی به این صفحه را ندارید";
            else if (id == (int)Enum_PageError.PAGE_NOT_FOUND)
                error.Title = "صفحه مورد نظر در سیستم یافت نشد";
            else if (id == (int)Enum_PageError.SERVER_ERROR)
                error.Title = "خطای داخلی سرور";
            else if (id == (int)Enum_PageError.CAN_NOT_DELETE)
                error.Title = "این آیتم در بخشی از نرم افزار استفاده شده و قابل حذف نمی باشد";
            else if (id == (int)Enum_PageError.DB_VALIDATION)
                error.Title = "مقادیر غیر معتبر وارد شده است";
            return View(error);
        }
    }
}