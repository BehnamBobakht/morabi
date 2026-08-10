using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SyncServer
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            SyncServer_Scheduler.Start();

            ThreadPool.QueueUserWorkItem(o => PingServers());
            MapAll.Maping();
        }

        private static void PingServers()
        {
            UnitOfWork _context = new UnitOfWork();
            string local_type = Enum_Code.SYSTEM_TYPE_LOCAL.ToString();
            string sync_type = Enum_Code.SYSTEM_TYPE_SYNCSERVER.ToString();
            string api_type = Enum_Code.SYSTEM_TYPE_API.ToString();
            string oauth_type = Enum_Code.SYSTEM_TYPE_OAUTH.ToString();

            while (true)
            {
                List<WebsiteDomain> listDomain = _context.WebsiteDomain.Where(p =>
                    p.Website.Code.Label == local_type ||
                    p.Website.Code.Label == sync_type ||
                    p.Website.Code.Label == api_type ||
                    p.Website.Code.Label == oauth_type).ToList();

                foreach (WebsiteDomain domain in listDomain)
                {
                    string url = domain.Domain.Replace("/api/", "");
                    try
                    {
                        var webClient = new WebClient();
                        webClient.DownloadString(url);
                    }
                    catch (Exception) {}
                }
                Thread.Sleep(TimeSpan.FromMinutes(3));
            }
        }
    }
}
