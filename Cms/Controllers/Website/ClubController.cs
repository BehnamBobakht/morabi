using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cms.Controllers
{
    public class ClubController : Controller
    {
        UnitOfWork _context = new UnitOfWork();
        public ActionResult Search(
           DataLayer.Enumarables.Enum_Code typeId,
     int? stateId = null,
     bool? isTop = false,
     bool? active = true,


        int? baIndex = 1,
     int? baSize = 20,
     string baviewName = "search"

)
        {
            baIndex = baIndex != null ? baIndex.Value : 1;
            baSize = baSize != null ? baSize.Value : 10;
            List<Clubs> list = _context.Clubs.Search(
                stateId: stateId,
                typeId: typeId,
                isTop: isTop,
                index: baIndex.Value,
                pageSize: baSize.Value,
                active: active);
            ViewBag.TypeId = typeId.ToString();
            if (stateId != null)
            {
                ViewBag.StateName = _context.State.GetById(stateId).Name;
            }
            
            ViewBag.TotalCount = _context.Clubs.SearchCount(
                stateId: stateId,
                typeId: typeId,
                isTop: isTop,
                active: active);
            ViewBag.StateId = stateId;
            ViewBag.IsTop = isTop;
            ViewBag.PageIndex = baIndex;
            ViewBag.PageSize = baSize;
            return PartialView(BaseController.GetView(this), list);
        }
        public ActionResult Search2(
          DataLayer.Enumarables.Enum_Code typeId,
    int? stateId = null,
    bool? isTop = true,
    bool? active = true,


       int? baIndex = 1,
    int? baSize = 20,
    string baviewName = "search2"

)
        {
            baIndex = baIndex != null ? baIndex.Value : 1;
            baSize = baSize != null ? baSize.Value : 10;
            List<Clubs> list = _context.Clubs.Search(
                stateId: stateId,
                typeId: typeId,
                isTop: isTop,
                index: baIndex.Value,
                pageSize: baSize.Value,
                active: active);
            ViewBag.TypeId = typeId.ToString();
            ViewBag.Typeuser = typeId;

            ViewBag.TotalCount = _context.Clubs.SearchCount(
                stateId: stateId,
                typeId: typeId,
                isTop: isTop,
                active: active);
            ViewBag.StateId = stateId;
            ViewBag.IsTop = isTop;
            ViewBag.PageIndex = baIndex;
            ViewBag.PageSize = baSize;
            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, baviewName, list);
        }
        public ActionResult Map(
           DataLayer.Enumarables.Enum_Code typeId,
     int? stateId = 8,
     bool? isTopMap = true,
     bool? active = true,


        int? baIndex = 1,
     int? baSize = 100,
     string baviewName = "Map"

)
        {
            baIndex = baIndex != null ? baIndex.Value : 1;
            baSize = baSize != null ? baSize.Value : 100;
            List<Clubs> list = _context.Clubs.Search(
                stateId: stateId,
                typeId: typeId,
                isTopMap: isTopMap,
                index: baIndex.Value,
                pageSize: baSize.Value,
                active: active);
            ViewBag.TypeId = typeId.ToString();
            ViewBag.Typeuser = typeId;

            ViewBag.TotalCount = _context.Clubs.SearchCount(
                stateId: stateId,
                typeId: typeId,
                isTop: isTopMap,
                active: active);
            ViewBag.StateId = stateId;
            ViewBag.IsTop = isTopMap;
            ViewBag.PageIndex = baIndex;
            ViewBag.PageSize = baSize;

            return PartialView(BaseController.GetView(this), list);
        }
        public ActionResult Index(int id)
        {


            Clubs model = _context.Clubs.GetById(id);
           
            return PartialView(BaseController.GetView(this), model);

        }
        public ActionResult Create(string typeId)
        {

            ViewBag.TypeId = typeId.ToString();
            Clubs model = new Clubs();
            var id = _context.Code.FirstOrDefault(s => s.Label == typeId.ToString()).ID;
            model.Type = id;
            return PartialView(BaseController.GetView(this), model);

        }
        [HttpPost]
        public ActionResult Create(Clubs model)
        {
            model.ShowNumbr = 10000;
            _context.Clubs.Insert(model);
            _context.Save();
            Clubs result = new Clubs();
            result.Type = model.Type;
            ViewBag.TypeId = _context.Code.FirstOrDefault(s => s.ID == model.Type).Label;
            ViewBag.Message = "اطلاعات شما با موفقیت ثبت شد";
            return PartialView(BaseController.GetView(this), result);

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