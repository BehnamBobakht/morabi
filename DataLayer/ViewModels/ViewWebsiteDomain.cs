using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewWebsiteDomain
    {
        public int Id { get; set; }
        public string RowId { get; set; }
        public string Website { get; set; }
        public string Domain { get; set; }
        public string ActivationKey { get; set; }

        public ViewWebsiteDomain(WebsiteDomain domain, int index, string MaxZero)
        {
            this.Id = domain.ID;
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
            this.Domain = domain.Domain;
            this.Website = domain.Website.Title;
            this.ActivationKey = domain.ActivationKey;
        }
    }
}
