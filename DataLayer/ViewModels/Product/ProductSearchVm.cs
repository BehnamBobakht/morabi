using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ProductSearchVm
    {
        public int TotalCount { get; set; } 
        public double MaxPrice { get; set; }
        public double MinPrice { get; set; }
        public double EuroPrice { get; set; }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        

        public string Name { get; set; }
        public string PrcustomLabel { get; set; }
        public int? ProductTypeId { get; set; }
        public int? ProductOptionId { get; set; }

        public int? ProductCategoryId { get; set; }
        public int? ProductSubCategoryId { get; set; }
        public bool? HasDiscount { get; set; }      
        public string BrandId { get; set; }
        public string MinDescription { get; set; }
        public string Description { get; set; }
        public string LandingText { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string CityName { get; set; }

        public string Title { get; set; }
        public string H1 { get; set; }
        public string IconUrl { get; set; }
        public string PictureUrl { get; set; }
        public string MetaDescription { get; set; }
        public string PageUrl { get; set; }
        public string PageCanonical { get; set; }
        public bool NoFollow { get; set; }
        public bool NoIndex { get; set; }
        public bool IncorrectCustomLabel { get; set; }
        public bool? Active { get; set; }
        public List<ProductBrandVm> ProductBrand { get; set; }
        public string ColorId { get; set; }
        public double? PriceFrom { get; set; }
        public double? PriceTo { get; set; }
        public List<int> SelectedColors { get; set; }
        public List<int> SelectedCustomFields { get; set; }
        public List<int> SelectedBrands { get; set; }
        public List<ColorVm> Colors { get; set; }
        public List<ProductCustomFieldVm> ProductCustomFields { get; set; }
        public bool? Prstock { get; set; }
        public bool? HasExpire { get; set; }

        
        public Enum_Code? Status { get; set; }
        public Enum_ProductOrder prorder  { get; set; }
        public List<ProductMinVm> Products { get; set; }
        public List<ProductShowVm> Products2 { get; set; }

        public ProductTypeVm ProductType { get; set; }
        public ProductCategoryVm ProductCategory { get; set; }
        public ProductSubCategoryVm ProductSubCategory { get; set; }
        public List<ProductTypeVm> ProductTypes { get; set; }

        public ProductSearchVm()
        {
            Products = new List<ProductMinVm>();
            Products2 = new List<ProductShowVm>();

            SelectedColors = new List<int>();
            Colors = new List<ColorVm>();
        }

    }
}
