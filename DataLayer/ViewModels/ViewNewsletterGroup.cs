using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewNewsletterGroup
    {
        public int Id { get; set; }
        public string RowId { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public int ItemCount { get; set; }

        public ViewNewsletterGroup() { }

        public ViewNewsletterGroup(NewsletterGroup group)
        {
            if (group != null)
            {
                this.Id = group.ID;
                this.Label = group.Label;
                this.Name = group.Name;
            }
        }

        public ViewNewsletterGroup(NewsletterGroup group, int index, string MaxZero)
        {
            if (group != null)
            {
                this.Id = group.ID;
                this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
                this.Label = group.Label;
                this.Name = group.Name;
                this.ItemCount = group.NewsletterItem.Count;
            }
        }
    }
}
