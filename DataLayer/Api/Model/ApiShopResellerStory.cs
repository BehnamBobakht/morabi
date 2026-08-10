using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Api.Model
{
    public class ApiShopResellerStory : ApiResponse
    {
        public static ApiResult Get(UnitOfWork _context)
        {
            List<ShopResellerStory> list = _context.ShopResellerStory.Search();
            List<ViewShopResellerStory> listStory = new List<ViewShopResellerStory>();
            var groupReseller = list.GroupBy(p => p.ShopResellerId);
            foreach (var item in groupReseller)
            {
                int resellerId = item.Key;
                ShopReseller reseller = list.FirstOrDefault(p => p.ShopResellerId == resellerId).ShopReseller;
                ViewShopResellerStory resellerItem = new ViewShopResellerStory
                {
                    ResellerId = resellerId,
                    Picture = new ViewPicture(reseller.Picture),
                    Name = reseller.Name,
                    Link = reseller.GetLink()
                };
                List<ShopResellerStory> listStoryItem = list.Where(p => p.ShopResellerId == resellerId).ToList();
                List<ViewShopResellerStoryItem> storyItemList = new List<ViewShopResellerStoryItem>();
                foreach (var storyItem in listStoryItem)
                {
                    storyItemList.Add(new ViewShopResellerStoryItem() {
                        Id = storyItem.ID,
                        Title = storyItem.ShopReseller.Name,
                        Picture = new ViewPicture(storyItem.Picture)
                    });
                }
                resellerItem.Items = storyItemList;
                listStory.Add(resellerItem);
            }
            return CreateSuccessResult(listStory);
        }
    }
}
