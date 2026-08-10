using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Panel.Areas.Start.Controllers
{
    public class SetupController : Controller
    {
        UnitOfWork Context = new UnitOfWork();
        public ActionResult Index()
        {
            return RedirectToAction("domains");
        }

        public ActionResult Tabs()
        {
            List<SetupLevel> list = Context.SetupLevel.GetAll();
            return PartialView(list);
        }
        
        public ActionResult Domains()
        {
            Context.SetupLevel.SetCurrnetLevel(Enum_SetupLevel.DOMAINS);
            List<WebsiteDomain> list = Context.WebsiteDomain.GetAll();
            ViewBag.Message = BaseMessage.GetMessage();
            return View(list);
        }

        [HttpPost]
        public ActionResult Domains(string[] SelectedDomains)
        {
            Context.SetupLevel.SetCurrnetLevel(Enum_SetupLevel.DOMAINS);
            List<WebsiteDomain> list = Context.WebsiteDomain.GetAll();
            for (int i = 0; i < list.Count; i++)
            {
                WebsiteDomain domain = list[i];
                domain.Domain = SelectedDomains[i];
                Context.WebsiteDomain.Update(domain);
            }
            Context.Save();
            ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.SUCCESS, Enum_Message.SUCCESSFULL_SUBMIT);
            return View(list);
        }

        public ActionResult Languages()
        {
            Context.SetupLevel.SetCurrnetLevel(Enum_SetupLevel.LANGUAGES);
            List<Language> list = Context.Language.GetAll();
            ViewBag.Message = BaseMessage.GetMessage();
            return View(list);
        }

        [HttpPost]
        public ActionResult Languages(int[] SelectedLanguages)
        {
            Context.SetupLevel.SetCurrnetLevel(Enum_SetupLevel.LANGUAGES);
            List<Language> list = Context.Language.GetAll();
            if (SelectedLanguages != null)
            {
                foreach (Language item in list)
                {
                    bool IsSelected = SelectedLanguages.Any(p => p == item.ID);
                    //item.IsSelected = IsSelected;
                    Context.Language.Update(item);
                }
                Context.Save();
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.SUCCESS, Enum_Message.SUCCESSFULL_SUBMIT);
            }
            else
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_SELECT_LANGUAGE);
            return View(list);
        }

        public ActionResult Modules()
        {
            Context.SetupLevel.SetCurrnetLevel(Enum_SetupLevel.MODULES);
            List<Module> list = Context.Module.GetAll();
            ViewBag.Message = BaseMessage.GetMessage();
            return View(list);
        }

        [HttpPost]
        public ActionResult Modules(int[] SelectedModules)
        {
            Context.SetupLevel.SetCurrnetLevel(Enum_SetupLevel.MODULES);
            List<Module> list = Context.Module.GetAll();
            if (SelectedModules != null)
            {
                foreach (Module item in list)
                {
                    bool IsSelected = SelectedModules.Any(p => p == item.ID);
                    item.IsSelected = IsSelected;
                    Context.Module.Update(item);
                }
                Context.Save();
            }
            ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.SUCCESS, Enum_Message.SUCCESSFULL_SUBMIT);
            return View(list);
        }

        public ActionResult Admin()
        {
            Context.SetupLevel.SetCurrnetLevel(Enum_SetupLevel.ADMIN);
            SiteUser user = Context.SiteUser.FirstOrDefault();
            if (user == null)
            {
                user = new SiteUser() {
                    ID = 0
                };
            }
            ViewBag.Message = BaseMessage.GetMessage();
            return View(user);
        }

        public ActionResult Email()
        {
            Context.SetupLevel.SetCurrnetLevel(Enum_SetupLevel.EMAIL);
            EmailHost host = Context.EmailHost.FirstOrDefault();
            if (host == null)
            {
                host = new EmailHost()
                {
                    ID = 0
                };
            }
            ViewBag.Message = BaseMessage.GetMessage();
            return View(host);
        }

        [HttpPost]
        public ActionResult Email(EmailHost host)
        {
            Context.SetupLevel.SetCurrnetLevel(Enum_SetupLevel.EMAIL);

            if (host.ID == 0)
                Context.EmailHost.Insert(host);
            else
                Context.EmailHost.Update(host);
            Context.Save();

            ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.SUCCESS, Enum_Message.SUCCESSFULL_SUBMIT);
            return View(host);
        }

        public ActionResult Sms()
        {
            /*
            Context.SetupLevel.SetCurrnetLevel(Enum_SetupLevel.SMS);
            ViewBag.ServiceId = new SelectList(Context.SmsService.GetAll(), "ID", "WebsiteDomain", 0);
            SmsSetting sms = Context.SmsSetting.FirstOrDefault();
            if (sms == null)
            {
                sms = new SmsSetting()
                {
                    ID = 0
                };
            }
            ViewBag.Message = BaseMessage.GetMessage();
            return View(sms);
            */

            return null;
        }

        [HttpPost]
        public ActionResult Sms(SmsSetting sms)
        {
            /*
            Context.SetupLevel.SetCurrnetLevel(Enum_SetupLevel.SMS);

            ViewBag.ServiceId = new SelectList(Context.SmsService.GetAll(), "ID", "WebsiteDomain", sms.ServiceId);

            sms.ApiKey = sms.ApiKey == null ? "" : sms.ApiKey;

            if (sms.ID == 0)
                Context.SmsSetting.Insert(sms);
            else
                Context.SmsSetting.Update(sms);

            Context.Save();

            ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.SUCCESS, Enum_Message.SUCCESSFULL_SUBMIT);

            return View(sms);
            */

            return null;
        }

        public ActionResult Review()
        {
            Context.SetupLevel.SetCurrnetLevel(Enum_SetupLevel.REVIEW);

            ViewBag.Domains = Context.WebsiteDomain.GetAll();
            ViewBag.Languages = Context.Language.GetAll();
            ViewBag.Modules = Context.Module.GetAll();
            ViewBag.Email = Context.EmailHost.FirstOrDefault();
            ViewBag.Sms = Context.SmsSetting.FirstOrDefault();

            return PartialView();
        }

        [HttpPost]
        public RedirectToRouteResult Review(int? id)
        {
            List<SetupLevel> list = Context.SetupLevel.GetAll();
            foreach (SetupLevel item in list)
            {
                item.IsDone = true;
            }
            Context.Save();
            return RedirectToAction("index", "home", new { area = "" });
        }
    }
}