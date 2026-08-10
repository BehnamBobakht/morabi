using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data;
using ClosedXML.Excel;
using System.IO;
using DataLayer.ViewModels;

namespace Panel.Areas.Crm.Controllers
{
    public class AccountController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(string fullName = null, string NationalCode = null, string fromDate = null,
            string toDate = null, string pageIndex = null, string Mobile = null,
           string pageSize = null,
           int? productId = null, int? typeId = null)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account);
            int size = pageSize == null ? 50 : pageSize.GetInteger();
            int index = pageIndex == null ? 1 : pageIndex.GetInteger();
            DateTime from = string.IsNullOrEmpty(fromDate) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(fromDate))).GetValueOrDefault();
            DateTime to = string.IsNullOrEmpty(toDate) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(toDate))).GetValueOrDefault();
            ViewBag.FromDatetime = fromDate;
            ViewBag.ToDatetime = toDate;
            ViewBag.typeId = typeId;

            ViewBag.PageIndex = index.ToString();
            ViewBag.PageSize = size.ToString();
            ViewBag.fullName = fullName;
            ViewBag.NationalCode = NationalCode;
            ViewBag.Mobile = Mobile;

            ViewBag.productId = productId;
            ViewBag.TotalCount = _context.Account.SearchCount(
                   fullName: fullName,
                   NationalCode: NationalCode,
                    fromDatetime: from,
                    Mobile: Mobile,
                    toDatetime: to,
                    productId: productId
                    );

            List<Account> listOrder = _context.Account.Search(
                    fullName: fullName,
                     fromDatetime: from,
                     NationalCode: NationalCode,
                    toDatetime: to,
                     Mobile: Mobile,
                    productId: productId,
                    pageSize: size,
                    index: index
                  )
                .ToList();
            var list = listOrder.ToView();
            if (productId != null)
            {
                foreach (var item in list)
                {
                    var order = _context.AccountOrderProduct.Where(p => (p.ProductId == productId || p.Product.ProductsId.Contains(productId.ToString())) && (p.AccountOrder.StatusId == 2068 || p.AccountOrder.StatusId == 2073) && p.AccountOrder.AccountId == item.Id);
                    string price = "0";
                    foreach (var sub in order)
                    {
                        price += sub.Price.ToString() + "-";
                    }
                    item.Price = price;
                }

            }
            return View(list);
            //return View(_context.Account.Where(s=>s.Deleted==false).ToList().ToView());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account_Details);
            Account account = _context.Account.GetById(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account_New);
            FillDropDowns(null);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Account account)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account_New);

            if (IsModelValid(account))
            {
                _context.Account.Insert(account);
                _context.Save();
                return RedirectToAction("Index");
            }

            FillDropDowns(account);
            return View(account);
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Account account = _context.Account.GetById(id);
            if (account == null)
                return HttpNotFound();

            FillDropDowns(account);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(account);
        }
        public ActionResult Exame(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var list = _context.ExameAccountAnswer.Where(s => s.AccountId == id).ToList();
            ViewBag.Message = BaseMessage.GetMessage();
            return View(list);
        }
        public ActionResult Course(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            List<AccountOrderProduct> results = _context.AccountOrderProduct.Search(
                 accountId: id,
                 status: Enum_Code.ORDER_STATUS_NONE, typeId: 2, statusId2: "2068-2073");
            List<Students> results2 = _context.Students.Where(s => s.UserId == id).ToList();
            List<Product> list = new List<Product>();
            foreach (var item in results)
            {
                list.Add(item.Product);
            }
            foreach (var item in results2)
            {
                list.Add(item.Product);
            }
            list = list.Distinct().ToList();
            return View(list);
        }
        public ActionResult Report(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var exameAnswer = _context.ExameAccountAnswer.FirstOrDefault(s => s.ID == id);
            ViewBag.Message = BaseMessage.GetMessage();
            var exameAnswerLast = _context.ExameAccountAnswer.Where(s => s.AccountId == exameAnswer.AccountId).OrderByDescending(s => s.ID).Take(10);
            ViewReport model = new ViewReport();
            try
            {


                int i = 1;
                List<ViewReportByLastAvrage> listreport = new List<ViewReportByLastAvrage>();
                foreach (var item in exameAnswerLast)
                {
                    if (i == 2)
                    {
                        model.TotalLastLevel = item.TotalLevel;
                    }
                    ViewReportByLastAvrage report = new ViewReportByLastAvrage();
                    report.X = item.Date.ToPersian();
                    report.Y = item.TotalLevel;
                    report.Id = item.ID;
                    listreport.Add(report);
                    i++;
                }

                model.LastAvrageLevel = listreport.OrderBy(s => s.Id).ToList();
                model.AvrageLevelCountry = _context.ExameAccountAnswer.Where(s => s.ExameId == exameAnswer.ExameId).Average(s => s.TotalLevel);
                //var topAvrageLevelCountry = _context.ExameAccountAnswer.Where(s => s.ExameId == exameAnswer.ExameId).OrderByDescending(s => s.TotalLevel.Value);
                //if (topAvrageLevelCountry.FirstOrDefault() != null)
                //{
                //    model.TopAvrageLevelCountry = topAvrageLevelCountry.FirstOrDefault().TotalLevel;
                //}
                ViewReportModel accountAnswerMode = new ViewReportModel();
                accountAnswerMode.Exame = exameAnswer.Exame.Name;
                accountAnswerMode.AccountId = exameAnswer.AccountId;
                accountAnswerMode.FullName = exameAnswer.Account.FullName;

                accountAnswerMode.NationalCode = exameAnswer.Account.NationalCode;
                accountAnswerMode.TotalRate = exameAnswer.TotalRate;
                accountAnswerMode.TotalRateWithoutFalse = exameAnswer.TotalRateWithoutFalse;
                accountAnswerMode.TotalRatePercent = exameAnswer.TotalRatePercent;
                accountAnswerMode.TotalRateInCountry = exameAnswer.TotalRateInCountry;
                accountAnswerMode.TotalRateInCity = exameAnswer.TotalRateInCity;
                accountAnswerMode.CountQuestion = exameAnswer.Exame.ExameQuestion.Count();
                accountAnswerMode.CountTrue = exameAnswer.CountTrue;
                accountAnswerMode.CountFalse = exameAnswer.CountFalse;
                accountAnswerMode.CountNull = exameAnswer.CountNull;
                accountAnswerMode.TotalLevel = exameAnswer.TotalLevel;

                accountAnswerMode.Date = exameAnswer.Date.ToPersian();

                List<ViewExameAccountAnswerDetails> listAnswerDetails = new List<ViewExameAccountAnswerDetails>();
                foreach (var item in exameAnswer.ExameAccountAnswerDetails.OrderBy(s => s.ID))
                {
                    ViewExameAccountAnswerDetails AnswerDetails = new ViewExameAccountAnswerDetails();
                    //AnswerDetails.CategoryName = item.ExameQuestion.ExameCourseCategory.Name;
                    int h = 1;
                    foreach (var ExameAnswerItemitem in item.ExameQuestion.ExameAnswerItem.Where(s => s.Active != false).ToList())
                    {

                        if (ExameAnswerItemitem.ID == item.AnswerId)
                        {
                            AnswerDetails.QuestionId = h;
                        }
                        if (ExameAnswerItemitem.ID == item.ExameQuestion.AnswerId)
                        {
                            AnswerDetails.TrueId = h;
                        }
                        h++;
                    }
                    AnswerDetails.IsTrue = item.IsTrue;
                    listAnswerDetails.Add(AnswerDetails);
                }
                accountAnswerMode.AccountAnswerDetails = listAnswerDetails;
                List<ViewExameReportByCourse> listExameReportByCourse = new List<ViewExameReportByCourse>();
                foreach (var item in exameAnswer.ExameReportByCourse.Where(s => s.AnswerId == exameAnswer.ID).OrderBy(s => s.ID))
                {
                    ViewExameReportByCourse ExameReportByCourse = new ViewExameReportByCourse();
                    ExameReportByCourse.Name = item.ExameCourse.Name;
                    ExameReportByCourse.Type = item.ExameCourse.Code.Name;
                    ExameReportByCourse.CountTrue = item.CountTrue;
                    ExameReportByCourse.CountNull = item.CountNull;
                    ExameReportByCourse.CountFalse = item.CountFalse;
                    ExameReportByCourse.TotalLevel = item.TotalLevel;

                    ExameReportByCourse.RatePercent = item.RatePercent;
                    ExameReportByCourse.TotalRateWithoutFalse = item.TotalRateWithoutFalse;
                    ExameReportByCourse.RateInCountry = item.RateInCountry;

                    listExameReportByCourse.Add(ExameReportByCourse);
                }
                accountAnswerMode.ExameReportByCourse = listExameReportByCourse;

                model.ExameAccountAnswer = accountAnswerMode;
                model.ExameReportByCourseGeneral = exameAnswer.ExameReportByCourse.Where(s => s.ExameCourse.Code.Label == Enum_Code.FIELD_COURSE_General.ToString()).ToList();
                model.ExameReportByCourseProfessional = exameAnswer.ExameReportByCourse.Where(s => s.ExameCourse.Code.Label == Enum_Code.FIELD_COURSE_Professional.ToString()).ToList();
                model.TotalCountG = model.ExameReportByCourseGeneral.Sum(s => s.CountTrue) + model.ExameReportByCourseGeneral.Sum(s => s.CountFalse) + model.ExameReportByCourseGeneral.Sum(s => s.CountNull);
                model.TotalCountP = model.ExameReportByCourseProfessional.Sum(s => s.CountTrue) + model.ExameReportByCourseProfessional.Sum(s => s.CountFalse) + model.ExameReportByCourseProfessional.Sum(s => s.CountNull);
                model.RateInCountryG = double.Parse(model.ExameReportByCourseGeneral.Average(s => s.RateInCountry).ToString());
                model.RateInCountryP = double.Parse(model.ExameReportByCourseProfessional.Average(s => s.RateInCountry).ToString());
                var listCat = _context.ExameCourseCategory.Where(s => s.Active != false);
                List<ViewReportByCategory> ReportByCategory = new List<ViewReportByCategory>();
                foreach (var item in exameAnswer.ExameReportByCourse.ToList())
                {

                    foreach (var cat in item.ExameCourse.ExameCourseCategory)
                    {
                        ViewReportByCategory category = new ViewReportByCategory();
                        category.Name = cat.Name;
                        category.TotalCount = cat.ExameQuestion.Where(s => s.Active != false).Count();
                        category.CountFalse = item.ExameAccountAnswer.ExameAccountAnswerDetails.Where(s => s.ExameQuestion.ExameCourseCategoryId == cat.ID && s.IsTrue == false).Count();
                        category.CountTrue = item.ExameAccountAnswer.ExameAccountAnswerDetails.Where(s => s.ExameQuestion.ExameCourseCategoryId == cat.ID && s.IsTrue == true).Count();
                        category.CountNull = item.ExameAccountAnswer.ExameAccountAnswerDetails.Where(s => s.ExameQuestion.ExameCourseCategoryId == cat.ID && s.IsTrue == null).Count();
                        ReportByCategory.Add(category);

                    }

                }
                model.ReportByCategory = ReportByCategory;
            }
            catch
            {

            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Account account)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account_Edit);
            if (!string.IsNullOrEmpty(account.UserName))
            {
                account.UserName = account.UserName.Trim().ToLower();
            }
            else
            {
                account.UserName = "";
            }

            if (IsModelValid(account, true))
            {

                var temp = _context.Account.GetById(account.ID);
                if (!string.IsNullOrEmpty(account.UserName))
                {
                    account.UserName = account.UserName.Trim().Replace(" ", "");
                    if (_context.Account.UserNameNotDuplicate(temp, account.UserName))
                    {
                        temp.UserName = account.UserName;
                    }
                    else
                    {
                        ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.DUPLICATED_ACCOUNT_USERNAME);
                        FillDropDowns(account);
                        return View(account);
                    }
                }
                temp.TypeId = account.TypeId;
                temp.FullName = account.FullName;
                temp.FullNameEn = account.FullNameEn;

                temp.LatName = account.LatName;
                temp.LatNameEn = account.LatNameEn;

                temp.ChangeUserName = account.ChangeUserName;
                temp.UpdateDatetime = DateTime.Now;
                temp.NationalCode = account.NationalCode;
                temp.Mobile = account.Mobile;
                temp.Email = account.Email;
                temp.Phone = account.Phone;
                temp.Address = account.Address;
                temp.Job = account.Job;
                temp.IsMale = account.IsMale;
                temp.Description = account.Description;
                temp.SpacialAccount = account.SpacialAccount;


                _context.Account.Update(temp);
                _context.Save();
                return RedirectToAction("Index");
            }
            FillDropDowns(account);
            return View(account);
        }

        public ActionResult Password(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Account account = _context.Account.GetById(id);
            if (account == null)
                return HttpNotFound();

            ViewBag.Message = BaseMessage.GetMessage();
            return View(account);
        }

        [HttpPost]
        public ActionResult Password(int? id, string newPassword, string confirmPassword)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Account account = _context.Account.GetById(id);
            if (account == null)
                return HttpNotFound();

            bool result = false;
            if (newPassword == "")
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PASSWORD);
            else if (confirmPassword == "")
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PASSWORD_CONFIRM);
            else if (newPassword != confirmPassword)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.INVALID_PASSWORD_CONFIRM);
            else
                result = true;

            if (result == true)
            {
                account.Password = BaseSecurity.HashMd5(newPassword);
                account.UpdateDatetime = DateTime.Now;
                _context.Account.Update(account);
                _context.Save();
                return RedirectToAction("Index");
            }

            return View(account);
        }

        private bool IsModelValid(Account account, bool edit = false)
        {
            bool result = false;
            if (account.FullName == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_Message.REQUIRED_ACCOUNT_FULLNAME);
            else if (account.UserName != null && BaseSecurity.IsValidInput(account.UserName, Enum_Validation.USERNAME) == false)
                ViewBag.Message = BaseMessage.GetMessage(Enum_Message.INVALID_USERNAME);

            //else if (account.Email == null)
            //    ViewBag.Message = BaseMessage.GetMessage(Enum_Message.REQUIRED_ACCOUNT_EMAIL);
            //else if (BaseSecurity.IsValidInput(account.Email, Enum_Validation.EMAIL) == false)
            //    ViewBag.Message = BaseMessage.GetMessage(Enum_Message.INVALID_ACCOUNT_EMAIL);
            //else if ((_context.Account.CheckEmailAndMobile(account, false)) == false)
            //    ViewBag.Message = BaseMessage.GetMessage(Enum_Message.DUPLICATED_ACCOUNT_EMAIL);
            else if (account.Mobile == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_Message.REQUIRED_ACCOUNT_MOBILE);
            else if (BaseSecurity.IsValidInput(account.Mobile, Enum_Validation.MOBILE) == false)
                ViewBag.Message = BaseMessage.GetMessage(Enum_Message.INVALID_ACCOUNT_MOBILE);
            else if ((_context.Account.CheckEmailAndMobile(account, true)) == false)
                ViewBag.Message = BaseMessage.GetMessage(Enum_Message.DUPLICATED_ACCOUNT_MOBILE);

            else if (account.Password == null && edit == false)
                ViewBag.Message = BaseMessage.GetMessage(Enum_Message.REQUIRED_ACCOUNT_PASSWORD);

            else
                result = true;
            return result;
        }

        private void FillDropDowns(Account account)
        {
            ViewBag.TypeId = new SelectList(_context.Code.GetAllByCodeGroup(Enum_CodeGroup.ACCOUNT_TYPE), "ID", "Name", account != null ? account.TypeId : 0);
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Account account = _context.Account.GetById(id);
            if (account == null)
                return HttpNotFound();

            return View(account);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account_Delete);
            Account account = _context.Account.GetById(id);
            try
            {
                _context.AccountLog.DeleteByAccountId(account.ID);
                _context.Account.Delete(account);
                _context.Save();
            }
            catch
            {
                account.Deleted = true;
                _context.Account.Update(account);
                _context.Save();
            }

            return RedirectToAction("Index");
        }
        public ActionResult ExelExport(string fullName = null, string fromDate = null,
            string toDate = null, string NationalCode = null, int? productId = null, int? typeId = null, int? size = 99999,
        int? index = 1)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account);

            DateTime fromDatetime = string.IsNullOrEmpty(fromDate) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(fromDate))).GetValueOrDefault();
            DateTime toDatetime = string.IsNullOrEmpty(toDate) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(toDate))).GetValueOrDefault();
            List<Account> list = _context.Account.Search(
  fullName: fullName,
                    fromDatetime: fromDatetime,
                    NationalCode: NationalCode,
                    productId: productId,
                    toDatetime: toDatetime, pageSize: size, index: index)
                ;
            var list2 = list.ToView();

            DataTable dt = new DataTable("sheet1");
            if (productId != null)
            {


                dt.Columns.AddRange(new DataColumn[12] { new DataColumn("#"),
                                                     new DataColumn("نام و نام خانوادگی"),
                                                     new DataColumn("نام لاتین "),

                                                     new DataColumn("موبایل"),
                                                     new DataColumn("ایمیل"),
                                                     new DataColumn("کد ملی"),
                                                     new DataColumn("کد معرف"),

                                                     new DataColumn("آدرس"),

                                                     new DataColumn("تاریخ ثبت نام"),


                                                     new DataColumn("مبالغ پرداختی"),
                                                     new DataColumn("عکس پرسنلی"),
                                                     new DataColumn("پاسخ سوال"),


                                                  });
                List<ViewAccount2> newlist = new List<ViewAccount2>();
                foreach (var item in list2)
                {
                    var order = _context.AccountOrderProduct.Where(p => (p.ProductId == productId || p.Product.ProductsId.Contains(productId.ToString())) && (p.AccountOrder.StatusId == 2068 || p.AccountOrder.StatusId == 2073) && p.AccountOrder.AccountId == item.Id);
                    string price = "";
                    foreach (var sub in order)
                    {
                        price += sub.Price.ToString() + "-";
                        item.Datetime = sub.Datetime;
                    }
                    item.Price = price;

                }

                
                foreach (var item in list2.OrderByDescending(s => s.Datetime))
                {
                    dt.Rows.Add(item.Id, item.FullName + " " + item.LatName, item.FullNameEn + " " + item.LatNameEn, item.Mobile, item.Email, item.NationalCode, "10" + item.Id, item.Address, item.Datetime.ToPersian(), item.Price, item.PictureUrl, item.QuestionAnswer);

                }
            }
            else
            {
                dt.Columns.AddRange(new DataColumn[9] { new DataColumn("#"),
                                                     new DataColumn("نام و نام خانوادگی"),
                                                     new DataColumn("نام لاتین "),
                                                     new DataColumn("موبایل"),
                                                     new DataColumn("ایمیل"),
                                                     new DataColumn("کد ملی"),
                                                     new DataColumn("کد معرف"),

                                                     new DataColumn("آدرس"),

                                                     new DataColumn("تاریخ ثبت نام")
                                                  });
                for (int i = 0; i < list.Count(); i++)
                {
                    var item = list[i];

                    dt.Rows.Add(i + 1, item.FullName + " " + item.LatName, item.FullNameEn + " " + item.LatNameEn, item.Mobile, item.Email, item.NationalCode, "10" + item.ID, typeId == 3 ? item.BookPayment.FirstOrDefault(s => s.BookId == productId).Address : item.Address, item.CreateDatetime.ToPersian());
                }
            }
            string fileName = string.Format("{0} - {1}.xlsx", DateTime.Now.Date.ToShortDateString(), 10);
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