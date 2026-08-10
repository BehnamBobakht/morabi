using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DataLayer.Api.Model
{
    public class ApiUploadPicture : ApiResponse
    {
        public static ApiResult Post(UnitOfWork _context, Enum_Code system_type)
        {
            ApiResult result = new ApiResult();
            HttpFileCollection MyFileCollection = HttpContext.Current.Request.Files;
            int MaxSizeImage = 10000000;
            if (MyFileCollection.Count > 0)
            {
                var file = MyFileCollection[0];
                if (MyFileCollection[0].ContentLength < MaxSizeImage)
                {
                    Picture picture = _context.Picture.CreateAndUpload(system_type, file);
                    return CreateSuccessResult(new ViewPicture(picture, system_type));
                }
                else
                    return CreateErrorResult(Enum_Message.ERROR_FILE_MAX_SIZE);
            }
            else
                return CreateErrorResult(Enum_Message.REQUIRED_FILE);
        }
    }
}
