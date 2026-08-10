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
    public class ExameAnswerItemController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account);
            ExameQuestion exame = _context.ExameQuestion.GetById(id);
            if (exame != null)
            {
                ViewBag.ExameQuestion = exame;
                return View(_context.ExameAnswerItem.Where(s => s.ExameQuestionId == id).ToList());
            }
            else
            {
                return RedirectToAction("Index");
            }
        }



        public ActionResult Create(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account_New);
            ExameQuestion exame = _context.ExameQuestion.GetById(id);
            if (exame != null)
            {
                ViewBag.ExameQuestion = exame;

                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }

        }

        [HttpPost]
        [System.Web.Mvc.ValidateInput(false)]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(ExameAnswerItem ExameQuestionanswer, HttpPostedFileBase file)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account_New);

            if (IsModelValid(ExameQuestionanswer))
            {
                if (file != null)
                {
                    Picture picture = _context.Picture.CreateAndUpload(Enum_Code.SYSTEM_TYPE_PANEL, file);
                    ExameQuestionanswer.PictureId = picture.ID;
                }


                _context.ExameAnswerItem.Insert(ExameQuestionanswer);
                _context.Save();
                return RedirectToAction("Index", new { id = ExameQuestionanswer.ExameQuestionId });
            }

            return View(ExameQuestionanswer);
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ExameAnswerItem ExameQuestionanswer = _context.ExameAnswerItem.GetById(id);
            if (ExameQuestionanswer == null)
                return HttpNotFound();

            ViewBag.Message = BaseMessage.GetMessage();
            return View(ExameQuestionanswer);
        }

        [HttpPost]
        [System.Web.Mvc.ValidateInput(false)]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(ExameAnswerItem ExameQuestionanswer, HttpPostedFileBase file)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account_Edit);
            if (IsModelValid(ExameQuestionanswer))
            {

                if (file != null)
                {
                    Picture picture = _context.Picture.CreateAndUpload(Enum_Code.SYSTEM_TYPE_PANEL, file);
                    ExameQuestionanswer.PictureId = picture.ID;
                }

                _context.ExameAnswerItem.Update(ExameQuestionanswer);
                _context.Save();
                return RedirectToAction("Index", new { id = ExameQuestionanswer.ExameQuestionId });
            }
            return View(ExameQuestionanswer);
        }

        private bool IsModelValid(ExameAnswerItem ExameQuestionanswer)
        {
            bool result = false;
            if (ExameQuestionanswer.Text == null)
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

            ExameAnswerItem ExameAnswerItem = _context.ExameAnswerItem.GetById(id);
            if (ExameAnswerItem == null)
                return HttpNotFound();

            return View(ExameAnswerItem);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account_Delete);
            ExameAnswerItem ExameAnswerItem = _context.ExameAnswerItem.GetById(id);
            ViewBag.ExameId = ExameAnswerItem.ExameQuestionId;
            try
            {
                _context.ExameAnswerItem.Delete(ExameAnswerItem);
                _context.Save();
                return RedirectToAction("Index", new { id = ViewBag.ExameId });
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(ExameAnswerItem);
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