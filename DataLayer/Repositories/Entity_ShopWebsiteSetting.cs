using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ShopWebsiteSetting : BaseRepository<ShopWebsiteSetting>
    {
        private DatabaseEntities _context;
        public Entity_ShopWebsiteSetting(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public ShopWebsiteSetting GetSettingByWebsiteId(int websiteId)
        {
            var setting = _context.ShopWebsiteSetting.FirstOrDefault(p =>
                 p.WebsiteId == websiteId
            );
            if (setting == null)
            {
                setting = GetDefaultValues();
                setting.WebsiteId = websiteId;
                _context.ShopWebsiteSetting.Add(setting);
                _context.SaveChanges();
            }
            return setting;
        }

        public ShopWebsiteSetting GetDefaultValues()
        {
            return new ShopWebsiteSetting()
            {
                HasBrand = false,
                HasColor = false,
                HasCompany = false,
                HasCustomFields = true,
                HasDescription = false,
                HasGarranty = false,
                HasLastPrice = false,
                HasMinOrder = false,
                HasPicture = true,
                HasPrice = true,
                HasProductCategory = true,
                HasProductSubCategory = false,
                HasProductType = true,
                HasShop = false,
                HasSize = false,
                HasStatus = true,
                HasSummary = false,
                HasShowHomePage = false,
                HasShowNumber = false,
                MaxShopProductCount = 10000,
                HasDocument = false,
                HasQuantity = false,
                HasQuantityColor = false,
                HasCodeValue = false,
                HasUnit = false,
                HasActive = false,
                HasStore = false,
                HasProductOption = false,
                HasRequierdCity = false,
                HasRequierdState = false,
                HasSeo = false,
                HasUserName = false,
                HasService=false,
                HasUnitPrice=false,
                UnitPrice=0,
                MobileRegix = @"^[0][9]\d{9}",
                PhoneRegix = "",
                ZipCodeRegix = @"^\d{10}$",
                EmailRegix = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",
                PasswordRegix = @"^(?=.*\d)(?=.*[a-z]).{6,}$",
        };
        }
    }
}
