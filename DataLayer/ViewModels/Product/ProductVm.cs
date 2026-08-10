using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ProductVm
    {

        public int ID { get; set; }
        public Nullable<int> ShopId { get; set; }
        public Nullable<int> BrandId { get; set; }
        public Nullable<int> ProductTypeId { get; set; }
        public Nullable<int> ProductCategoryId { get; set; }
        public Nullable<int> ProductSubCategoryId { get; set; }
        public Nullable<int> UnitId { get; set; }
        public Nullable<int> PictureId { get; set; }
        public Nullable<int> StatusId { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> DocId { get; set; }
        public Nullable<int> Doc2Id { get; set; }
        public string SyncId { get; set; }
        public string Name { get; set; }
        public string CodeValue { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public Nullable<int> ShowNumber { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<double> MinOrder { get; set; }
        public Nullable<double> LastPrice { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<int> VisitCount { get; set; }
        public bool ShowHomePage { get; set; }
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
        public double InexpensiveItemPrice { get; set; }
        public int DiscountPercent { get; set; }
        public Nullable<System.DateTime> DiscountExpireDate { get; set; }
        public Nullable<bool> IsCopy { get; set; }
        public string ProductUrl { get; set; }
        public bool HasEarnest { get; set; }
        public virtual CodeVm Code { get; set; }
        //public virtual File File { get; set; }
        public  PictureVm Picture { get; set; }
        public  WebsiteDocumentVm WebsiteDocument { get; set; }
        public  WebsiteDocumentVm WebsiteDocument1 { get; set; }
        public ProductBrandVm ProductBrand { get; set; }
        public  ProductCategoryVm ProductCategory { get; set; }
        public  ProductOptionVm ProductOption { get; set; }
        public  ProductSubCategoryVm ProductSubCategory { get; set; }
        public  ProductTypeVm ProductType { get; set; }
        //public virtual Unit Unit { get; set; }
        //public virtual ICollection<ProductBarcode> ProductBarcode { get; set; }
        public  List<ProductCommentVm> ProductComment { get; set; }
        //public virtual ICollection<ProductCustomValue> ProductCustomValue { get; set; }
        //public virtual ICollection<ProductPiceHistory> ProductPiceHistory { get; set; }
        //public virtual ICollection<ProductPicture> ProductPicture { get; set; }
        //public virtual ICollection<ProductSize> ProductSize { get; set; }
        //public virtual ICollection<Review> Review { get; set; }

    }
}
