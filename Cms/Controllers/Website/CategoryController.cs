using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Binbin.Linq;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System.Web.Http;



namespace Cms.Controllers.Website
{
    public class CategoryController : Controller
    {
        // GET: Category
        UnitOfWork _context = new UnitOfWork();
        public ActionResult Index(string name = "",string viewName="index")
        {
            var model = _context.Category.GetByLabel(name);
            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, viewName, model);
        }
        public ActionResult Search(
              int? pctypeId = null,
              string pctype = null,
              int? pcnotId = null,
              int? pcindex = null,
              int? pcpageSize = null,
              string pcname = null,
              string pcviewName = null,
              string keyWord = null,
              Enum_ResultType pcresultType = Enum_ResultType.RESULT_TYPE_CONTROLLER)
        {
            var MyQuery = PredicateBuilder.True<Category>();
            pcpageSize = pcpageSize == null ? 10 : pcpageSize;
            pcindex = pcindex == null ? 1 : pcindex;
            List<Category> list = _context.Category.Search(
                notId: pcnotId,
                index: pcindex.Value,
                name: pcname,
                pageSize: pcpageSize.Value,
                keyWord: keyWord,
                typeId: pctypeId,
                type : pctype

                );

            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, pcviewName, list);
        }

    }
}