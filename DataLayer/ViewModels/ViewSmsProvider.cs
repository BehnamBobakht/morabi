using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewSmsProvider
    {
        public int Id { get; set; }
        public string RowId { get; set; }
        public string Name { get; set; }
        public string SiteUrl { get; set; }
        public string ServiceUrl { get; set; }
        public string Label { get; set; }

        public ViewSmsProvider() { }

        public ViewSmsProvider(SmsProvider provider)
        {
            this.Id = provider.ID;
            this.Name = provider.Name;
            this.Label = provider.Label;
            this.ServiceUrl = provider.ServiceUrl;
            this.SiteUrl = provider.SiteUrl;
        }

        public ViewSmsProvider(SmsProvider provider, int index, string MaxZero)
        {
            this.Id = provider.ID;
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
            this.Name = provider.Name;
            this.Label = provider.Label;
            this.ServiceUrl = provider.ServiceUrl;
            this.SiteUrl = provider.SiteUrl;
        }
    }
}
