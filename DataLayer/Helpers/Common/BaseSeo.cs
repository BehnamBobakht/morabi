using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;
using DataLayer.ViewModels;

namespace DataLayer.Base
{
    public static class BaseSeo
    {
        public static ViewSeoPack GetSeoPack(this Post post)
        {
            string WebsiteName = BaseWebsite.ShopWebsite.Title;
            ViewSeoPack pack = new ViewSeoPack();
            pack.Title = post.Name;
            pack.Description = post.Summary;
            pack.Keywords = post.Name + ", " + post.Category.Name;
            pack.OpenGraph = new ViewSeoOpenGraph()
            {
                Url = post.GetLink(),
                Card = post.Summary,
                Description = post.Summary,
                SiteName = WebsiteName,
                Title = post.Name,
                Type = post.Category.Name,
                Locale = "ir_FA",
            };
            return pack;
        }

        public static ViewSeoPack GetSeoPack(this Product product, int amount = 0)
        {
            string image = product.Picture.GetUrl();
            ViewSeoPack pack = new ViewSeoPack();
            pack.Title = product.GetName();
            pack.Description = product.Summary;
            pack.Keywords = product.Name;
            pack.OpenGraph = new ViewSeoOpenGraph() {
                Url = product.GetLink(),
                Card = product.Summary,
                Description = product.Summary,
                SiteName = product.Shop.Name,
                Title = product.GetName(),
                Type = "Products",
                Locale = "ir_FA",
                Amount = amount.ToString(),
                Image = product.Picture.GetUrl(),
                Image_Type = image.GetImageType(),
                Image_Width = "250",
                Image_Height = "250",
                Currency = "IRR"
            };
            return pack;
        }
    }
}
