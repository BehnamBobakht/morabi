using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Panel.Areas.Crm.Controllers
{
    public class ExameQuestionController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account);
            Exame exame = _context.Exame.GetById(id);
            if (exame != null)
            {
                ViewBag.Exame = exame;
                return View(_context.ExameQuestion.Where(s => s.ExameId == id).ToList());
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult Print(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account);
            Exame exame = _context.Exame.GetById(id);

            if (exame != null)
            {
                ViewBag.Exame = exame;
                return View(_context.ExameQuestion.Where(s => s.ExameId == id).ToList());
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        public EmptyResult Export(int id)
        {
            string GridHtml = "";

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account);
            Exame exame = _context.Exame.GetById(id);
            var list = _context.ExameQuestion.Where(s => s.ExameId == id).ToList();
            GridHtml += "<table><tr><td>";
            foreach (var course in list.GroupBy(s => s.ExameCourse))
            {
                GridHtml += "<h4>"+course.Key.Name;
                foreach (var item in course.ToList())
                {
                    GridHtml += "<p>";
                    if (item.Formula != "" && item.Formula != null)
                    {
                        GridHtml += "<p>" + item.Formula + "</p>";
                    }
                    else
                    {
                        GridHtml += "<p>" + @item.Text + "</p>";
                    }
                    GridHtml += "<p>";
                    foreach (var answer in item.ExameAnswerItem.Where(s => s.Active != false).ToList())
                    {
                        if(item.AnswerId == answer.ID)
                        {
                            GridHtml += "<INPUT TYPE='radio' CHECKED/>";

                        }
                        else
                        {
                            GridHtml += "<INPUT TYPE='radio' />";

                        }
                        if (answer.Formula != null && answer.Formula != "")
                        {
                            GridHtml += "<span>" + answer.Formula + "</span>";
                        }
                        else
                        {
                            GridHtml += "<span>" + answer.Text + "</span>";
                        }

                    }
                    GridHtml += "</p></p>";

                }
                GridHtml += "</h4>";
            }
            GridHtml += "</table></tr></td>";
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("Content-Disposition", "inline; filename=sample.doc");
            Response.Charset = "UTF-8";
            Response.ContentType = "application/vnd.ms-word";
            byte[] bytes = Encoding.Default.GetBytes("<p>کدام یک از گزینه های زیر از نیاز های ثانویه امادگی جسمانی بشمار می رود؟</p><INPUT TYPE='radio' />تهیه غذا");
            Response.Output.Write(GridHtml);
            Response.Flush();
            Response.End();
            return new EmptyResult();

        }
        public ActionResult ExportPdf(int id)
        {
            string GridHtml = "";

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account);
            Exame exame = _context.Exame.GetById(id);
            var list = _context.ExameQuestion.Where(s => s.ExameId == id).ToList();
            GridHtml += "<table style='text-align:right;direction: rtl;'><tr><td>";
            foreach (var course in list.GroupBy(s => s.ExameCourse))
            {
                GridHtml += "<h1 style='border-bottom:1px solid #000; font-family: 'Iran-Sans';font-weight:bold;'>" + course.Key.Name;
                GridHtml += "</h1>";
                foreach (var item in course.ToList())
                {
                    GridHtml += "<p></p>";
                       GridHtml += "<p style='border-bottom:1px solid #7a7a7a; font-family: 'Iran-Sans';font-weight:bold;'>";
                    if (item.Formula != "" && item.Formula != null)
                    {
                        GridHtml += "<p>" + item.Formula + "</p>";
                    }
                    else
                    {
                        GridHtml += "<p>" + @item.Text + "</p>";
                    }
                    GridHtml += "<p>";
                    foreach (var answer in item.ExameAnswerItem.Where(s => s.Active != false).ToList())
                    {
                        if (item.AnswerId == answer.ID)
                        {
                            GridHtml += "<INPUT TYPE='radio' CHECKED/>";

                        }
                        else
                        {
                            GridHtml += "<INPUT TYPE='radio' />";

                        }
                        if (answer.Formula != null && answer.Formula != "")
                        {
                            GridHtml += "<span>" + answer.Formula + "</span>";
                        }
                        else
                        {
                            GridHtml += "<span>" + answer.Text + "</span>";
                        }

                    }
                    GridHtml += "</p></p>";

                }
            }

            GridHtml += "</table></tr></td>";
            // read parameters from the webpage
            string htmlString = GridHtml;
            string baseUrl ="";
            int webPageWidth = 1024;
            int webPageHeight = 0;
            // instantiate a html to pdf converter object
            HtmlToPdf converter = new HtmlToPdf();
            // set converter options
            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.PdfPageOrientation= PdfPageOrientation.Portrait;
            converter.Options.WebPageWidth = webPageWidth;
            converter.Options.WebPageHeight = webPageHeight;
            // create a new pdf document converting an url
            PdfDocument doc = converter.ConvertHtmlString(htmlString, baseUrl);
            // save pdf document
            byte[] pdf = doc.Save();
            // close pdf document
            doc.Close();
            // return resulted pdf document
            FileResult fileResult = new FileContentResult(pdf, "application/pdf");
            fileResult.FileDownloadName = "Document.pdf";
            return fileResult;

        }
        public ActionResult Answer(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account);
            Exame exame = _context.Exame.GetById(id);

            if (exame != null)
            {
                ViewBag.Exame = exame;
                return View(_context.ExameQuestion.Where(s => s.ExameId == id).ToList());
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public ActionResult Answer(List<ViewExameAnswer> list)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account);
            try
            {
                ViewMessage error = new ViewMessage() { Type = Enum_MessageType.SUCCESS };
                foreach (var item in list)
                {
                    var question = _context.ExameQuestion.GetById(item.QuestionId);
                    if (question != null && item.AnswerId != null)
                    {

                        question.AnswerId = item.AnswerId;
                        question.OrderBy = Guid.NewGuid();
                        _context.ExameQuestion.Update(question);
                        _context.Save();
                    }

                }
                return new JsonResult() { Data = error };
            }
            catch (Exception ex)
            {
                throw;

            }

            //return RedirectToAction("Index", new { id = ExameId });

        }

        public ActionResult Create(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account_New);
            Exame exame = _context.Exame.GetById(id);
            if (exame != null)
            {
                ViewBag.Exame = exame;
                FillDropDowns(null);
                ExameQuestion model = new ExameQuestion();
                model.ExameCourseCategoryId = 0;
                model.ExameCourseId = 0;
                return View(model);
            }
            else
            {
                return RedirectToAction("Index");
            }

        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [System.Web.Mvc.ValidateInput(false)]

        public ActionResult Create(ExameQuestion ExameQuestion, HttpPostedFileBase file)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account_New);

            if (IsModelValid(ExameQuestion))
            {
                if (file != null)
                {
                    Picture picture = _context.Picture.CreateAndUpload(Enum_Code.SYSTEM_TYPE_PANEL, file);
                    ExameQuestion.PictureId = picture.ID;
                }


                _context.ExameQuestion.Insert(ExameQuestion);
                _context.Save();
                return RedirectToAction("Index", new { id = ExameQuestion.ExameId });
            }

            return View(ExameQuestion);
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ExameQuestion ExameQuestion = _context.ExameQuestion.GetById(id);
            if (ExameQuestion == null)
                return HttpNotFound();
            FillDropDowns(ExameQuestion);

            ViewBag.Message = BaseMessage.GetMessage();
            return View(ExameQuestion);
        }
        private void FillDropDowns(ExameQuestion ExameQuestion)
        {

            ViewBag.ExameCourseId = new SelectList(_context.ExameCourse.GetAll().Where(s=>s.Deleted!=true).ToList(), "ID", "Name", ExameQuestion != null ? ExameQuestion.ExameCourseId : 0);

        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [System.Web.Mvc.ValidateInput(false)]

        public ActionResult Edit(ExameQuestion ExameQuestion, HttpPostedFileBase file)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account_Edit);
            if (IsModelValid(ExameQuestion))
            {

                if (file != null)
                {
                    Picture picture = _context.Picture.CreateAndUpload(Enum_Code.SYSTEM_TYPE_PANEL, file);
                    ExameQuestion.PictureId = picture.ID;
                }

                _context.ExameQuestion.Update(ExameQuestion);
                _context.Save();
                return RedirectToAction("Index", new { id = ExameQuestion.ExameId });
            }
            return View(ExameQuestion);
        }

        private bool IsModelValid(ExameQuestion ExameQuestion)
        {
            bool result = false;
            if (ExameQuestion.Text == null)
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
            ExameQuestion ExameQuestion = _context.ExameQuestion.GetById(id);
            if (ExameQuestion == null)
                return HttpNotFound();
            ViewBag.ExameId = ExameQuestion.ExameId;

            return View(ExameQuestion);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account_Delete);
            ExameQuestion ExameQuestion = _context.ExameQuestion.GetById(id);
            ViewBag.ExameId = ExameQuestion.ExameId;
            try
            {
                _context.ExameQuestion.Delete(ExameQuestion);
                _context.Save();
                return RedirectToAction("Index", new { id = ViewBag.ExameId });

            }
            catch
            {
                ExameQuestion = _context.ExameQuestion.GetById(id);
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                ViewBag.ExameId = ExameQuestion.ExameId;
                return View(ExameQuestion);
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