using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Binbin.Linq;
using DataLayer.Entities;
using static DataLayer.ViewModels.ViewProductLike;
using static DataLayer.ViewModels.ViewProductLikeFavorit;

namespace DataLayer.Repositories
{
    public class Entity_ProductLike : BaseRepository<ProductLike>
    {
        private DatabaseEntities _context;
        public Entity_ProductLike(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public bool IsAny(int ProductId, int AccountId)
        {
            return _context.ProductLike.Any(P => P.AccountId == AccountId && P.ProductId == ProductId);
        }

        public int GetCountByShopId(int shopId)
        {
            return _context.ProductLike.Count(p =>
                p.Product.ShopId == shopId
            );
        }

        public ProductLike GetByProductIdAndAccountId(int ProductId, int AccountId)
        {
            return _context.ProductLike.FirstOrDefault(p =>
                p.AccountId == AccountId &&
                p.ProductId == ProductId
            );
        }

        public List<ProductLike> GetAllByProductId(int productId)
        {
            return _context.ProductLike.Where(p =>
                p.ProductId == productId
            ).ToList();
        }
        public List<ProductLike> GetAllByAccountId(int accountId, int index = 1, int pageSize = 10)
        {
            var query = GetSearchQuery(accountId: accountId);
            int skipValue = pageSize * (index - 1);
            int pageValue = pageSize;
            return _context.ProductLike.Where(query).OrderByDescending(o => o.ID).Skip(skipValue)
                    .Take(pageValue)
                    .ToList();
        }
        public int GetSearchCount(int accountId)
        {
            var query = GetSearchQuery(accountId: accountId);
            return _context.ProductLike.Where(query).Count();
        }


        public void DeleteByProductId(int productId)
        {
            List<ProductLike> list = GetAllByProductId(productId);
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }
        public void DeleteByProductIdAndAccountId(int accountId, int productId)
        {
            ProductLike like = GetByProductIdAndAccountId(productId, accountId);
            Delete(like);
        }


        public Expression<Func<ProductLike, bool>> GetSearchQuery(int accountId)
        {
          
            var MyQuery = PredicateBuilder.True<ProductLike>();
            MyQuery = MyQuery.And(p => p.AccountId == accountId);
            return MyQuery;
        }
        public List<ProductLike> Search(int index = 1, int pageSize = 10, Expression<Func<ProductLike, bool>> query=null)
        {
            List<ProductLike> results = new List<ProductLike>();
            int skipValue = pageSize * (index - 1);
            int pageValue = pageSize;
            IEnumerable<ProductLike> q = _context.ProductLike.Where(query);
            results = q
                   .Skip(skipValue)
                   .Take(pageValue)
                   .ToList();
            return results;
        }
        public ViewProductLikeDetail SearchDetail( Expression<Func<ProductLike, bool>> query = null)
        {
            var result = new ViewProductLikeDetail();
           
            IEnumerable<ProductLike> q = _context.ProductLike.Where(query);

            if (q.Any())
            {
                result.Count = q.Count();
                
            }
            return result;
        }
    }
}
