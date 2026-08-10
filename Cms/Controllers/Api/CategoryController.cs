using DataLayer.Api;
using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Windows.Input;

namespace Cms.Controllers.Api
{
    public class CategoryController : ApiController
    {
        // GET: City
        UnitOfWork _context = new UnitOfWork();

        [HttpGet]
        public ApiResult Get(string keyWord = null)
        {
            Category category = _context.Category.Search(keyWord: keyWord).FirstOrDefault();
            ViewCategory model = new ViewCategory();
            model.Body = category.Body;
            model.Picture = category.Picture.GetUrl();
            return ApiResponse.CreateSuccessResult(model);
        }
    }
}