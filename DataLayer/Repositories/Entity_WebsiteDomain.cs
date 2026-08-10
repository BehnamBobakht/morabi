using DataLayer.Entities;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_WebsiteDomain : BaseRepository<WebsiteDomain>
    {
        DatabaseEntities _context;
        public Entity_WebsiteDomain(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public WebsiteDomain GetByWebsiteType(string type_string)
        {
            return _context.WebsiteDomain.FirstOrDefault(p => p.Website.Code.Label == type_string);
        }

        public bool HasAnyByWebsiteTypeAndDomain(string type, string domain, string key)
        {
            return _context.WebsiteDomain.Any(p => 
                p.Website.Code.Label == type &&
                p.Domain == domain &&
                p.ActivationKey == key);
        }

        public List<WebsiteDomain> GetAllByWebsiteId(int WebsiteId)
        {
            return _context.WebsiteDomain.Where(p => p.WebsiteId == WebsiteId).ToList();
        }

        public WebsiteDomain GetByUrl(Uri url)
        {
            string label = url.Host == "localhost" ? url.Host + ":" + url.Port : url.Host;
            return  _context
                    .WebsiteDomain
                    .FirstOrDefault(p =>
                        p.Domain == "http://" + label + "/" ||
                        p.Domain == "http://" + label ||
                        p.Domain == "https://" + label + "/" ||
                        p.Domain == "https://" + label ||
                        p.Domain == label);

        }
    }
}
