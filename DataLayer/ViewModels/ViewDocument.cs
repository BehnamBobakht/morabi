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
    public class ViewDocument
    {
        public int Id { get; set; }
        public string Url { get; set; }

        public ViewDocument() { }

        public ViewDocument(WebsiteDocument doc)
        {
            if (doc != null)
            {
                this.Id = doc.ID;
                this.Url = doc.Url;
            }
            else
                this.Url = "";
        }

        public ViewDocument(WebsiteDocument doc, Enum_Code systemType)
        {
            if (doc != null)
            {
                this.Id = doc.ID;
                if (systemType == Enum_Code.SYSTEM_TYPE_PANEL)
                {
                    string panel_url = BaseWebsite.PanelUrl;
                    this.Url = doc.Url.Replace(Enum_Code.SYSTEM_TYPE_PANEL.ToString(), panel_url);
                }
                else
                    this.Url = doc.Url;
            }
            else
                this.Url = "";
        }
    }
}
