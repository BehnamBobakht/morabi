using DataLayer.ViewModels;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using DataLayer.Enumarables;
using DataLayer.Base;
using System.Web.Helpers;
using System.Drawing;
using System.Drawing.Imaging;

namespace Panel.Controllers
{
    public class UploadController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        [HttpPost]
        public ActionResult UploadPhoto()
        {
            JsonResult result = new JsonResult();
            HttpFileCollectionBase FileCollection = Request.Files;
            if (FileCollection.Count > 0)
            {
                HttpPostedFileBase file = FileCollection[0];
                string extension = Path.GetExtension(file.FileName);
                extension = extension.ToLower();

                if (extension == ".webp" || extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".bmp" || extension == ".mp4" || extension == ".pdf" || extension == ".mp3" || extension == ".svg" || extension == ".gif")
                {
                    if (file != null)
                    {
                        Picture picture = _context.Picture.CreateAndUpload(Enum_Code.SYSTEM_TYPE_PANEL, file);
                        result = new JsonResult() { Data = new ViewPicture(picture) };
                    }
                }
            }
            return result;
        }

        [HttpPost] 
        public ActionResult UploadDocument() 
        {
            JsonResult result = new JsonResult();
            HttpFileCollectionBase FileCollection = Request.Files;
            if (FileCollection.Count > 0)
            {
                HttpPostedFileBase file = FileCollection[0];
                string extension = Path.GetExtension(file.FileName);
                extension = extension.ToLower();
                if (file != null)
                {
                    WebsiteDocument doc = _context.WebsiteDocument.CreateAndUpload(Enum_Code.SYSTEM_TYPE_PANEL, file);
                    result = new JsonResult() { Data = new ViewDocument(doc) };
                }
            }
            return result;
        }


        [HttpPost]
        public ActionResult UploadJustSaveDocument()
        {
            JsonResult result = new JsonResult();
            HttpFileCollectionBase FileCollection = Request.Files;
            if (FileCollection.Count > 0)
            {
                HttpPostedFileBase file = FileCollection[0];
                string extension = Path.GetExtension(file.FileName);
                extension = extension.ToLower();
                if (file != null)
                {
                    WebsiteDocument doc = _context.WebsiteDocument.Upload(Enum_Code.SYSTEM_TYPE_PANEL, file);
                    result = new JsonResult() { Data = new ViewDocument(doc) };
                }
            }
            return result;
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}