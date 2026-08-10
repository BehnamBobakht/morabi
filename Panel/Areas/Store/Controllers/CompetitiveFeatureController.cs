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

namespace Panel.Areas.Store.Controllers
{
    [ValidateInput(false)]
    public class CompetitiveFeatureController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(
            int? index = null,
            int? pagesize = null,
            string name = null
           )
        {
            index = index == null ? 1 : index;
            pagesize = pagesize == null ? 10 : pagesize;
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.CompetitiveFeature);
            List<CompetitiveFeature> list = _context.CompetitiveFeature.Search(index: index.GetValueOrDefault(), pageSize: pagesize.GetValueOrDefault(), name: name);
            ViewBag.TotalCount = _context.CompetitiveFeature.SearchCount(name: name);
            return View(list.ToView());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.CompetitiveFeature_Details);
            CompetitiveFeature competitive = _context.CompetitiveFeature.GetById(id);
            if (competitive == null)
            {
                return HttpNotFound();
            }
            return View(competitive);
        }

        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.CompetitiveFeature_New);
            CompetitiveFeature competitive = new CompetitiveFeature();
            ViewBag.Message = BaseMessage.GetMessage();
            return View(competitive);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CompetitiveFeature competitive)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.CompetitiveFeature_New);

            if (IsModelValid(competitive))
            {

                _context.CompetitiveFeature.Insert(competitive);
                _context.Save();
                return RedirectToAction("Index");
            }

            return View(competitive);
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.CompetitiveFeature_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            CompetitiveFeature competitive = _context.CompetitiveFeature.GetById(id);
            if (competitive == null)
                return HttpNotFound();

            ViewBag.Message = BaseMessage.GetMessage();
            return View(competitive);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CompetitiveFeature competitive)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.CompetitiveFeature_Edit);
            if (IsModelValid(competitive))
            {
                _context.CompetitiveFeature.Update(competitive);
                _context.Save();
                return RedirectToAction("Index");
            }

            return View(competitive);
        }


        private bool IsModelValid(CompetitiveFeature competitiveFeature)
        {

            bool result = false;
            if (competitiveFeature.TexCompetitiveFeature == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_CompetitiveFeature_NAME);
            else
                result = true;
            return result;
        }



        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.CompetitiveFeature_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            CompetitiveFeature competitiveFeature = _context.CompetitiveFeature.GetById(id);
            if (competitiveFeature == null)
                return HttpNotFound();

            return View(competitiveFeature);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.CompetitiveFeature_Delete);
            CompetitiveFeature competitiveFeature = _context.CompetitiveFeature.GetById(id);
            try
            {
                _context.CompetitiveFeature.Delete(competitiveFeature);
                _context.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(competitiveFeature);
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