using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer.Enumarables;
using DataLayer.Base;


namespace Cms.Controllers.Website
{
    public class OnlineOrderController : Controller
    {
        // GET: OnlineOrder
        UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            OnlineOrder onlineOrderModel = new OnlineOrder();
            return PartialView(BaseController.GetView(this), onlineOrderModel);
        }

        [HttpPost]
        public ActionResult Index(OnlineOrder order)
        {
            string randomnumebr = "";

            if (order.NationalCode == null || order.FileId == null)
            {
                ViewBag.onlineOrder = "لطفا اطلاعات ضروری را وراد کنید";
                return PartialView(BaseController.GetView(this), order);
            }
            else if (BaseSecurity.IsValidInput(order.NationalCode.ToString(), Enum_Validation.NationalCode) == false)
            {
                ViewBag.onlineOrder = " فرمت کد ملی صحیح نمی باشد";

            }
            else
            {
                do
                {
                    randomnumebr = DataLayer.Base.BaseRandom.GetRandomNumber(3);
                } while (
                   _context.OnlineOrder.CheckNotExistRefrenceCode(randomnumebr)
                );
                order.DateTimeOrder = DateTime.Now;
                order.RefrenceCode = randomnumebr;
                order.StatusOrder = _context.Code.GetByLabel(Enum_Code.Order_Register).ID;
                _context.OnlineOrder.Insert(order);
                _context.Save();
            }
            return PartialView(BaseController.GetView(this, "orderTrack"), order);
        }
        public ActionResult trackorder()
        {
            OnlineOrder onlineOrderModel = new OnlineOrder();
            return PartialView(BaseController.GetView(this), onlineOrderModel);
        }

        [HttpPost]
        public ActionResult trackorder(OnlineOrder order)
        {
            if (order.RefrenceCode == null)
            {
                ViewBag.onlineOrder = "لطفا کد پیگیری را وارد کنید";
            }
            else
            {
                OnlineOrder orderModel = _context.OnlineOrder.FirstOrDefault(x => x.RefrenceCode == order.RefrenceCode);
                if (orderModel == null)
                {
                    ViewBag.onlineOrder = " کد پیگیری صحیح نمی باشد";
                }
                else
                {
                    return PartialView(BaseController.GetView(this, "orderTrack"), orderModel);
                }
            }
            return PartialView(BaseController.GetView(this), order);
        }


        public ActionResult sendSpell()
        {
            OnlineOrder onlineOrderModel = new OnlineOrder();
            return PartialView(BaseController.GetView(this), onlineOrderModel);
        }


        [HttpPost]
        public ActionResult sendSpell(OnlineOrder order)
        {
            if (order.RefrenceCode == null || order.PassportFileID == null)
            {
                ViewBag.onlineOrder = "لطفا موارد ضروری را وارد کنید";
            }
            else
            {
                OnlineOrder orderModel = _context.OnlineOrder.FirstOrDefault(x => x.RefrenceCode == order.RefrenceCode);
                if (orderModel == null)
                {
                    ViewBag.onlineOrder = " کد پیگیری صحیح نمی باشد";
                }
                else
                {
                    orderModel.PassportFileID = order.PassportFileID;
                    orderModel.PlaceBirth = order.PlaceBirth;
                    orderModel.BirthDay = order.BirthDay;
                    orderModel.FatherName = order.FatherName;
                    orderModel.Name = order.Name;
                    _context.OnlineOrder.Update(orderModel);
                    _context.Save();
                    ViewBag.onlineOrder = "با موفقیت ثبت انجام شد";
                }
            }

            return PartialView(BaseController.GetView(this), order);

        }


    }
}