using DataLayer.Entities;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Api.Model
{
    public class ApiProductLike : ApiResponse
    {
        public static ApiResult Get(UnitOfWork _context, int accountId, int index = 1, int pageSize = 10)
        {
            var exteraModel = new ViewModels.ViewApiExtera();
            List<ProductLike> list = _context.ProductLike.GetAllByAccountId(accountId,index:index,pageSize:pageSize);

            exteraModel.Count = _context.ProductLike.GetSearchCount(accountId: accountId);
            exteraModel.List = list.ToApi();
            return CreateSuccessResult(exteraModel);
        }

        public static ApiResult Post(UnitOfWork _context, int accountId, int productId)
        {
            bool isRepeated = _context.ProductLike.IsAny(productId, accountId);
            if (isRepeated == false)
            {
                ProductLike like = new ProductLike()
                {
                    AccountId = accountId,
                    ProductId = productId,
                    Datetime = DateTime.Now
                };
                _context.ProductLike.Insert(like);
                _context.Save();
            }

            _context = new UnitOfWork();
            return Get(_context, accountId);
        }

        public static ApiResult Delete(UnitOfWork _context, int accountId, int productId)
        {
            ProductLike like = _context.ProductLike.GetByProductIdAndAccountId(productId, accountId);
            if (like != null)
            {
                _context.ProductLike.Delete(like);
                _context.Save();
            }

            _context = new UnitOfWork();
            return Get(_context, accountId);
        }
    }
}
