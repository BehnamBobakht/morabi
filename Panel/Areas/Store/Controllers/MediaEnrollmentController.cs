using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace Panel.Areas.Store.Controllers
{
    public class MediaEnrollmentController : Controller
    {
        private readonly UnitOfWork _context = new UnitOfWork();

        [HttpGet]
        public ActionResult Index(
            string fullName = null,
            string nationalCode = null,
            string mobile = null,
            int[] productIds = null,
            int pageIndex = 1,
            int pageSize = 50)
        {
            _context.Permission.CheckPagePermission(
                _context.SiteUser.GetCurrentUser(),
                Enum_Permission.Product_Edit);

            pageIndex = Math.Max(1, pageIndex);
            pageSize = Math.Max(10, Math.Min(200, pageSize));

            ViewBag.FullName = fullName;
            ViewBag.NationalCode = nationalCode;
            ViewBag.Mobile = mobile;
            productIds = (productIds ?? new int[0]).Where(id => id > 0).Distinct().ToArray();
            ViewBag.ProductIds = productIds;
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalCount = _context.Account.SearchCount(
                fullName: fullName,
                NationalCode: nationalCode,
                Mobile: mobile);

            ViewBag.Products = new MultiSelectList(
                _context.Product.Search(
                    typeId: "2",
                    IsHidden: true,
                    order: Enum_ProductOrder.NEW,
                    pageSize: 10000),
                "ID",
                "Name",
                productIds);

            var accounts = _context.Account.Search(
                    fullName: fullName,
                    NationalCode: nationalCode,
                    Mobile: mobile,
                    index: pageIndex,
                    pageSize: pageSize)
                .ToList();

            return View(accounts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Enroll(int[] productIds, long[] accountIds)
        {
            var siteUser = _context.SiteUser.GetCurrentUser();
            _context.Permission.CheckPagePermission(siteUser, Enum_Permission.Product_Edit);

            productIds = (productIds ?? new int[0])
                .Where(id => id > 0)
                .Distinct()
                .ToArray();

            accountIds = (accountIds ?? new long[0])
                .Where(id => id > 0)
                .Distinct()
                .ToArray();

            if (productIds.Length == 0)
            {
                TempData["MediaError"] = "حداقل یک دوره را انتخاب کنید.";
                return RedirectToAction("Index");
            }

            if (accountIds.Length == 0)
            {
                TempData["MediaError"] = "حداقل یک کاربر را انتخاب کنید.";
                return RedirectToAction("Index");
            }

            var inserted = 0;
            var skipped = 0;
            var errors = new List<string>();

            using (var connection = CreateSqlConnection())
            {
                connection.Open();
                foreach (var productId in productIds)
                {
                    foreach (var accountId in accountIds)
                    {
                        try
                        {
                            using (var command = new SqlCommand("dbo.Media_EnrollAccountInCourse", connection))
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.Add("@AccountId", SqlDbType.BigInt).Value = accountId;
                                command.Parameters.Add("@ProductId", SqlDbType.Int).Value = productId;

                                using (var reader = command.ExecuteReader())
                                {
                                    if (reader.Read() && Convert.ToBoolean(reader["Inserted"]))
                                        inserted++;
                                    else
                                        skipped++;
                                }
                            }
                        }
                        catch (Exception exception)
                        {
                            errors.Add("کاربر " + accountId + "، دوره " + productId + ": " + exception.Message);
                        }
                    }
                }
            }

            TempData["MediaSuccess"] =
                inserted + " عضویت ثبت شد و " + skipped + " عضویت از قبل وجود داشت.";

            if (errors.Any())
                TempData["MediaError"] = string.Join("<br/>", errors.Take(5));

            return RedirectToAction("Index");
        }

        private static SqlConnection CreateSqlConnection()
        {
            var setting = ConfigurationManager.ConnectionStrings["DatabaseEntities"];
            if (setting == null)
                throw new ConfigurationErrorsException("Connection string 'DatabaseEntities' was not found.");

            var entityBuilder = new EntityConnectionStringBuilder(setting.ConnectionString);
            return new SqlConnection(entityBuilder.ProviderConnectionString);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _context.Dispose();
            base.Dispose(disposing);
        }
    }
}
