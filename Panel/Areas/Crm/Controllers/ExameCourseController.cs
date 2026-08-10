using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Panel.Areas.Crm.Controllers
{
    public class ExameCourseController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account);
            return View(_context.ExameCourse.GetAll().Where(s=>s.Deleted!=true));
        }


        private void FillDropDowns(ExameCourse ExameCourse)
        {
            ViewBag.Type = new SelectList(_context.Code.GetAllByCodeGroup(Enum_CodeGroup.COURSE_TYPE), "ID", "Name", ExameCourse != null ? ExameCourse.Type : 0);
        }
        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account_New);
            FillDropDowns(null);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ExameCourse ExameCourse)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account_New);

            if (IsModelValid(ExameCourse))
            {

                _context.ExameCourse.Insert(ExameCourse);
                _context.Save();
                return RedirectToAction("Index");
            }

            return View(ExameCourse);
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ExameCourse ExameCourse = _context.ExameCourse.GetById(id);
            if (ExameCourse == null)
                return HttpNotFound();
            FillDropDowns(ExameCourse);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(ExameCourse);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ExameCourse ExameCourse)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account_Edit);
            if (IsModelValid(ExameCourse))
            {

                _context.ExameCourse.Update(ExameCourse);
                _context.Save();
                return RedirectToAction("Index");
            }
            return View(ExameCourse);
        }

        private bool IsModelValid(ExameCourse ExameCourse)
        {
            bool result = false;
            if (ExameCourse.Name == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_CATEGORY_NAME);
            else
                result = true;
            return result;
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ExameCourse ExameCourse = _context.ExameCourse.GetById(id);
            if (ExameCourse == null)
                return HttpNotFound();

            return View(ExameCourse);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account_Delete);
            ExameCourse ExameCourse = _context.ExameCourse.GetById(id);
            try
            {
                ExameCourse.Deleted = true;
                _context.ExameCourse.Update(ExameCourse);
                _context.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(ExameCourse);
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