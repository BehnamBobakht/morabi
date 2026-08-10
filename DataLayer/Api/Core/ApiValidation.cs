using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DataLayer.Api
{
    public class ApiValidation
    {
        public static bool WebValidate(UnitOfWork _context)
        {
            string host = BaseUrl.CurrentUrl;
            string api_type = Enum_Code.SYSTEM_TYPE_API.ToString();
            string web_token = HttpContext.Current.Request.Headers["WEB_TOKEN"];
            return _context.WebsiteDomain.HasAnyByWebsiteTypeAndDomain(api_type, host, web_token);
        }

        public static AccountLog Validate(UnitOfWork _context)
        {
            string UNIQUE_ID = HttpContext.Current.Request.Headers["UNIQUE_ID"];
            string UNIQUE_KEY = HttpContext.Current.Request.Headers["UNIQUE_KEY"];

            if (UNIQUE_ID.IsGuid() && UNIQUE_KEY.IsGuid())
            {
                return _context.AccountLog.GetByKeyAndAccountUniqueId(UNIQUE_KEY.GetGuid(), UNIQUE_ID.GetGuid());
            }
            return null;
        }  
        public static string CartToken()
        {
            string cartToken = HttpContext.Current.Request.Headers["CART_TOKEN"];

            if (string.IsNullOrEmpty(cartToken))
            {
                return cartToken;
            }
            return null;
        }

        public static PaymentWebsite ExternalPaymentValidate(UnitOfWork _context, string token = null, bool withOutToken = false)
        {
            //string ip = HttpContext.Current.Request.UserHostAddress;
            string ip = Base.BaseSecurity.GetClientIPAddress();
            string domain = HttpContext.Current.Request.Url.Host.Replace("www", "").Replace("http://", "").Replace("https://", "");
            //string domain = "talfighbeta.com";
            return _context.PaymentWebsite.Authenticate(token, ip, domain, withOutToken);

        }
        public static string ExternalPaymentValidateParams(string token, string merchant, int amount, string currency, string orderId, string sign)
        {      
            var hash = Base.BaseSecurity.GetSha1(token + merchant + amount + currency + orderId);          
            if (hash == sign)
                return "1";
            return hash;
        }
    }
}
