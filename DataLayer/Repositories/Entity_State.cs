using Binbin.Linq;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using ExpressMapper.Extensions;
using DataLayer.ViewModels;

namespace DataLayer.Repositories
{
    public class Entity_State : BaseRepository<State>
    {
        private DatabaseEntities _context;
        public Entity_State(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<StateVm> Search(
            int? countryId = null,
            bool withAll = true,
            int? index = null,
            int? pageSize = null,
            string name = null)
        {
            var MyQuery = PredicateBuilder.True<State>();
            index = index == null ? 1 : index;
            pageSize = pageSize == null ? 40 : pageSize;

            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;

            if (withAll == false)
                MyQuery = MyQuery.And(p => p.ID !=(int) Enumarables.StateCity.AllStateId);
            if (countryId != null && countryId != 0)
                MyQuery = MyQuery.And(p => p.CountryId == countryId);
            if (name != null)
                MyQuery = MyQuery.And(p => p.Name.Contains(name));
           
            return _context
                .State
                .OrderBy(p => p.ID)
                .Where(MyQuery)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList().Map<List<State>,List<StateVm>>();
        }
    }
}
