//using DataLayer.ViewModels;
//using DataLayer.Entities;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Threading;
//using System.Web;
//using System.Web.Mvc;
//using DataLayer.Enumarables;
//using DataLayer.Base;
//using System.Web.Helpers;
//using System.Drawing;
//using System.Drawing.Imaging;
//using Microsoft.Office.Interop.Excel;
//using Microsoft.Office.Interop;
//using System.Data.OleDb;
//using System.Data;
//using static Stimulsoft.Report.StiOptions.Export;


//namespace Panel.Controllers
//{
//    public class DownloadController : Controller
//    {
//        private UnitOfWork _context = new UnitOfWork();
//        public string excelParsing()
//        {
//            var path = Server.MapPath("/panel/Uploads/5/3/5/6a4c6bac-4af5-438a-ae5c-f523d358af8c.xlsx");

//            string data = "";
//            //Create COM Objects. Create a COM object for everything that is referenced
//            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
//            Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(path);
//            Microsoft.Office.Interop.Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
//            Microsoft.Office.Interop.Excel.Range xlRange = xlWorksheet.UsedRange;

//            int rowCount = xlRange.Rows.Count;
//            int colCount = xlRange.Columns.Count;

//            //iterate over the rows and columns and print to the console as it appears in the file
//            //excel is not zero based!!
//            for (int i = 1; i <= rowCount; i++)
//            {
//                for (int j = 1; j <= colCount; j++)
//                {
//                    //either collect data cell by cell or DO you job like insert to DB 
//                    if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
//                        data += xlRange.Cells[i, j].Value2.ToString();
//                }
//            }

//            return data;
//        }


//        public ActionResult UpdatePictures()
//        {
//            var items = _context.Picture.GetAll();
//            foreach (var item in items)
//            {
//                var url = item.Url.Split('/');
//                item.Name = url[5].Split('.')[0];
//                item.Extension = url[5].Split('.')[1];
//                item.ContentType = "image/" + url[5].Split('.')[1];
//                item.DirectUrl = item.Url.Replace("SYSTEM_TYPE_PANEL", "/panel").Replace("/", "_");

//                _context.Picture.Update(item);
//            }
//            _context.Save();
//            return null;
//        }
//        [CustomCache(Duration = 1, cacheType = Enum_OutputCache.Picture,
//            VaryByParam = "*",Location =System.Web.UI.OutputCacheLocation.ServerAndClient)]
//        public ActionResult GetThumb(string picurl, int width = 0, int height = 0, int quality = 0,bool orginal=false)
//        {
//            try
//            {
//                if(quality==0)
//                {
//                    quality = BaseWebsite.WebsiteSetting.CdnPicturesQuality;
//                }
//                var address = picurl.Replace("__", ".").Replace("_", "/");
//                var path = Server.MapPath(address);

//                var extension = address.Split('.')[1].ToLower();

//                var contentType = "";

//                if ((extension == "jpg" || extension == "jpeg" || extension == "png" || extension == "bmp") && orginal==false )
//                {

//                        contentType = "image/jpeg";


//                    Image sourceFile = new Bitmap(path);

//                   var orginalProperties= sourceFile.PropertyItems;
//                    #region Processing Image Files

//                    Stream stream = (new MemoryStream());
//                    if(width>0 && height == 0)
//                    {
//                        var ratio = (float)width / (float)sourceFile.Width;
//                        height = (int)((float)sourceFile.Height * ratio);
//                    }
//                    if (height>0 && width == 0)
//                    {
//                        var ratio = (float)height / (float)sourceFile.Height;
//                        width = (int)((float)sourceFile.Width * ratio);
//                    }
//                    width = width > 0 ? width : sourceFile.Width;
//                    height = height > 0 ? height : sourceFile.Height;

//                    //if (sourceFile.Height * width / sourceFile.Width > height)
//                    //{
//                    //    sourceFile = new Bitmap(sourceFile, width, sourceFile.Height * width / sourceFile.Width);
//                    //}
//                    //else
//                    //{
//                    //    sourceFile = new Bitmap(sourceFile, sourceFile.Width * height / sourceFile.Height, height);
//                    //}



//                    Image resultImage = new Bitmap(width: width, height: height);
//                    using (Graphics graphics = Graphics.FromImage(resultImage))
//                    {
//                        graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
//                        graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;
//                        graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
//                        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

//                        graphics.FillRectangle(Brushes.White, 0, 0, resultImage.Width, resultImage.Height);

//                        //graphics.DrawImage(sourceFile, new System.Drawing.Rectangle(0, 0, width, height), new System.Drawing.Rectangle(new System.Drawing.Point((sourceFile.Width - width) / 2, height > width ? (sourceFile.Height - height) / 2 : 0), new System.Drawing.Size(width, height)), GraphicsUnit.Pixel);
//                        graphics.DrawImage(sourceFile, new System.Drawing.Rectangle(0, 0, width, height), new System.Drawing.Rectangle(0, 0, sourceFile.Width, sourceFile.Height), GraphicsUnit.Pixel);

//                        EncoderParameters parameters = new EncoderParameters(1);
//                        if (quality > 0)
//                        {
//                            parameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
//                        }
//                        else
//                        {
//                            parameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Version, quality);
//                        }
//                        foreach (var pr in orginalProperties)
//                        {
//                        resultImage.SetPropertyItem(pr);
//                        }
//                        resultImage.Save(stream, ImageCodecInfo.GetImageEncoders().FirstOrDefault(s => s.MimeType == contentType), parameters);
//                        stream.Position = 0;

//                        #region Caching this Thumb
//                        Stream cacheStram = (new MemoryStream());
//                        stream.CopyTo(cacheStram);
//                        stream.Position = 0;
//                        #endregion
//                        var guid = Guid.NewGuid().ToString();
//                        return File(stream, contentType);
//                    }


//                    #endregion

//                }
//                else
//                {
//                    return base.File(path, "image/" + extension);
//                }
//            }
//            catch
//            {
//                return null;
//            }
           
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                _context.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}