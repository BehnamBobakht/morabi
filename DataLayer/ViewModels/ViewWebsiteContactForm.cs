using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewWebsiteContactForm
    {
        public int Id { get; set; }
        public string RowId { get; set; }
        public ViewWebsite Website { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Body { get; set; }
        public string AnswerString { get; set; }
        public Nullable<bool> IsRead { get; set; }
        public System.DateTime Datetime { get; set; }
        public Nullable<System.DateTime> AnswerDatetime { get; set; }
        public string Subject { get; set; }

        public ViewWebsiteContactForm(WebsiteContactForm contactForm, int index, string MaxZero)
        {
            this.Id = contactForm.ID;
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
            this.FullName =contactForm.FullName;
            this.Email = contactForm.Email;
            this.Mobile = contactForm.Mobile;
            this.Subject = contactForm.Subject;
            this.Body = contactForm.Body;
            this.Datetime = contactForm.Datetime;
            //this.Website = new ViewWebsite(contactForm.Website);
        }

        public ViewWebsiteContactForm(WebsiteContactForm contactForm)
        {
            this.Id =contactForm.ID;
            this.FullName = contactForm.FullName;
            this.FullName = contactForm.FullName;
            this.Email = contactForm.Email;
            this.Mobile = contactForm.Mobile;
            this.Subject = contactForm.Subject;
            this.Body = contactForm.Body;
            this.Datetime = contactForm.Datetime;
            //this.Website = new ViewWebsite(contactForm.Website);
        }
    }
}
