using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DataLayer.Repositories
{
    public class Entity_WebsiteLanguage : BaseRepository<WebsiteLanguage>
    {
        private DatabaseEntities _context;
        public Entity_WebsiteLanguage(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<WebsiteLanguage> GetAllByWebsiteId(int WebsiteId)
        {
            return _context.WebsiteLanguage.Where(p => p.WebsiteId == WebsiteId).ToList();
        }

        public void SetCurrentLanguage(string languageId)
        {
            HttpCookie mylang = new HttpCookie("LANGUAGE")
            {
                Value = languageId,
                Expires = DateTime.Now.AddYears(1)
            };
            HttpContext.Current.Response.Cookies.Add(mylang);
            //HttpContext.Current.Session["LANGUAGE"] = languageId;
        }
    }
}
