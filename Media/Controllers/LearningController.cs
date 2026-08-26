using Media.Infrastructure;
using Media.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Web.SessionState;

namespace Media.Controllers
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class LearningController : BaseMediaController
    {
        public ActionResult Index()
        {
            var result = new List<LearningCourseViewModel>();
            const string sql = @"
SELECT p.ID,p.Name,REPLACE(pic.Url,'SYSTEM_TYPE_PANEL','/panel') ImageUrl,
 (SELECT COUNT(*) FROM dbo.CourseVideoSession s WHERE s.ProductId=p.ID AND s.Active=1) VideoCount,
 (SELECT COUNT(*) FROM dbo.CourseVideoSession s INNER JOIN dbo.CourseVideoVisit v ON v.CourseVideoSessionId=s.ID AND v.AccountId=@A WHERE s.ProductId=p.ID AND s.Active=1) ViewedVideoCount,
 (SELECT COUNT(*) FROM dbo.CourseDocument d WHERE d.ProductId=p.ID AND d.Active=1) DocumentCount,
 (SELECT COUNT(*) FROM dbo.CourseDocument d INNER JOIN dbo.CourseDocumentVisit v ON v.CourseDocumentId=d.ID AND v.AccountId=@A WHERE d.ProductId=p.ID AND d.Active=1) ViewedDocumentCount,
 CONVERT(bit,CASE WHEN EXISTS(SELECT 1 FROM dbo.Exame e WHERE e.ProductId=p.ID AND ISNULL(e.Active,1)=1 AND ISNULL(e.Deleted,0)=0) THEN 1 ELSE 0 END) HasExam
FROM dbo.Product p
LEFT JOIN dbo.Picture pic ON pic.ID=p.PictureId
WHERE EXISTS(SELECT 1 FROM dbo.AccountOrderProduct aop INNER JOIN dbo.AccountOrder ao ON ao.ID=aop.OrderId LEFT JOIN dbo.Code c ON c.ID=ao.StatusId WHERE aop.ProductId=p.ID AND ao.AccountId=@A AND (c.Label='ORDER_STATUS_SUCCESS' OR ao.StatusId IN(2068,2073)))
AND (EXISTS(SELECT 1 FROM dbo.CourseVideoSession s WHERE s.ProductId=p.ID AND s.Active=1) OR EXISTS(SELECT 1 FROM dbo.CourseDocument d WHERE d.ProductId=p.ID AND d.Active=1))
ORDER BY p.ID DESC";
            using (var cn = MediaDatabase.Open())
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.Add("@A", SqlDbType.BigInt).Value = CurrentAccountId;
                using (var r = cmd.ExecuteReader())
                    while (r.Read())
                    {
                        var videoCount = (int)r["VideoCount"];
                        var viewedVideoCount = (int)r["ViewedVideoCount"];
                        var documentCount = (int)r["DocumentCount"];
                        var viewedDocumentCount = (int)r["ViewedDocumentCount"];
                        result.Add(new LearningCourseViewModel
                        {
                            ID = (int)r["ID"], Title = Convert.ToString(r["Name"]), ImageUrl = Convert.ToString(r["ImageUrl"]),
                            HasVideos = videoCount > 0, HasDocuments = documentCount > 0, HasExam = (bool)r["HasExam"],
                            VideoCount = videoCount, ViewedVideoCount = viewedVideoCount,
                            DocumentCount = documentCount, ViewedDocumentCount = viewedDocumentCount,
                            ExamUnlocked = (videoCount == 0 || viewedVideoCount >= videoCount) && (documentCount == 0 || viewedDocumentCount >= documentCount)
                        });
                    }
            }
            return View(result);
        }
    }
}
