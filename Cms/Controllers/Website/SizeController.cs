using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cms.Controllers.Website
{
    public class SizeController : Controller
    {
        UnitOfWork _context = new UnitOfWork();

        [HttpGet]
        public ActionResult Search(
            int? clnotId = null,
            int? cltypeId = null,
            int? clindex = null,
            int? clpageSize = null,
            string clname = null,
            string clviewName = null)
        {

            List<Size> results = _context.Size.Search(
                notId: clnotId,
                pageSize: clpageSize,
                typeId: cltypeId,
                name: clname
               );

            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, clviewName, results);
        }


    }
}