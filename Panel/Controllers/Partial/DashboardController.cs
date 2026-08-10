using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Panel.Controllers
{
    public class DashboardController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Comment(string viewurl)
        {
            int count = _context.ProductComment.Search(notSee: true, pageSize: 1000).Count;
            return View(viewurl, count);
        }
        
        public ActionResult Order(string viewurl)
        {
            string payment_success_status = Enum_Code.ORDER_STATUS_SUCCESS.ToString();
            int[] statusId = { 2068 };
            int count = _context.AccountOrder.SearchCount(statusId: statusId);
            return View(viewurl, count);
        }

        public ActionResult Chat(string viewurl, int shopId)
        {
            Shop shop = _context.Shop.GetById(shopId);
            List<ViewShopChat> listChats = new List<ViewShopChat>();
            List<ShopChat> listShopChat = shop.ShopChat.OrderByDescending(p => p.Datetime).ToList();
            var listChat = listShopChat.GroupBy(p => p.AccountId);
            foreach (var item in listChat)
            {
                ShopChat lastChat = listShopChat.FirstOrDefault(s => s.AccountId == item.Key);
                listChats.Add(new ViewShopChat()
                {
                    UnreadCount = listShopChat.Count(p =>
                        p.IsRead == false &&
                        p.AccountId == item.Key &&
                        p.IsAccountSend == true),
                    Body = lastChat.Body,
                    Datetime = lastChat.Datetime.Value,
                    Id = item.Key.Value,
                    IsAccountSend = lastChat.IsAccountSend.Value,
                    IsRead = lastChat.IsRead.Value,
                    Account = new ViewAccount(lastChat.Account)
                });
            }
            
            return View(viewurl, listChats.OrderBy(p => p.Id).ToList());
        }





        public ActionResult Package(string viewurl, int shopId)
        {
            ViewBag.ShopId = shopId;
            return View(viewurl, _context.Package.GetByShopId(shopId));
        }
        public ActionResult StoreProduct(string viewurl, int shopId)
        {
            ViewBag.ShopId = shopId;
            return View(viewurl, _context.StoreProduct.GetAllNotApproved());
        }

        public ActionResult ShopResellerProduct(string viewurl, int ? shopId=null)
        {
            ViewBag.ShopId = shopId;
            return View(viewurl, _context.ShopResellerProduct.GetAllNotApproved());
        }

        public ActionResult VisitLastWeek(string viewurl)
        {
            return View(viewurl);
        }

        public ActionResult AlexaRank(string viewurl)
        {
            List<AlexaRank> listRank = new List<AlexaRank>();
            List<Website> listWebsite = _context.Website.GetAllByType(Enum_Code.SYSTEM_TYPE_CMS, Enum_Code.SYSTEM_TYPE_SHOP, Enum_Code.SYSTEM_TYPE_WEBSITE);
            foreach (Website website in listWebsite)
            {
                List<WebsiteDomain> listDomain = website.WebsiteDomain.Where(p =>
                    p.Domain.Contains("localhost") == false &&
                    p.Domain.Contains("*") == false).ToList();

                foreach (WebsiteDomain domain in listDomain)
                {
                    AlexaRank rank = domain.AlexaRank.OrderByDescending(p => p.ID).FirstOrDefault();
                    if (rank != null)
                    {
                        listRank.Add(rank);
                    }
                }
            }
            return View(viewurl, listRank);
        }

        public ActionResult CheckRebate(string viewurl)
        {
            return PartialView(viewurl);
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