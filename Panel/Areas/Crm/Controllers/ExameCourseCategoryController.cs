using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Panel.Areas.Crm.Controllers
{
    public class ExameCourseCategoryController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account);
            ExameCourse ExameCourse = _context.ExameCourse.GetById(id);
            if (ExameCourse != null)
            {
                ViewBag.ExameCourse = ExameCourse;
                return View(_context.ExameCourseCategory.Where(s => s.CourseId == id).ToList());
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult Answer(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account);
            ExameCourse ExameCourse = _context.ExameCourse.GetById(id);
            if (ExameCourse != null)
            {
                ViewBag.ExameCourse = ExameCourse;
                return View(_context.ExameCourseCategory.Where(s => s.CourseId == id).ToList());
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult Create(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account_New);
            ExameCourse ExameCourse = _context.ExameCourse.GetById(id);
            if (ExameCourse != null)
            {
               
                ViewBag.ExameCourse = ExameCourse;
                FillDropDowns(null);

                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }

        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(ExameCourseCategory ExameCourseCategory)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account_New);

            if (IsModelValid(ExameCourseCategory))
            {
               


                _context.ExameCourseCategory.Insert(ExameCourseCategory);
                _context.Save();
                return RedirectToAction("Index", new { id = ExameCourseCategory.CourseId });
            }

            return View(ExameCourseCategory);
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ExameCourseCategory ExameCourseCategory = _context.ExameCourseCategory.GetById(id);
            if (ExameCourseCategory == null)
                return HttpNotFound();
            FillDropDowns(ExameCourseCategory);

            ViewBag.Message = BaseMessage.GetMessage();
            return View(ExameCourseCategory);
        }
        private void FillDropDowns(ExameCourseCategory ExameCourseCategory)
        {

            ViewBag.CourseId = new SelectList(_context.ExameCourse.GetAll().ToList(), "ID", "Name", ExameCourseCategory != null ? ExameCourseCategory.CourseId : 0);

        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(ExameCourseCategory ExameCourseCategory)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account_Edit);
            if (IsModelValid(ExameCourseCategory))
            {

               
                _context.ExameCourseCategory.Update(ExameCourseCategory);
                _context.Save();
                return RedirectToAction("Index", new { id = ExameCourseCategory.CourseId });
            }
            return View(ExameCourseCategory);
        }

        private bool IsModelValid(ExameCourseCategory ExameCourseCategory)
        {
            bool result = false;
            if (ExameCourseCategory.Name == null)
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

            ExameCourseCategory ExameCourseCategory = _context.ExameCourseCategory.GetById(id);
            if (ExameCourseCategory == null)
                return HttpNotFound();

            return View(ExameCourseCategory);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account_Delete);
            ExameCourseCategory ExameCourseCategory = _context.ExameCourseCategory.GetById(id);
            try
            {
                _context.ExameCourseCategory.Delete(ExameCourseCategory);
                _context.Save();
                return RedirectToAction("Index", new { id = ExameCourseCategory.CourseId });
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(ExameCourseCategory);
            }

        }
        public ActionResult CategoryDropDown(int? Id = null, int? value = null)
        {
            ViewBag.ExameCourseCategoryId = new SelectList(_context.ExameCourseCategory.Where(s=>s.CourseId==Id), "ID", "Name", value);
            return PartialView();
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