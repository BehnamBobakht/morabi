using Binbin.Linq;
using DataLayer.Entities;
using DataLayer.ViewModels;
using ExpressMapper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_QuestionRegister : BaseRepository<QuestionRegister>
    {
        private DatabaseEntities _context;
        public Entity_QuestionRegister(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<QuestionRegister> Search(int? typeId = null, int? pageSize = null, int? index = null, int? categoryId = null, int ?subcategoryId = null, int? brandId = null, bool inSearchResult = false)
        {
            List<QuestionRegister> results = new List<QuestionRegister>();
            var MyQuery = PredicateBuilder.True<QuestionRegister>();
            pageSize = pageSize == null ? 10 : pageSize;
            index = index == null ? 1 : index;
            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;

            typeId = typeId == 0 ? null : typeId;
            brandId = brandId == 0 ? null : brandId;
            categoryId = categoryId == 0 ? null : categoryId;
            subcategoryId = subcategoryId == 0 ? null : subcategoryId;

            
            results = _context
                .QuestionRegister
                .OrderBy(p => p.ShowNumber)
                .Where(MyQuery)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList();

            return results;
        }
        public List<QuestionRegister> SearchVm(int? typeId = null, int? pageSize = null, int? index = null, int? categoryId = null, int ?subcategoryId = null, int? brandId = null, bool? inSearchResult = false)
        {
            List<QuestionRegister> results = new List<QuestionRegister>();
            var MyQuery = PredicateBuilder.True<QuestionRegister>();
            pageSize = pageSize == null ? 10 : pageSize;
            index = index == null ? 1 : index;
            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;

            typeId = typeId == 0 ? null : typeId;
            brandId = brandId == 0 ? null : brandId;
            categoryId = categoryId == 0 ? null : categoryId;
            subcategoryId = subcategoryId == 0 ? null : subcategoryId;
            results = _context
                .QuestionRegister
                .OrderBy(p => p.ShowNumber)
                .Where(MyQuery)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList();

            return results;
        }

  
       

    }
}
