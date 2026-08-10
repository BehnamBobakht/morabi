using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.ViewModels;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DataLayer.Repositories
{
    public class Entity_WebsiteView : BaseRepository<WebsiteView>
    {
        private DatabaseEntities _context;
        public Entity_WebsiteView(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public void IncreaseView(int? accountId = null)
        {
            HttpContext user_context = HttpContext.Current;
            var Request = user_context.Request;
            if (!Request.Url.PathAndQuery.ToLower().Contains("/sync"))
            {
                WebsiteView view = new WebsiteView()
                {
                    Browser = Request.Browser.Browser,
                    Datetime = DateTime.Now,
                    Host = Request.Url.Scheme + "://" + Request.Url.Host,
                    // OperatingSystem = user_context.Request.Browser.Platform,
                    SessionId = user_context.Session.SessionID,
                    IP = BaseSecurity.GetClientIPAddress(),
                    AccountId = accountId,
                    Url = Request.Url.PathAndQuery
                };

                if (Request.UrlReferrer != null)
                {
                    view.RefererHost = Request.UrlReferrer.Host;
                    view.RefererAddress = Request.UrlReferrer.ToString();
                }
                view.City = "-";
                view.Country = "-";
                view.Latitude = "0";
                view.Longitude = "0";
                view.Region = "-";
                Insert(view);
                Save();
            }

        }

        public List<ViewKeyValueString> GetVisitReportByWeekDays()
        {
            try
            {
                StringBuilder Query = new StringBuilder();
                Query.AppendLine("SELECT CONVERT(varchar(10), CONVERT(DATE, [DateTime])) AS [Key], CONVERT(varchar(10), COUNT(*)) AS [Value] FROM [WebsiteView]");
                Query.AppendLine("WHERE [WebsiteView].DateTime > DATEADD(DAY, -7, CURRENT_TIMESTAMP)");
                Query.AppendLine("GROUP BY CONVERT(DATE, [DateTime])");
                Query.AppendLine("ORDER BY CONVERT(DATE, [DateTime]) DESC");
                List<ViewKeyValueString> list = _context.Database.SqlQuery<ViewKeyValueString>(Query.ToString()).Cast<ViewKeyValueString>().ToList();
                foreach (ViewKeyValueString item in list)
                {
                    item.Key = DateTime.Parse(item.Key).ToPersian().ToPersian();
                }
                return list;
            }
            catch
            {
                return new List<ViewKeyValueString>();
            }
        }
    }
}
