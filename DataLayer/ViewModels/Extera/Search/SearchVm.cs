using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class SearchVm : StatusVm
    {
        public string Q { get; set; }
        public int Size { get; set; }
        public int TotalCount { get; set; }
        public List<CategoriesSearchVm> Categories { get; set; }
        public List<BrandsSearchVm> Brands { get; set; }
        public List<ProductsSearchVm> Products { get; set; }
        public List<PostsSearchVm> Posts { get; set; }
        public SearchVm()
        {
            Categories = new List<CategoriesSearchVm>();
            Brands = new List<BrandsSearchVm>();
            Products = new List<ProductsSearchVm>();
            Posts = new List<PostsSearchVm>();
        }

    }
    public class CategoriesSearchVm
    {
        public int ID { get; set; }
        public string ProductTypeName { get; set; }
        public string ProductCategoryName { get; set; }
        public string ProductSubCategoryName { get; set; }
        public string Href { get; set; }
        public string Fullname { get; set; }



        public PictureVm Picture { get; set; }
    }  
    public class BrandsSearchVm
    {
        public int ID { get; set; }
        public string Name { get; set; }       
        public string Href { get; set; }
        public PictureVm Picture { get; set; }

    }
    public class ProductsSearchVm
    {
        public int ID { get; set; }
        public string ProductTypeName { get; set; }
        public string ProductCategoryName { get; set; }
        public string ProductSubCategoryName { get; set; }
        public string Name { get; set; }       
        public string Href { get; set; }
        public PictureVm Picture { get; set; }
        public string Fullname { get; set; }

    }
    public class PostsSearchVm
    {
        public int ID { get; set; }
        public string CategoryName { get; set; }
        public string CategoryBody { get; set; }

        public string Name { get; set; }       
        public string Href { get; set; }
        public PictureVm Picture { get; set; }

    }

}
