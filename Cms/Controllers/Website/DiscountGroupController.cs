using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Documents;

namespace Cms.Controllers.Website
{
    public class DiscountGroupController : Controller
    {
        UnitOfWork _context = new UnitOfWork();
        public ActionResult GetByLabel(string label,string dgviewName)
        {
            var item =_context.DiscountGroup.GetByLabel(label);
            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, dgviewName, item);
        }
        public ActionResult Search(
            Enum_Code dgType = Enum_Code.DISCOUNT_TYPE_NONE,
            Enum_Code dgValueType = Enum_Code.DISCOUNTGROUP_NONE,
            string dglabel = null,
            int? dgnotId = null,
            int? dgIndex = null,
            int? dgSize = null,
            Boolean? active = null ,
            string dgviewName = null)
        {
            dgIndex = dgIndex != null ? dgIndex.Value : 1;
            dgSize = dgSize != null ? dgSize.Value : 10;
            List<DiscountGroup> list = _context.DiscountGroup.Search(
                valueType: dgValueType,
                type: dgType,
                notId: dgnotId,
                label: dglabel,
                index: dgIndex.Value,
                pageSize: dgSize.Value ,
               active : active
                );
            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, dgviewName, list);
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