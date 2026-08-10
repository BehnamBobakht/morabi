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
    public class ViewProductPicture
    {
        public int Id { get; set; }
        public int? ColorId { get; set; }
        public string url { get; set; }

        public ViewProductPicture(Picture picture)
        {
            if (picture != null)
            {
                this.Id = picture.ID;
                string url = picture.Url;
                url = url.Replace(Enum_Code.SYSTEM_TYPE_PANEL.ToString(), BaseWebsite.PanelUrl);
                url = url.Replace(Enum_Code.SYSTEM_TYPE_SHOP.ToString(), BaseWebsite.ShopUrl);
                url = url.Replace(Enum_Code.SYSTEM_TYPE_SYNCSERVER.ToString(), BaseWebsite.SyncUrl);
                this.url = url;
            }
            else
                this.url = "";
        }

        public ViewProductPicture(Picture picture, int? colorId = null)
        {
            if (picture != null)
            {
                this.Id = picture.ID;
                this.ColorId = colorId;
                string url = picture.Url;
                url = url.Replace(Enum_Code.SYSTEM_TYPE_PANEL.ToString(), BaseWebsite.PanelUrl);
                url = url.Replace(Enum_Code.SYSTEM_TYPE_SHOP.ToString(), BaseWebsite.ShopUrl);
                url = url.Replace(Enum_Code.SYSTEM_TYPE_SYNCSERVER.ToString(), BaseWebsite.SyncUrl);
                this.url = url;
            }
            else
                this.url = "";
            this.url = this.url;
        }
    }
}
