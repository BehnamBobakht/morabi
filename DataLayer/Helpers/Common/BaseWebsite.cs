using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DataLayer.Base
{
    public class BaseWebsite
    {
        private static string panel_url = null;
        private static string shop_url = null;
        private static string post_url = null;
        private static string sync_url = null;
        private static string wildcard_url = null;
        //private static string website_label = null;
        private static ViewWebsite shop_website = null;
        private static ViewAccount _account = null;
        private static SiteUser _user = null;
        private static ViewShopWebsiteSetting _websiteSetting = null;

        public static string WebsiteLabel
        {
            set
            {
                HttpContext.Current.Session["WEBSITE_LABEL"] = value;
            }
            get
            {
                return "Morabiiran";
                //if (HttpContext.Current.Session["WEBSITE_LABEL"] == null)
                //{
                //    UnitOfWork _context = new UnitOfWork();
                //    Uri url = HttpContext.Current.Request.Url;
                //    string label = url.Host == "localhost" ? url.Host + ":" + url.Port : url.Host;
                //    string wildCardDomain = WildCardDomain;
                //    if (label.Count(p => p == '.') > 1 && string.IsNullOrEmpty(wildCardDomain) == false)
                //    {
                //        string[] domains = label.Split(new char[] { '.' });
                //        string subDomain = domains[0];
                //        label = label.Replace(subDomain + ".", "");
                //    }
                //    WebsiteDomain domain = _context
                //        .WebsiteDomain
                //        .FirstOrDefault(p => (
                //            p.Domain == "http://" + label + "/" ||
                //            p.Domain == "http://" + label ||
                //            p.Domain == "https://" + label + "/" ||
                //            p.Domain == "https://" + label ||
                //            p.Domain == label) && p.Website.Code.Label == Enum_Code.SYSTEM_TYPE_SHOP.ToString());
                //    if (domain != null)
                //    {
                //        Template template = domain.Website.Template;
                //        if (template != null)
                //            label = template.Label;
                //        else
                //            label = "Default";

                //        if (domain.LanguageId != null)
                //        {
                //            _context.WebsiteLanguage.SetCurrentLanguage(domain.Language.Culture);
                //        }

                //        HttpContext.Current.Session["WEBSITE_LABEL"] = label;
                //    }
                //    else
                //        label = "Default";
                //    _context.Dispose();
                //    return label;
                //}
                //else
                //    return HttpContext.Current.Session["WEBSITE_LABEL"].ToString();
            }
        }

        public static ViewAccount CurrentAccount
        {
            get
            {
                UnitOfWork _context = new UnitOfWork();
                _account = _context.Account.GetCurrentAccount();
                _context.Dispose();
                return _account;
            }
        }
        public static ViewAccount CurrentAccountOnline
        {
            get
            {
                UnitOfWork _context = new UnitOfWork();
                _account = _context.Account.GetCurrentAccount();
                if (_account != null)
                {
                    var account = _context.Account.GetById(_account.Id);
                    ViewAccount entity = new ViewAccount(account);
                    _context.Dispose();
                    return entity;
                }
                else
                {
                    return _account;
                }
              
            }
        }
        public static ViewLocation CurrentLocation
        {
            get
            {
                UnitOfWork _context = new UnitOfWork();
                ViewLocation location = _context.Account.GetCurrentLocation();
                _context.Dispose();
                return location;
            }
        }
        public static SiteUser CurrentSiteUser
        {
            set
            {
                _user = value;
            }
            get
            {
                if (_user == null)
                {
                    UnitOfWork _context = new UnitOfWork();
                    _user = _context.SiteUser.GetCurrentUser();
                    _context.Dispose();
                }
                return _user;
            }
        }

        public static List<AccountBasket> GetCurrentBasket
        {
            get
            {
                List<AccountBasket> list = new List<AccountBasket>();
                UnitOfWork _context = new UnitOfWork();
                ViewAccount account = CurrentAccount;
                if (account != null)
                    list = _context.AccountBasket.GetAllByAccount(account.Id);
                else
                    list = _context.AccountBasket.GetAllFromCookie();
                _context.Dispose();
                return list;
            }
        }

        public static string SyncUrl
        {
            get
            {
                if (sync_url == null)
                {
                    UnitOfWork _context = new UnitOfWork();
                    string type_sync = Enum_Code.SYSTEM_TYPE_SYNCSERVER.ToString();
                    WebsiteDomain domain = _context.WebsiteDomain.FirstOrDefault(p => p.Website.Code.Label == type_sync);
                    if (domain != null)
                        sync_url = domain.Domain;
                    _context.Dispose();
                }
                return sync_url;
            }
        }

        public static string ShopUrl
        {
            get
            {
                if (shop_url == null)
                {
                    UnitOfWork _context = new UnitOfWork();
                    string type_shop = Enum_Code.SYSTEM_TYPE_SHOP.ToString();
                    WebsiteDomain domain = _context.WebsiteDomain.FirstOrDefault(p => p.Website.Code.Label == type_shop);
                    if (domain != null)
                        shop_url = domain.Domain;
                    _context.Dispose();
                }
                return shop_url;
            }
        }
        public static string PostUrl
        {
            get
            {
                if (post_url == null)
                {
                    UnitOfWork _context = new UnitOfWork();
                    string type_shop = Enum_Code.SYSTEM_TYPE_SHOP.ToString();
                    WebsiteDomain domain = _context.WebsiteDomain.FirstOrDefault(p => p.Website.Code.Label == type_shop);
                    if (domain != null && (!string.IsNullOrEmpty(domain.PostUrl)))
                        post_url = domain.PostUrl;
                    else
                    {
                        post_url = "po";
                    }
                    _context.Dispose();
                }
                return post_url;
            }
        }

        public static ViewWebsite ShopWebsite
        {
            get
            {
                if (shop_website == null)
                {
                    UnitOfWork _context = new UnitOfWork();
                    string type_shop = Enum_Code.SYSTEM_TYPE_SHOP.ToString();
                    WebsiteDomain domain = _context.WebsiteDomain.FirstOrDefault(p => p.Website.Code.Label == type_shop);
                    if (domain != null)
                    {
                        shop_website = new ViewWebsite(domain.Website);
                        shop_website.Domain = domain.Domain;
                    }
                    _context.Dispose();
                }
                return shop_website;
            }
        }

        public static string PanelUrl
        {
            get
            {
                if (panel_url == null)
                {
                    UnitOfWork _context = new UnitOfWork();
                    string type_panel = Enum_Code.SYSTEM_TYPE_PANEL.ToString();
                    WebsiteDomain domain = _context.WebsiteDomain.FirstOrDefault(p => p.Website.Code.Label == type_panel);
                    if (domain != null)
                        panel_url = domain.Domain;
                    _context.Dispose();
                }
                return panel_url;
            }
        }

        public static int WebsiteId
        {
            get
            {
                int Id = 0;
                UnitOfWork _context = new UnitOfWork();
                Uri url = HttpContext.Current.Request.Url;

                string label = url.Host == "localhost" ? url.Host + ":" + url.Port : url.Host;
                if (label.Count(p => p == '.') > 1)
                {
                    string[] domains = label.Split(new char[] { '.' });
                    string subDomain = domains[0];
                    label = label.Replace(subDomain + ".", "");
                }

                WebsiteDomain domain = _context.WebsiteDomain.FirstOrDefault(p =>
                    p.Domain == "http://" + label + "/" ||
                    p.Domain == "http://" + label ||
                    p.Domain == "https://" + label + "/" ||
                    p.Domain == "https://" + label);

                if (domain != null)
                    Id = domain.WebsiteId;
                _context.Dispose();
                return Id;
            }
        }

        public static string WildCardDomain
        {
            get
            {
                if (wildcard_url == null)
                {
                    string domainUrl = null;
                    UnitOfWork _context = new UnitOfWork();
                    Uri url = HttpContext.Current.Request.Url;
                    string label = url.Host == "localhost" ? url.Host + ":" + url.Port : url.Host;
                    WebsiteDomain domain = _context.WebsiteDomain.FirstOrDefault(p => p.Domain.Contains("*."));
                    if (domain != null)
                        domainUrl = domain.Domain;
                    else
                        domainUrl = "";
                    _context.Dispose();
                    return domainUrl;
                }
                else
                    return wildcard_url;
            }
        }
        public static ViewShopWebsiteSetting WebsiteSetting
        {
            get
            {
                if (_websiteSetting == null)
                {
                    UnitOfWork _context = new UnitOfWork();
                    Website website = _context.Website.GetFirstByType(Enum_Code.SYSTEM_TYPE_SHOP);
                    var setting = _context.ShopWebsiteSetting.GetSettingByWebsiteId(website.ID);
                    _context.Dispose();
                    var shopSetting = new ViewShopWebsiteSetting(setting);
                    _websiteSetting = shopSetting;
                    return shopSetting;
                }
                else
                {
                    return _websiteSetting;
                }
            }
        }
        public static void UpdatingLastContent()
        {
            try
            {
                UnitOfWork _context = new UnitOfWork();
                Website website = _context.Website.GetFirstByType(Enum_Code.SYSTEM_TYPE_SHOP);
                var setting = _context.ShopWebsiteSetting.GetSettingByWebsiteId(website.ID);
                setting.LastContentUpdate = DateTime.Now;
                _context.ShopWebsiteSetting.Update(setting);
                _context.Save();
                _context.Dispose();
            }
            catch
            {

            }

        }
    }
}
