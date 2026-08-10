using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Panel.Areas.Content.Controllers
{
    public class PostQuestionController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Post);
            ViewBag.post = _context.Post.GetPostVm(id);
            var postQuestion = _context.PostQuestion.Where(s=>s.PostId==id).ToList();
            return View(postQuestion);
        }
        [HttpGet]
        public ActionResult Create(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Post_New);
            var model = new PostQuestion();
            model.PostId = id;
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(PostQuestion postQuestion)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Post_New);
            _context.PostQuestion.Insert(postQuestion);
            _context.Save();
            return RedirectToAction("index", new { id = postQuestion.PostId });
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Post_Edit);
            var model = _context.PostQuestion.GetById(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(PostQuestion postQuestion)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Post_Edit);
            _context.PostQuestion.Update(postQuestion);
            _context.Save();

            return RedirectToAction("index", new { id = postQuestion.PostId });
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Post_Delete);
            var model = _context.PostQuestion.GetById(id);
            var postId = model.PostId;
            if(model!=null)
            {
                _context.PostQuestion.Delete(model);
                _context.Save();
            }
            return RedirectToAction("index",new { id= postId });
        }
    }
}