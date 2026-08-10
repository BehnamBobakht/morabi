using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Base
{
    public class BasePath
    {
        public static string UploadPicture { get { return "/Uploads/"/*+BaseWebsite.WebsiteLabel+"/"*/; } }
    }
}
