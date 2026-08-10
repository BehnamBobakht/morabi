using DataLayer.Entities;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewProduct
    {
        public string Id { get; set; }
        public string RowId { get; set; }
        public string SyncId { get; set; }
        public string Name { get; set; }
        public string CodeValue { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public DateTime CreateDateTime { get; set; }
        //public ViewDiscount Discount { get; set; }
        public DateTime? UpdateDatetime { get; set; }
        public DateTime? SyncDatetime { get; set; }
        public ViewShop Shop { get; set; }
        public ViewDiscount Discount { get; set; }
        public ViewProductSubCategory ProductSubCategory { get; set; }
        public ViewProductCategory ProductCategory { get; set; }
        public ViewProductType ProductType { get; set; }
        public ViewProductBrand ProductBrand { get; set; }
        public List<ViewProductCustomValue> Items { get; set; }
        //public List<ViewStore> Stores { get; set; }
        public int LikesCount { get; set; }
        public ViewCode Status { get; set; }
        public double? LastPrice { get; set; }
        public double Price { get; set; }
        public double? Rate { get; set; }
        public int Quantity { get; set; }
        public double DiscountPrice { get; set; }
        public int? DiscountPercent { get; set; }
        public double? MinOrder { get; set; }
        public ViewPicture Picture { get; set; }
        public ViewDocument Document { get; set; }
        public List<ViewProductPicture> Pictures { get; set; }
        public List<ViewColor> Colors { get; set; }
        public List<ViewSize> Sizes { get; set; }
        public ViewProductOption Option { get; set; }

        public bool IsLiked { get; set; }
        public bool Active { get; set; }
        public double MinPrice { get; set; }
        public Nullable<bool> IsService { get; set; }
        public double DiscountValue { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Url { get; set; }
        public ViewProduct() { }

        public ViewProduct(DataLayer.Entities.Product product)
        {
            if (product != null)
            {
                this.Id = product.ID.ToString();
                this.Name = product.GetName();
                this.Price = product.Price == null ? 0 : product.Price.Value;
                this.ProductType = new ViewProductType(product.ProductType);
                this.ProductBrand = new ViewProductBrand(product.ProductBrand, true);
                this.ProductCategory = new ViewProductCategory(product.ProductCategory);
                this.ProductSubCategory = new ViewProductSubCategory(product.ProductSubCategory);
                this.Picture = product.PictureId != null ? new ViewPicture(product.Picture) : null;
                this.IsService =  product.IsService ;
                this.Latitude = product.Latitude;
                this.Longitude = product.Longitude;
            }
        }

        public ViewProduct(DataLayer.Entities.Product product, int index, string MaxZero)
        {
            this.Id = product.ID.ToString();
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
            this.Name = product.Name;
            this.SyncId = product.SyncId;
            this.Status = new ViewCode(product.Code);
            this.ProductType = new ViewProductType(product.ProductType);
            this.ProductCategory = new ViewProductCategory(product.ProductCategory);
            this.ProductSubCategory = new ViewProductSubCategory(product.ProductSubCategory);
            this.LikesCount = product.ProductLike.Count;
            this.IsService = product.IsService;
            this.Latitude =product.Latitude;
            this.Longitude = product.Longitude;
        }

        //Extera
        public class ViewProductDetail
        {
            public int Count { get; set; }
            public double  MinPrice { get; set; }
            public double ? MaxPrice { get; set; }
        }
    }
}
