using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewPicture
    {
        public int Id { get; set; }
        public string Url { get; set; }

        public ViewPicture() { }

        public ViewPicture(Picture picture)
        {
            if (picture != null)
            {
                this.Id = picture.ID;
                string url = picture.Url;
                url = url.Replace(Enum_Code.SYSTEM_TYPE_SHOP.ToString(), BaseWebsite.ShopUrl);
                url = url.Replace(Enum_Code.SYSTEM_TYPE_PANEL.ToString(), BaseWebsite.PanelUrl);
                url = url.Replace(Enum_Code.SYSTEM_TYPE_SYNCSERVER.ToString(), BaseWebsite.SyncUrl);
                this.Url = url;
            }
            else
                this.Url = "";
        }

        public ViewPicture(Picture picture, Enum_Code systemType)
        {
            if (picture != null)
            {
                this.Id = picture.ID;

                string url = picture.Url;
                url = url.Replace(Enum_Code.SYSTEM_TYPE_PANEL.ToString(), BaseWebsite.PanelUrl);
                url = url.Replace(Enum_Code.SYSTEM_TYPE_SHOP.ToString(), BaseWebsite.ShopUrl);
                this.Url = url;
            }
            else
                this.Url = "";
        }
    }
}
