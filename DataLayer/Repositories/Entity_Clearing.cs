using Binbin.Linq;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_Clearing : BaseRepository<Clearing>
    {
        private DatabaseEntities _context;
        public Entity_Clearing(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public int SearchCount(
            string text=null,
            int? storeId=null,
            int[] codeId = null,
            List<string> codeStr = null,
            DateTime? fromDatetime = null,
            DateTime? toDatetime = null
           )
        {
            var MyQuery = GetSearchQuery(
                text: text,
                storeId:storeId,
                codeId: codeId,
                codeStr: codeStr,
                fromDatetime: fromDatetime,
                toDatetime: toDatetime
                );
            return _context.Clearing.Count(MyQuery);
        }

        public List<Clearing> Search(
            string text = null,
            int? storeId = null,
            int[] codeId = null,
            List<string> codeStr = null,
            DateTime? fromDatetime = null,
            DateTime? toDatetime = null,
            int? index = 1,
            int? pageSize = 10
            )
        {
            var MyQuery = GetSearchQuery(
                text: text,
                storeId: storeId,
                codeId: codeId,
                codeStr: codeStr,
                fromDatetime: fromDatetime,
                toDatetime: toDatetime
                );

            pageSize = pageSize == null ? 10 : pageSize;
            index = index == null ? 1 : index;
            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;

            List<Clearing> results = _context
                .Clearing
                .OrderByDescending(p => p.ID)
                .Where(MyQuery)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList();

            return results;
        }

        private Expression<Func<Clearing, bool>> GetSearchQuery(
            string text = null,
            int? storeId = null,
            int[] codeId = null,
            List<string> codeStr = null,
            DateTime? fromDatetime = null,
            DateTime? toDatetime = null
            )
        {
            List<Clearing> results = new List<Clearing>();
            var MyQuery = PredicateBuilder.True<Clearing>();

            if (storeId != null)
                MyQuery = MyQuery.And(p => p.StoreId == storeId);

            if (IsNullOrEmptyId(text) == false)
            {
                var nameQuery = PredicateBuilder.True<Clearing>();

                    nameQuery = nameQuery.And(p =>
                    p.Detail.Contains(text));

                MyQuery = MyQuery.And(nameQuery);
            }

            if (codeId != null && codeId.Any())
            {
                var QueryStr = PredicateBuilder.False<Clearing>();
                foreach (var item in codeId)
                {
                    int itemId = item;
                    if (itemId != 0)
                    {
                        QueryStr = QueryStr.Or(p =>
                            p.CodeId == itemId
                        );
                    }
                }
                MyQuery = MyQuery.And(QueryStr);
            }

            if (codeStr != null)
            {
                var QueryStr = PredicateBuilder.False<Clearing>();
                foreach (var item in codeStr)
                {
                    if (string.IsNullOrEmpty(item) == false)
                    {
                        QueryStr = QueryStr.Or(p => p.Code.Label == item);
                    }
                }
                MyQuery = MyQuery.And(QueryStr);
            }

            if (fromDatetime != null && fromDatetime != default(DateTime))
                MyQuery = MyQuery.And(p => p.CreateDate >= fromDatetime);

            if (toDatetime != null && toDatetime != default(DateTime))
            {
                DateTime toDatetimeTemp = toDatetime.Value.AddDays(1);
                MyQuery = MyQuery.And(p => p.CreateDate <= toDatetimeTemp);
            }

            return MyQuery;
        }
    }
}
