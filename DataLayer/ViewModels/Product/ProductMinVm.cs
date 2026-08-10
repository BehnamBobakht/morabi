using DataLayer.Entities;
using System;
using System.Collections.Generic;


namespace DataLayer.ViewModels
{
    public class ProductMinVm
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public bool IsFree { get; set; }

        public bool IsSelectMulti { get; set; }
        public string NameId { get; set; }
        public string CodeValue { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string H1 { get; set; }
        public string Title { get; set; }
        public double DiscountValue { get; set; }
        public double InexpensiveItemBasePrice { get; set; }
        public double InexpensiveItemPrice { get; set; }
        public int DiscountPercent { get; set; }
        public Nullable<System.DateTime> DiscountExpireDate { get; set; }
        public string ProductUrl { get; set; }
        public double RateAvarage { get; set; }
        public double RateMin { get; set; }
        public double RateMax { get; set; }
        public double ? EuroPrice { get; set; }
        public bool ? HasEuro { get; set; }
        public bool? IsOffer { get; set; }
        public DateTime ? StartDatetime { get; set; }
        public DateTime ? EndDatetime { get; set; }
        public bool? HasExpire { get; set; }

        public int RateCount { get; set; }
        public int? VisitCount { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public bool HasEarnest { get; set; }
        public int ProductTypeId { get; set; }

        public string ProductTypeName { get; set; }
        public string ProductCategoryName { get; set; }
        public string ProductSubCategoryName { get; set; }
        public Nullable<System.DateTime> UpdateDatetime { get; set; }

        public List<ProductCommentVm> ProductComment { get; set; }
        public List<ProductCustomValueVm> ProductCustomValue { get; set; }
        public CodeVm Code { get; set; }
        public PictureVm Picture { get; set; }
        public WebsiteDocumentVm WebsiteDocument { get; set; }
        public ProductBrandVm ProductBrand { get; set; }
        public  List<PictureVm> Pictures { get; set; }

    }
}
