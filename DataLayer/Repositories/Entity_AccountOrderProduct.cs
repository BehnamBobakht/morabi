using Binbin.Linq;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_AccountOrderProduct : BaseRepository<AccountOrderProduct>
    {
        private DatabaseEntities _context;
        public Entity_AccountOrderProduct(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public bool Isvalid(
        int? accountId = null,

       
        int ?ProductId = null
        )
        {
         
            return _context.AccountOrderProduct.Any(s => s.AccountOrder.AccountId == accountId && s.ProductId == ProductId && (s.AccountOrder.Code.Label == Enum_Code.ORDER_STATUS_SUCCESS.ToString() || s.AccountOrder.StatusId == 2073 || s.AccountOrder.StatusId == 2068));
        }
        public IQueryable<ViewPrice> GetPrice(
       int? accountId = null,


       int? ProductId = null
       )
        {
             var x= _context.AccountOrderProduct.Where(p => (p.ProductId == ProductId || p.Product.ProductsId.Contains(ProductId.ToString())) && (p.AccountOrder.StatusId == 2068 || p.AccountOrder.StatusId == 2073) && p.AccountOrder.AccountId == accountId).Select(s => new ViewPrice { Datetime = s.Datetime, Price = s.Price }).Take(1);
            return x;
        }
        public int SearchCount(
         int? accountId = null,

         Enum_Code status = Enum_Code.ORDER_STATUS_NONE,
         int[] statusId = null,
         string product = null,
         DateTime? fromDatetime = null,
         DateTime? toDatetime = null,

           int? typeId = null
         )
        {
            var MyQuery = GetSearchQuery(
                accountId: accountId,

                fromDatetime: fromDatetime,
                product: product,
                status: status,
                statusId: statusId,
                toDatetime: toDatetime,

                typeId: typeId
                );
            return _context.AccountOrderProduct.Count(MyQuery);
        }
        public IQueryable<AccountOrderProduct> Report(

             int? accountId = null,

         Enum_Code status = Enum_Code.ORDER_STATUS_NONE,
         int[] statusId = null,
         string product = null,
         DateTime? fromDatetime = null,
         DateTime? toDatetime = null,
         int? optionId = null,

           int? typeId = null
            )
        {
            var MyQuery = GetSearchQuery(
                accountId: accountId,
                optionId: optionId,
                fromDatetime: fromDatetime,
                product: product,
                status: status,
                statusId: statusId,
                toDatetime: toDatetime,

                typeId: typeId
                );

            var results = _context
                .AccountOrderProduct
                .Where(MyQuery);

            return results;
        }
        private Expression<Func<AccountOrderProduct, bool>> GetSearchQuery(
         int? accountId = null,
         int? optionId = null,
         Enum_Code status = Enum_Code.ORDER_STATUS_NONE,
         int[] statusId = null,
         string product = null,
         DateTime? fromDatetime = null,
         DateTime? toDatetime = null,
         string statusId2 = "",
           int? typeId = null
         )
        {
            List<AccountOrderProduct> results = new List<AccountOrderProduct>();
            var MyQuery = PredicateBuilder.True<AccountOrderProduct>();

            if (accountId != null)
                MyQuery = MyQuery.And(p => p.AccountOrder.AccountId == accountId);
            if (optionId != null && optionId!=0)
                MyQuery = MyQuery.And(p => p.StoreProductQuantity.ProductQuantity.OptionItemId == optionId);

            if (typeId != null && typeId!=0 && typeId != 5)
                MyQuery = MyQuery.And(p => (p.Product.ProductTypeId == typeId));
            if (typeId != null && typeId != 0 && typeId == 5) 
                MyQuery = MyQuery.And(p => (p.Product.ProductTypeId == 1001) || (p.Product.ProductTypeId == 2 && p.Product.Exame.Count()>0));

            if (statusId2 != "")
            {
                var ides = statusId2.Split('-');
                var QueryStr = PredicateBuilder.False<AccountOrderProduct>();
                foreach (var item in ides)
                {
                    int itemId =int.Parse(item);
                    if (itemId != 0)
                    {
                        QueryStr = QueryStr.Or(p =>
                            p.AccountOrder.StatusId == itemId
                        );
                    }
                }
                MyQuery = MyQuery.And(QueryStr);
            }

            if (status != Enum_Code.ORDER_STATUS_NONE)
            {

                MyQuery = MyQuery.And(p => p.AccountOrder.Code.Label == status.ToString());
            }

            if (statusId != null && statusId.Any())
            {
                var QueryStr = PredicateBuilder.False<AccountOrderProduct>();
                foreach (var item in statusId)
                {
                    int itemId = item;
                    if (itemId != 0)
                    {
                        QueryStr = QueryStr.Or(p =>
                            p.AccountOrder.StatusId == itemId
                        );
                    }
                }
                MyQuery = MyQuery.And(QueryStr);
            }

            if (fromDatetime != null && fromDatetime != default(DateTime))
                MyQuery = MyQuery.And(p => p.AccountOrder.Datetime >= fromDatetime);

            if (toDatetime != null && toDatetime != default(DateTime))
            {
                DateTime toDatetimeTemp = toDatetime.Value.AddDays(1);
                MyQuery = MyQuery.And(p => p.AccountOrder.Datetime <= toDatetimeTemp);
            }

            return MyQuery;
        }
        public List<AccountOrderProduct> Search(
         int? index = 1,
         int? pageSize = null,
         int? accountId = null,

         Enum_Code status = Enum_Code.ORDER_STATUS_NONE,
         int[] statusId = null,
         List<string> statusStr = null,
         string product = null,

         DateTime? fromDatetime = null,
         DateTime? toDatetime = null,
         int? optionId = null,
         int? typeId = null,
         string statusId2 = ""
         )
        {
            var MyQuery = GetSearchQuery(
                accountId: accountId,
                optionId: optionId,
                fromDatetime: fromDatetime,
                product: product,
                status: status,
                statusId: statusId,
                statusId2: statusId2,

                toDatetime: toDatetime,

                typeId: typeId
                );

            pageSize = pageSize == null ? 1000 : pageSize;
            index = index == null ? 1 : index;
            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;

            List<AccountOrderProduct> results = _context
                .AccountOrderProduct
                .OrderByDescending(p => p.ID)
                .Where(MyQuery)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList();

            return results;
        }
    }
}
