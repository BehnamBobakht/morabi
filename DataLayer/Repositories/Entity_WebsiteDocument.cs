using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DataLayer.Repositories
{
    public class Entity_WebsiteDocument : BaseRepository<WebsiteDocument>
    {
        private DatabaseEntities _context;
        public Entity_WebsiteDocument(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public WebsiteDocument CreateAndUpload(Enum_Code SiteType, HttpPostedFileBase file)
        {
            if (file != null)
            {
                string PicName = GetFilePath(file.FileName);

                //string PicName = "/Uploads/2/" + file.FileName;

                WebsiteDocument document = new WebsiteDocument();
                document.Url = SiteType + PicName;
                document.PathUrl = HttpContext.Current.Server.MapPath("~/" + PicName);
                file.SaveAs(HttpContext.Current.Server.MapPath("~" + PicName));
                Insert(document);
                Save();
                return document;
            }
            else
                return null;
        }


        public WebsiteDocument Upload(Enum_Code SiteType, HttpPostedFileBase file)
        {
            if (file != null)
            {
                string PicName = GetFilePath(file.FileName);
                WebsiteDocument document = new WebsiteDocument();
                document.Url = SiteType + PicName;
                document.PathUrl = HttpContext.Current.Server.MapPath("~/" + PicName);
                file.SaveAs(HttpContext.Current.Server.MapPath("~" + PicName));

                return document;
            }
            else
                return null;
        }


        public WebsiteDocument CreateAndUpload(Enum_Code SiteType, HttpPostedFile file)
        {
            if (file != null)
            {
                string PicName = GetFilePath(file.FileName);
                WebsiteDocument document = new WebsiteDocument();
                document.Url = SiteType + PicName;
                document.PathUrl = HttpContext.Current.Server.MapPath("~/" + PicName);
                file.SaveAs(HttpContext.Current.Server.MapPath(PicName));
                Insert(document);
                Save();
                return document;
            }
            else
                return null;
        }

        private string GetFilePath(string fileName)
        {
            Random rand = new Random();
            int folder1 = rand.Next(1, 11);
            int folder2 = rand.Next(1, 11);
            int folder3 = rand.Next(1, 11);
            string PicName = BasePath.UploadPicture + folder1 + "/" + folder2 + "/" + folder3 + "/" + Guid.NewGuid() + Path.GetExtension(fileName);
            return PicName;
        }
    }
}
