using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_StaticPage : BaseRepository<StaticPage>
    {
        private DatabaseEntities _context;
        public Entity_StaticPage(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public StaticPage GetByCategory(string enumCategory)
        {
            var model = _context.StaticPage.FirstOrDefault(s => s.Category.Label.ToLower() == enumCategory.ToLower());
            return model;
        }
    }
}
