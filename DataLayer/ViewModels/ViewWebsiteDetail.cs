using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewWebsiteDetail
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string AboutUs { get; set; }
        public string ContactUs { get; set; }
        public string Rules { get; set; }
        public string PurchaseGuide { get; set; }
        public string Abstract { get; set; }
        public string Address { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Instagram { get; set; }
        public string Telegram { get; set; }
        public string Whatsapp { get; set; }
        public string Twitter { get; set; }
        public string Facebook { get; set; }
        public string Youtube { get; set; }
        public string Skype { get; set; }
        public string Pinterest { get; set; }
        public string Phone2 { get; set; }
        public string Mobile2 { get; set; }
        public string Address2 { get; set; }
        public Nullable<int> LanguageId { get; set; }
        public string Waze { get; set; }
        public string Linkedin { get; set; }
        public string Aparat { get; set; }
        public string Summary { get; set; }
        public string LogoAddress { get; set; }
        public string IndexTitle { get; set; }
        public string OtherTitle { get; set; }
        public double ? CerPyzicalPrice { get; set; }
        public double? CerOnlinePrice { get; set; }

        public ViewWebsiteDetail() { }

        public ViewWebsiteDetail(WebsiteDetail websiteDetail)
        {
            Aparat = websiteDetail.Aparat;
            ID = websiteDetail.ID;
            CerPyzicalPrice = websiteDetail.CerPyzicalPrice;
            CerOnlinePrice = websiteDetail.CerOnlinePrice;

            Title = websiteDetail.Title;
            AboutUs = websiteDetail.AboutUs;
            ContactUs = websiteDetail.ContactUs;
            Rules = websiteDetail.Rules;
            PurchaseGuide = websiteDetail.PurchaseGuide;
            Abstract = websiteDetail.Abstract;
            Address = websiteDetail.Address;
            Address2 = websiteDetail.Address2;
            Latitude = websiteDetail.Latitude;
            Longitude = websiteDetail.Longitude;
            Phone = websiteDetail.Phone;
            Phone2 = websiteDetail.Phone2;
            Fax = websiteDetail.Fax;
            Mobile = websiteDetail.Mobile;
            Mobile2 = websiteDetail.Mobile2;
            Email = websiteDetail.Email;
            Instagram = websiteDetail.Instagram;
            Telegram = websiteDetail.Telegram;
            Whatsapp = websiteDetail.Whatsapp;
            Twitter = websiteDetail.Twitter;
            Facebook = websiteDetail.Facebook;
            Youtube = websiteDetail.Youtube;
            Linkedin = websiteDetail.Linkedin;
            Waze = websiteDetail.Waze;
            Summary = websiteDetail.Summary;
            IndexTitle = websiteDetail.IndexTitle;
            OtherTitle = websiteDetail.OtherTitle;
            //LogoAddress = Base.BaseWebsite.ShopUrl + "assets/" + Base.BaseWebsite.WebsiteLabel + "/images/logo.png";
        }

    }
}
