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
    public class ClubsController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(DataLayer.Enumarables.Enum_Code typeId)
        {
            ViewBag.TypeId = typeId;
            List<Clubs> listOrder = _context.Clubs.Where(s => s.Code.Label == typeId.ToString())
                .ToList();
            return View(listOrder);
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

            Clubs clubs = _context.Clubs.GetById(id);
            if (clubs == null)
                return HttpNotFound();
            ViewBag.Message = BaseMessage.GetMessage();
            return View(clubs);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Clubs clubs)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account_Edit);

            var temp = _context.Clubs.GetById(clubs.ID);
            temp.Active = clubs.Active;
            temp.ShowNumbr = clubs.ShowNumbr;
            temp.IsTop = clubs.IsTop;
            temp.IsTopMap = clubs.IsTopMap;
            temp.ShowMobile= clubs.ShowMobile;
            temp.ShowInstagram = clubs.ShowInstagram;

            temp.ShowCertificate2 = clubs.ShowCertificate2;
            temp.ShowName = clubs.ShowName;
            temp.ShowBio = clubs.ShowBio;
            temp.ShowImage2 = clubs.ShowImage2;
            temp.ShowState = clubs.ShowState;

            _context.Clubs.Update(temp);
            _context.Save();
            if(temp.Code.Label== DataLayer.Enumarables.Enum_Code.FIELD_CLUB_ATHENTES.ToString())
            {
                return RedirectToAction("Index", new { typeId = DataLayer.Enumarables.Enum_Code.FIELD_CLUB_ATHENTES });

            }
            else if (temp.Code.Label == DataLayer.Enumarables.Enum_Code.FIELD_CLUB_TEACHERS.ToString())
            {
                return RedirectToAction("Index", new { typeId = DataLayer.Enumarables.Enum_Code.FIELD_CLUB_TEACHERS });

            }
            else
            {
                return RedirectToAction("Index", new { typeId = DataLayer.Enumarables.Enum_Code.FIELD_CLUB_OWNER});

            }
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

            else if (account.Email == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_Message.REQUIRED_ACCOUNT_EMAIL);
            else if (BaseSecurity.IsValidInput(account.Email, Enum_Validation.EMAIL) == false)
                ViewBag.Message = BaseMessage.GetMessage(Enum_Message.INVALID_ACCOUNT_EMAIL);
            else if ((_context.Account.CheckEmailAndMobile(account, false)) == false)
                ViewBag.Message = BaseMessage.GetMessage(Enum_Message.DUPLICATED_ACCOUNT_EMAIL);
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
            string toDate = null, string NationalCode = null, int? productId = null, int? typeId = null)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account);
            int size = 99999;
            int index = 1;
            DateTime fromDatetime = string.IsNullOrEmpty(fromDate) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(fromDate))).GetValueOrDefault();
            DateTime toDatetime = string.IsNullOrEmpty(toDate) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(toDate))).GetValueOrDefault();


            int total = _context.Account.SearchCount(
                    fullName: fullName,
                    fromDatetime: fromDatetime,
                    toDatetime: toDatetime,
                    NationalCode: NationalCode,
                    productId: productId);

            List<Account> list = _context.Account.Search(
  fullName: fullName,
                    fromDatetime: fromDatetime,
                    NationalCode: NationalCode,
                    productId: productId,
                    toDatetime: toDatetime, pageSize: size, index: index)
                ;
            DataTable dt = new DataTable("sheet1");
            dt.Columns.AddRange(new DataColumn[7] { new DataColumn("#"),
                                                     new DataColumn("نام و نام خانوادگی"),
                                                     new DataColumn("موبایل"),
                                                     new DataColumn("ایمیل"),
                                                     new DataColumn("کد ملی"),

                                                     new DataColumn("آدرس"),

                                                     new DataColumn("تاریخ ثبت نام")
                                                  });
            for (int i = 0; i < list.Count(); i++)
            {
                var item = list[i];

                dt.Rows.Add(i + 1, item.FullName, item.Mobile, item.Email, item.NationalCode, typeId == 3 ? item.BookPayment.FirstOrDefault(s => s.BookId == productId).Address : item.Address, item.CreateDatetime.ToPersian());
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