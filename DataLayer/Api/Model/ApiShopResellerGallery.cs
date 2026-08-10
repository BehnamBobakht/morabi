using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Api
{
    public class ApiShopResellerGallery : ApiResponse
    {
        public static ApiResult Get(UnitOfWork _context, int resellerId)
        {
            _context = new UnitOfWork();
            List<ShopResellerGallery> listGallery = _context.ShopResellerGallery.GetAllByResellerId(resellerId);
            return CreateSuccessResult(listGallery.ToApi());
        }

        public static ApiResult Post(UnitOfWork _context, ShopResellerGallery gallery)
        {
            if (string.IsNullOrEmpty(gallery.Name))
                return CreateErrorResult(Enum_Message.REQUIRED_RESELLER_GALLERY_NAME);
            if (gallery.PictureId == null)
                return CreateErrorResult(Enum_Message.REQUIRED_RESELLER_GALLERY_IMAGE);
            if (gallery.FileId == null)
                return CreateErrorResult(Enum_Message.REQUIRED_RESELLER_GALLERY_FILE);
            _context.ShopResellerGallery.Insert(gallery);
            _context.Save();

            _context.ShopResellerStory.InsertNewStory(Enum_StoryType.GALLERY, gallery.ResellerId, gallery.ID, gallery.PictureId, "گالری جدید! - " + gallery.Name);
            return CreateSuccessResult(Enum_Message.SUCCESSFULL_API);
        }
    }
}
