using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cms.Controllers.Website;
//using OfficeOpenXml;
using System.Net.Http;
using DataLayer.Api;

namespace Cms.Controllers
{
    public class PostController : Controller
    {
        UnitOfWork _context = new UnitOfWork();
        //public ActionResult Index(int? poid, string poidname = "")
        //{
        //    ViewPostModel model = new ViewPostModel();
        //    Post post = _context.Post.GetById(poid);
        //    if (!string.IsNullOrEmpty(post.URL))
        //    {
        //        if (post.URL.StandardUrl() != poidname)
        //        {
        //            return RedirectPermanent(post.GetLinkWithUrl());
        //        }
        //    }
        //    string currentUrl = Request.Url.ToString().ToLower();
        //    if (currentUrl != post.GetLinkWithUrl())
        //    {

        //        HttpContext.Response.Status = "301 Moved Permanently";
        //        HttpContext.Response.AddHeader("Location", post.GetLinkWithUrl());
        //    }
        //    post.VisitCount = post.VisitCount + 1;
        //    model.Post = post;
        //    model.RelatedPost = _context.Post.Where(s => s.CategoryId == post.CategoryId && s.ID != poid).Take(3).OrderByDescending(s => s.CreateDateTime).ToList();
        //    _context.Post.Update(post);
        //    _context.Save();
        //    ViewBag.CurrentAccount = _context.Account.GetCurrentAccount();

        //    int prpageSize = 10;
        //    int prindex = 1;
        //    List<Product> results = new List<Product>();
        //    foreach (var item in model.Post.TagPost)
        //    {

        //        foreach (var tagsub in item.Tag.TagSubcategory)
        //        {
        //            var searchQuery = _context.Product.GetSearchQuery(subCategoryId: tagsub.SubCategoryId.ToString());
        //            var products = _context.Product.Search(
        //     pageSize: prpageSize,
        //     index: prindex,
        //    query: searchQuery);

        //            foreach (var product in products)
        //            {
        //                results.Add(product);
        //            }
        //        }

