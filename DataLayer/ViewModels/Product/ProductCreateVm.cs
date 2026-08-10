using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ProductCreateVm
    {

        public int ID { get; set; }
        public int? InstallmentCount { get; set; }
        public Nullable<int> BrandId { get; set; }
        public bool HasPrint { get; set; }

        public Nullable<int> ProductTypeId { get; set; }
        public Nullable<int> ProductCategoryId { get; set; }
        public Nullable<int> ProductSubCategoryId { get; set; }
        public Nullable<int> StateId { get; set; }
        public Nullable<int> CityId { get; set; }
        public Nullable<int> CoursId { get; set; }

        public bool IsFree { get; set; }
        public Nullable<int> PictureId { get; set; }
        public string PictureUrl { get; set; }
        public string TeacherNameFa { get; set; }
        public string TeacherNameEn { get; set; }
        public string Hours { get; set; }

        public string ProductsId { get; set; }
        public bool IsPackage { get; set; }
        public bool IsSelectMulti { get; set; }
        public bool IsHidden { get; set; }

        public bool IsQuestion { get; set; }


        public Nullable<int> StatusId { get; set; }
        public Nullable<int> DocId { get; set; }
        public Nullable<int> Doc2Id { get; set; }
        public DateTime? StartDatetime { get; set; }
        public DateTime? EndDatetime { get; set; }
        public DateTime? FromDatetime { get; set; }
        public DateTime? ToDatetime { get; set; }
        public string Name { get; set; }
        public string LinkTelegram { get; set; }
        public string Image { get; set; }
        public string NumberString { get; set; }
        public string NameFa { get; set; }
        public string NameEn { get; set; }
        public string DateFa { get; set; }
        public string DateEn { get; set; }
        public string DatePrint { get; set; }


        public string CodeValue { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public Nullable<int> ShowNumber { get; set; }
        public bool ShowHomePage { get; set; }
        public bool Active { get; set; }
        public bool IsService { get; set; }
        public bool IsCopy { get; set; }
        public bool HasEuro { get; set; }
        public bool IsOffer { get; set; }
        public bool IsPictureId { get; set; }
        public bool IsNationalId { get; set; }
        public bool IsParport { get; set; }
        public bool IsEduaction { get; set; }
        public bool IsShenasnameh { get; set; }
        public bool IsCoaching2 { get; set; }
        public bool IsCoaching3 { get; set; }
        public bool IsBirthday { get; set; }
        public double EuroPrice { get; set; }

        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string H1 { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string MetaTagDescription { get; set; }
        public string Canonical { get; set; }
        public bool NoFollow { get; set; }
        public bool NoIndex { get; set; }
        public double Weight { get; set; }
        public Nullable<int> VideoDemoFileId { get; set; }
        public Nullable<int> ProductOptionId { get; set; }
        public string ProductOptionName { get; set; }
        public bool HasColor { get; set; }
        public bool HasSize { get; set; }
        public bool HasOption { get; set; }
        public bool HasEarnest { get; set; }

        public CodeVm Code { get; set; }
        public PictureVm Picture { get; set; }
        public WebsiteDocumentVm WebsiteDocument { get; set; }
        public WebsiteDocumentVm WebsiteDocument1 { get; set; }
        public ProductBrandVm ProductBrand { get; set; }
        public ProductOptionVm ProductOption { get; set; }
        public ProductSubCategoryVm ProductSubCategory { get; set; }
        public ProductTypeVm ProductType { get; set; }
        public List<ColorVm> ProductColor { get; set; }
        public List<int> ColorIds { get; set; }
        public List<int> SizeIds { get; set; }
        

        public List<int> OptionItemIds { get; set; }
        public List<ProductCustomValueVm> ProductCustomValue { get; set; }
        public List<ProductCustomFieldVm> ProductCustomField { get; set; }
        public List<PictureVm> ProductPictures { get; set; }
        public List<SizeVm> ProductSize { get; set; }
        public List<ReviewVm> Reviews { get; set; }
        public ProductCategoryVm ProductCategory { get; set; }
        public List<ProductLanguageVm> ProductLanguages { get; set; }

        public ProductCreateVm()
        {
            ProductLanguages = new List<ProductLanguageVm>();
            Reviews = new List<ReviewVm>();
            ProductPictures = new List<PictureVm>();
        }

    }
}
