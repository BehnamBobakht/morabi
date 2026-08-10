using Binbin.Linq;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_DiscountGroup : BaseRepository<DiscountGroup>
    {
        private DatabaseEntities _context;

        public Entity_DiscountGroup(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public DiscountGroup GetByLabel(string label)
        {
            return FirstOrDefault(s => s.Label == label);
        }
        public List<DiscountGroup> Search(
            Enum_Code type = Enum_Code.DISCOUNT_TYPE_NONE,
            Enum_Code valueType = Enum_Code.DISCOUNTGROUP_NONE,
            string label = null,
            int? notId = null,
            int? index = null,
            Boolean? active = null,
            Boolean? activeDate = null,
            int? pageSize = null)
        {
            var MyQuery = PredicateBuilder.True<DiscountGroup>();
            index = index == null ? 1 : index;
            pageSize = pageSize == null ? 10 : pageSize;

            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;
            string typeString = type.ToString();
            MyQuery = MyQuery.And(p => p.Deleted ==false);

            if (notId != null && notId != 0)
                MyQuery = MyQuery.And(p => p.ID != notId);
            if (label != null)
                MyQuery = MyQuery.And(p => p.Label == label);
            if (type != Enum_Code.DISCOUNT_TYPE_NONE)
                MyQuery = MyQuery.And(p => p.Code.Label == typeString);
            switch(valueType)
            {
                case Enum_Code.DISCOUNTGROUP_TYPE_PRICE:
                    {
                        MyQuery = MyQuery.And(p => p.Code.Label== Enum_Code.DISCOUNTGROUP_TYPE_PRICE.ToString());

                        break;
                    }
                case Enum_Code.DISCOUNTGROUP_TYPE_PERCENT:
                    {
                        MyQuery = MyQuery.And(p => p.Code.Label == Enum_Code.DISCOUNTGROUP_TYPE_PERCENT.ToString());
                        break;
                    }
            }
            if (active != null)
            {
                MyQuery = MyQuery.And(p => p.Active == active);
            }
            if (activeDate != null)
            {
                MyQuery = MyQuery.And(p => (p.StartDatetime==null || (p.StartDatetime.HasValue && p.StartDatetime.Value < DateTime.Now)) && (p.EndDatetime == null || (p.EndDatetime.HasValue && p.EndDatetime.Value >= DateTime.Now)));
            }

            return _context
                .DiscountGroup
                .OrderByDescending(p => p.ID)
                .Where(MyQuery)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList();
        }
    }
}
