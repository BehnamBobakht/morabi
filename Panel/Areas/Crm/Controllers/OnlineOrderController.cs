using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Panel.Areas.Crm.Controllers
{
    public class OnlineOrderController : Controller
    {
        // GET: Crm/OnlineOrder

        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            var model = _context.OnlineOrder.GetAllOnlineOrder();
            return View(model);
        }

        public ActionResult Edit(int? Id)
        {
            var model = _context.OnlineOrder.Where(x => x.ID == Id.Value).FirstOrDefault();
            List<SelectListItem> StatusOrder = new List<SelectListItem>();
            var modelCode = _context.Code.Where(x => x.CodeGroup.Label == "OrderStatusOnline").ToList();

            foreach (var item in modelCode)
            {
                StatusOrder.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.ID.ToString(),
                    Selected = model.StatusOrder == item.ID ? true : false
                });
            }
            ViewBag.StatusOrder = StatusOrder;
            return View(model);
        }


        [HttpPost]
        public ActionResult Edit(OnlineOrder model)
        {
            try
            {
                var modelOnlineOrder = _context.OnlineOrder.Where(x => x.ID == model.ID).FirstOrDefault();
                modelOnlineOrder.Price = model.Price;
                modelOnlineOrder.StatusOrder = model.StatusOrder;
                _context.OnlineOrder.Update(modelOnlineOrder);
                _context.Save();
                ViewBag.onlineMessage = "به روز رسانی با موفقیت انجام شد";
            }
            catch
            {
                ViewBag.onlineMessage = "لطفا اطلاعات را صحیح وارد کنید ";
            }
            var modelCode = _context.Code.Where(x => x.CodeGroup.Label == "OrderStatusOnline").ToList();
            List<SelectListItem> StatusOrder = new List<SelectListItem>();
            foreach (var item in modelCode)
            {
                StatusOrder.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.ID.ToString(),
                    Selected = model.StatusOrder == model.ID ? true : false
                });
            }

            ViewBag.StatusOrder = StatusOrder;

            return View(model);
        }
    }
}