        //    }
        //    model.RelatedProduct = results;
        //    return PartialView(BaseController.GetView(this), model);
        //}
        public ActionResult Index(int poid, string poidname = "")
        {
            var post = _context.Post.GetPostVm(poid, visit: true);
            //try
            //{
            if (post == null)
            {
                return Redirect(BaseController.RedirectToError(410));
            }
            else
            {
                BaseUrl.Redirect301(post.Href);
                return View(BaseController.GetView(this), model: post);
            }
            //}
            //catch
            //{
            //    return View(BaseController.GetView(this), model: post);
            //}

        }
        public ActionResult Post(int poid, string poidname = "")
        {
            List<ApiUrlParameter> urlparameter = new List<ApiUrlParameter>();
           
            string url = "https://panel.bhpsolution.com/api/Post/"+ poid;
            var result = ApiRequest.CreateApiRequest<ResultPostIndex>(domain: url, method: Enum_RequestMethod.GET, api: Enum_Api.NONE, headerList: null, urlList: urlparameter, entity: null);

            ResultPostIndex post = (ResultPostIndex)result.Value;

          
            //try
            //{
            if (post == null)
            {
                return Redirect(BaseController.RedirectToError(410));
            }
            else
            {
                return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, "index2", post.Data);

                //return View(BaseController.GetView(this), model: post);
            }
            //}
            //catch
            //{
            //    return View(BaseController.GetView(this), model: post);
            //}

        }
        public ActionResult ManagersState(string poidlabel = "Managers", string poviewName = "")
        {
            ViewPostModel model = new ViewPostModel();
            List<Post> results = _context.Post.Search(
              categoryLabel: poidlabel
              );
            var listState = _context.State.Where(s => s.Post.Any()).ToList();
            //var listState = _context.Post.Search(
            //    categoryLabel: poidlabel,
            //    pageSize:1000,
            //    index:1
            //    ).Select(s => s.State).Distinct().ToList();
            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, poviewName, listState);

        }
        public ActionResult Managers(string poidlabel = "Managers", int? stateId = null, int? poIndex = null, int? poSize = null)
        {
            ViewPostModel model = new ViewPostModel();
            poIndex = poIndex != null ? poIndex.Value : 1;
            poSize = poSize != null ? poSize.Value : 10;
            List<Post> results = _context.Post.Search(
              categoryLabel: poidlabel,
              index: poIndex.Value,
              pageSize: poSize.Value,
              stateId: stateId

              );
            var listState = _context.Post.Search(
                categoryLabel: poidlabel,
                index: 1,
                pageSize: 500

                ).Select(s => s.State).Distinct();
            ViewBag.StateId = new SelectList(listState, "ID", "Name").ToList();
            ViewSearchPost search = new ViewSearchPost()
            {
                TotalCount = _context.Post.SearchCount(categoryLabel: poidlabel, stateId: stateId),
                Category = _context.Category.FirstOrDefault(p => p.Label == poidlabel),
                PageIndex = poIndex.Value,
                PageSize = poSize.Value,
                Results = results
            };
            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, poidlabel, search);

        }
        //public ActionResult SearchCategory(string poidlabel = null, string poviewName = null, int? poIndex = null, int? poSize = null, Enum_PostOrder poorder = Enum_PostOrder.NONE)
        //{
        //    poIndex = poIndex != null ? poIndex.Value : 1;
        //    poSize = poSize != null ? poSize.Value : 10;
        //    poviewName = poviewName == null ? poidlabel : poviewName;

        //    List<Post> results = _context.Post.Search(
        //        categoryLabel: poidlabel,
        //        index: poIndex.Value,
        //        pageSize: poSize.Value,
        //        order: poorder


        //        );
        //    ViewSearchPost search = new ViewSearchPost()
        //    {
        //        TotalCount = _context.Post.SearchCount(categoryLabel: poidlabel),
        //        Category = _context.Category.FirstOrDefault(p => p.Label == poidlabel),
        //        PageIndex = poIndex.Value,
        //        PageSize = poSize.Value,
        //        Results = results
        //    };
        //    return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, poviewName, search);
        //}
        //[CustomCache(Duration = 1, cacheType =Enum_OutputCache.PostSearch, VaryByParam = "*")]
        public ActionResult Search(
            string pocategory = null,
            int? notId = null,
            int? pocategoryId = null,
            int? poIndex = null,
            int? poSize = null,
            int? poTypeId = null,
            string pokeyword = null,
            string poviewName = null,
            Enum_PostOrder poorder = Enum_PostOrder.NONE,
            Enum_PostOutput pooutput = Enum_PostOutput.ENTITY,
            string name = null,
            bool? active = true,
            string lang = ""
            )

        {
            poIndex = poIndex != null ? poIndex.Value : 1;
            poSize = poSize != null ? poSize.Value : 20;
            poviewName = poviewName == null ? pocategory : poviewName;
            var result = new PostSearchVm();
            result = _context.Post.SearchVm(
                categoryId: pocategoryId,
                categoryLabel: pocategory,
                keyword: pokeyword,
                index: poIndex.Value,
                pageSize: poSize.Value,
                order: poorder,
                name: name,
                notId: notId,
                typeId: poTypeId,
                active: active
                );
            result.categoryId = pocategoryId;
            result.categoryLable = pocategory;


            result.Category = _context.Category.GetByLabelVm(pocategory);
            result.notId = notId;
            result.PageIndex = poIndex.Value;
            result.PageSize = poSize.Value;
            result.Name = name;
            result.typeId = poTypeId;
            result.keyword = pokeyword;
            result.ViewName = poviewName;
            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, poviewName, result);
        }
        public ActionResult SearchByApi(
            string pocategory = null,
            int? notId = null,
            int? pocategoryId = null,
            int? poIndex = null,
            int? poSize = null,
            int? poTypeId = null,
            string pokeyword = null,
            string poviewName = null,
            int poorder = 0,
            Enum_PostOutput pooutput = Enum_PostOutput.ENTITY,
            string name = null,
            bool? active = true,
            string lang = "",
            string hashtags=null
            )

        {
            poIndex = poIndex != null ? poIndex.Value : 1;
            poSize = poSize != null ? poSize.Value : 20;
            poviewName = poviewName == null ? pocategory : poviewName;
            //var result = new PostSearchVm();
            List<ApiUrlParameter> urlparameter = new List<ApiUrlParameter>();
            urlparameter.Add(new ApiUrlParameter { Name = "pageIndex", Value = poIndex.ToString() });
            urlparameter.Add(new ApiUrlParameter { Name = "pageSize", Value = poSize.ToString() });
            urlparameter.Add(new ApiUrlParameter { Name = "sortBy", Value = poorder.ToString() });

            urlparameter.Add(new ApiUrlParameter { Name = "categoryLabel", Value = pocategory });
           if(hashtags != null && hashtags != "")
            {
                urlparameter.Add(new ApiUrlParameter { Name = "hashtags", Value = hashtags });

            }
            if (pocategoryId != null )
            {
                urlparameter.Add(new ApiUrlParameter { Name = "categoryId", Value = pocategoryId.ToString() });

            }
            if (name != null && name != "")
            {
                urlparameter.Add(new ApiUrlParameter { Name = "q", Value = name });

            }
            string url = "https://panel.bhpsolution.com/api/Post";
            var result = ApiRequest.CreateApiRequest<ResultPost>(domain: url, method: Enum_RequestMethod.GET, api: Enum_Api.NONE, headerList: null, urlList: urlparameter, entity: null);

            ResultPost res = (ResultPost)result.Value;

            res.PageIndex = poIndex.Value;
            res.PageSize = poSize.Value;
            res.categoryLable = pocategory;
            res.ViewName = poviewName;
            res.Name = name;
            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, poviewName, res);
        }
        public ActionResult Page(
    string pocategory = null,
    int? notId = null,
    int? pocategoryId = null,
    int? poIndex = null,
    int? poSize = null,
    int? poTypeId = null,
    string pokeyword = null,
    string poviewName = null,
    Enum_PostOrder poorder = Enum_PostOrder.NONE,
    Enum_PostOutput pooutput = Enum_PostOutput.ENTITY,
    string name = null,
    bool? active = true,
    string lang = ""
    )

        {
            poIndex = poIndex != null ? poIndex.Value : 1;
            poSize = poSize != null ? poSize.Value : 20;
            poviewName = poviewName == null ? pocategory : poviewName;
            var result = new PostSearchVm();
            result = _context.Post.SearchVm(
                categoryId: pocategoryId,
                categoryLabel: pocategory,
                keyword: pokeyword,
                index: poIndex.Value,
                pageSize: poSize.Value,
                order: poorder,
                name: name,
                notId: notId,
                typeId: poTypeId,
                active: active
                );
            result.categoryId = pocategoryId;
            result.categoryLable = pocategory;
            result.Category = _context.Category.GetByLabelVm(pocategory);
            result.notId = notId;
            result.PageIndex = poIndex.Value;
            result.PageSize = poSize.Value;
            result.Name = name;
            result.typeId = poTypeId;
            result.keyword = pokeyword;
            result.ViewName = poviewName;
            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, poviewName, result);
        }
        public ActionResult Content(
    string pocategory = null,
    int? notId = null,
    int? pocategoryId = null,
    int? poIndex = null,
    int? poSize = null,
    int? poTypeId = null,
    string pokeyword = null,
    string poviewName = null,
    int poorder = 0,
    Enum_PostOutput pooutput = Enum_PostOutput.ENTITY,
    string name = null,
    bool? active = true,
    string lang = "",
    string hashtags = ""
    )

        {
            poIndex = poIndex != null ? poIndex.Value : 1;
            poSize = poSize != null ? poSize.Value : 12;
            poviewName = poviewName == null ? pocategory : poviewName;
            List<ApiUrlParameter> urlparameter = new List<ApiUrlParameter>();
            urlparameter.Add(new ApiUrlParameter { Name = "pageIndex", Value = poIndex.ToString() });
            urlparameter.Add(new ApiUrlParameter { Name = "pageSize", Value = poSize.ToString() });
            urlparameter.Add(new ApiUrlParameter { Name = "sortBy", Value = poorder.ToString() });

            urlparameter.Add(new ApiUrlParameter { Name = "categoryLabel", Value = pocategory });
            if (hashtags != null && hashtags != "")
            {
                urlparameter.Add(new ApiUrlParameter { Name = "hashtags", Value = hashtags });

            }
            if (name != null && name != "")
            {
                urlparameter.Add(new ApiUrlParameter { Name = "q", Value = name });

            }
            string url = "https://panel.bhpsolution.com/api/Post";
            var result = ApiRequest.CreateApiRequest<ResultPost>(domain: url, method: Enum_RequestMethod.GET, api: Enum_Api.NONE, headerList: null, urlList: urlparameter, entity: null);

            ResultPost res = (ResultPost)result.Value;

            res.PageIndex = poIndex.Value;
            res.PageSize = poSize.Value;
            res.categoryLable = pocategory;
            res.ViewName = poviewName;
            res.Name = name;

            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, poviewName, res);
        }
        public ActionResult Landing(
    string pocategory = null,
    int? notId = null,
    int? pocategoryId = null,
    int? poIndex = null,
    int? poSize = null,
    int? poTypeId = null,
    string pokeyword = null,
    string poviewName = null,
    Enum_PostOrder poorder = Enum_PostOrder.NONE,
    Enum_PostOutput pooutput = Enum_PostOutput.ENTITY,
    string name = null,
    bool? active = true,
    string lang = ""
    )

        {
            poIndex = poIndex != null ? poIndex.Value : 1;
            poSize = poSize != null ? poSize.Value : 20;
            poviewName = poviewName == null ? "land" : poviewName;
            var result = new PostSearchVm();
            result = _context.Post.SearchVm(
                categoryId: pocategoryId,
                categoryLabel: pocategory,
                keyword: pokeyword,
                index: poIndex.Value,
                pageSize: poSize.Value,
                order: poorder,
                name: name,
                notId: notId,
                typeId: poTypeId,
                active: active
                );
            result.categoryId = pocategoryId;
            result.categoryLable = pocategory;
            result.Category = _context.Category.GetByLabelVm(pocategory);
            result.notId = notId;
            result.PageIndex = poIndex.Value;
            result.PageSize = poSize.Value;
            result.Name = name;
            result.typeId = poTypeId;
            result.keyword = pokeyword;
            result.ViewName = poviewName/*+"_landing"*/;
            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, result.ViewName, result);
        }
        public ActionResult Search2(
   string pocategory = null,
   string pocategory2 = null,

   int? notId = null,
   int? pocategoryId = null,
   int? poIndex = null,
   int? poSize = null,
   int? poTypeId = null,
   string pokeyword = null,
   string poviewName = null,
   Enum_PostOrder poorder = Enum_PostOrder.NONE,
   Enum_PostOutput pooutput = Enum_PostOutput.ENTITY,
   string name = null,
   bool? active = true,
   string lang = ""
   )

        {
            poIndex = poIndex != null ? poIndex.Value : 1;
            poSize = poSize != null ? poSize.Value : 20;
            poviewName = poviewName == null ? "search" : poviewName;
            var result = new PostSearchVm();
            result = _context.Post.SearchVm(
                categoryId: pocategoryId,
                categoryLabel: pocategory,
                categoryLabel2: pocategory2,
                keyword: pokeyword,
                index: poIndex.Value,
                pageSize: poSize.Value,
                order: poorder,
                name: name,
                notId: notId,
                typeId: poTypeId,
                active: active
                );
            result.categoryId = pocategoryId;
            result.categoryLable = pocategory;
            result.Category = _context.Category.GetByLabelVm(pocategory);
            result.notId = notId;
            result.PageIndex = poIndex.Value;
            result.PageSize = poSize.Value;
            result.Name = name;
            result.typeId = poTypeId;
            result.keyword = pokeyword;
            result.ViewName = poviewName;
            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, poviewName, result);
        }

        //[CustomCache(Duration = 1, cacheType = Enum_OutputCache.PostSearch,
        //   VaryByParam = "*")]

        public ActionResult GetByLabel(string label, string poviewName, string lang = "")
        {
            var post = _context.Post.GetByLabel(label);
            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, poviewName, post);

        }
        public ActionResult Exel(string label, string viewName)
        {
            var post = _context.Post.GetByLabel(label);
            if (post != null)
            {
                var file = post.WebsiteDocument;
                if (file != null)
                {
                    var path = Server.MapPath(file.Url.Replace(BaseWebsite.ShopUrl, "/"));
                    var fs = System.IO.File.OpenRead(path);
                    var bytes = new byte[fs.Length];
                    fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
                    //using (var package = new ExcelPackage(fs))
                    //{
                    //    var currentSheet = package.Workbook.Worksheets;
                    //    var workSheet = currentSheet.First();
                    //    var row = Convert.ToInt32(workSheet.Cells[1, 1].Value);
                    //    var col = Convert.ToInt32(workSheet.Cells[1, 2].Value);
                    //    var model = new string[row][];
                    //    for (int i = 1; i <= row; i++)
                    //    {
                    //        model[i - 1] = new string[col];

                    //        int j = 0;
                    //        for (; j < col; j++)
                    //        {
                    //            string value = workSheet.Cells[i + 1, j + 1].Value?.ToString();
                    //            if (value == null)
                    //            {
                    //                value = "";
                    //            }
                    //            model[i - 1][j] = value;
                    //        }
                    //    }

                    //    var a = model;
                    //    return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, viewName, model);

                    //}
                }
            }

            return null;

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}