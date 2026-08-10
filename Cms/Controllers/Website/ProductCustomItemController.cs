using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cms.Controllers
{
    public class ProductCustomItemController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Search(int? pitypeId = null, int? pifieldId = null, int? pinotId = null, string pifieldName = null, string pivalue = null, int? piIndex = null, int? pipageSize = null, string piviewName = null)
        {
            pipageSize = pipageSize == null ? 10 : pipageSize;
            List<ProductCustomItem> list = _context.ProductCustomItem.Search(
                typeId: pitypeId,
                fieldId: pifieldId,
                fieldName: pifieldName,
                notId: pinotId,
                value: pivalue,
                pageSize: pipageSize,
                index: piIndex);
            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, piviewName, list);
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