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
   public static class BaseContent
    {
        public static ViewWebsiteDetail WebsiteDetail
        {
            get
            {
                UnitOfWork _context = new UnitOfWork();
                var langId = Base.BaseLanguage.GetCurrentLanguageId(_context);
                var websiteDetail = _context.WebsiteDetail.GetWebsiteDetail(langId);
                _context.Dispose();
                return new ViewWebsiteDetail(websiteDetail);

            }
        }
    }
}
