using DataLayer.Base;
using DataLayer.Enumarables;
using Media.Infrastructure;
using Media.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace Media.Controllers
{
    public class CheckoutController : BaseMediaController
    {
        [HttpGet]
        public ActionResult Index(int id)
        {
            return Purchase(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Buy(int id)
        {
            return Purchase(id);
        }

        private ActionResult Purchase(int id)
        {
            if (Owns(id)) return RedirectToAction("Watch", "Courses", new { id });
            var model = Product(id);
            if (model == null)
            {
                if (IsPackageWithoutQuantity(id))
                    return Redirect(BaseWebsite.ShopUrl.TrimEnd('/') + "/pr/" + id + "/package");
                return HttpNotFound();
            }

            var quickSyncCode = Guid.NewGuid().ToString();
            var quickCart = new DataLayer.ViewModels.CartUpdateVm
            {
                AccountId = null,
                SyncCode = quickSyncCode,
                StoreProductQuantityId = model.QuantityId,
                Count = 1,
                UpdateType = CartUpdateType.Add
            };
            var update = BaseCart.Update(Context, quickCart);
            if (update.Status != ResultStatus.Success)
            {
                CleanupQuickCart(quickSyncCode);
                model.Error = update.Message;
                return View("Index", model);
            }

            var cart = Context.Cart.GetCartEntity(null, quickSyncCode);
            cart.AccountId = (int)CurrentAccountId;
            cart.AddressId = 0;
            var freeDelivery = Context.Delivery.FirstOrDefault(x => x.BasePrice == 0 && x.MaxDays == 0);
            if (freeDelivery != null)
            {
                cart.DeliveryId = freeDelivery.ID;
                foreach (var store in cart.CartStore)
                {
                    store.DeliveryId = freeDelivery.ID;
                    store.DeliveryPrice = 0;
                }
            }
            Context.Cart.Update(cart);
            Context.Save();

            var order = BaseCart.NewOrder(Context, new DataLayer.ViewModels.CartUpdateVm
            {
                AccountId = null,
                SyncCode = quickSyncCode
            });
            CleanupQuickCart(quickSyncCode);
            if (order.Status != ResultStatus.Success)
            {
                model.Error = order.Message;
                return View("Index", model);
            }

            if (order.Model.PaymentTypeId == 2)
            {
                var payment = Context.Payment.GetById(order.Model.Payment.ID);
                payment.RefNumber = "پرداخت رایگان";
                payment.StatusId = Context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_SUCCESSFUL);
                MediaPaymentGateway.CompleteOrderWithoutCart(Context, payment);
                Context.Payment.Update(payment);
                Context.Save();
                return RedirectToAction("Callback", "Payment", new { id = payment.ID });
            }
            return RedirectToAction("Start", "Payment", new { id = order.Model.Payment.ID });
        }

        private void CleanupQuickCart(string syncCode)
        {
            var cart = Context.Cart.FirstOrDefault(x => x.SyncCode == syncCode);
            if (cart == null) return;
            foreach (var store in cart.CartStore.ToList())
            {
                foreach (var item in store.CartStoreItem.ToList()) Context.CartStoreItem.Delete(item);
                Context.CartStore.Delete(store);
            }
            Context.Cart.Delete(cart);
            Context.Save();
        }

        private QuickCheckoutViewModel Product(int id)
        {
            const string sql=@"SELECT TOP 1 p.ID,p.Name,spq.ID QuantityId,spq.Price,
                REPLACE(pic.Url,'SYSTEM_TYPE_PANEL','/panel') ImageUrl
                FROM dbo.Product p
                INNER JOIN dbo.StoreProduct sp ON sp.ProductId=p.ID
                INNER JOIN dbo.StoreProductQuantity spq ON spq.StoreProductId=sp.ID
                LEFT JOIN dbo.Picture pic ON pic.ID=p.PictureId
                WHERE p.ID=@P AND p.ProductTypeId=2 AND ISNULL(p.Deleted,0)=0
                ORDER BY spq.ID";
            using(var cn=MediaDatabase.Open())using(var cmd=new SqlCommand(sql,cn))
            {cmd.Parameters.Add("@P",SqlDbType.Int).Value=id;using(var reader=cmd.ExecuteReader()){if(!reader.Read())return null;return new QuickCheckoutViewModel{ProductId=id,QuantityId=(int)reader["QuantityId"],ProductTitle=Convert.ToString(reader["Name"]),ImageUrl=Convert.ToString(reader["ImageUrl"]),Price=Convert.ToDouble(reader["Price"])};}}
        }

        private bool IsPackageWithoutQuantity(int id)
        {
            const string sql = @"SELECT CASE WHEN EXISTS(
                SELECT 1 FROM dbo.Product p
                WHERE p.ID=@P AND p.ProductTypeId=2 AND ISNULL(p.Deleted,0)=0
                  AND NULLIF(LTRIM(RTRIM(ISNULL(p.ProductsId,''))), '') IS NOT NULL
                  AND NOT EXISTS(
                    SELECT 1 FROM dbo.StoreProduct sp
                    INNER JOIN dbo.StoreProductQuantity spq ON spq.StoreProductId=sp.ID
                    WHERE sp.ProductId=p.ID)
                ) THEN 1 ELSE 0 END";
            using (var cn = MediaDatabase.Open())
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.Add("@P", SqlDbType.Int).Value = id;
                return Convert.ToBoolean(cmd.ExecuteScalar());
            }
        }
        private bool Owns(int id)
        {
            const string sql=@"SELECT CASE WHEN EXISTS(SELECT 1 FROM dbo.AccountOrderProduct aop INNER JOIN dbo.AccountOrder ao ON ao.ID=aop.OrderId INNER JOIN dbo.Product pp ON pp.ID=aop.ProductId LEFT JOIN dbo.Code c ON c.ID=ao.StatusId WHERE ao.AccountId=@A AND (c.Label='ORDER_STATUS_SUCCESS' OR ao.StatusId IN(2068,2073)) AND (aop.ProductId=@P OR ','+REPLACE(ISNULL(pp.ProductsId,''),' ','')+',' LIKE '%,'+CONVERT(varchar(20),@P)+',%')) THEN 1 ELSE 0 END";
            using(var cn=MediaDatabase.Open())using(var cmd=new SqlCommand(sql,cn)){cmd.Parameters.Add("@A",SqlDbType.BigInt).Value=CurrentAccountId;cmd.Parameters.Add("@P",SqlDbType.Int).Value=id;return Convert.ToBoolean(cmd.ExecuteScalar());}
        }
    }
}