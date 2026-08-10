using DataLayer.Entities;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cms.Areas.Store.Controllers
{
    public class StateCityController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();
        public ActionResult GetStatesDD(int stateId=0,bool withAll=true)
        {   
            var model = _context.State.Search(withAll:withAll);
            ViewBag.StateId = new SelectList(model, "ID", "Name",stateId);
            return PartialView(model);
        }    
        public ActionResult GetCitiesDD(int stateId = 0,int cityId=0,bool withAll=true)
        {
            var model = _context.City.Search(stateId:stateId,withAll:withAll);
            ViewBag.CityId = new SelectList(model, "ID", "Name", cityId);
            return PartialView(model);
        }

    }
}