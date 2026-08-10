using DataLayer.Api;
using DataLayer.Api.Model;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.Helpers.Google;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer.ViewModels;
using System.Text;
using System.IO;
using System.Collections;
using System.Drawing.Imaging;
using SelectPdf;

namespace Cms.Controllers
{
    public class AccountController : Controller
    {
        UnitOfWork _context = new UnitOfWork();


        [HttpGet]
        public ActionResult Message()
        {
            if (_context.Account.GetCurrentAccount() == null)
                return RedirectToAction("login", new { controller = "account", back = "profile/message" });

            ViewAccount account = _context.Account.GetCurrentAccount();
            List<ShopChat> list = _context.ShopChat.GetAllUnreadAndAccountId(account.Id);

            return PartialView(BaseController.GetView(this), list.ToView());
        }

        [HttpGet]
        public ActionResult MessageShop(int id)
        {
            ViewAccount account = _context.Account.GetCurrentAccount();
            if (account == null)
                return RedirectToAction("login", new { controller = "account", back = "profile/newaddress/" + id });

            Shop shop = _context.Shop.GetById(id);
            ViewBag.CurrentAccount = account;

            _context.ShopChat.DoReadAllChatByShopForAccount(shop, account.Id);
            _context.Save();

            return PartialView(BaseController.GetView(this), shop);
        }

        [HttpGet]
        public ActionResult PrintCertificate(int id)
        {
            ViewAccount account = _context.Account.GetCurrentAccount();
            if (account == null)
                return RedirectToAction("login", new { controller = "account", back = "profile/newaddress/" + id });

            PaymentCertificate PaymentCertificate = _context.PaymentCertificate.GetById(id);
            // read parameters from the webpage
            string htmlString = @"<html><style>@font-face {
    font-family: 'dana';
    font-style: normal;
	font-weight: 100;
    src: url('https://morabiiran.com/assets/MorabiIran/fonts/YekanBakhFaNum-Regular.ttf');}</style>
    <body><div style='width: 4961px;display: flex;justify-content: center;align-items: center;height: 3508px;direction: rtl;'>
    <div class='report-back seminar-box' style='height: 100%;position: relative;width:100%;'>
        
        <div class='report-back-details'>
            <div class='report-back-detailbox align' >
              <img src='https://morabiiran.com/assets/MorabiIran/images/cer.jpg' style='width:100%;margin: auto;display: block;height: 100%;object-fit: contain;'>
                <span style='position: absolute;top: 919px;left: 630px;font-size: 62px;font-family: dana !important;font-family: 'dana'!important;'>";
            htmlString += PaymentCertificate.Product.NumberString;
            htmlString += @"</span>
            </div>
            <div class='report-back-detailbox align'>
                <span style='position: absolute;top: 770px;left: 490px;font-size: 62px;font-family: dana !important;font-family: 'dana' !important;'>";
            htmlString += PaymentCertificate.Product.DatePrint;
            htmlString += @"</span>
            </div>
        </div>
        <div>
            <br>
            <br>
            <br>
            <br>
           
            <br>
            <div class='margin-text'>
                <p style='position: absolute;top: 1718px;right: 1800px;font-size: 65px;font-weight: bold;font-family: dana !important;font-family: 'dana' !important;'>";

            htmlString += PaymentCertificate.Account.FullName + " " + PaymentCertificate.Account.LatName;
            htmlString += @"</p>
                <p class='text-justify'>
				<p style='position: absolute;top: 1888px;right: 952px;font-size: 65px;font-weight: bold;font-family: dana !important;font-family: 'dana' !important;'>";
            htmlString += PaymentCertificate.Account.NationalCode;
            htmlString += @"</p>
				<p style='position: absolute;top: 1901px;right: 2621px;font-size: 65px;font-weight: bold;font-family: dana !important;font-family: 'dana' !important;'>";
            htmlString += PaymentCertificate.Product.NameFa;
            htmlString += @"</p>
				<p style='position: absolute;top: 2056px;right: 1057px;font-size: 65px;font-weight: bold;font-family: dana !important;font-family: 'dana' !important;'>";
            htmlString += PaymentCertificate.Product.TeacherNameFa;
            htmlString += @"</p>
				<p style='position: absolute;top: 2048px;right: 2291px;font-weight: 600;font-size: 65px;font-family: dana !important;font-family: 'dana' !important;'>";
            htmlString += PaymentCertificate.Product.Hours;
            htmlString += @"</p>
				<p style='position: absolute;top: 2060px;right: 2877px;font-size: 65px;font-weight: bold;font-family: dana !important;font-family: 'dana' !important;'>";
            htmlString += PaymentCertificate.Product.DateFa;
            htmlString += @"</p>
                </p>

            </div>
            <br>
            <br>
           
            <br>
            <div class='margin-text ' style='direction: ltr;'>
                <p style='position: absolute;top: 2231px;left: 1822px;font-size: 65px;font-weight: bold;
'>";
            htmlString += PaymentCertificate.Account.LatNameEn;
            htmlString += @"</p>
                <p class='font-change' style='text-align: left;'>
				<p style='position: absolute;top: 2403px;left: 1051px;font-size: 65px;font-weight: bold;'>";
            htmlString += PaymentCertificate.Account.NationalCode.ToEnglishDigit();
            htmlString += @"</p>
				<p style='position: absolute;top: 2406px;left: 3061px;font-size: 65px;font-weight: bold;width: 1500px;
'>";
            htmlString += PaymentCertificate.Product.NameEn;
            htmlString += @"</p>
				<p style='position: absolute;top: 2548px;left: 927px;font-size: 75px;font-weight: bold;'>";
            htmlString += PaymentCertificate.Product.Hours;
            htmlString += @"</p>
				<p style='position: absolute;top: 2570px;left: 1300px;font-size: 65px;font-weight: bold;'>";
            htmlString += PaymentCertificate.Product.DateEn;
            htmlString += @"</p>
				<p style='position: absolute;top: 2569px;left: 1967px;font-size: 65px;font-weight: bold;
'> ";
            htmlString += PaymentCertificate.Product.TeacherNameEn;
            htmlString += @"</p>
               
                </p>

