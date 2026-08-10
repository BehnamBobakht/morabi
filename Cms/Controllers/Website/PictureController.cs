using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.ViewModels;
using System.Web.Mvc;

namespace Cms.Controllers.Website
{
    public class PictureController : Controller
    {
        UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ThumbUrl(string picurl, int width = 0, int height = 0, int quality = 0,bool orginal=false)
        {
            if (orginal && BaseWebsite.WebsiteSetting.CdnPictures)
            {
                picurl = string.Format("{0}/o{1}", picurl, orginal);
            }
             else if (BaseWebsite.WebsiteSetting.CdnPictures)
            {
                picurl = string.Format("{0}/w{1}/h{2}/q{3}", picurl, width, height, quality);
            }
            
            return PartialView("~/Views/Default/Partial/SimpleString.cshtml", picurl);
        }
        public ActionResult ThumbId(int picid, int width = 0, int height = 0, int quality = 0, bool orginal = false)
        {
            var picture = _context.Picture.GetPictureVm(picid);

            if (picture != null)
            {
            var url = "";

                if (orginal && BaseWebsite.WebsiteSetting.CdnPictures)
                {
                    url = string.Format("{0}/o{1}", picture.Url, orginal);
                }
                else if(BaseWebsite.WebsiteSetting.CdnPictures)
                    url = string.Format("{0}/w{1}/h{2}/q{3}", picture.Url, width, height, quality);
                else
                    url = picture.Url;
                return PartialView("~/Views/Default/Partial/SimpleString.cshtml", url);
            }

            return null;
        }
        public ActionResult ThumbVm(PictureVm pic, int width = 0, int height = 0, int quality = 0, bool orginal = false)
        {
            if (pic != null)
            {
                var url = "";
                if (orginal && BaseWebsite.WebsiteSetting.CdnPictures)
                {
                    url = string.Format("{0}/o{1}", pic.Url, orginal);
                    return PartialView("~/Views/Default/Partial/SimpleString.cshtml", url);

                }
                else if (BaseWebsite.WebsiteSetting.CdnPictures)
                {
                     url = string.Format("{0}/w{1}/h{2}/q{3}", pic.Url, width, height, quality);
                    return PartialView("~/Views/Default/Partial/SimpleString.cshtml", url);
                }  
                else 
                {
                    return PartialView("~/Views/Default/Partial/SimpleString.cshtml", pic.Url);
                }

            }
            return null;
        }
    }
}