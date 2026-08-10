using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewShopWebsiteSetting
    {
        public int Id { get; set; }
        public int WebsiteId { get; set; }
        public bool HasShop { get; set; }
        public bool HasProductType { get; set; }
        public bool HasProductCategory { get; set; }
        public bool HasProductSubCategory { get; set; }
        public bool HasBrand { get; set; }
        public bool HasCompany { get; set; }
        public bool HasGarranty { get; set; }
        public bool HasColor { get; set; }
        public bool HasSize { get; set; }
        public bool HasStatus { get; set; }
        public bool HasLastPrice { get; set; }
        public bool HasPrice { get; set; }
        public bool HasMinOrder { get; set; }
        public bool HasCustomFields { get; set; }
        public bool HasPicture { get; set; }
        public bool HasDescription { get; set; }
        public bool HasSummary { get; set; }
        public bool HasShowNumber { get; set; }
        public bool HasShowHomePage { get; set; }
        public bool HasDocument { get; set; }
        public int? MaxShopProductCount { get; set; }
        public bool HasQuantity { get; set; }
        public bool HasQuantityColor { get; set; }
        public bool HasUnit { get; set; }
        public bool HasCodeValue { get; set; }
        public bool HasActive { get; set; }
        public bool HasStore { get; set; }
        public bool HasProductOption { get; set; }
        public bool HasSeo { get; set; }
        public bool HasRequierdState { get; set; }
        public bool HasRequierdCity { get; set; }
        public bool HasUserName { get; set; }
        public string MobileRegix { get; set; }
        public string ZipCodeRegix { get; set; }
        public string PhoneRegix { get; set; }
        public string EmailRegix { get; set; }
        public string PasswordRegix { get; set; }
        public string NationalCodeRegix { get; set; }

        public Boolean HasUnitPrice { get; set; }
        public bool HasService { get; set; }
        public bool ProductMaxOrderCount { get; set; }

        public double UnitPrice { get; set; }
        public Boolean IsLoginWithNationalCode { get; set; }
        public bool HasPriceRange { get; set; }
        public bool HasSendByPost { get; set; }
        public bool HasCopyProduct { get; set; }
        public bool HasCentralizedDelivery { get; set; }
        public bool NationalCodeIsRequired { get; set; }
        public bool EmailIsRequired { get; set; }
        public bool PhoneIsRequired { get; set; }
        public bool MobileIsRequired { get; set; }
        public bool ZipCodeIsRequired { get; set; }
        public bool CdnPictures { get; set; }
        public int CdnPicturesQuality { get; set; }
        public int Earnest { get; set; }
        public bool HasLoginWithSms { get; set; }
        public Nullable<System.DateTime> LastContentUpdate { get; set; }

        public ViewShopWebsiteSetting() { }
        public ViewShopWebsiteSetting(ShopWebsiteSetting setting)
        {
            this.Id = setting.ID;
            this.WebsiteId = setting.WebsiteId;
            this.HasShop = setting.HasShop;
            this.HasProductType = setting.HasProductType;
            this.HasProductCategory = setting.HasProductCategory;
            this.HasProductSubCategory = setting.HasProductSubCategory;
            this.HasBrand = setting.HasBrand;
            this.HasCompany = setting.HasCompany;
            this.HasGarranty = setting.HasGarranty;
            this.HasColor = setting.HasColor;
            this.HasSize = setting.HasSize;
            this.HasStatus = setting.HasStatus;
            this.HasLastPrice = setting.HasLastPrice;
            this.HasPrice = setting.HasPrice;
            this.HasCopyProduct = setting.HasCopyProduct;
            this.HasMinOrder = setting.HasMinOrder;
            this.HasCustomFields = setting.HasCustomFields;
            this.HasPicture = setting.HasPicture;
            this.HasDescription = setting.HasDescription;
            this.HasSummary = setting.HasSummary;
            this.HasShowNumber = setting.HasShowNumber;
            this.HasShowHomePage = setting.HasShowHomePage;
            this.HasDocument = setting.HasDocument;
            this.HasQuantity = setting.HasQuantity;
            this.HasQuantityColor = setting.HasQuantityColor;
            this.HasUnit = setting.HasUnit;
            this.HasCodeValue = setting.HasCodeValue;
            this.HasActive = setting.HasActive;
            this.HasStore = setting.HasStore;
            this.HasProductOption = setting.HasProductOption;
            this.HasSeo = setting.HasSeo;
            this.HasRequierdState = setting.HasRequierdState;
            this.HasRequierdCity = setting.HasRequierdCity;
            this.HasUserName = setting.HasUserName;
            this.MobileRegix = setting.MobileRegix;
            this.ZipCodeRegix = setting.ZipCodeRegix;
            this.PhoneRegix = setting.PhoneRegix;
            this.EmailRegix = setting.EmailRegix;
            this.PasswordRegix = setting.PasswordRegix;
            this.HasUnitPrice = setting.HasUnitPrice;
            this.UnitPrice = setting.UnitPrice;
            this.HasRequierdState = setting.HasRequierdState;
            this.HasRequierdCity = setting.HasRequierdCity;
            this.HasService = setting.HasService;
            this.IsLoginWithNationalCode = setting.IsLoginWithNationalCode;
            this.ProductMaxOrderCount = setting.ProductMaxOrderCount;
            this.HasPriceRange = setting.HasPriceRange;
            this.HasSendByPost = setting.HasSendByPost;
            this.HasCentralizedDelivery = setting.HasCentralizedDelivery;
            this.NationalCodeRegix = setting.NationalCodeRegix;
            this.NationalCodeIsRequired = setting.NationalCodeIsRequired;
            this.EmailIsRequired = setting.EmailIsRequired;
            this.PhoneIsRequired = setting.PhoneIsRequired;
            this.MobileIsRequired = setting.MobileIsRequired;
            this.ZipCodeIsRequired = setting.ZipCodeIsRequired;
            this.CdnPicturesQuality = setting.CdnPicturesQuality;
            this.CdnPictures = setting.CdnPictures;
            this.Earnest = setting.Earnest;
            this.HasLoginWithSms = setting.HasLoginWithSms;
            this.LastContentUpdate = setting.LastContentUpdate;
        }

    }
}
