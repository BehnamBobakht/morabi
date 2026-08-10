using ClosedXML.Excel;
using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Panel.Areas.Content.Controllers
{
    public class ContactUsController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();
        public ActionResult Index(string name = null, string title = null, string pageIndex = null,
   string pageSize = null, string from = null, string to = null)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ContactUs);
            int size = pageSize == null ? 50 : pageSize.GetInteger();
            int index = pageIndex == null ? 1 : pageIndex.GetInteger();
            DateTime fromDatetime = string.IsNullOrEmpty(from) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(from))).GetValueOrDefault();
            DateTime toDatetime = string.IsNullOrEmpty(to) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(to))).GetValueOrDefault();

            ViewBag.PageIndex = index.ToString();
            ViewBag.PageSize = size.ToString();
            ViewBag.pagename = name;
            ViewBag.pagetitle = title;
            ViewBag.fromDatetime = from;
            ViewBag.toDatetime = to;
            ViewBag.TotalCount = _context.ContactUs.SearchCount(
                   title: title,
                   name: name,
                   fromDatetime: fromDatetime,
                   toDatetime: toDatetime);

            List<ContactUs> listcontacts = _context.ContactUs.Search(
 title: title,
                   name: name,
                   fromDatetime: fromDatetime,
                   toDatetime: toDatetime,
                   pageSize: size,
                    index: index
                  )
                .ToList();

            return View(listcontacts);
        }


        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ContactUs);

            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            ContactUs contactus = _context.ContactUs.GetById(id);
            if (contactus == null)
                return HttpNotFound();

            ViewBag.Message = BaseMessage.GetMessage();
            return View(contactus);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ContactUs contactus)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ContactUs);
            _context.ContactUs.Update(contactus);
            _context.Save();
            ViewBag.Message = BaseMessage.GetMessage();
            //return View(contactus);
            return RedirectToAction("Index");
        }
        //private bool IsModelValid(Gallery gallery)
        //{
        //    bool result = false;
        //    if (gallery.Name == null)
        //        ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_GALLERY_NAME);
        //    else
        //        result = true;
        //    return result;
        //}

        public ActionResult ExelExport(string name = null, string title = null
    , string from = null, string to = null)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ContactUs);
            int size = 99999;
            int index = 1;
            DateTime fromDatetime = string.IsNullOrEmpty(from) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(from))).GetValueOrDefault();
            DateTime toDatetime = string.IsNullOrEmpty(to) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(to))).GetValueOrDefault();


            int total = _context.ContactUs.SearchCount(
                   title: title,
                   name: name,
                   fromDatetime: fromDatetime,
                   toDatetime: toDatetime);

            List<ContactUs> listcontacts = _context.ContactUs.Search(
 title: title,
                   name: name,
                   fromDatetime: fromDatetime,
                   toDatetime: toDatetime,
                   pageSize: size,
                    index: index
                  )
                .ToList();
            DataTable dt = new DataTable("sheet1");
            dt.Columns.AddRange(new DataColumn[8] { new DataColumn("#"),
                new DataColumn("عنوان"),
                new DataColumn("تاریخ و زمان"),

                                                     new DataColumn("نام و نام خانوادگی"),
                                                     new DataColumn("تلفن"),
                                                     new DataColumn("ایمیل"),
                                                     new DataColumn("سایر"),
                                                     new DataColumn("توضیحات"),

                                                  });
            for (int i = 0; i < listcontacts.Count(); i++)
            {
                var item = listcontacts[i];

                string fields = "";
                foreach (var fi in item.ContactUsField)
                {
                    fields += $"{fi.Name} : {fi.Value} | ";
                }
                dt.Rows.Add(i + 1, item.Title, item.CreateDate.ToPersian(), item.Name, item.Phone, item.Email, fields, item.Body);
            }

            string fileName = string.Format("{0} - {1}.xlsx", DateTime.Now.Date.ToShortDateString(), total);
            using (XLWorkbook wb = new XLWorkbook()) //Install ClosedXml from Nuget for XLWorkbook  
            {
                wb.RightToLeft = true;
                wb.Worksheets.Add(dt);

                using (MemoryStream stream = new MemoryStream()) //using System.IO;  
                {
                    wb.SaveAs(stream);

                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
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