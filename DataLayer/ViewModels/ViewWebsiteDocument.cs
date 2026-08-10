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
    public class ViewWebsiteDocument
    {
        public int Id { get; set; }
        public string Url { get; set; }

        public ViewWebsiteDocument() { }

        public ViewWebsiteDocument(WebsiteDocument doc)
        {
            if (doc != null)
            {
                this.Id = doc.ID;
                string url = doc.Url;
                url = url.Replace(Enum_Code.SYSTEM_TYPE_PANEL.ToString(), BaseWebsite.PanelUrl);
                url = url.Replace(Enum_Code.SYSTEM_TYPE_SHOP.ToString(), BaseWebsite.ShopUrl);
                this.Url = url;
            }
            else
                this.Url = "";
        }

        public ViewWebsiteDocument(WebsiteDocument doc, Enum_Code systemType)
        {
            if (doc != null)
            {
                this.Id = doc.ID;
                string url = doc.Url;
                url = url.Replace(Enum_Code.SYSTEM_TYPE_PANEL.ToString(), BaseWebsite.PanelUrl);
                url = url.Replace(Enum_Code.SYSTEM_TYPE_SHOP.ToString(), BaseWebsite.ShopUrl);
                this.Url = url;
            }
            else
                this.Url = "";
        }
    }
}
