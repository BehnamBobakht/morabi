using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ProductNotify : BaseRepository<ProductNotify>
    {
        private DatabaseEntities _context;
        public Entity_ProductNotify(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public bool IsAny(int productId, int accountId)
        {
            return _context.ProductNotify.Any(p => 
                p.AccountId == accountId && 
                p.ProductId == productId);
        }

        public List<ProductNotify> GetAllByProductId(int productId)
        {
            return _context.ProductNotify.Where(p =>
                p.ProductId == productId
            ).ToList();
        }
        public List<ProductNotify> GetAllByAccountId(int accountId)
        {
            return _context.ProductNotify.Where(p =>
                p.AccountId == accountId
            ).ToList();
        }

        public void DeleteByProductId(int productId)
        {
            List<ProductNotify> list = GetAllByProductId(productId);
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
            Save();
        }
        public void SendMessage(Product product,UnitOfWork _context)
        {
            List<ProductNotify> list = GetAllByProductId(product.ID);

            ViewWebsite website = BaseWebsite.ShopWebsite;
            foreach (var item in list)
            {        
                StringBuilder str = new StringBuilder();
            string token1 = product.GetName();
            string token2 = website.Domain+ product.GetLinkWithUrl();
            string token3 = "";
            str.AppendLine(string.Format(Resource.Pattern.ProductNotify,token1,Base.BaseContent.WebsiteDetail.Title,token2));

            _context.Sms.SaveNewSms(item.Account.Mobile, Enum_SmsType.PRODUCT_NOTIFY, str.ToString(), token1, token2, token3);
            _context.Email.SaveNewEmail(item.Account.Email, Enum_EmailType.PRODUCT_NOTIFY, str.ToString(), Resource.Lang.Available);
            _context.Save();
                DeleteByProductId(product.ID);
            }


        }
    }
}
