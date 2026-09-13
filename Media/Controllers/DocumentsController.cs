using Media.Infrastructure;
using Media.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace Media.Controllers
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class DocumentsController : BaseMediaController
    {
        private static readonly HttpClient PdfHttpClient = new HttpClient
        {
            Timeout = TimeSpan.FromMinutes(10)
        };

        public ActionResult Index(int id)
        {
            if (!Owns(id))
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            var docs = new List<CourseDocumentViewModel>();
            string title = null;

            const string sql = @"
                SELECT 
                    p.Name ProductTitle,
                    d.ID,
                    d.Title,
                    d.Description,
                    d.PdfUrl,
                    d.SortOrder,
                    CONVERT(bit,CASE WHEN v.ID IS NULL THEN 0 ELSE 1 END) IsViewed
                FROM dbo.CourseDocument d
                INNER JOIN dbo.Product p ON p.ID=d.ProductId
                LEFT JOIN dbo.CourseDocumentVisit v 
                    ON v.CourseDocumentId=d.ID 
                    AND v.AccountId=@A
                WHERE d.ProductId=@P
                  AND d.Active=1
                ORDER BY d.SortOrder,d.ID";

            using (var cn = MediaDatabase.Open())
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.Add("@A", SqlDbType.BigInt).Value = CurrentAccountId;
                cmd.Parameters.Add("@P", SqlDbType.Int).Value = id;

                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        title = Convert.ToString(r["ProductTitle"]);

                        docs.Add(new CourseDocumentViewModel
                        {
                            ID = (int)r["ID"],
                            Title = Convert.ToString(r["Title"]),
                            Description = Convert.ToString(r["Description"]),
                            PdfUrl = Convert.ToString(r["PdfUrl"]),
                            SortOrder = (int)r["SortOrder"],
                            IsViewed = (bool)r["IsViewed"]
                        });
                    }
                }
            }

            if (title == null)
                title = ProductTitle(id);

            return View(new CourseDocumentListViewModel
            {
                ProductId = id,
                ProductTitle = title,
                Documents = docs
            });
        }


        public ActionResult Read(int id)
        {
            CourseDocumentViewModel doc = null;
            int productId = 0;
            string productTitle = null;

            const string sql = @"
                SELECT 
                    d.ID,
                    d.ProductId,
                    d.Title,
                    d.Description,
                    d.PdfUrl,
                    d.SortOrder,
                    p.Name ProductTitle
                FROM dbo.CourseDocument d
                INNER JOIN dbo.Product p ON p.ID=d.ProductId
                WHERE d.ID=@ID
                  AND d.Active=1";

            using (var cn = MediaDatabase.Open())
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;

                using (var r = cmd.ExecuteReader())
                {
                    if (r.Read())
                    {
                        productId = (int)r["ProductId"];
                        productTitle = Convert.ToString(r["ProductTitle"]);

                        doc = new CourseDocumentViewModel
                        {
                            ID = (int)r["ID"],
                            Title = Convert.ToString(r["Title"]),
                            Description = Convert.ToString(r["Description"]),
                            PdfUrl = Convert.ToString(r["PdfUrl"]),
                            SortOrder = (int)r["SortOrder"],
                            IsViewed = true
                        };
                    }
                }
            }

            if (doc == null)
                return HttpNotFound();

            if (!Owns(productId))
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            MarkDocumentViewed(id);

            var documents = new List<CourseDocumentViewModel>();
            const string documentsSql = @"
                SELECT d.ID,d.Title,d.Description,d.PdfUrl,d.SortOrder,
                       CONVERT(bit,CASE WHEN v.ID IS NULL THEN 0 ELSE 1 END) IsViewed
                FROM dbo.CourseDocument d
                LEFT JOIN dbo.CourseDocumentVisit v
                    ON v.CourseDocumentId=d.ID AND v.AccountId=@A
                WHERE d.ProductId=@P AND d.Active=1
                ORDER BY d.SortOrder,d.ID";

            using (var cn = MediaDatabase.Open())
            using (var cmd = new SqlCommand(documentsSql, cn))
            {
                cmd.Parameters.Add("@A", SqlDbType.BigInt).Value = CurrentAccountId;
                cmd.Parameters.Add("@P", SqlDbType.Int).Value = productId;
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        documents.Add(new CourseDocumentViewModel
                        {
                            ID = (int)r["ID"],
                            Title = Convert.ToString(r["Title"]),
                            Description = Convert.ToString(r["Description"]),
                            PdfUrl = Convert.ToString(r["PdfUrl"]),
                            SortOrder = (int)r["SortOrder"],
                            IsViewed = (bool)r["IsViewed"] || (int)r["ID"] == id
                        });
                    }
                }
            }

            return View(new CourseDocumentReadViewModel
            {
                ProductId = productId,
                ProductTitle = productTitle,
                Document = doc,
                Documents = documents
            });
        }


        [HttpGet]
        public async Task<ActionResult> Pdf(int id)
        {
            int productId = 0;
            string pdfUrl = null;

            const string sql = @"
                SELECT 
                    ProductId,
                    PdfUrl
                FROM dbo.CourseDocument
                WHERE ID=@ID
                  AND Active=1";

            using (var cn = MediaDatabase.Open())
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;

                using (var r = cmd.ExecuteReader())
                {
                    if (!r.Read())
                        return HttpNotFound();

                    productId = Convert.ToInt32(r["ProductId"]);
                    pdfUrl = Convert.ToString(r["PdfUrl"]);
                }
            }

            if (!Owns(productId))
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            if (string.IsNullOrWhiteSpace(pdfUrl))
                return HttpNotFound();

            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, pdfUrl))
                {
                    var range = Request.Headers["Range"];
                    if (!string.IsNullOrWhiteSpace(range))
                        request.Headers.TryAddWithoutValidation("Range", range);

                    using (var response = await PdfHttpClient.SendAsync(
                        request, HttpCompletionOption.ResponseHeadersRead))
                    {
                        if (!response.IsSuccessStatusCode)
                            return new HttpStatusCodeResult(response.StatusCode);

                        Response.StatusCode = (int)response.StatusCode;
                        Response.BufferOutput = false;
                        Response.ContentType = "application/pdf";
                        Response.Cache.SetCacheability(HttpCacheability.Private);
                        Response.Cache.SetMaxAge(TimeSpan.FromMinutes(30));
                        Response.Cache.SetValidUntilExpires(true);
                        Response.Headers["X-Content-Type-Options"] = "nosniff";
                        Response.AppendHeader("Accept-Ranges", "bytes");

                        if (response.Content.Headers.ContentLength.HasValue)
                            Response.AppendHeader("Content-Length",
                                response.Content.Headers.ContentLength.Value.ToString());

                        if (response.Content.Headers.ContentRange != null)
                            Response.AppendHeader("Content-Range",
                                response.Content.Headers.ContentRange.ToString());

                        await response.Content.CopyToAsync(Response.OutputStream);
                        return new EmptyResult();
                    }
                }
            }
            catch
            {
                return HttpNotFound();
            }
        }


        private void MarkDocumentViewed(int id)
        {
            const string sql = @"
                UPDATE dbo.CourseDocumentVisit
                SET LastVisitDateTime=GETDATE()
                WHERE CourseDocumentId=@ID
                  AND AccountId=@A;

                IF @@ROWCOUNT=0
                BEGIN TRY

                    INSERT dbo.CourseDocumentVisit
                    (
                        CourseDocumentId,
                        AccountId,
                        FirstVisitDateTime,
                        LastVisitDateTime
                    )
                    VALUES
                    (
                        @ID,
                        @A,
                        GETDATE(),
                        GETDATE()
                    );

                END TRY
                BEGIN CATCH

                    IF ERROR_NUMBER() IN(2601,2627)

                        UPDATE dbo.CourseDocumentVisit
                        SET LastVisitDateTime=GETDATE()
                        WHERE CourseDocumentId=@ID
                          AND AccountId=@A;

                    ELSE
                        THROW;

                END CATCH";

            using (var cn = MediaDatabase.Open())
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@A", SqlDbType.BigInt).Value = CurrentAccountId;

                cmd.ExecuteNonQuery();
            }
        }


        private bool Owns(int productId)
        {
            const string sql = @"
                SELECT CASE WHEN EXISTS
                (
                    SELECT 1
                    FROM dbo.AccountOrderProduct aop
                    INNER JOIN dbo.AccountOrder ao ON ao.ID=aop.OrderId
                    INNER JOIN dbo.Product purchasedProduct ON purchasedProduct.ID=aop.ProductId
                    LEFT JOIN dbo.Code c ON c.ID=ao.StatusId
                    WHERE ao.AccountId=@A
                      AND (c.Label='ORDER_STATUS_SUCCESS' OR ao.StatusId IN(2068,2073))
                      AND
                      (
                          aop.ProductId=@P
                          OR ','+REPLACE(ISNULL(purchasedProduct.ProductsId,''),' ','')+','
                             LIKE '%,'+CONVERT(varchar(20),@P)+',%'
                      )
                ) THEN 1 ELSE 0 END";

            using (var cn = MediaDatabase.Open())
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.Add("@A", SqlDbType.BigInt).Value = CurrentAccountId;
                cmd.Parameters.Add("@P", SqlDbType.Int).Value = productId;
                return Convert.ToBoolean(cmd.ExecuteScalar());
            }
        }


        private string ProductTitle(int id)
        {
            using (var cn = MediaDatabase.Open())
            using (var cmd = new SqlCommand(
                "SELECT Name FROM dbo.Product WHERE ID=@ID",
                cn))
            {
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;

                return Convert.ToString(cmd.ExecuteScalar());
            }
        }
    }
}