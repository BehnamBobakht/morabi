using Binbin.Linq;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using ExpressMapper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_Category : BaseRepository<Category>
    {
        DatabaseEntities _context;



        public Entity_Category(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

    public List<Category> GetAllByType(Enum_Code Type, bool HasDefault = false)
        {
            string type = Type.ToString();
            List<Category> list = new List<Category>();
            if (HasDefault == true)
                list.Add(new Category() { Name = "انتخاب" });
            list.AddRange(_context.Category.Where(p => p.Code.Label == type).ToList());
            return list;
        }

        public List<Category> GetAllByType(Enum_CodeGroup Type, bool HasDefault = false)
        {
            string type = Type.ToString();
            List<Category> list = new List<Category>();
            if (HasDefault == true)
                list.Add(new Category() { Name = "انتخاب" });
            list.AddRange(_context.Category.Where(p => p.Code.CodeGroup.Label == type).ToList());
            return list;
        }

        public CategoryVm GetByLabelVm(string label)
        {
            var model= _context.Category.FirstOrDefault(p => p.Label == label);
            if (model != null)
                return model.Map<Category, CategoryVm>();
            return null;
        }  
        public Category GetByLabel(string label)
        {
            var model= _context.Category.FirstOrDefault(p => p.Label == label);
            if (model != null)
                return model;
            return null;
        }

        public List<Category> Search(
            int? notId = 0,
            string type = null,
            int? pageSize = 10,
            int? index = 1,
            string name = null,
            string keyWord =null,
            int? typeId = 0

            )
        {
            var MyQuery = PredicateBuilder.True<Category>();
            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;
            if (!string.IsNullOrEmpty(type) )
                MyQuery = MyQuery.And(p => p.Code.Label.ToLower() == type.ToLower());
            if (notId != null && notId != 0)
                MyQuery = MyQuery.And(p => p.ID != notId);
            if (typeId != null && typeId != 0)
                MyQuery = MyQuery.And(p => p.TypeId == typeId);
            if (name != null)
                MyQuery = MyQuery.And(p => p.Name.Contains(name));
            if(keyWord != null)
                MyQuery = MyQuery.And(p => p.Label == keyWord);

            return _context
                .Category
                .OrderBy(p => p.ID)
                .Where(MyQuery)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList();
        }
    }
}
