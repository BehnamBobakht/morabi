using DataLayer.Api;
using DataLayer.Api.Model;
using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System.Linq;
using System.Web.Mvc;

namespace Media.Controllers
{
    public class AccountController : Controller
    {
        private readonly UnitOfWork _context = new UnitOfWork();

        [HttpGet]
        public ActionResult Login(string back = "/learning")
        {
            var currentAccount = _context.Account.GetCurrentAccount();
            if (currentAccount != null)
            {
                return SafeRedirect(back);
            }
            ViewBag.Back = back;
            return View(new Account());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Account account, bool remember = true, string back = "/learning")
        {
            if (account.Email == null) account.Email = account.Mobile;
            var result = ApiAccount.Post_DoLogin(_context, account, account.Password, true, password2: account.Password);
            if (result.Code == ApiResult.ResponseCode.Success)
            {
                var accountVm = (ViewAccount)result.Value;
                _context.Account.SetCurrentAccount(accountVm, account.Password, remember);
                MergeCart(accountVm.Id);
                return SafeRedirect(back);
            }
            ViewBag.Back = back;
            ViewBag.Error = result.Message == null ? "اطلاعات ورود صحیح نیست." : result.Message.ToString();
            return View(account);
        }

        [HttpGet]
        public ActionResult Forget()
        {
            if (_context.Account.GetCurrentAccount() != null) return RedirectToRoute("Learning");
            return View(new Account());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Forget(string mobile)
        {
            var account = new Account { Mobile = mobile ?? string.Empty, Email = string.Empty };
            var result = ApiAccountPasswordForget.Post(_context, account);

            if (result.Code == ApiResult.ResponseCode.Success)
            {
                ViewBag.Success = "رمز عبور جدید به شماره موبایل شما ارسال شد. با آن وارد رسانه شوید.";
            }
            else
            {
                ViewBag.Error = result.Message == null ? "بازیابی رمز عبور انجام نشد؛ شماره موبایل را بررسی کنید." : result.Message.ToString();
            }

            return View(account);
        }

        [HttpGet]
        public ActionResult Logout(string back = "/")
        {
            _context.Account.Logout();
            return Redirect(Url.IsLocalUrl(back) ? back : "/");
        }

        [HttpGet]
        public ActionResult Register(string back = "/learning")
        {
            if (_context.Account.GetCurrentAccount() != null) return SafeRedirect(back);
            ViewBag.Back = back;
            LoadStates(null);
            return View(new Account());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Account account, string confirmPassword, string back = "/learning")
        {
            ViewBag.Back = back;
            LoadStates(account == null ? null : account.StateId);
            var result = ApiAccount.Post_DoRegister(_context, account, true, confirmPassword);
            if (result.Code == ApiResult.ResponseCode.Success)
            {
                var accountVm = new ViewAccount(account);
                _context.Account.SetCurrentAccount(accountVm, account.Password, true);
                MergeCart(account.ID);
                return SafeRedirect(back);
            }
            ViewBag.Error = result.Message == null ? "ثبت‌نام انجام نشد؛ اطلاعات را بررسی کنید." : result.Message.ToString();
            return View(account);
        }

        private void LoadStates(int? selectedStateId)
        {
            var states = _context.State.GetAll().OrderBy(x => x.Name).ToList();
            ViewBag.States = new SelectList(states, "ID", "Name", selectedStateId);
        }
        private void MergeCart(int accountId)
        {
            var cartAccount = BaseCart.GetCurrentCartAccount();
            cartAccount.UpdateType = CartUpdateType.SetAccountId;
            cartAccount.AccountId = accountId;
            var cart = BaseCart.MergCart(_context, cartAccount);
            if (cart != null && cart.Model != null) BaseCookie.SetCookie(CookieName.CardCookie.ToString(), cart.Model.SyncCode, 5);
        }

        private ActionResult SafeRedirect(string back)
        {
            if (string.IsNullOrWhiteSpace(back) || back == "/learning" || back == "/media/learning")
                return RedirectToRoute("Learning");

            return Redirect(Url.IsLocalUrl(back) ? back : Url.RouteUrl("Learning"));
        }

        protected override void Dispose(bool disposing) { if (disposing) _context.Dispose(); base.Dispose(disposing); }
    }
}
