using DataLayer.Entities;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewProductCustomField
    {
        public int Id { get; set; }
        public string RowId { get; set; }
        public ViewProductType ProductType { get; set; }
        public ViewProductBrand ProductBrand { get; set; }
        public ViewProductCategory ProductCategory { get; set; }
        public ViewProductSubCategory ProductSubCategory { get; set; }
        public ViewCode Type { get; set; }
        public string Name { get; set; }
        public bool IsRequired { get; set; }
        public string SyncName { get; set; }
        public string ProductFieldValue { get; set; }
        public int? ProductFieldItem { get; set; }
        public bool InSearchResult { get; set; }

        public List<ViewProductCustomItem> Items { get; set; }

        public ViewProductCustomField() { }

        public ViewProductCustomField(ProductCustomField field)
        {
            this.Id = field.ID;
            this.IsRequired = field.IsRequired;
            this.Name = field.Name;
            this.ProductSubCategory = new ViewProductSubCategory(field.ProductSubCategory);
            this.ProductCategory = new ViewProductCategory(field.ProductCategory);
            this.ProductType = new ViewProductType(field.ProductType);
            this.Type = new ViewCode(field.Code);
            this.SyncName = field.SyncName;
            this.Items = field.ProductCustomItem.ToList().ToView();
            this.InSearchResult = field.InSearchResult;
        }

        public ViewProductCustomField(ProductCustomField field, int index, string MaxZero)
        {
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
            this.Id = field.ID;
            this.IsRequired = field.IsRequired;
            this.Name = field.Name;
            this.ProductSubCategory = new ViewProductSubCategory(field.ProductSubCategory);
            this.ProductCategory = new ViewProductCategory(field.ProductCategory);
            this.ProductType = new ViewProductType(field.ProductType);
            this.ProductBrand = new ViewProductBrand(field.ProductBrand, true);
            this.Type = new ViewCode(field.Code);
            this.SyncName = field.SyncName;
            this.Items = field.ProductCustomItem.ToList().ToView();
            this.InSearchResult = field.InSearchResult;

        }
    }
}
