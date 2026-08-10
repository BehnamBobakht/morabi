using DataLayer.Entities;
using System;
using System.Collections.Generic;


namespace DataLayer.ViewModels
{
    public class ProductShowVm
    {
        public int ID { get; set; }
        public bool IsFree { get; set; }

        public Nullable<int> BrandId { get; set; }
        public Nullable<int> ProductTypeId { get; set; }
        public Nullable<int> ProductCategoryId { get; set; }
        public Nullable<int> ProductSubCategoryId { get; set; }
        public string ProductTypeUrl { get; set; }
        public string Image { get; set; }
        public string ProductsId { get; set; }
        public bool IsPackage { get; set; }
        public bool IsSelectMulti { get; set; }

        public bool IsHidden { get; set; }

        public string ProductTypePictureUrl { get; set; }
        public bool? IsShenasnameh { get; set; }

        public bool? HasExpire { get; set; }
        public bool? IsPictureId { get; set; }
        public bool? IsNationalId { get; set; }
        public bool? IsParport { get; set; }
        public bool? IsEduaction { get; set; }
        public bool? IsQuestion { get; set; }
        public bool? IsCoaching2 { get; set; }
        public bool? IsCoaching3 { get; set; }
        public bool? IsBirthday { get; set; }
        public string ProductCategoryUrl { get; set; }
        public string ProductSubCategoryUrl { get; set; }
        public Nullable<int> PictureId { get; set; }
        public Nullable<int> StatusId { get; set; }
        public Nullable<int> DocId { get; set; }
        public Nullable<int> Doc2Id { get; set; }
        public string Name { get; set; }
        public string NameId { get; set; }
        public string CodeValue { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public Nullable<double> LastPrice { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<int> VisitCount { get; set; }
        public bool ShowHomePage { get; set; }
        public double? EuroPrice { get; set; }
        public bool? HasEuro { get; set; }
        public bool? IsOffer { get; set; }


        public Nullable<System.DateTime> UpdateDatetime { get; set; }
        public Nullable<System.DateTime> SyncDatetime { get; set; }
        public bool Active { get; set; }
        public double MinPrice { get; set; }
        public bool IsService { get; set; }
        public double DiscountValue { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public Nullable<int> ProductOptionId { get; set; }
        public Nullable<int> SellCount { get; set; }
        public string H1 { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Img { get; set; }
        public double Weight { get; set; }
        public bool Deleted { get; set; }
        public Nullable<int> MaxOrderCount { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public System.DateTime CreateDateTime { get; set; }
        public Nullable<int> VideoDemoFileId { get; set; }
        public double InexpensiveItemBasePrice { get; set; }
        public double InexpensiveItemPrice { get; set; }
        public int DiscountPercent { get; set; }
        public Nullable<System.DateTime> DiscountExpireDate { get; set; }
        public Nullable<bool> IsCopy { get; set; }
        public string ProductUrl { get; set; }
        public double RateAvarage { get; set; }
        public double RateMin { get; set; }
        public double RateMax { get; set; }
        public int RateCount { get; set; }
        public int RateCount1 { get; set; }
        public int RateCount2 { get; set; }
        public int RateCount3 { get; set; }
        public int RateCount4 { get; set; }
        public int RateCount5 { get; set; }
        public int ? CityId { get; set; }
        public int? StateId { get; set; }


        public Nullable<int> Quantity { get; set; }
        public VarietyVm Variety { get; set; }
        public string ProductTypeName { get; set; }

        public string ProductCategoryName { get; set; }
        public string ProductSubCategoryName { get; set; }       
        public string MetaTagDescription { get; set; }
        public string Canonical { get; set; }
        public bool NoFollow { get; set; }
        public bool NoIndex { get; set; }
        public bool HasEarnest { get; set; }
        public DateTime ? StartDatetime { get; set; }
        public DateTime? EndDatetime { get; set; }

        public DateTime? FromDatetime { get; set; }
        public DateTime? ToDatetime { get; set; }
        public string LinkTelegram { get; set; }

        public CodeVm Code { get; set; }
        public PictureVm Picture { get; set; }
        public WebsiteDocumentVm WebsiteDocument { get; set; }
        public WebsiteDocumentVm WebsiteDocument1 { get; set; }
        public ProductBrandVm ProductBrand { get; set; }
        //public ProductCategoryVm ProductCategory { get; set; }
        public ProductOptionVm ProductOption { get; set; }
        //public ProductSubCategoryVm ProductSubCategory { get; set; }
        //public ProductTypeVm ProductType { get; set; }
        public List<QuestionRegisterProductVm> QuestionRegisterProduct { get; set; }

        public List<ProductCommentVm> ProductComment { get; set; }
        public  List<ProductCustomValueVm> ProductCustomValue { get; set; }
        public  List<ProductPiceHistoryVm> ProductPiceHistory { get; set; }
        public  List<PictureVm> Pictures { get; set; }
        public  List<ReviewVm> Review { get; set; }

    }
}
