using Binbin.Linq;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_Clubs : BaseRepository<Clubs>
    {
        private DatabaseEntities _context;
        public Entity_Clubs(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        private Expression<Func<Clubs, bool>> GetSearchQuery(DataLayer.Enumarables.Enum_Code? typeId = null,
          int? stateId = null,
     bool? isTop = null,
     bool ? isTopMap=null,
     bool? active = null)
        {
            var MyQuery = PredicateBuilder.True<Clubs>();
            if (stateId != null)
                MyQuery = MyQuery.And(p => p.StateId == stateId);
            if (isTop != null)
            {
                if (isTop == false)
                {
                    MyQuery = MyQuery.And(p => p.IsTop == isTop || p.IsTop==null);
                }
                else
                {
                    MyQuery = MyQuery.And(p => p.IsTop == isTop);
                }

            }
            if (isTopMap != null)
            {
                
                    MyQuery = MyQuery.And(p => p.IsTopMap == isTopMap);
                

            }
            if (active != null)
            {
                MyQuery = MyQuery.And(p => p.Active == active);

            }
            if (typeId != null)
            {
                MyQuery = MyQuery.And(p => p.Code.Label == typeId.ToString());

            }

            return MyQuery;
        }
        public List<Clubs> Search(
            DataLayer.Enumarables.Enum_Code? typeId = null,
              int? stateId = null,
     bool? isTop = null,
     bool ? isTopMap=null,
      bool? active = null,
          int index = 1, int pageSize = 10)
        {
            var MyQuery = GetSearchQuery(isTopMap: isTopMap,typeId: typeId, stateId: stateId, isTop: isTop, active: active);
            int skipValue = pageSize * (index - 1);
            int pageValue = pageSize;
            List<Clubs> results = new List<Clubs>();

            results = _context
                .Clubs
                .OrderBy(p => p.ShowNumbr).
                ThenBy(p => p.ID)
                .Where(MyQuery)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList();


            return results;
        }

        public int SearchCount(DataLayer.Enumarables.Enum_Code? typeId = null, int? stateId = null,
     bool? isTop = null,
      bool? active = null
          )
        {
            var MyQuery = GetSearchQuery(typeId: typeId, stateId: stateId, isTop: isTop, active: active);
            return _context.Clubs.Count(MyQuery);
        }
    }
}

