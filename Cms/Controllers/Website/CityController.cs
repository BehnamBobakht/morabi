using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cms.Controllers.Website
{
    public class CityController : Controller
    {
        UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(int? ciid, string ciidname = null)
        {

            City city = _context.City.GetById(ciid);
            if (city == null)
                return RedirectPermanent("/error/404");
            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, null, city);
        }

        public ActionResult Search(
            int? ciIndex = null,
            int? ciPageSize = null,
            int? ciStateId = null,
            string civiewName = null)
        {
            ciIndex = ciIndex != null ? ciIndex.Value : 1;
            ciPageSize = ciPageSize != null ? ciPageSize.Value : 10;
            List<CityVm> list = _context.City.Search(
                stateId: ciStateId,
                withAll:true,
                pageSize: ciPageSize.Value,
                index: ciIndex.Value);

            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, civiewName, list);
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