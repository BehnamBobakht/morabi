using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cms.Controllers.Website
{
    public class StateController : Controller
    {
        UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(int? ciid, string ciidname = null)
        {
            State state = _context.State.GetById(ciid);
            if (state == null)
                return RedirectPermanent("/error/404");

            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, null, state);
        }
        public ActionResult GetAll2(string civiewName, DataLayer.Enumarables.Enum_Code typeId)
        {
            var listState = _context.State.GetAll().OrderBy(s => s.Name).ToList();
          
            foreach(var item in listState)
            {
                item.Latitude = typeId.ToString();
            }
            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, civiewName, listState);
        }
        public ActionResult GetAll(string civiewName)
        {
            var listState = _context.State.GetAll().OrderBy(s=>s.Name).ToList();
            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, civiewName, listState);
        }

    }
}