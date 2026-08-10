using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer.Base;
using DataLayer.ViewModels;

namespace Cms.Controllers.Website
{
    public class CartController : Controller
    {
        UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            var currentCartAccount = BaseCart.GetCurrentCartAccount();
            BaseCart.Update(_context: _context, cartUpdate: currentCartAccount);
            return View(BaseController.GetView(this));
        }
        public ActionResult IndexInner(CartUpdateType cartUpdateType = CartUpdateType.None, int quantityId = 0, int count = 0)
        {

            var cart = new ResultVm<CartVm>();
            var currentCartAccount = BaseCart.GetCurrentCartAccount();
            if (!string.IsNullOrEmpty(currentCartAccount.SyncCode) || currentCartAccount.AccountId.HasValue)
            {
                currentCartAccount.StoreProductQuantityId = quantityId;
                currentCartAccount.Count = count;
                currentCartAccount.UpdateType = cartUpdateType;
                if (cartUpdateType != CartUpdateType.None)
                {
                    cart = BaseCart.Update(_context: _context, cartUpdate: currentCartAccount);
                }
                else
                {
                    cart = BaseCart.GetCurrentCard(_context: _context, accountId: currentCartAccount.AccountId, cartSyncCode: currentCartAccount.SyncCode);
                }
                if (cart.Status == ResultStatus.Success)
                    BaseCookie.SetCookie(key: CookieName.CardCookie.ToString(), cart.Model.SyncCode, 5);

            }
            return PartialView(BaseController.GetView(this), model: cart);
        }
        public ActionResult Insert(int quantityId, int count, string description = null)
        {
            var currentCartAccount = BaseCart.GetCurrentCartAccount();
            currentCartAccount.StoreProductQuantityId = quantityId;
            currentCartAccount.Count = count;
            currentCartAccount.UpdateType = CartUpdateType.Add;

            var currentCart = BaseCart.Update(_context: _context, cartUpdate: currentCartAccount, description: description);
            if (currentCart.Status == ResultStatus.Success)
                BaseCookie.SetCookie(key: CookieName.CardCookie.ToString(), currentCart.Model.SyncCode, 5);

            return PartialView(BaseController.GetView(this, "CartBadge"), model: currentCart);

        }
        public ActionResult Address()
        {
            ViewAccount currentAccount = BaseWebsite.CurrentAccount;
            var currentCartAccount = BaseCart.GetCurrentCartAccount();

            if (currentAccount == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                var cart = BaseCart.GetCurrentCard(_context: _context, accountId: currentCartAccount.AccountId, cartSyncCode: currentCartAccount.SyncCode);
                if (cart.Status == ResultStatus.Success)
                    BaseCookie.SetCookie(key: CookieName.CardCookie.ToString(), cart.Model.SyncCode, 5);

                if (cart.Model.ItemCount < 1)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(BaseController.GetView(this), cart);
                }

            }
        }
        public ActionResult AddressInner(int? addressId)
        {
            var currentCartAccount = BaseCart.GetCurrentCartAccount();

            currentCartAccount.UpdateType = CartUpdateType.SetAddress;
            currentCartAccount.AddressId = addressId;
            var cart = BaseCart.Update(_context: _context, currentCartAccount);
            return PartialView(BaseController.GetView(this), cart);
        }
        [HttpPost]
        public ActionResult AddressNextPage()
        {
            var result = new ResultVm<CartVm>();
            var currentCartAccount = BaseCart.GetCurrentCartAccount();
            var cart = BaseCart.GetCurrentCard(_context: _context, accountId: currentCartAccount.AccountId, cartSyncCode: currentCartAccount.SyncCode);
            if (cart.Status == ResultStatus.Success)
                BaseCookie.SetCookie(key: CookieName.CardCookie.ToString(), cart.Model.SyncCode, 5);

            if (cart.Model.HaveAddress == true)
            {
                if (cart.Model.AddressId > 0)
                {
                    result.Status = ResultStatus.Success;
                    result.Message = "/cart/confirm";
                }
                else
                {
                    result.Status = ResultStatus.Error;
                    result.Message = Resource.Notify.PleaseSelectAddress;
                    ViewBag.Message = Resource.Notify.PleaseSelectAddress;
                }
            }
            else
            {
                result.Status = ResultStatus.Success;
                result.Message = "/cart/confirm";
            }

            return Json(result);
        }

        public ActionResult Confirm()
        {
            ViewAccount currentAccount = BaseWebsite.CurrentAccount;
            var currentCartAccount = BaseCart.GetCurrentCartAccount();


            if (currentAccount == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                var cart = BaseCart.GetCurrentCard(_context: _context, accountId: currentCartAccount.AccountId, cartSyncCode: currentCartAccount.SyncCode);
                if (cart.Status == ResultStatus.Success)
                    BaseCookie.SetCookie(key: CookieName.CardCookie.ToString(), cart.Model.SyncCode, 5);

                if (cart.Model.ItemCount < 1)
                {
                    return RedirectToAction("Index");
                }
                else if (!cart.Model.AddressId.HasValue)
                {
                    return RedirectToAction("Address");
                }
                else
                {
                    return View(BaseController.GetView(this), cart);
                }
            }
        }
        public ActionResult ConfirmInner(ResultVm<CartVm> model)
        {
            var currentCartAccount = BaseCart.GetCurrentCartAccount();

            if (!string.IsNullOrEmpty(currentCartAccount.SyncCode) || currentCartAccount.AccountId.HasValue)
            {
                //var activeMerchants = _context.Merchant.GetAllActive().FirstOrDefault();
                //if (activeMerchants != null)
                //{
                //    currentCartAccount.MerchantId = activeMerchants.ID;
                //}
                if (model.Model != null)
                {


                    HttpCookie intercodeCookie = HttpContext.Request.Cookies[".intercode"];
                    if (intercodeCookie != null)
                    {
                        if (!string.IsNullOrEmpty(intercodeCookie.Values["code"]))

                        {
                            currentCartAccount.GiftMessage = intercodeCookie.Values["code"];
                        }
                        else
                        {
                            currentCartAccount.GiftMessage = model.Model.GiftMessage;
                        }
                    }
                    else
                    {
                        currentCartAccount.GiftMessage = model.Model.GiftMessage;
                    }
                    currentCartAccount.UpdateType = CartUpdateType.SetOther;

                    currentCartAccount.CustomerMessage = model.Model.CustomerMessage;
                    model = BaseCart.Update(_context: _context, cartUpdate: currentCartAccount);

                }
                else
                {

                    model = BaseCart.GetCurrentCard(_context: _context, accountId: currentCartAccount.AccountId, cartSyncCode: currentCartAccount.SyncCode);

                    if (model.Status == ResultStatus.Success)
                        BaseCookie.SetCookie(key: CookieName.CardCookie.ToString(), model.Model.SyncCode, 5);
                    HttpCookie intercodeCookie = HttpContext.Request.Cookies[".intercode"];
                    if (intercodeCookie != null)
                    {
                        if (!string.IsNullOrEmpty(intercodeCookie.Values["code"]))
                        {
                            currentCartAccount.GiftMessage = intercodeCookie.Values["code"];
                            currentCartAccount.UpdateType = CartUpdateType.SetOther;
                            model = BaseCart.Update(_context: _context, cartUpdate: currentCartAccount);
                        }
                       
                    }
                    
                   

                }
            }
            return PartialView(BaseController.GetView(this), model: model);

        }
        [HttpPost]
        public ActionResult ChangeDelivery(int? storeId, int deliveryId)
        {
            var currentCartAccount = BaseCart.GetCurrentCartAccount();
            currentCartAccount.UpdateType = CartUpdateType.SetDelivery;
            currentCartAccount.StoreId = storeId;
            currentCartAccount.DeliveryId = deliveryId;
            BaseCart.Update(_context: _context, currentCartAccount);
            return RedirectToAction("ConfirmInner");
        }
        public ActionResult SetRebate(ResultVm<CartVm> model)
        {
            if (model.Model != null)
            {
                var currentCartAccount = BaseCart.GetCurrentCartAccount();
                currentCartAccount.UpdateType = CartUpdateType.SetRebate;
                currentCartAccount.RebateCode = model.Model.RebateCode;
                model = BaseCart.Update(_context: _context, currentCartAccount);
            }
            else
            {
                model = new ResultVm<CartVm>() { Model = new CartVm() };
            }

            return PartialView(BaseController.GetView(this), model: model);
        }
        public ActionResult SetMessage(ResultVm<CartVm> model)
        {
            var currentCartAccount = BaseCart.GetCurrentCartAccount();
            if (model.Model != null)
            {

                currentCartAccount.UpdateType = CartUpdateType.SetOther;
                currentCartAccount.CustomerMessage = model.Model.CustomerMessage;
                currentCartAccount.GiftMessage = model.Model.GiftMessage;
                model = BaseCart.Update(_context: _context, currentCartAccount);
            }
            else
            {
                model = BaseCart.GetCurrentCard(_context: _context, accountId: currentCartAccount.AccountId, cartSyncCode: currentCartAccount.SyncCode);

            }

            return PartialView(BaseController.GetView(this), model: model);
        }
        public ActionResult SetMerchant(int? merchantId)
        {
            var currentCartAccount = BaseCart.GetCurrentCartAccount();
            currentCartAccount.UpdateType = CartUpdateType.SetMerchant;
            currentCartAccount.MerchantId = merchantId;
            var model = BaseCart.Update(_context: _context, currentCartAccount);
            return PartialView(BaseController.GetView(this), model);
        }
        public ActionResult GotoPayment()
        {
            var currentCartAccount = BaseCart.GetCurrentCartAccount();
            ViewAccount currentAccount = BaseWebsite.CurrentAccount;
            if (currentAccount == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                var order = BaseCart.NewOrder(_context: _context, cartUpdate: currentCartAccount);
                if (order.Status == ResultStatus.Success)
                {
                    if (order.Model.PaymentTypeId == 2)
                    {

                        var payment = _context.Payment.GetById(order.Model.Payment.ID);
                        payment.RefNumber = "پرداخت رایگان";
                        payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_SUCCESSFUL);
                        //_context.Payment.DoPaymentServices(_context, payment);
                        BaseCart.DoPaymentCallBack(_context, payment);
                        _context.Payment.Update(payment);
                        _context.Save();
                        return Redirect("/payment/callback?id=" + payment.ID);
                    }
                    else
                    {
                        return Redirect(order.Model.Payment.PaymentAddress);

                    }
                }
                else
                {
                    var cart = BaseCart.GetCurrentCard(_context: _context, accountId: currentCartAccount.AccountId, cartSyncCode: currentCartAccount.SyncCode);
                    cart.Status = ResultStatus.Error;
                    cart.Message = order.Message;
                    return View(BaseController.GetView(this, "Confirm"), cart);
                }
            }
        }

        public ActionResult CartBadge()
        {
            var cart = new ResultVm<CartVm>();
            cart.Model = new CartVm();
            var currentCartAccount = BaseCart.GetCurrentCartAccount();
            if (currentCartAccount.AccountId.HasValue || (!string.IsNullOrEmpty(currentCartAccount.SyncCode)))
            {
                cart = BaseCart.GetCurrentCard(_context: _context, accountId: currentCartAccount.AccountId, cartSyncCode: currentCartAccount.SyncCode);
                if (cart.Status == ResultStatus.Success)
                    BaseCookie.SetCookie(key: CookieName.CardCookie.ToString(), cart.Model.SyncCode, 5);

            }

            return PartialView(BaseController.GetView(this), model: cart);
        }
        public ActionResult ShowError()
        {
            var cart = new ResultVm<CartVm>();
            cart.Model = new CartVm();
            var currentCartAccount = BaseCart.GetCurrentCartAccount();
            if (currentCartAccount.AccountId.HasValue || (!string.IsNullOrEmpty(currentCartAccount.SyncCode)))
            {
                cart = BaseCart.GetCurrentCard(_context: _context, accountId: currentCartAccount.AccountId, cartSyncCode: currentCartAccount.SyncCode);
                if (cart.Status == ResultStatus.Success)
                    BaseCookie.SetCookie(key: CookieName.CardCookie.ToString(), cart.Model.SyncCode, 5);

            }

            return PartialView(BaseController.GetView(this), model: cart);
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