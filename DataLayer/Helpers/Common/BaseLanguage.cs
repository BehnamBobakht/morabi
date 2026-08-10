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
    public static class BaseLanguage
    {
        public static Enum_Lang GetCurrentLanguage()
        {
            string mylang = null;
            try
            {
                if (mylang == null && HttpContext.Current!=null)
                {

                    if (HttpContext.Current?.Request.Headers["lang"] != null)
                    {
                        mylang = HttpContext.Current.Request.Headers["lang"];
                    } 
                    else if (HttpContext.Current?.Request.Cookies["LANGUAGE"] != null)
                    {
                        mylang = HttpContext.Current.Request.Cookies["LANGUAGE"].Value;
                    }
                }
                if (mylang == null && HttpContext.Current != null)
                {
                    UnitOfWork _context = new UnitOfWork();
                    WebsiteDomain domain = _context.WebsiteDomain.GetByUrl(HttpContext.Current.Request.Url);
                    if (domain != null && domain.LanguageId.HasValue)
                    {
                        mylang = domain.Language.Culture;
                    }
                    _context.Dispose();
                }
                if (mylang == null)
                {
                    mylang = Enum_Lang.FA.ToString();
                }

                string value = mylang.ToUpper();
                if (value == Enum_Lang.FA.ToString())
                    return Enum_Lang.FA;
                else if (value == Enum_Lang.EN.ToString())
                    return Enum_Lang.EN;
                else if (value == Enum_Lang.RU.ToString())
                    return Enum_Lang.RU;
                else if (value == Enum_Lang.AR.ToString())
                    return Enum_Lang.AR;
                else if (value == Enum_Lang.TR.ToString())
                    return Enum_Lang.TR;
                else if (value == Enum_Lang.GR.ToString())
                    return Enum_Lang.GR;
                else if (value == Enum_Lang.ZH.ToString())
                    return Enum_Lang.ZH;
                else
                    return Enum_Lang.FA;
            }
            catch
            {
                return Enum_Lang.FA;
            }
            //if (HttpContext.Current.Session != null)
            //{
            //    mylang = HttpContext.Current.Session["LANGUAGE"] != null ?
            //        HttpContext.Current.Session["LANGUAGE"].ToString() : null;
            //}
            //if (HttpContext.Current.Session != null)
            //{
            //    mylang = HttpContext.Current.Session["LANGUAGE"] != null ?
            //        HttpContext.Current.Session["LANGUAGE"].ToString() : null;
            //}
            //try {

            //    mylang = HttpContext.Current.Request.Cookies["LANGUAGE"].Value;
            //}
            //catch { mylang = null; }


        }
        public static int GetCurrentLanguageId(UnitOfWork context)
        {
            var current = GetCurrentLanguage().ToString();
            if (context == null)
            {
                context = new UnitOfWork();
            }
            var language = context.Language.GetByLabel(current);
            return language.ID;
        }
        public static string GetCurrentLanguageString()
        {
            var current = GetCurrentLanguage().ToString().ToLower();

            return current;
        }


        public static string GetName(this Product product, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return product.Name;
            else
            {
                ProductLanguage temp = product.ProductLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.Name)) ? temp.Name : product.Name;
            }
        }

        public static string GetH1(this Product product, Enum_Lang lang = Enum_Lang.NONE)
        {
            string h1 = "";
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
            {
                h1 = product.H1;
                if(string.IsNullOrEmpty(h1))
                {
                    h1 = product.Name;
                }

            }
            else
            {
                ProductLanguage temp = product.ProductLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
               if(temp != null)
                {
                    h1 = temp.H1;
                    if (string.IsNullOrEmpty(h1))
                    {
                        h1 = temp.Name;
                    }
                }
            }
            return h1;
        }
        public static string GetDescription(this Product product, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return product.Description;
            else
            {
                ProductLanguage temp = product.ProductLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.Body)) ? temp.Body : product.Description;
            }
        }  
        public static string GetMetaDescription(this Product product, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return product.MetaTagDescription;
            else
            {
                ProductLanguage temp = product.ProductLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.Description)) ? temp.Description : product.MetaTagDescription;
            }
        }
        public static string GetSummery(this Product product, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return product.Summary;
            else
            {
                ProductLanguage temp = product.ProductLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null &&(!string.IsNullOrEmpty(temp.Summary))? temp.Summary : product.Summary;
            }
        }
        public static string GetTitle(this Product product, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return product.Title;
            else
            {
                ProductLanguage temp = product.ProductLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.Title)) ? temp.Title : product.Title;
            }
        }


        public static string GetUrl(this ProductSubCategory productSubCategory, Enum_Lang lang = Enum_Lang.NONE)
        {

                string customLabel = "";
                if (!string.IsNullOrEmpty( productSubCategory.CustomLabel))
                customLabel = productSubCategory.CustomLabel;
                else customLabel = "product-subcategory";
                return BaseWebsite.ShopUrl + "search/" + customLabel;

        }


        public static string GetUrl(this Post post, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return post.URL;
            else
            {
                PostLanguage temp = post.PostLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null&&(!string.IsNullOrEmpty(temp.URL)) ? temp.URL : post.URL;
            }
        }


        public static string GetUrl(this ProductType productType, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return productType.Url;
            else
            {
                ProductTypeLanguage temp = productType.ProductTypeLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.Url)) ? temp.Url : productType.Url;
            }
        }
        public static string GetUrlNew(this ProductType productType, Enum_Lang lang = Enum_Lang.NONE)
        {

                string customLabel = "";
                if (!string.IsNullOrEmpty(productType.CustomLabel ))
                customLabel = productType.CustomLabel;
                else 
                customLabel = "product-type";
                return BaseWebsite.ShopUrl + "search/" + customLabel;
          


        }
        //public static string GetUrl(this ProductType productType,int prcategoryId,int prtypeId, Enum_Lang lang = Enum_Lang.NONE)
        //{
        //    string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();


        //    if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
        //    {
        //        string customLabel = "";
        //        if (productType.CustomLabel != null && productType.CustomLabel != "") customLabel = productType.CustomLabel;
        //        else customLabel = "product-category";
        //        return "/search/" + customLabel + "?proutput=" + Enum_ProductOutput.SEARCH.ToString().ToLower() + "&prcategoryId=" + prcategoryId + "&prtypeId=" + prtypeId;

        //    }

        //    else
        //    {
        //        ProductTypeLanguage temp = productType.ProductTypeLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
        //        string customLabel = "";
        //        if (temp.ProductType.CustomLabel != null && temp.ProductType.CustomLabel != "") customLabel = temp.ProductType.CustomLabel;
        //        else customLabel = "product-category";
        //        return "/search/"+ customLabel+ "?proutput="+ Enum_ProductOutput.SEARCH.ToString().ToLower()+ "&prcategoryId="+ prcategoryId+ "&prtypeId=" + prtypeId;
        //    }
        //}

        public static string GetUrl(this Product product, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return product.Url;
            else
            {
                ProductLanguage temp = product.ProductLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.Url)) ? temp.Url : product.Url;
            }
        }

        public static string GetName(this State state, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return state.Name;
            else
            {
                StateLanguage temp = state.StateLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.Name)) ? temp.Name : state.Name;
            }
        }
        public static string GetUrl(this ProductCategory productCategory, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return productCategory.Url;
            else
            {
                ProductCategoryLanguage temp = productCategory.ProductCategoryLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.Url)) ? temp.Url : productCategory.Url;
            }
        }
        public static string GetUrlNew(this ProductCategory productCategory, Enum_Lang lang = Enum_Lang.NONE)
        {

                string customLabel = "";
                if (!string.IsNullOrEmpty( productCategory.CustomLabel)) customLabel = productCategory.CustomLabel;
                else customLabel = "product-category";
                return BaseWebsite.ShopUrl + "search/" + customLabel;

        }
        public static string GetUrlDefaultLanguage(this ProductCategory productCategory, Enum_Lang lang = Enum_Lang.NONE)
        {
            string customLabel = "";
            if (productCategory.CustomLabel != null && productCategory.CustomLabel != "") customLabel = productCategory.CustomLabel;
            else customLabel = "product-category";
            return BaseWebsite.ShopUrl + "search/" + customLabel;
        }

        public static string GetH1(this ProductCategory productCategory, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return productCategory.H1;
            else
            {
                ProductCategoryLanguage temp = productCategory.ProductCategoryLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.H1)) ? temp.H1 : productCategory.H1;
            }
        }

        public static string GetMinBody(this ProductCategory productCategory, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return productCategory.MinBody;
            else
            {
                ProductCategoryLanguage temp = productCategory.ProductCategoryLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.MinBody)) ? temp.MinBody : productCategory.MinBody;
            }
        }
        public static string GetH1(this ProductType productType, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return productType.H1;
            else
            {
                ProductTypeLanguage temp = productType.ProductTypeLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.H1)) ? temp.H1 : productType.H1;
            }
        }



        public static string GetH1(this ProductSubCategory productSubCategory, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return productSubCategory.H1;
            else
            {
                ProductSubCategoryLanguage temp = productSubCategory.ProductSubCategoryLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.H1)) ? temp.H1 : productSubCategory.H1;
            }
        }

        public static string GetName(this Post post, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return post.Name;
            else
            {
                PostLanguage temp = post.PostLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.Name)) ? temp.Name : post.Name;

                //return temp != null ? temp.Name : post.Name;
            }
        }
        public static string GetH1(this Post post, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return post.H1;
            else
            {
                PostLanguage temp = post.PostLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.H1)) ? temp.H1 : post.H1;

            }
        }
        public static string GetName(this Gallery gallery, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return gallery.Name;
            else
            {
                GalleryLanguage temp = gallery.GalleryLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.Name)) ? temp.Name : gallery.Name;

            }
        }
        public static string GetName(this GalleryPictures gallery, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return gallery.Name;
            else
            {
              
                return (!string.IsNullOrEmpty(gallery.NameEn)) ? gallery.NameEn : gallery.Name;

            }
        }
        public static string GetBody(this Gallery gallery, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return gallery.Body;
            else
            {
                GalleryLanguage temp = gallery.GalleryLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.Description)) ? temp.Description : gallery.Body;

            }
        }
        public static string GetBody(this Post post, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return post.Body;
            else
            {
                PostLanguage temp = post.PostLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.Body)) ? temp.Body : post.Body;

            }
        }


        public static string GetDescription(this ProductType productType, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return productType.Description;
            else
            {
                ProductTypeLanguage temp = productType.ProductTypeLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.Description)) ? temp.Description : productType.Description;
            }
        }



        public static string GetDescription(this ProductSubCategory productSubCategory, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return productSubCategory.Description;
            else
            {
                ProductSubCategoryLanguage temp = productSubCategory.ProductSubCategoryLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.description)) ? temp.description : productSubCategory.Description;
            }
        }
        public static string GetMetaDescriptionLang(this ProductCategory productCategory, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return productCategory.DescriptionMeta;
            else
            {
                ProductCategoryLanguage temp = productCategory.ProductCategoryLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.DescriptionMeta)) ? temp.DescriptionMeta : productCategory.DescriptionMeta;
            }
        }

        public static string GetDescriptionLang(this ProductCategory productCategory, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return productCategory.Description;
            else
            {
                ProductCategoryLanguage temp = productCategory.ProductCategoryLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.Description)) ? temp.Description : productCategory.Description;
            }
        }

        public static string GetSummery(this Post post, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return post.Summary;
            else
            {
                PostLanguage temp = post.PostLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.Summary)) ? temp.Summary : post.Summary;

            }
        }

        public static string GetKeywords(this Post post, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return post.Keywords;
            else
            {
                PostLanguage temp = post.PostLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.Keywords)) ? temp.Keywords : post.Keywords;
            }
        }
        public static string GetName(this Size size, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return size.Name;
            else
            {
                SizeLanguage temp = size.SizeLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.Name)) ? temp.Name : size.Name;
            }
        }
        public static string GetName(this ProductOption option, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return option.Name;
            else
            {
                ProductOptionLanguage temp = option.ProductOptionLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.Name)) ? temp.Name : option.Name;
            }
        }
        public static string GetValue(this ProductOptionItem option, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return option.Value;
            else
            {
                ProductOptionItemLanguage temp = option.ProductOptionItemLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.Name)) ? temp.Name : option.Value;
            }
        }
        public static string GetName(this Color color, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return color.Name;
            else
            {
                ColorLanguage temp = color.ColorLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.Name)) ? temp.Name : color.Name;
            }
        }
        public static string GetName(this City city, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return city.Name;
            else
            {
                CityLanguage temp = city.CityLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.Name)) ? temp.Name : city.Name;
            }
        }
        public static string GetName(this ProductType productType, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return productType.Name;
            else
            {
                ProductTypeLanguage temp = productType.ProductTypeLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.Name)) ? temp.Name : productType.Name;
            }
        }

        public static string GetName(this ProductCategory productCategory, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return productCategory.Name;
            else
            {
                ProductCategoryLanguage temp = productCategory.ProductCategoryLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.Name)) ? temp.Name : productCategory.Name;
            }
        }
        public static string GetName(this ProductBrand productBrand, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return productBrand.Name;
            else
            {
                ProductBrandLanguage temp = productBrand.ProductBrandLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.Name)) ? temp.Name : productBrand.Name;
            }
        }
        public static string GetSummary(this ProductBrand productBrand, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return productBrand.Summary;
            else
            {
                ProductBrandLanguage temp = productBrand.ProductBrandLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.Summary)) ? temp.Summary : productBrand.Summary;
            }
        }
        public static string GetDescription(this ProductBrand productBrand, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return productBrand.Description;
            else
            {
                ProductBrandLanguage temp = productBrand.ProductBrandLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.Description)) ? temp.Description : productBrand.Description;
            }
        }
        public static string GetTitle(this ProductBrand productBrand, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return productBrand.Title;
            else
            {
                ProductBrandLanguage temp = productBrand.ProductBrandLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.Title)) ? temp.Title : productBrand.Title;
            }
        }
        public static string GetName(this ProductSubCategory productSubCategory, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return productSubCategory.Name;
            else
            {
                ProductSubCategoryLanguage temp = productSubCategory.ProductSubCategoryLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.Name)) ? temp.Name : productSubCategory.Name;
            }
        }   
        public static string GetLandingText(this ProductSubCategory productSubCategory, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return productSubCategory.LandingText;
            else
            {
                ProductSubCategoryLanguage temp = productSubCategory.ProductSubCategoryLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.LandingText)) ? temp.LandingText : productSubCategory.LandingText;
            }
        } 
        public static string GetLandingText(this ProductCategory productCategory, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return productCategory.LandingText;
            else
            {
                ProductCategoryLanguage temp = productCategory.ProductCategoryLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.LandingText)) ? temp.LandingText : productCategory.LandingText;
            }
        }  
        public static string GetLandingText(this ProductType productType, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return productType.LandingText;
            else
            {
                ProductTypeLanguage temp = productType.ProductTypeLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.LandingText)) ? temp.LandingText : productType.LandingText;
            }
        }


        public static string GetTitle(this ProductType productType, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return productType.title;
            else
            {
                ProductTypeLanguage temp = productType.ProductTypeLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.title)) ? temp.title : productType.title;
            }
        }

        public static string GetTitlePage(this ProductCategory productCategory, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return productCategory.TitlePage;
            else
            {
                ProductCategoryLanguage temp = productCategory.ProductCategoryLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.TitlePage)) ? temp.TitlePage : productCategory.TitlePage;
            }
        }




        public static string GetTitle(this ProductSubCategory productSubcategory, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return productSubcategory.Title;
            else
            {
                ProductSubCategoryLanguage temp = productSubcategory.ProductSubCategoryLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.Title)) ? temp.Title : productSubcategory.Title;
            }
        }

        public static string GetBody(this ProductSubCategory productSubCategory, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return productSubCategory.Body;
            else
            {
                ProductSubCategoryLanguage temp = productSubCategory.ProductSubCategoryLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.body)) ? temp.body : productSubCategory.Body;
            }
        }
        public static string GetMinDescription(this ProductSubCategory productSubCategory, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return productSubCategory.MinDescription;
            else
            {
                ProductSubCategoryLanguage temp = productSubCategory.ProductSubCategoryLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.MinDescription)) ? temp.MinDescription : productSubCategory.MinDescription;
            }
        }

        public static string GetBody(this ProductType productType, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return productType.body;
            else
            {
                ProductTypeLanguage temp = productType.ProductTypeLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.body)) ? temp.body : productType.body;
            }
        }
        public static string GetMinBody(this ProductType productType, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return productType.MinBody;
            else
            {
                ProductTypeLanguage temp = productType.ProductTypeLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.MinBody)) ? temp.MinBody : productType.MinBody;
            }
        }


        public static string GetName(this Category category, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return category.Name;
            else
            {
                CategoryLanguage temp = category.CategoryLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.Name)) ? temp.Name : category.Name;
            }
        }
        public static string GetBody(this Category category, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return category.Body;
            else
            {
                CategoryLanguage temp = category.CategoryLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.Body)) ? temp.Body : category.Body;
            }
        }
        public static string GetName(this ProductCustomField productCustomField, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return productCustomField.Name;
            else
            {
                ProductCustomFieldLanguage temp = productCustomField.ProductCustomFieldLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.Name)) ? temp.Name : productCustomField.Name;
            }
        }

        public static string GetValue(this ProductCustomItem productCustomItem, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return productCustomItem.Value;
            else
            {
                ProductCustomItemLanguage temp = productCustomItem.ProductCustomItemLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.Value)) ? temp.Value : productCustomItem.Value;
            }
        }

        public static string GetName(this Slider slider, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return slider.Name;
            else
            {
                SliderLanguage temp = slider.SliderLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.Name)) ? temp.Name : slider.Name;
            }
        }
        public static string GetSecondName(this Slider slider, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return slider.SecondName;
            else
            {
                SliderLanguage temp = slider.SliderLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.SecondName)) ? temp.SecondName : slider.SecondName;
            }
        }
        public static string GetName(this Banner banner, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return banner.Name;
            else
            {
                BannerLanguage temp = banner.BannerLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.Name)) ? temp.Name : banner.Name;
            }
        }
        public static string GetSummery(this Banner banner, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return banner.Summary;
            else
            {
                BannerLanguage temp = banner.BannerLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.Summary)) ? temp.Summary : banner.Summary;
            }
        }
        public static string GetName(this Survey survey, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return survey.Name;
            else
            {
                SurveyLanguage temp = survey.SurveyLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.Name)) ? temp.Name : survey.Name;
            }
        }

        public static string GetQuestion(this SurveyQuestion question, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return question.Question;
            else
            {
                SurveyQuestionLanguage temp = question.SurveyQuestionLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.Question)) ? temp.Question : question.Question;
            }
        }
        public static string GetName(this SurveyQuestionItem questionItem, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return questionItem.Name;
            else
            {
                SurveyQuestionItemLanguage temp = questionItem.SurveyQuestionItemLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null && (!string.IsNullOrEmpty(temp.Name)) ? temp.Name : questionItem.Name;
            }
        }


        public static List<WebsiteLanguage> GetWebsiteLanguages(bool withoutFa = false)
        {
            var result = new List<WebsiteLanguage>();
            UnitOfWork _context = new UnitOfWork();
            Uri url = HttpContext.Current.Request.Url;
            string panel_type = Enum_Code.SYSTEM_TYPE_PANEL.ToString();
            string label = url.Host == "localhost" ? url.Host + ":" + url.Port : url.Host;
            WebsiteDomain domain = _context.WebsiteDomain.FirstOrDefault(p =>
                p.Website.Code.Label == panel_type &&
                (
                    p.Domain == "http://" + label + "/" ||
                    p.Domain == "http://" + label ||
                    p.Domain == "https://" + label + "/" ||
                    p.Domain == "https://" + label)
                );

            if (domain != null)
            {
                result = domain.Website.WebsiteLanguage.ToList();
                if (withoutFa)
                {
                    result = result.Where(s => s.Language.Label != Enum_Lang.FA.ToString()).ToList();
                }
            }
            return result;
        }
        public static List<WebsiteLanguageVm> GetWebsiteLanguagesVm(bool withoutFa = false)
        {
            return GetWebsiteLanguages(withoutFa).Map<List<WebsiteLanguage>, List<WebsiteLanguageVm>>();
        }


        public static bool HasLanguage(Enum_Lang lang)
        {
            UnitOfWork _context = new UnitOfWork();
            Uri url = HttpContext.Current.Request.Url;
            string panel_type = Enum_Code.SYSTEM_TYPE_PANEL.ToString();
            string lang_string = lang.ToString();
            string label = url.Host == "localhost" ? url.Host + ":" + url.Port : url.Host;
            WebsiteDomain domain = _context.WebsiteDomain.FirstOrDefault(p =>
                p.Website.Code.Label == panel_type &&
                p.Website.WebsiteLanguage.Any(s =>
                    s.Language.Label == lang_string ||
                    s.Language.Culture == lang_string
                ) &&
                (
                    p.Domain == "http://" + label + "/" ||
                    p.Domain == "http://" + label ||
                    p.Domain == "https://" + label + "/" ||
                    p.Domain == "https://" + label)
                );
            return domain != null ? true : false;
        }


    }
}
