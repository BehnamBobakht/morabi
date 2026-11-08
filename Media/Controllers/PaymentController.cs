using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using Media.Infrastructure;
using Media.Models;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace Media.Controllers
{
    public class PaymentController : Controller
    {
        private readonly UnitOfWork _context = new UnitOfWork();

        public ActionResult Start(int id)
        {
            var account = _context.Account.GetCurrentAccount();
            if (account == null) return RedirectToRoute("MediaLogin", new { back = Request.RawUrl });
            var payment = _context.Payment.GetById(id);
            if (payment == null || payment.AccountId != account.Id) return HttpNotFound();
            var insertedStatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_INSERTED);
            var startedStatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_STARTED);
            if (payment.StatusId != insertedStatusId && payment.StatusId != startedStatusId)
                return RedirectToAction("Callback", new { id });

            var callbackBaseUrl = ConfigurationManager.AppSettings["MediaPaymentCallbackBaseUrl"];
            if (string.IsNullOrWhiteSpace(callbackBaseUrl))
                return View("Result", new MediaPaymentResultViewModel
                {
                    Success = false,
                    Message = "آدرس Callback پرداخت در Web.config مدیا تنظیم نشده است."
                });
            var callbackUrl = callbackBaseUrl.Trim();
            callbackUrl += (callbackUrl.Contains("?") ? "&" : "?") + "id=" + id;
            var paying = MediaPaymentGateway.Start(_context, payment, callbackUrl);
            if (paying.IsLink) return Redirect(paying.Link);
            return View("Result", new MediaPaymentResultViewModel { Success = false, Message = paying.Message });
        }

        public ActionResult Callback(int id, string Status, string Authority)
        {
            var payment = _context.Payment.GetById(id);
            if (payment == null) return HttpNotFound();
            if (payment.Code.Label == Enum_Code.PAYMENT_STATUS_STARTED.ToString())
                payment = MediaPaymentGateway.Verify(_context, payment, Status, Authority);

            var product = PaymentProduct(id);
            var successStatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_SUCCESSFUL);
            var success = payment != null && payment.StatusId == successStatusId;
            return View("Result", new MediaPaymentResultViewModel
            {
                Success = success,
                ReferenceNumber = payment == null ? null : payment.RefNumber,
                Message = success ? "پرداخت با موفقیت انجام شد و دوره برای شما فعال است." : "پرداخت کامل نشد. در صورت کسر وجه، مبلغ طبق روال بانک بازمی‌گردد.",
                ProductId = product == null ? (int?)null : product.Item1,
                ProductTitle = product == null ? null : product.Item2
            });
        }

        private Tuple<int,string> PaymentProduct(int paymentId)
        {
            const string sql=@"SELECT TOP 1 p.ID,p.Name FROM dbo.PaymentProductOrder ppo
                INNER JOIN dbo.AccountOrder childOrder ON childOrder.ParentId=ppo.OrderId
                INNER JOIN dbo.AccountOrderProduct aop ON aop.OrderId=childOrder.ID
                INNER JOIN dbo.Product p ON p.ID=aop.ProductId WHERE ppo.PaymentId=@ID ORDER BY aop.ID";
            using(var cn=MediaDatabase.Open())using(var cmd=new SqlCommand(sql,cn)){cmd.Parameters.Add("@ID",SqlDbType.Int).Value=paymentId;using(var r=cmd.ExecuteReader()){return r.Read()?Tuple.Create((int)r["ID"],Convert.ToString(r["Name"])):null;}}
        }

        protected override void Dispose(bool disposing){if(disposing)_context.Dispose();base.Dispose(disposing);}
    }
}