            </div>
        </div>
        <br>
        <br>
        <br>
        <br>
        
    </div>
</div> </body>
</html>";
            string baseUrl = "";


            ImageFormat imageFormat = ImageFormat.Png;

            imageFormat = ImageFormat.Jpeg;


            int webPageWidth = 4961;

            int webPageHeight = 3508;

            // instantiate a html to image converter object
            HtmlToImage imgConverter = new HtmlToImage();

            // set converter options
            imgConverter.WebPageWidth = webPageWidth;
            imgConverter.WebPageHeight = webPageHeight;

            // create a new image converting an url
            System.Drawing.Image image =
                imgConverter.ConvertHtmlString(htmlString, baseUrl);

            // get image bytes
            byte[] img = ImageToByteArray(image, imageFormat);

            // return resulted image
            FileResult fileResult = new FileContentResult(img, "image/" +
                imageFormat.ToString().ToLower());
            fileResult.FileDownloadName = "image.jpg";
            return fileResult;
            //ViewAccount account = _context.Account.GetCurrentAccount();
            //if (account == null)
            //    return RedirectToAction("login", new { controller = "account", back = "profile/newaddress/" + id });

            //PaymentCertificate PaymentCertificate = _context.PaymentCertificate.GetById(id);


            //return PartialView(BaseController.GetView(this), PaymentCertificate);
        }
        private byte[] ImageToByteArray(
            System.Drawing.Image imageIn,
            ImageFormat imageFormat)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, imageFormat);
            return ms.ToArray();
        }
        public ActionResult Report(int exameId)
        {
            ViewBag.ExameId = exameId;
            ViewAccount tempAccount = _context.Account.GetCurrentAccount();
            if (tempAccount == null)
                return RedirectToAction("index", new { controller = "home" });
            Account account = _context.Account.GetById(tempAccount.Id);
            if (tempAccount == null)
                return RedirectToAction("index", new { controller = "home" });
            var exameAnswer = _context.ExameAccountAnswer.FirstOrDefault(s => s.AccountId == account.ID && s.ExameId == exameId);
            var exameAnswerLast = _context.ExameAccountAnswer.Where(s => s.AccountId == account.ID).OrderByDescending(s => s.ID).Take(10);

            ViewReport2 model = new ViewReport2();
            int i = 1;
            List<ViewReportByLastAvrage> listreport = new List<ViewReportByLastAvrage>();

            try
            {
                model.LastAvrageLevel = listreport.OrderBy(s => s.Id).ToList();
                model.AvrageLevelCountry = _context.ExameAccountAnswer.Where(s => s.ExameId == exameId).Average(s => s.TotalLevel);
                var topAvrageLevelCountry = _context.ExameAccountAnswer.Where(s => s.ExameId == exameId).OrderByDescending(s => s.TotalLevel.Value);
                if (topAvrageLevelCountry.FirstOrDefault() != null)
                {
                    model.TopAvrageLevelCountry = topAvrageLevelCountry.FirstOrDefault().TotalLevel;
                }
            }
            catch
            {

            }
            ViewReportModel accountAnswerMode = new ViewReportModel();

            try
            {
                accountAnswerMode.Exame = exameAnswer.Exame.Name;
                accountAnswerMode.ExameId = exameAnswer.Exame.ID;
                accountAnswerMode.QuestionAnswer = exameAnswer.QuestionAnswer;

                accountAnswerMode.ID = exameAnswer.ID;

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
            }
            catch
            {

            }
            try
            {
                List<ViewExameAccountAnswerDetails> listAnswerDetails = new List<ViewExameAccountAnswerDetails>();
                foreach (var item in exameAnswer.ExameAccountAnswerDetails.OrderBy(s => s.ID))
                {
                    ViewExameAccountAnswerDetails AnswerDetails = new ViewExameAccountAnswerDetails();
                    AnswerDetails.AnswerDetails = item.ExameQuestion.AnswerDetails;

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


            }
            catch
            {

            }
            try
            {
                model.ExameAccountAnswer = accountAnswerMode;

                //model.ExameReportByCourseProfessional = exameAnswer.ExameReportByCourse.Where(s => s.ExameCourse.Code.Label == Enum_Code.FIELD_COURSE_Professional.ToString()).ToList();
                //model.TotalCountG = model.ExameReportByCourseGeneral.Sum(s => s.Course.CountTrue) + model.ExameReportByCourseGeneral.Sum(s => s.Course.CountFalse) + model.ExameReportByCourseGeneral.Sum(s => s.Course.CountNull);
                //model.TotalCountP = model.ExameReportByCourseProfessional.Sum(s => s.CountTrue) + model.ExameReportByCourseProfessional.Sum(s => s.CountFalse) + model.ExameReportByCourseProfessional.Sum(s => s.CountNull);
                //model.RateInCountryG = double.Parse(model.ExameReportByCourseGeneral.Average(s => s.Course.RateInCountry).ToString());
                //model.RateInCountryP = double.Parse(model.ExameReportByCourseProfessional.Average(s => s.RateInCountry).ToString());
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
                List<ViewReportExameReportByCourse> course = new List<ViewReportExameReportByCourse>();
                foreach (var item in exameAnswer.ExameReportByCourse.Where(s => s.ExameCourse.Type == null))
                {
                    ViewReportExameReportByCourse temp = new ViewReportExameReportByCourse();
                    temp.Course = item;
                    var product = _context.Product.FirstOrDefault(s => s.CoursId == item.CourseId);
                    var paymentExame = _context.PaymentExameCourseId.Where(s => s.CourseId == item.CourseId && s.PaymentExame.AccountId==account.ID && s.PaymentExame.ExameId==exameId && s.PaymentExame.StatusId== 1072).ToList();
                    foreach(var ex in paymentExame)
                    {
                        if (ex.Number != null)
                        {
                            temp.Number = "-" + temp.Number + Math.Round(ex.Number.Value, 1);

                        }
                    }
                    if (product != null)
                    {
                        temp.ProductId = product.ID;

                    }
                    course.Add(temp);
                }
                model.ExameReportByCourseGeneral = course;
            }
            catch
            {

            }
            model.PassValue = exameAnswer.Exame.PassValue;
            return PartialView(BaseController.GetView(this), model);
        }
        public ActionResult AnswerSheet(int exameId)
        {
            ViewAccount tempAccount = _context.Account.GetCurrentAccount();
            if (tempAccount == null)
                return RedirectToAction("index", new { controller = "home" });
            Account account = _context.Account.GetById(tempAccount.Id);
            if (tempAccount == null)
                return RedirectToAction("index", new { controller = "home" });
            var exameAnswer = _context.ExameAccountAnswer.FirstOrDefault(s => s.AccountId == account.ID && s.ExameId == exameId);

            return PartialView(BaseController.GetView(this), exameAnswer);
        }
        public ActionResult ReportPrint(int exameId)
        {

            ViewAccount tempAccount = _context.Account.GetCurrentAccount();
            if (tempAccount == null)
                return RedirectToAction("index", new { controller = "home" });
            Account account = _context.Account.GetById(tempAccount.Id);
            if (tempAccount == null)
                return RedirectToAction("index", new { controller = "home" });
            var exameAnswer = _context.ExameAccountAnswer.FirstOrDefault(s => s.AccountId == account.ID && s.ExameId == exameId);
            var exameAnswerLast = _context.ExameAccountAnswer.Where(s => s.AccountId == account.ID).OrderByDescending(s => s.ID).Take(10);

            ViewReport model = new ViewReport();
            model.PassValue = exameAnswer.Exame.PassValue;
            int i = 1;
            List<ViewReportByLastAvrage> listreport = new List<ViewReportByLastAvrage>();
            try
            {
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
            }
            catch { }

            try
            {
                model.LastAvrageLevel = listreport.OrderBy(s => s.Id).ToList();
                model.AvrageLevelCountry = _context.ExameAccountAnswer.Where(s => s.ExameId == exameId).Average(s => s.TotalLevel);
                var topAvrageLevelCountry = _context.ExameAccountAnswer.Where(s => s.ExameId == exameId).OrderByDescending(s => s.TotalLevel.Value);
                if (topAvrageLevelCountry.FirstOrDefault() != null)
                {
                    model.TopAvrageLevelCountry = topAvrageLevelCountry.FirstOrDefault().TotalLevel;
                }
            }
            catch
            {

            }
            ViewReportModel accountAnswerMode = new ViewReportModel();

            try
            {
                accountAnswerMode.Exame = exameAnswer.Exame.Name;
                accountAnswerMode.ExameId = exameAnswer.Exame.ID;
                accountAnswerMode.QuestionAnswer = exameAnswer.QuestionAnswer;

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
            }
            catch
            {

            }
            try
            {
                List<ViewExameAccountAnswerDetails> listAnswerDetails = new List<ViewExameAccountAnswerDetails>();
                foreach (var item in exameAnswer.ExameAccountAnswerDetails.OrderBy(s => s.ID))
                {
                    ViewExameAccountAnswerDetails AnswerDetails = new ViewExameAccountAnswerDetails();
                    AnswerDetails.CategoryName = item.ExameQuestion.ExameCourseCategory.Name;
                    AnswerDetails.AnswerDetails = item.ExameQuestion.AnswerDetails;

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
                    //ExameReportByCourse.TotalRate = item.TotalRate;
                    ExameReportByCourse.TotalLevel = item.TotalLevel;

                    ExameReportByCourse.RatePercent = item.RatePercent;
                    ExameReportByCourse.TotalRateWithoutFalse = item.TotalRateWithoutFalse;
                    ExameReportByCourse.RateInCountry = item.RateInCountry;

                    listExameReportByCourse.Add(ExameReportByCourse);
                }
                accountAnswerMode.ExameReportByCourse = listExameReportByCourse;

            }
            catch
            {

            }
            try
            {
                model.ExameAccountAnswer = accountAnswerMode;
                model.ExameReportByCourseGeneral = exameAnswer.ExameReportByCourse.Where(s => s.ExameCourse.Type == null).ToList();
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
                        category.NumberString = category.TotalCount.ToString().ToPersianNumber();
                        ReportByCategory.Add(category);

                    }

                }
                model.ReportByCategory = ReportByCategory;
            }
            catch
            {

            }
            return PartialView(BaseController.GetView(this), model);
        }

        [HttpPost]
        public ActionResult MessageShop(int ShopId, string Body)
        {
            if (string.IsNullOrEmpty(Body) == false)
            {
                ShopChat chat = new ShopChat()
                {
                    AccountId = _context.Account.GetCurrentAccount().Id,
                    Body = Body,
                    Datetime = DateTime.Now,
                    IsAccountSend = true,
                    IsRead = false,
                    ShopId = ShopId
                };
                _context.ShopChat.Insert(chat);
                _context.Save();
            }
            return RedirectToAction("MessageShop", new { @id = ShopId });
        }

        [HttpGet]
        public ActionResult Follow()
        {
            ViewAccount account = _context.Account.GetCurrentAccount();
            if (account == null)
                return RedirectToAction("login", new { controller = "account", back = "profile/follow" });

            List<ShopFollow> list = _context.ShopFollow.GetAllByAccountId(account.Id);
            return PartialView(BaseController.GetView(this), list);
        }

        [HttpGet]
        public ActionResult Tracking()
        {
            ViewAccount account = _context.Account.GetCurrentAccount();
            return PartialView(BaseController.GetView(this), account);
        }



        [HttpGet]
        public ActionResult Logout(string back = "/")
        {
            _context.Account.Logout();
            return Redirect(back);
        }
        [HttpGet]
        public ActionResult Login(string back = "/")
        {
            if (BaseWebsite.WebsiteSetting.HasLoginWithSms)
            {
                return RedirectToAction("signin", new { back = back });
            }
            if (_context.Account.GetCurrentAccount() == null)
            {
                ViewBag.back = back;
                return PartialView(BaseController.GetView(this));
            }

            if (Request.QueryString["back"] == null)
                return RedirectToAction("profile", new { controller = "account" });
            return RedirectToAction("profile", new { controller = "account", back = Request.QueryString["back"] });
        }
        [HttpPost]
        public ActionResult Login(Account account, bool remember = true, string back = "/", FormCollection form = null)
        {
            string gRecaptchaResponse = form["g-recaptcha-response"];

            if (Recaptcha.Check(gRecaptchaResponse))
            {
                if (account.Email == null)
                {
                    account.Email = account.Mobile;
                }
                ApiResult result = ApiAccount.Post_DoLogin(_context, account, account.Password, true, password2: account.Password);
                if (result.Code == ApiResult.ResponseCode.Success)
                {
                    var accountVm = (ViewAccount)result.Value;
                    _context.Account.SetCurrentAccount(accountVm, account.Password, remember);
                    var currentCartAccount = BaseCart.GetCurrentCartAccount();
                    currentCartAccount.UpdateType = CartUpdateType.SetAccountId;
                    currentCartAccount.AccountId = accountVm.Id;
                    var cart = BaseCart.MergCart(_context: _context, currentCartAccount);
                    BaseCookie.SetCookie(key: CookieName.CardCookie.ToString(), cart.Model.SyncCode, 5);

                    return Redirect(back);
                }
                else
                {
                    int? CaptchaCount = ViewBag.CaptchaCount;
                    CaptchaCount = CaptchaCount.GetValueOrDefault() + 1;
                    ViewBag.CaptchaCount = CaptchaCount;
                    ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, result.Message);
                }
            }
            else
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.INVALID_RECAPTCHA);
            ViewBag.back = back;
            return PartialView(BaseController.GetView(this), account);
        }

        [HttpGet]
        public ActionResult NewAddress(string back = "/login")
        {
            _context.Account.Logout();
            return Redirect(back);
        }

        public ActionResult Recover(string k = null, string a = null)
        {
            AccountPasswordForget forget = null;
            if (k != null || a != null)
            {
                string emailKey = k;
                Guid accountKey = a.GetGuid();
                forget = _context.AccountPasswordForget.GetByEmailKeyAndAccountKey(emailKey, accountKey);
                if (forget != null)
                {
                    return PartialView(BaseController.GetView(this), forget);
                }
                else
                    return Redirect("~/error/404");
            }
            else
                return Redirect("~/error/404");
        }

        [HttpPost]
        public ActionResult Recover(AccountPasswordForget forget, string password, string confirm)
        {
            ViewBag.Password = password;
            ViewBag.PasswordConfirm = confirm;
            ApiResult result = ApiAccountPasswordForget.Post_ChangePassword(_context, forget, password, confirm);
            if (result.Code == ApiResult.ResponseCode.Success)
            {
                Account account = (Account)result.Value;
                ViewAccount CurrentAccount = new ViewAccount(account);
                _context.Account.SetCurrentAccount(CurrentAccount, confirm, true);
                return Redirect("/");
            }
            else
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, result.Message);
            return PartialView(BaseController.GetView(this), forget);
        }
        public ActionResult Forget()
        {
            if (_context.Account.GetCurrentAccount() == null)
            {
                ViewBag.Message = new ViewMessage();
                return PartialView(BaseController.GetView(this));
            }

            return RedirectToAction("profile", new { controller = "account" });
        }

        [HttpPost]
        public ActionResult Forget(string Email, string Mobile, FormCollection form = null)
        {
            string gRecaptchaResponse = form["g-recaptcha-response"];
            if (Recaptcha.Check(gRecaptchaResponse))
            {
                Account account = new Account()
                {
                    Email = Email,
                    Mobile = Mobile.GetEnglish()
                };
                ApiResult result = ApiAccountPasswordForget.Post(_context, account);
                ViewBag.Email = Email;
                ViewBag.Mobile = Mobile;
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, result.Message);
                return PartialView(BaseController.GetView(this));
            }
            else
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.INVALID_RECAPTCHA);
            return PartialView(BaseController.GetView(this));
        }

        public ActionResult Register(string back = "/")
        {
            if (BaseWebsite.WebsiteSetting.HasLoginWithSms)
            {
                return RedirectToAction("signup", new { back = back });
            }
            if (_context.Account.GetCurrentAccount() == null)
            {
                ViewBag.back = back;
                return PartialView(BaseController.GetView(this));
            }
            if (back == null)
                return RedirectToAction("profile", new { controller = "account" });
            return RedirectToAction("profile", new { controller = "account", back = back });
        }

        [HttpPost]
        public ActionResult Register(Account account, string ConfirmPassword = null, string Captcha = null, FormCollection form = null, string back = "/")
        {
            ViewBag.back = back;
            string gRecaptchaResponse = form["g-recaptcha-response"];
            if (Recaptcha.Check(gRecaptchaResponse) == false)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.INVALID_RECAPTCHA);
            else
            {
                if (System.Web.HttpContext.Current.Session["Captcha"] != null && string.IsNullOrEmpty(Captcha))
                    ViewBag.Message = BaseMessage.GetMessage(Enum_Message.REQUIRED_ACCOUNT_CAPTCHA);

                if (System.Web.HttpContext.Current.Session["Captcha"] != null && Captcha != System.Web.HttpContext.Current.Session["Captcha"].ToString())
                    ViewBag.Message = BaseMessage.GetMessage(Enum_Message.CHECK_ACCOUNT_CAPTCHA);

                ApiResult result = ApiAccount.Post_DoRegister(_context, account, true, ConfirmPassword);
                if (result.Code == ApiResult.ResponseCode.Success)
                {
                    ViewAccount entity = new ViewAccount(account);
                    _context.Account.SetCurrentAccount(entity, account.Password, true);
                    var currentCartAccount = BaseCart.GetCurrentCartAccount();
                    currentCartAccount.UpdateType = CartUpdateType.SetAccountId;
                    currentCartAccount.AccountId = account.ID;
                    var cart = BaseCart.MergCart(_context: _context, currentCartAccount);
                    BaseCookie.SetCookie(key: CookieName.CardCookie.ToString(), cart.Model.SyncCode, 5);
                    if (string.IsNullOrEmpty(back))
                        return RedirectToAction("index", new { controller = "home" });
                    else
                        return Redirect(back);
                }
                else
                    ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, result.Message);
            }
            return PartialView(BaseController.GetView(this), account);
        }
        [HttpPost]
        public new ActionResult Profile(Account updateAccount, string faBirthDay = null)
        {
            ViewAccount tempAccount = _context.Account.GetCurrentAccount();
            if (tempAccount == null)
            {
                return RedirectToAction("Index", new { controller = "Home" });
            }
            Account account = _context.Account.GetById(tempAccount.Id);
            Boolean flg = true;
            //Validatuon
            if (updateAccount.Mobile != null && !BaseSecurity.IsValidInput(updateAccount.Mobile, Enum_Validation.MOBILE))
            {
                flg = false;
            }
            if (updateAccount.Email != null && !BaseSecurity.IsValidInput(updateAccount.Email, Enum_Validation.EMAIL))
            {
                flg = false;
            }
            if (updateAccount.UserName != null)
            {
                string uName = updateAccount.UserName;
                var accountList = _context.Account.GetAll().Find(a => a.UserName == uName && a.ID != updateAccount.ID);
                if (accountList != null)
                {
                    //ViewBag.Message = BaseMessage.GetMessage(Enum_Message.DUPLICATED_ACCOUNT_USERNAME);
                    return RedirectToAction("EditProfile", new { controller = "account", back = "EditProfile" });
                    //return View("/Account/EditProfile", account);
                }
            }

            if (!flg)
            {
                ViewBag.Message = "اطلاعات با فرمت درست وارد نشده است";
                return PartialView(BaseController.GetView(this), account);
            }

            if (!string.IsNullOrEmpty(faBirthDay))
            {
                updateAccount.BirthDay = BaseDate.GetGregorian(new CustomDate(DateTime.Parse(faBirthDay))).GetValueOrDefault();
            }
            else
            {
                updateAccount.BirthDay = null;
            }

            if (!string.IsNullOrEmpty(updateAccount.NationalCode))
            {
                //pattern
                if (BaseSecurity.IsValidInput(updateAccount.NationalCode, Enum_Validation.NationalCode) == false)
                {

                    ViewBag.Message = "فرمت کد ملی صحیح نمی باشد";
                    return PartialView(BaseController.GetView(this), account);
                }
            }

            if (account.ID == updateAccount.ID)
            {
                if (string.IsNullOrEmpty(updateAccount.FullName))
                {
                    ViewBag.Message = "Enter The FullName,Please";
                    return PartialView(BaseController.GetView(this, "EditProfile"), account);
                }
                account = _context.Account.GetById(account.ID);
                account.Address = updateAccount.Address;
                account.Company = updateAccount.Company;
                account.Email = updateAccount.Email;
                account.FullName = updateAccount.FullName;
                account.IsMale = updateAccount.IsMale;
                account.Job = updateAccount.Job;
                account.Phone = updateAccount.Phone;
                account.NationalCode = updateAccount.NationalCode;
                account.BirthDay = updateAccount.BirthDay;
                account.IsMale = updateAccount.IsMale;
                account.Website = updateAccount.Website;
                account.Fax = updateAccount.Fax;
                account.Instagram = updateAccount.Instagram;
                account.Telegram = updateAccount.Telegram;
                account.WhatsApp = updateAccount.WhatsApp;
                account.CompanyNo = updateAccount.CompanyNo;
                account.Lat = updateAccount.Lat;
                account.Long = updateAccount.Long;
                account.Linkedin = updateAccount.Linkedin;
                account.Facebook = updateAccount.Facebook;
                account.Profession = updateAccount.Profession;
                account.AboutMe = updateAccount.AboutMe;
                account.FavoritLink = updateAccount.FavoritLink;
                account.FavoritTitle = updateAccount.FavoritTitle;

                _context.Account.Update(account);
                _context.Save();
                tempAccount = new ViewAccount(account);
                _context.Account.SetCurrentAccount(tempAccount, tempAccount.Password, false);
                return PartialView(BaseController.GetView(this), account);
            }
            else
                return RedirectToAction("login", new { controller = "account", back = "index" });
        }
        public new ActionResult Profile()
        {
            //var a = GetResxNameByValue("ADDRESS");

            ViewAccount tempAccount = _context.Account.GetCurrentAccount();
            if (tempAccount == null)
                return RedirectToAction("Index", new { controller = "Home" });
            //return RedirectToAction("index", new { controller = "account", back = "profile" });

            Account account = _context.Account.GetById(tempAccount.Id);

            return PartialView(BaseController.GetView(this), account);
        }

        public ActionResult EditProfile()
        {

            ViewAccount tempAccount = _context.Account.GetCurrentAccount();
            if (tempAccount == null)
                //return RedirectToAction("login", new { controller = "account", back = "EditProfile" });
                return RedirectToAction("index", new { controller = "home" });

            Account account = _context.Account.GetById(tempAccount.Id);
            if (tempAccount == null)
                //return RedirectToAction("login", new { controller = "account", back = "EditProfile" });
                return RedirectToAction("index", new { controller = "home" });

            return PartialView(BaseController.GetView(this), account);
        }

        public ActionResult EditUserName(Account updateAccount)
        {
            ViewAccount tempAccount = _context.Account.GetCurrentAccount();
            Account account = _context.Account.GetById(tempAccount.Id);
            Boolean flg = false;
            //Validatuon
            if ((updateAccount.UserName != null) && (updateAccount.UserName != ""))
            {
                string uName = updateAccount.UserName;
                var accountList = _context.Account.FirstOrDefault(a => a.UserName == uName && a.ID != updateAccount.ID);
                if (accountList != null)
                {
                    flg = true;
                }
            }
            if (flg == true)
            {
                ViewBag.Message = "This Username is in Use";
                return PartialView(BaseController.GetView(this), account);
            }
            else if (account.ID == updateAccount.ID && account.ChangeUserName == false)
            {
                account = _context.Account.GetById(account.ID);
                account.UserName = updateAccount.UserName;
                account.ChangeUserName = true;
                _context.Account.Update(account);
                _context.Save();
                tempAccount = new ViewAccount(account);
                _context.Account.SetCurrentAccount(tempAccount, tempAccount.Password, false);
                return View(BaseController.GetView(this), account);
            }
            else
                return RedirectToAction("login", new { controller = "account", back = "index" });
        }

        public ActionResult Address()
        {
            ViewAccount tempAccount = _context.Account.GetCurrentAccount();
            if (tempAccount == null)
                //return RedirectToAction("login", new { controller = "account", back = "Address" });
                return RedirectToAction("index", new { controller = "home" });

            Account account = _context.Account.GetById(tempAccount.Id);
            if (tempAccount == null)
                //return RedirectToAction("login", new { controller = "account", back = "Address" });
                return RedirectToAction("index", new { controller = "home" });
            return PartialView(BaseController.GetView(this), account);
        }
        public ActionResult Dashboard()
        {
            ViewAccount tempAccount = _context.Account.GetCurrentAccount();
            if (tempAccount == null)
                //return RedirectToAction("login", new { controller = "account", back = "Address" });
                return RedirectToAction("index", new { controller = "home" });

            Account account = _context.Account.GetById(tempAccount.Id);
            if (tempAccount == null)
                //return RedirectToAction("login", new { controller = "account", back = "Address" });
                return RedirectToAction("index", new { controller = "home" });
            return PartialView(BaseController.GetView(this), account);
        }
        public ActionResult UserComment()
        {
            ViewAccount tempAccount = _context.Account.GetCurrentAccount();
            if (tempAccount == null)
                //return RedirectToAction("login", new { controller = "account", back = "Address" });
                return RedirectToAction("index", new { controller = "home" });

            Account account = _context.Account.GetById(tempAccount.Id);
            if (tempAccount == null)
                //return RedirectToAction("login", new { controller = "account", back = "Address" });
                return RedirectToAction("index", new { controller = "home" });
            return PartialView(BaseController.GetView(this));
        }
        public ActionResult GiftCard()
        {
            ViewAccount tempAccount = _context.Account.GetCurrentAccount();
            if (tempAccount == null)
                //return RedirectToAction("login", new { controller = "account", back = "Address" });
                return RedirectToAction("index", new { controller = "home" });

            Account account = _context.Account.GetById(tempAccount.Id);
            if (tempAccount == null)
                //return RedirectToAction("login", new { controller = "account", back = "Address" });
                return RedirectToAction("index", new { controller = "home" });
            return PartialView(BaseController.GetView(this));
        }
        public ActionResult TestSina()
        {
            ViewAccount tempAccount = _context.Account.GetCurrentAccount();
            if (tempAccount == null)
                //return RedirectToAction("login", new { controller = "account", back = "Address" });
                return RedirectToAction("index", new { controller = "home" });

            Account account = _context.Account.GetById(tempAccount.Id);
            return PartialView(BaseController.GetView(this), account);
        }

        //[Route("~/{username}")]
        public ActionResult Preview(string username)
        {

            ViewAccount tempAccount = _context.Account.GetAccountbyUserName(username);
            Account account = _context.Account.GetById(tempAccount.Id);
            if (account == null)
            {
                return RedirectToAction("index", new { controller = "home" });
            }
            else
            {
                return View(BaseController.GetView(this), account);
            }
        }

        [HttpGet]
        public ActionResult Captcha()
        {


            string CapImage = "data:image/png;base64," +
            Convert.ToBase64String(new Utility().VerificationTextGenerator());
            return Json(new { CapImage = CapImage }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCurrentAccount(string ViewName = null)
        {
            ViewAccount CurrentAccount = _context.Account.GetCurrentAccount();
            ViewBag.CurrentAccount = CurrentAccount;

            if (ViewName == null)
                return null;
            else
                return PartialView(BaseController.GetView(this, ViewName), CurrentAccount);
        }
        private bool IsModelValid(Account account)
        {
            bool result = false;
            if (account.FullName == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_Message.REQUIRED_ACCOUNT_FULLNAME);
            else if (account.Email == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_Message.REQUIRED_ACCOUNT_EMAIL);
            else if (BaseSecurity.IsValidInput(account.Email, Enum_Validation.EMAIL) == false)
                ViewBag.Message = BaseMessage.GetMessage(Enum_Message.INVALID_ACCOUNT_EMAIL);
            else if (account.Password == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_Message.REQUIRED_ACCOUNT_PASSWORD);
            else if (_context.Account.GetByEmail(account.Email) != null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_Message.DUPLICATED_ACCOUNT_EMAIL);
            else if (account.UserName == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_Message.REQUIRED_ACCOUNT_USERNAME);
            //else if (_context.Account.GetByUserName(account.UserName) != null)
            //    ViewBag.Message = BaseMessage.GetMessage(Enum_Message.DUPLICATED_ACCOUNT_USERNAME);

            else
                result = true;

            return result;
        }

        public ActionResult vCardDownload(string username)
        {
            var ua = HttpContext.Request.UserAgent;
            var extention = ".vcf";
            if (ua.Contains("Android"))
                extention = ".txt";


            ViewAccount tempAccount = _context.Account.GetAccountbyUserName(username);
            Account account = _context.Account.GetById(tempAccount.Id);
            string path = "";
            if (account.Picture != null)
            {
                string x = account.Picture.Url.ToString();
                string m = x.Replace("SYSTEM_TYPE_PANEL/Uploads", "");
                m = "/Panel/Uploads" + m;
                path = Server.MapPath(@m);
            }
            var card = new VCard
            {
                FullName = account.FullName,

                Addrress = account.Address,

                JobTitle = account.Job,

                Phone = account.Phone,

                Mobile = account.Mobile,

                Email = account.Email,

                Fax = account.Fax,

                Instagram = account.Instagram,

                Telegram = account.Telegram,
                WhatsApp = account.WhatsApp,
                Website = account.Website,
                HomePage = "Vipbcard.com/" + account.UserName
            };
            if (path != "")
            {
                card.Image = System.IO.File.ReadAllBytes(path);
            }
            string fileName = "v_" + account.UserName + extention;
            string fpath = Server.MapPath("/Panel/Uploads/") + fileName;

            System.IO.File.Create(fpath).Dispose();
            using (var file = System.IO.File.OpenWrite(fpath))

            using (var writer = new System.IO.StreamWriter(file))
            {
                if (extention == ".vcf")
                    writer.Write(card.ToIos());
                else
                    writer.Write(card.ToAndroid());


            }
            // Image = account.PictureId,

            Response.Clear();

            Response.ContentType = "text/vcard";


            var cardString = "";
            if (extention == ".vcf")
                cardString = card.ToIos();
            else
                cardString = card.ToAndroid();

            var inputEncoding = Encoding.Default;

            var outputEncoding = Encoding.GetEncoding("windows-1257");

            var cardBytes = inputEncoding.GetBytes(cardString);

            var outputBytes = Encoding.Convert(inputEncoding, outputEncoding, cardBytes);

            Response.OutputStream.Write(outputBytes, 0, outputBytes.Length);

            Response.End();

            return File(System.IO.File.ReadAllBytes("/Panel/Uploads/" + fileName), "text /vcard");
        }
        private string GetResxNameByValue(string value)
        {
            System.Resources.ResourceManager rm = new global::System.Resources.ResourceManager("Resources.Lang", global::System.Reflection.Assembly.Load("App_GlobalResources"));


            var entry =
                rm.GetResourceSet(System.Threading.Thread.CurrentThread.CurrentCulture, true, true)
                  .OfType<DictionaryEntry>()
                  .FirstOrDefault(e => e.Value.ToString() == value);

            var key = entry.Key.ToString();
            return key;

        }
        //private string getInvitationEmail()
        //{
        //    StringBuilder html = new StringBuilder();
        //    try
        //    {
        //        html.Append("<b>دعوت به خوش کالا </b>" + "<br /><br />");
        //        html.Append("<b>خوشحال می شویم از سایت خوش کالا دیدن فرمایید </b>" + "<br /><br />");
        //        html.Append("https://khoshkala.com/" + "<br />");
        //        html.Append("<hr />");
        //    }
        //    catch (Exception) { }
        //    return html.ToString();
        //}
        //[HttpPost]
        //public ActionResult Invitation(string mestr)
        //{
        //    string emailBody = this.getInvitationEmail();
        //    UnitOfWork mainContext = new UnitOfWork();
        //    StringBuilder str_1 = new StringBuilder();
        //    string token_1 = mestr;
        //    string token_2 = "";
        //    string token_3 = "";
        //    if (mestr == null || mestr.Trim() == string.Empty)
        //    {
        //        ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, "خطا در ورود اطلاعات");
        //    }
        //    else if (mestr.Contains("@") == true)
        //    {
        //        mainContext.Email.SaveNewEmail(mestr.Trim(), Enum_EmailType.PRODUCT_NOTIFY, emailBody, "دعوت به خوش کالا");
        //    }
        //    else if (mestr.Trim().Substring(0, 2) == "09")
        //    {
        //        mainContext.Sms.SaveNewSms(mestr.Trim(), Enum_SmsType.PRODUCT_NOTIFY, str_1.ToString(), token_1, token_2, token_3);
        //    }
        //    else
        //    {
        //        ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, "فرمت اطلاعات وارد شده صحیح نیست");
        //    }


        //    return PartialView(BaseController.GetView(this));
        //}
        public ActionResult Order()
        {
            ViewAccount tempAccount = _context.Account.GetCurrentAccount();
            if (tempAccount == null)
                return RedirectToAction("index", new { controller = "home" });
            Account account = _context.Account.GetById(tempAccount.Id);
            if (tempAccount == null)
                return RedirectToAction("index", new { controller = "home" });
            return PartialView(BaseController.GetView(this), tempAccount);
        }
        public ActionResult OrderComment(int orderid)
        {
            ViewAccount tempAccount = _context.Account.GetCurrentAccount();
            if (tempAccount == null)
                return RedirectToAction("index", new { controller = "home" });
            Account account = _context.Account.GetById(tempAccount.Id);
            if (tempAccount == null)
                return RedirectToAction("index", new { controller = "home" });
            var ordercomment = _context.AccountOrderComment.Where(s => s.AccountOrder.AccountId == account.ID && s.OrderId == orderid).ToList();
            return PartialView(BaseController.GetView(this), ordercomment);
        }
        public ActionResult SearchAccountOrder(
                Enum_Code aoStatus = Enum_Code.ORDER_STATUS_NONE,
                string aoViewName = null, int? typeId = null)
        {
            ViewAccount tempAccount = _context.Account.GetCurrentAccount();
            if (tempAccount == null)
                return RedirectToAction("index", new { controller = "home" });
            Account account = _context.Account.GetById(tempAccount.Id);
            if (tempAccount == null)
                return RedirectToAction("index", new { controller = "home" });
            //poIndex = poIndex != null ? poIndex.Value : 1;
            //poSize = poSize != null ? poSize.Value : 10;
            aoViewName = aoViewName == null ? "Search" : aoViewName;
            List<AccountOrder> results = _context.AccountOrder.Search(
                accountId: account.ID,
                status: aoStatus, typeId: typeId);

            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, aoViewName, results);
        }
        public ActionResult SearchAccountOrder2(
               Enum_Code aoStatus = Enum_Code.ORDER_STATUS_NONE,
               string aoViewName = null, int? typeId = null, string statusId2 = "")
        {
            ViewAccount tempAccount = _context.Account.GetCurrentAccount();
            if (tempAccount == null)
                return RedirectToAction("index", new { controller = "home" });
            Account account = _context.Account.GetById(tempAccount.Id);
            if (tempAccount == null)
                return RedirectToAction("index", new { controller = "home" });
            //poIndex = poIndex != null ? poIndex.Value : 1;
            //poSize = poSize != null ? poSize.Value : 10;
            aoViewName = aoViewName == null ? "Search" : aoViewName;
            List<AccountOrderProduct> results = _context.AccountOrderProduct.Search(
                  accountId: account.ID,
                  status: aoStatus, typeId: typeId, statusId2: statusId2);

            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, aoViewName, results);
        }
        public ActionResult SearchProductVisit(
        string pvViewName = null,
        int pvSize = 10)
        {
            ViewAccount tempAccount = _context.Account.GetCurrentAccount();
            if (tempAccount == null)
                return RedirectToAction("index", new { controller = "home" });
            Account account = _context.Account.GetById(tempAccount.Id);
            if (account == null)
                return RedirectToAction("index", new { controller = "home" });
            //poIndex = poIndex != null ? poIndex.Value : 1;
            //poSize = poSize != null ? poSize.Value : 10;
            pvViewName = pvViewName == null ? "Search" : pvViewName;
            List<ProductVisit> results = _context.ProductVisit.GetAllByAccountId(accountId: account.ID, prpageSize: pvSize);

            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, pvViewName, results);
        }
        public ActionResult History(int prindex = 1, int prpageSize = 5)
        {
            if (_context.Account.GetCurrentAccount() == null)
                return RedirectToAction("Index", "Home", new { controller = "account", back = "/History" });

            ViewAccount account = _context.Account.GetCurrentAccount();
            List<ProductVisit> results = _context.ProductVisit.GetAllByAccountId(accountId: account.Id, prpageSize: prpageSize, prindex: prindex);


            //var searchQuery = _context.ProductLike.GetSearchQuery(account.Id);
            //List<ProductLike> results = _context.ProductLike.Search(prindex, prpageSize, searchQuery);
            ViewProductVisit search = new ViewProductVisit()
            {
                TotalCount = _context.ProductVisit.SearchDetail(account.Id).TotalCount,
                PageIndex = prindex,
                PageSize = prpageSize,
                Results = results
            };
            return View(BaseController.GetView(this), search);
        }
        [HttpGet]
        public ActionResult Favorite(int prindex = 1,
    int prpageSize = 5)
        {
            if (_context.Account.GetCurrentAccount() == null)

                return RedirectToAction("Index", "Home", new { controller = "account", back = "/favorite" });

            ViewAccount account = _context.Account.GetCurrentAccount();

            var searchQuery = _context.ProductLike.GetSearchQuery(account.Id);
            List<ProductLike> results = _context.ProductLike.Search(prindex, prpageSize, searchQuery);
            ViewProductLikeFavorit search = new ViewProductLikeFavorit()
            {
                TotalCount = _context.ProductLike.SearchDetail(query: searchQuery).Count,
                PageIndex = prindex,
                PageSize = prpageSize,
                Results = results
            };
            return View(BaseController.GetView(this), search);
        }

        public ActionResult PrintAccountOrder(string id)
        {
            AccountOrder accountOrder = _context.AccountOrder.GetByUniqueId(id);
            return View(BaseController.GetView(this), accountOrder);
        }
        [HttpGet]
        public ActionResult SignIn(string back = "/")
        {
            if (!BaseWebsite.WebsiteSetting.HasLoginWithSms)
            {
                return RedirectToAction("Login", new { back = back });
            }

            if (_context.Account.GetCurrentAccount() == null)
            {
                var signIn = new SignInVm() { BackUrl = back };
                return PartialView(BaseController.GetView(this), model: signIn);
            }

            if (Request.QueryString["back"] == null)
                return RedirectToAction("profile", new { controller = "account" });
            return RedirectToAction("profile", new { controller = "account", back = Request.QueryString["back"] });
        }
        [HttpPost]
        public ActionResult SignIn(SignInVm signIn)
        {

            SignInVm result = ApiAccount.Post_DoSignIn(_context, signIn);
            if (result.Status == ResultStatus.Success)
            {
                return RedirectToAction("Verify", new { controller = "account", mobile = signIn.Mobile, backurl = signIn.BackUrl });
            }
            else
            {
                return PartialView(BaseController.GetView(this), model: result);
            }
        }
        [HttpGet]
        public ActionResult SignUp(string back = "/")
        {
            if (!BaseWebsite.WebsiteSetting.HasLoginWithSms)
            {
                return RedirectToAction("Register", new { back = back });
            }
            if (_context.Account.GetCurrentAccount() == null)
            {
                var signUp = new SignUpVm() { BackUrl = back };
                return PartialView(BaseController.GetView(this), model: signUp);
            }

            if (Request.QueryString["back"] == null)
                return RedirectToAction("profile", new { controller = "account" });
            return RedirectToAction("profile", new { controller = "account", back = Request.QueryString["back"] });
        }
        [HttpPost]
        public ActionResult SignUp(SignUpVm signUp)
        {
            SignUpVm result = ApiAccount.Post_DoSignUp(_context, signUp);
            if (result.Status == ResultStatus.Success)
            {
                return RedirectToAction("Verify", new { controller = "account", mobile = signUp.Mobile, backurl = signUp.BackUrl });
            }
            else
            {
                return PartialView(BaseController.GetView(this), model: result);
            }
        }
        [HttpGet]
        public ActionResult Verify(string mobile, string backurl)
        {
            var signIn = new SignInVm() { Mobile = mobile, BackUrl = backurl };
            SignInVm result = ApiAccount.Post_DoSendVerifyCode(_context, signIn);
            if (result.Status == ResultStatus.Success)
            {
                return PartialView(BaseController.GetView(this), model: result);
            }
            else
            {
                return PartialView(BaseController.GetView(this), model: result);
            }

        }
        [HttpPost]
        public ActionResult Verify(SignInVm signIn)
        {
            SignInVm result = ApiAccount.Post_VerifyCode(_context, signIn);
            if (result.Status == ResultStatus.Success)
            {
                var account = _context.Account.GetByMobile(signIn.Mobile);
                var accountVm = new ViewAccount(account);
                _context.Account.SetCurrentAccount(account: accountVm, password: account.Password, remember: true);
                var currentCartAccount = BaseCart.GetCurrentCartAccount();
                currentCartAccount.UpdateType = CartUpdateType.SetAccountId;
                currentCartAccount.AccountId = accountVm.Id;
                var cart = BaseCart.MergCart(_context: _context, currentCartAccount);
                BaseCookie.SetCookie(key: CookieName.CardCookie.ToString(), cart.Model.SyncCode, 5);

                if (!string.IsNullOrEmpty(signIn.BackUrl))
                {
                    return Redirect(signIn.BackUrl);
                }
                else
                {
                    return RedirectToAction("profile", new { controller = "account" });
                }
            }
            else
            {
                return PartialView(BaseController.GetView(this), model: result);
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