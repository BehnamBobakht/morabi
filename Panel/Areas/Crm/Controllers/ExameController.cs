using ClosedXML.Excel;
using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Panel.Areas.Crm.Controllers
{
    public class ExameController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account);
            return View(_context.Exame.GetAll());
        }

        public ActionResult Users(int exameId, bool? IsPass = null, string pageIndex = null, string pageSize = null, string name = null)
        {
            ViewBag.ExameId = exameId;
            ViewBag.IsPass = IsPass;
            int size = pageSize == null ? 50 : pageSize.GetInteger();
            int index = pageIndex == null ? 1 : pageIndex.GetInteger();
            ViewBag.PageIndex = index.ToString();
            ViewBag.PageSize = size.ToString();
            ViewBag.Name = name;
            ViewBag.TotalCount = _context.ExameAccountAnswer.SearchCount(exameId: exameId, IsPass: IsPass, name: name);
            var list = _context.ExameAccountAnswer.Search(exameId: exameId, IsPass: IsPass, index: index, pageSize: size, name: name);
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account);
            return View(list);
        }
        public void ExelExport(int exameId, bool? IsPass = null)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account);

            int total = _context.ExameAccountAnswer.SearchCount(exameId: exameId, IsPass: IsPass);

            var list = _context.ExameAccountAnswer.Search(exameId: exameId, IsPass: IsPass);
            int row = 1;
            var excelPackage = new ExcelPackage();

            var sheet = excelPackage.Workbook.Worksheets.Add(Resource.Lang.Customerlist + DateTime.Now.ToString("yyyyMMddhhmmss"));
            int i = 4;
            sheet.Cells[row, 1].Value = "ردیف";
            sheet.Cells[row, 2].Value = "نام و نام خانوادگی";

            sheet.Cells[row, 3].Value = "کد ملی";
            var course = list.FirstOrDefault().ExameReportByCourse;
            foreach (var item in course)
            {
                sheet.Cells[row, i].Value = item.ExameCourse.Name;
                i++;
            }
            sheet.Cells[row, i].Value = "معدل";
            i++;
            sheet.Cells[row, i].Value = "وضعیت";

            int j = 1;
            row++;
            foreach (var item in list)
            {
                double? sum = (double)item.ExameReportByCourse.Sum(s => s.TotalRate) / item.ExameReportByCourse.Count();

                i = 4;
                sheet.Cells[row, 1].Value = j;

                sheet.Cells[row, 2].Value = item.Account.FullName + " " + item.Account.LatName;
                sheet.Cells[row, 3].Value = item.Account.NationalCode;

                foreach (var filed in item.ExameReportByCourse)
                {
                    if (filed.TotalRate != null)
                    {
                        sheet.Cells[row, i].Value = @Math.Round(filed.TotalRate.Value, 1).ToString().Replace("/", ".");
                        i++;
                    }
                    else
                    {
                        sheet.Cells[row, i].Value = "0";
                        i++;
                    }
                  
                }
                if (sum != null)
                {
                    sheet.Cells[row, i].Value = @Math.Round(sum.Value, 1).ToString().Replace("/", ".");
                    i++;
                }
                else
                {
                    sheet.Cells[row, i].Value = "0";
                    i++;
                }
                if (sum >= item.Exame.PassValue)
                {
                    sheet.Cells[row, i].Value = "قبول";

                }
                else
                {
                    sheet.Cells[row, i].Value = "مردود";
                }

                row++;
                j++;

            }


            sheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AppendHeader("Content-Disposition", "attachment; filename=Customerlist" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xlsx");
            Response.BinaryWrite(excelPackage.GetAsByteArray());
            Response.End();
        }

        public ActionResult AnswerUser(int Id)
        {
            var exameAccountAnswer = _context.ExameAccountAnswer.GetById(Id);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account);
            return View(exameAccountAnswer);
        }
        public ActionResult DeleteExameReportByCourse(int Id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account);

            var exameAccountAnswer = _context.ExameAccountAnswer.GetById(Id);
            int? exameId = exameAccountAnswer.ExameId;
            var ReportByCourse = exameAccountAnswer.ExameReportByCourse.ToList();
            foreach (var item in ReportByCourse)
            {
                _context.ExameReportByCourse.Delete(item);
                _context.Save();
            }
            var AnswerDetails = exameAccountAnswer.ExameAccountAnswerDetails.ToList();
            foreach (var item in AnswerDetails)
            {
                _context.ExameAccountAnswerDetails.Delete(item);
                _context.Save();
            }
            var temp = _context.ExameAccountAnswer.GetById(Id);
            var PaymentExame = exameAccountAnswer.PaymentExame.ToList();
            foreach (var item in PaymentExame)
            {
                var list = _context.PaymentExameCourseId.Where(s => s.PaymentExameId == item.ID);
                foreach (var cc in list)
                {
                    _context.PaymentExameCourseId.Delete(cc);

                }
                _context.Save();
                _context.PaymentExame.Delete(item);
                _context.Save();
            }
            _context.ExameAccountAnswer.Delete(exameAccountAnswer);
            _context.Save();
            return RedirectToAction("Users", new { exameId = exameId });
        }
        public ActionResult FinishExame1(int Id)
        {
            var exameAnswer = _context.ExameAccountAnswer.Where(s => s.ExameId == Id && s.Finish != true && s.TotalRate == null);
            foreach (var answer in exameAnswer)
            {


                var temp = _context.ExameAccountAnswer.GetById(answer.ID);
                temp.CheckResult = true;
                temp.Finish = true;
                temp.TotalRate = null;

                _context.ExameAccountAnswer.Update(temp);

            }
            _context.Save();
            return RedirectToAction("Users", new { exameId = Id });

        }
        public ActionResult FinishExame2(int Id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account);
            var Model = _context.Exame.GetById(Id);
            List<int> ids = new List<int>();
            string idsquestion = "-";
            foreach (var course in Model.ExameQuestion.Where(s => s.Active != false).GroupBy(s => s.ExameCourse))
            {
                foreach (var item in course.ToList())
                {

                }
                ids.Add(course.Key.ID);
                idsquestion += course.Key.ID.ToString() + "-";
            }

            var exameAnswer = _context.ExameAccountAnswer.Where(s => s.ExameId == Id && s.CheckResult == true);
            foreach (var answer in exameAnswer)
            {

                var exameAccountAnswer = _context.ExameAccountAnswer.GetById(answer.ID);
                var ReportByCourse = exameAccountAnswer.ExameReportByCourse.ToList();
                foreach (var item in ids)
                {
                    if (!ReportByCourse.Any(s => s.CourseId == item))
                    {
                        ExameReportByCourse report = new ExameReportByCourse();
                        report.AnswerId = answer.ID;
                        report.ExameAnswerId = answer.ID;

                        report.CourseId = item;
                        report.CountNull = 20;
                        report.CountTrue = 0;
                        report.CountFalse = 0;
                        report.TotalRate = 0;
                        _context.ExameReportByCourse.Insert(report);

                    }

                }

                var temp = _context.ExameAccountAnswer.GetById(answer.ID);
                temp.QuestionAnswer = idsquestion;
                _context.ExameAccountAnswer.Update(temp);

            }
            _context.Save();
            _context.Save();


            return RedirectToAction("Users", new { exameId = Id });
        }
        public ActionResult FinishExame3(int Id, int accountId)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account);
            var Model = _context.Exame.GetById(Id);
            List<int> ids = new List<int>();
            string idsquestion = "-";
            foreach (var course in Model.ExameQuestion.Where(s => s.Active != false).GroupBy(s => s.ExameCourse))
            {
                foreach (var item in course.ToList())
                {

                }
                ids.Add(course.Key.ID);
                idsquestion += course.Key.ID.ToString() + "-";
            }

            var exameAnswer = _context.ExameAccountAnswer.Where(s => s.ExameId == Id && s.AccountId == accountId);
            foreach (var answer in exameAnswer)
            {

                var exameAccountAnswer = _context.ExameAccountAnswer.GetById(answer.ID);
                var ReportByCourse = exameAccountAnswer.ExameReportByCourse.ToList();
                foreach (var item in ids)
                {
                    if (!ReportByCourse.Any(s => s.CourseId == item))
                    {
                        ExameReportByCourse report = new ExameReportByCourse();
                        report.AnswerId = answer.ID;
                        report.ExameAnswerId = answer.ID;

                        report.CourseId = item;
                        report.CountNull = 20;
                        report.CountTrue = 0;
                        report.CountFalse = 0;
                        report.TotalRate = 0;
                        _context.ExameReportByCourse.Insert(report);

                    }

                }

                var temp = _context.ExameAccountAnswer.GetById(answer.ID);
                temp.QuestionAnswer = idsquestion;
                _context.ExameAccountAnswer.Update(temp);

            }
            _context.Save();
            _context.Save();


            return RedirectToAction("Users", new { exameId = Id });
        }
        public ActionResult DeleteExameAnswerAll2(int Id)
        {
            ViewBag.ExameId = Id;
            return View();
        }
        public ActionResult DeleteExameAnswerAll(int Id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account);
            var exame = _context.Exame.GetById(Id);
            foreach (var exameAccountAnswer in exame.ExameAccountAnswer.ToList())
            {

                int? exameId = exameAccountAnswer.ExameId;
                var ReportByCourse = exameAccountAnswer.ExameReportByCourse.ToList();
                foreach (var item in ReportByCourse)
                {
                    _context.ExameReportByCourse.Delete(item);
                    _context.Save();
                }
                var AnswerDetails = exameAccountAnswer.ExameAccountAnswerDetails.ToList();
                foreach (var item in AnswerDetails)
                {
                    _context.ExameAccountAnswerDetails.Delete(item);
                    _context.Save();
                }
                _context.ExameAccountAnswer.Delete(exameAccountAnswer);
                _context.Save();
            }

            return RedirectToAction("Users", new { exameId = Id });
        }
        [HttpGet]
        public ActionResult ExameReportByCourse(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account);
            ViewMessage error = new ViewMessage() { Type = Enum_MessageType.SUCCESS };
            var answer = _context.ExameAccountAnswer.GetById(id);
            ViewBag.Exame = _context.Exame.FirstOrDefault(s => s.ID == answer.ExameId);
            var ReportByCourse = _context.ExameReportByCourse.Where(s=>s.AnswerId==id).ToList();
            return View(ReportByCourse);
        }
        [HttpPost]
        public ActionResult ExameReportByCourse(List<ExameReportByCourse> model)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account);
            ViewMessage error = new ViewMessage() { Type = Enum_MessageType.SUCCESS };
            foreach (var item in model)
            {
                var ReportByCourse = _context.ExameReportByCourse.GetById(item.ID);
                if (ReportByCourse != null)
                {

                    ReportByCourse.TotalRate = item.TotalRate;
                    _context.ExameReportByCourse.Update(ReportByCourse);
                    _context.Save();
                }

            }
            return new JsonResult() { Data = error };
        }
        public ActionResult Copy(int exameId)
        {

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account);
            var exame = _context.Exame.GetById(exameId);
            if (exame != null)
            {
                var listQuestion = exame.ExameQuestion.ToList();

                Exame tempExam = new Exame();
                tempExam.Name = exame.Name;
                tempExam.DateString = exame.DateString;
                tempExam.ShowUser = exame.ShowUser;
                tempExam.PassValue = exame.PassValue;
                tempExam.Active = exame.Active;
                tempExam.ProductId = exame.ProductId;
                tempExam.Time = exame.Time;
                tempExam.StartDate = exame.StartDate;
                tempExam.TiteString = exame.TiteString;
                _context.Exame.Insert(tempExam);
                _context.Save();
                foreach (var item in listQuestion)
                {
                    var answerList = item.ExameAnswerItem;
                    ExameQuestion tempQuestion = new ExameQuestion();
                    tempQuestion.ExameCourseId = item.ExameCourseId;
                    tempQuestion.Text = item.Text;
                    tempQuestion.ExameId = tempExam.ID;
                    //tempQuestion.AnswerId = item.AnswerId;
                    //var answertex = _context.ExameAnswerItem.GetById(item.AnswerId).Text;
                    tempQuestion.Active = item.Active;
                    _context.ExameQuestion.Insert(tempQuestion);
                    _context.Save();
                    foreach (var answer in answerList)
                    {
                        ExameAnswerItem tempAnswer = new ExameAnswerItem();
                        tempAnswer.Active = answer.Active;
                        tempAnswer.ExameQuestionId = tempQuestion.ID;
                        tempAnswer.Text = answer.Text;
                        _context.ExameAnswerItem.Insert(tempAnswer);
                        _context.Save();

                    }
                    try
                    {
                        if (_context.ExameAnswerItem.GetById(item.AnswerId) != null)
                        {
                            var answertex = _context.ExameAnswerItem.GetById(item.AnswerId).Text;

                            var answer2 = _context.ExameAnswerItem.FirstOrDefault(s => s.ExameQuestionId == tempQuestion.ID && s.Text == answertex);
                            tempQuestion.AnswerId = answer2.ID;
                            _context.ExameQuestion.Update(tempQuestion);
                            _context.Save();
                        }
                      
                    }
                    catch
                    {

                    }
                }
            }
            return RedirectToAction("Index");
        }
        public ActionResult PrintReport(int exameId, bool? IsPass = null,int ? userId=null)
        {
            ViewBag.ExameId = exameId;
            ViewBag.IsPass = IsPass;

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account);
            return View(_context.ExameAccountAnswer.Search(exameId: exameId, IsPass: IsPass, userId: userId));
        }
        private void FillDropDowns(Exame exame)
        {

            ViewBag.ProductId = new SelectList(_context.Product.Where(s => s.ProductTypeId == 1001 || s.ProductTypeId == 2).ToList(), "ID", "Name", exame != null ? exame.ProductId : 0);

        }
        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account_New);
            FillDropDowns(null);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Exame exame)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account_New);

            if (IsModelValid(exame))
            {

                _context.Exame.Insert(exame);
                _context.Save();
                return RedirectToAction("Index");
            }

            return View(exame);
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Exame exame = _context.Exame.GetById(id);
            if (exame == null)
                return HttpNotFound();
            FillDropDowns(exame);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(exame);
        }
        public ActionResult Details(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Exame exame = _context.Exame.GetById(id);
            if (exame == null)
                return HttpNotFound();
            FillDropDowns(exame);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(exame);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(Exame exame)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account_Edit);
            if (IsModelValid(exame))
            {

                _context.Exame.Update(exame);
                _context.Save();
                return RedirectToAction("Index");
            }
            return View(exame);
        }

        private bool IsModelValid(Exame exame)
        {
            bool result = false;
            if (exame.Name == null)
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

            Exame exame = _context.Exame.GetById(id);
            if (exame == null)
                return HttpNotFound();

            return View(exame);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account_Delete);
            Exame exame = _context.Exame.GetById(id);
            try
            {
                var listAccountAnswer = _context.ExameAccountAnswer.Where(s => s.ExameId == id);
                foreach (var item in listAccountAnswer)
                {
                    var ExameAccountAnswerDetails = _context.ExameAccountAnswerDetails.Where(s => s.ExameAnswerId == item.ID);

                    var ExameReportByCourse = _context.ExameReportByCourse.Where(s => s.AnswerId == item.ID);
                    foreach (var sunItem in ExameAccountAnswerDetails)
                    {
                        _context.ExameAccountAnswerDetails.Delete(sunItem);

                    }
                    foreach (var sunItem in ExameReportByCourse)
                    {
                        _context.ExameReportByCourse.Delete(sunItem);

                    }
                    _context.ExameAccountAnswer.Delete(item);

                }
                _context.Save();
                var listExameQuestion = _context.ExameQuestion.Where(s => s.ExameId == id);
                foreach (var item in listExameQuestion)
                {
                    var ExameAnswerItem = _context.ExameAnswerItem.Where(s => s.ExameQuestionId == item.ID);
                    foreach (var sunItem in ExameAnswerItem)
                    {
                        _context.ExameAnswerItem.Delete(sunItem);

                    }

                    _context.ExameQuestion.Delete(item);

                }
                _context.Save();
                _context.Exame.Delete(exame);
                _context.Save();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(exame);
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