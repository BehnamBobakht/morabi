using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using ExpressMapper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace DataLayer.Base
{
    public static class BaseSearch
    {
        public static SearchVm Search(UnitOfWork _context, SearchVm search)
        {
            var result = new SearchVm();
            result.Q = search.Q;
            result.Size = search.Size;
            if(string.IsNullOrEmpty(search.Q) || search.Size<1)
            {
                result.Status = ResultStatus.Error;
                result.Message = Resource.Notify.NothingFound;
                return result;
            }
            result.Status = ResultStatus.Success;
            result.Message = Resource.Notify.OperationSuccessful;
            int remaining = search.Size;
            result.Categories = _context.ProductType.SearchBox(name: search.Q, size: search.Size).Map<List<ProductType>, List<CategoriesSearchVm>>();
            result.TotalCount = result.Categories.Count();
            remaining -= result.TotalCount;
            if (remaining < 1)
            {
                return result;
            }
            result.Categories.AddRange( _context.ProductCategory.SearchBox(name: search.Q, size: remaining).Map<List<ProductCategory>, List<CategoriesSearchVm>>());
            result.TotalCount = result.Categories.Count();
            remaining = search.Size;
            remaining -= result.TotalCount;
            if (remaining < 1)
            {
                return result;
            }      
            result.Categories.AddRange( _context.ProductSubCategory.SearchBox(name: search.Q, size: remaining).Map<List<ProductSubCategory>, List<CategoriesSearchVm>>());
            result.TotalCount = result.Categories.Count();
            remaining = search.Size;
            remaining -= result.TotalCount;
            if (remaining < 1)
            {
                return result;
            }
            result.Brands.AddRange(_context.ProductBrand.SearchBox(name: search.Q, size: remaining).Map<List<ProductBrand>, List<BrandsSearchVm>>());
            result.TotalCount += result.Brands.Count();
            remaining = search.Size;
            remaining -= result.TotalCount;
            if (remaining < 1)
            {
                return result;
            }      
            result.Products.AddRange(_context.Product.SearchBox(name: search.Q, size: remaining).Map<List<Product>, List<ProductsSearchVm>>());
            result.TotalCount += result.Products.Count();
            remaining = search.Size;
            remaining -= result.TotalCount;
            if (remaining < 1)
            {
                return result;
            }
            result.Posts.AddRange(_context.Post.SearchBox(name: search.Q, size: remaining).Map<List<Post>, List<PostsSearchVm>>());
            result.TotalCount += result.Posts.Count();
            if(result.TotalCount <1)
            {
                result.Status = ResultStatus.Error;
                result.Message = Resource.Notify.NothingFound;
            }
            return result;

        }
    }
}
