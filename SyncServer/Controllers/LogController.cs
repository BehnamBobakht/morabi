using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DataLayer.Entities;

namespace SyncServer.Controllers
{
    public class LogController : Controller
    {
        UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            StringBuilder text = new StringBuilder();
            List<SyncLog> list = _context.SyncLog.GetLog(100);
            text.AppendLine("CURRENT SERVER DATETIME : " + DateTime.Now.ToString());
            foreach (SyncLog item in list)
            {
                text.AppendLine(item.ID + "\t" + item.Name + "\t" + item.Status + "\t" + item.StartDatetime + "\t" + item.EndDatetime + "\t" + item.Description);
            }
            return Content(text.ToString(), "text/plain");
        }
    }
}