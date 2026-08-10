using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using ExpressMapper.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DataLayer.Repositories
{
    public class Entity_Picture : BaseRepository<Picture>
    {
        DatabaseEntities _context;
        public Entity_Picture(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public PictureVm GetPictureVm(int id)
        {
            var picture = GetById(id);
            if(picture!=null)
            {
                return picture.Map<Picture, PictureVm>();
            }
            return null;
        }
        public Picture CreateAndUpload(Enum_Code SiteType, HttpPostedFileBase file)
        {
            if (file != null)
            {
                string PicName = GetFilePath(file.FileName);
                Picture picture = new Picture();
                var url = PicName.Split('/');
                picture.Name = url[5].Split('.')[0];
                picture.Extension = url[5].Split('.')[1];
                picture.ContentType = file.ContentType;
                picture.Url = SiteType + PicName;
                picture.PathUrl = HttpContext.Current.Server.MapPath("~/" + PicName);
                picture.DirectUrl = picture.Url.Replace("SYSTEM_TYPE_PANEL", "/panel").Replace("/", "_");
                file.SaveAs(HttpContext.Current.Server.MapPath("~/" + PicName));
                Insert(picture);
                Save();
                return picture;
            }
            else
                return null;
        }

        public Picture CreateAndUpload(Enum_Code SiteType, HttpPostedFile file)
        {
            if (file != null)
            {
                string PicName = GetFilePath(file.FileName);
                Picture picture = new Picture();
                picture.Url = SiteType + PicName;
                picture.PathUrl = HttpContext.Current.Server.MapPath("~/" + PicName);
                file.SaveAs(HttpContext.Current.Server.MapPath(PicName));
                Insert(picture);
                Save();
                return picture;
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

        public Picture CreateNotUpload(Enum_Code SiteType, string file)
        {
            if (file != null)
            {
                Random rand = new Random();
                int folder1 = rand.Next(1, 11);
                int folder2 = rand.Next(1, 11);
                int folder3 = rand.Next(1, 11);
                string PicName = BasePath.UploadPicture + Guid.NewGuid() + Path.GetExtension(file);
                Picture picture = new Picture();
                picture.Url = SiteType + PicName;
                picture.PathUrl = HttpContext.Current.Server.MapPath("~/" + PicName);
                Insert(picture);
                Save();
                return picture;
            }
            else
                return null;
        }
    }
}
