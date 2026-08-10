using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_WebsiteDetail : BaseRepository<WebsiteDetail>
    {
        private DatabaseEntities _context;
        public Entity_WebsiteDetail(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public WebsiteDetail GetWebsiteDetail(int langId)
        {
            var websiteDetail = _context.WebsiteDetail.FirstOrDefault(s=>s.LanguageId==langId);
            if (websiteDetail == null)
            {
                websiteDetail = new WebsiteDetail() { LanguageId=langId };
                _context.WebsiteDetail.Add(websiteDetail);
                Save();
            }
            return websiteDetail;
        }


    }
}
