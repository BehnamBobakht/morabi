using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Panel.Areas.Calendar.Controllers
{
    public class EventController : Controller
    {
        private UnitOfWork Context = new UnitOfWork();

        public ActionResult Index(long id)
        {
            ViewBag.Id = id;
            DateTime datetime = new DateTime(id);
            ViewBag.Title = "تقویم کاری " + datetime.ToPersianComplete();
            List<Event> list = Context.Event.GetAllForAccount(Context.SiteUser.GetCurrentUser().ID, datetime);
            List<ViewCalendarEvent> Output = new List<ViewCalendarEvent>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewCalendarEvent(list[i], i, MaxZero));
            }
            return View(Output);
        }

        public ActionResult Create(long id)
        {
            FillDropDowns(null);
            DateTime datetime = new DateTime(id);
            Event obj = new Event() {
                CreateDatetime = DateTime.Now,
                UserId = Context.SiteUser.GetCurrentUser().ID,
                Date = datetime
            };

            ViewBag.Date = datetime.ToPersian();
            ViewBag.Message = BaseMessage.GetMessage();
            ViewBag.Id = id;
            return View(obj);
        }

        [HttpPost]
        public ActionResult Create(Event obj)
        {
            //Context.Permission.CheckPagePermission(Context.SiteUser.GetCurrentUser(), Enum_Permission.Post_New);

            if (IsModelValid(obj))
            {
                Context.Event.Insert(obj);
                Context.Save();
                return RedirectToAction("Index");
            }

            ViewBag.Date = obj.Date.ToPersian();
            FillDropDowns(obj);
            return View(obj);
        }

        private bool IsModelValid(Event obj)
        {
            bool result = false;
            if (obj.Description == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_EVENT_DESCRIPTION);
            else
                result = true;

            return result;
        }

        private void FillDropDowns(Event obj)
        {
            ViewBag.TypeId = new SelectList(Context.Code.GetAllByCodeGroup(Enum_CodeGroup.EVENT_TYPE), "ID", "Name", obj != null ? obj.TypeId : 0);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}